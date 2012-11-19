using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameConstructLibrary;
using GraphicsLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using BEPUphysics;
using BEPUphysics.CollisionShapes;
using BEPUphysics.Collidables;
using BEPUphysics.CollisionShapes.ConvexShapes;

namespace MapEditor
{
    public class MapEntity
    {
        
        private const float speed = 0.5f;
        private const float sensitivity = 1.0f;
        private const float length = 2000.0f;

        private MapEditor mMapEditor;

        private KeyInputAction mForward;
        private KeyInputAction mBackward;
        private KeyInputAction mLeft;
        private KeyInputAction mRight;
        private KeyInputAction mDown;
        private KeyInputAction mUp;

        private MouseState mMouse;
        private Vector2 mMouseClick;
        private MouseButtonInputAction mLeftPressed;
        private MouseButtonInputAction mRightPressed;
        private MouseButtonInputAction mLeftHold;
        private MouseButtonInputAction mRightHold;
        private MouseButtonInputAction mLeftReleased;
        private MouseButtonInputAction mRightReleased;

        private DummyMap mDummyMap;
        public DummyMap DummyMap { get { return mDummyMap; } set { mDummyMap = value; } }
        private Viewport mViewport;

        private Camera mCamera;
        private Vector3 mMovement;
        private Vector3 mDirection;

        private InanimateModel mModel;
        private Vector3 mPosition;
        private Vector3 mScale;
        private Vector3 mOrientation;

        private float mSize;

        public MapEntity(MapEditor mapEditor, Camera camera, Viewport viewport)
        {

            mMapEditor = mapEditor;
            mCamera = camera;
            mViewport = viewport;

            Initialize();

        }

        private void Initialize()
        {
            mForward = new KeyInputAction(0, InputAction.ButtonAction.Down, Keys.W);
            mBackward = new KeyInputAction(0, InputAction.ButtonAction.Down, Keys.S);
            mLeft = new KeyInputAction(0, InputAction.ButtonAction.Down, Keys.A);
            mRight = new KeyInputAction(0, InputAction.ButtonAction.Down, Keys.D);
            mDown = new KeyInputAction(0, InputAction.ButtonAction.Down, Keys.Q);
            mUp = new KeyInputAction(0, InputAction.ButtonAction.Down, Keys.E);

            mMouseClick = new Vector2(0.0f, 0.0f);
            mLeftPressed = new MouseButtonInputAction(0, InputAction.ButtonAction.Pressed, MouseButtonInputAction.MouseButton.Left);
            mRightPressed = new MouseButtonInputAction(0, InputAction.ButtonAction.Pressed, MouseButtonInputAction.MouseButton.Right);
            mLeftHold = new MouseButtonInputAction(0, InputAction.ButtonAction.Down, MouseButtonInputAction.MouseButton.Left);
            mRightHold = new MouseButtonInputAction(0, InputAction.ButtonAction.Down, MouseButtonInputAction.MouseButton.Right);
            mLeftReleased = new MouseButtonInputAction(0, InputAction.ButtonAction.Released, MouseButtonInputAction.MouseButton.Left);
            mRightReleased = new MouseButtonInputAction(0, InputAction.ButtonAction.Released, MouseButtonInputAction.MouseButton.Right);

            mMovement = new Vector3(0.0f, 0.0f, 0.0f);
            mDirection = new Vector3(0.0f, 0.0f, 0.0f);

            mModel = new InanimateModel("sphere");
            mPosition = new Vector3(0);
            mSize = 1;

        }

        public void Update(GameTime gameTime)
        {
            InputAction.Update();
            mMouse = Mouse.GetState();

            UpdatePosition();
            UpdateOrientation();
            UpdatePicking();

            //mCamera.MoveUp(speed * mMovement.Z * gameTime.ElapsedGameTime.Milliseconds);
            mCamera.MoveForward(speed * mMovement.Y * gameTime.ElapsedGameTime.Milliseconds);
            mCamera.MoveRight(speed * mMovement.X * gameTime.ElapsedGameTime.Milliseconds);
            mCamera.RotatePitch(sensitivity * mDirection.Y * gameTime.ElapsedGameTime.Milliseconds);
            mCamera.RotateYaw(sensitivity * mDirection.X * gameTime.ElapsedGameTime.Milliseconds);

        }

        private void UpdatePosition()
        {
            mMovement.Z = (float)(mUp.Degree - mDown.Degree);
            mMovement.Y = (float)(mForward.Degree - mBackward.Degree);
            mMovement.X = -(float)(mRight.Degree - mLeft.Degree);
        }

        private void UpdateOrientation()
        {
            if (mRightPressed.Active == true)
            {
                mMouseClick.Y = mMouse.Y;
                mMouseClick.X = mMouse.X;
            }

            if (mRightHold.Active == true)
            {
                mDirection.Y = MathHelper.ToRadians((mMouse.Y - mMouseClick.Y) * sensitivity); // pitch
                mDirection.X = -MathHelper.ToRadians((mMouse.X - mMouseClick.X) * sensitivity); // yaw
                mMouseClick.Y = mMouse.Y;
                mMouseClick.X = mMouse.X;
            }

            if (mRightReleased.Active == true)
            {
                mDirection.Y = 0;
                mDirection.X = 0;
            }
        }

        private void UpdatePicking()
        {
            Vector3 nearScreen = new Vector3(mMouse.X, mMouse.Y, 0.0f);
            Vector3 farScreen = new Vector3(mMouse.X, mMouse.Y, 1.0f);
            Vector3 nearWorld = mViewport.Unproject(nearScreen, mCamera.ProjectionTransform, mCamera.ViewTransform, Matrix.Identity);
            Vector3 farWorld = mViewport.Unproject(farScreen, mCamera.ProjectionTransform, mCamera.ViewTransform, Matrix.Identity);
            Vector3 projectionDirection = (farWorld - nearWorld);
            projectionDirection.Normalize();
            Ray ray = new Ray(mCamera.Position, projectionDirection);
            RayHit result;
            if (mMapEditor.DummyMap.TerrainPhysics.StaticCollidable.RayCast(ray, length, out result))
            {

                mPosition = result.Location;
                if (mMapEditor.State == States.Height)
                {
                    mModel = new InanimateModel("sphere");

                    int dummySize;
                    int dummyIntensity;
                    bool dummyFeather;
                    bool dummySet;
                    mMapEditor.MapEditorDialog.GetHeightEditorInput(out dummySize, out dummyIntensity, out dummyFeather, out dummySet);
                    mSize = dummySize;

                    if (mLeftHold.Active)
                    {
                        mMapEditor.DummyMap.Action(result.Location);
                    }

                }
                else if (mMapEditor.State == States.Object)
                {
                    string dummyObjectType;
                    string dummyObjectModel;
                    Vector3 dummyObjectScale;
                    Vector3 dummyObjectOrientation;
                    string[] dummyObjectParameters;
                    
                    if (mMapEditor.MapEditorDialog.GetObjectEditorInput(out dummyObjectType, out dummyObjectModel, out dummyObjectScale, out dummyObjectOrientation, out dummyObjectParameters)){
                        mModel = new InanimateModel(dummyObjectModel);
                        mScale = dummyObjectScale;
                        mOrientation = dummyObjectOrientation;
                    }

                    if (mLeftPressed.Active)
                    {
                        mMapEditor.DummyMap.Action(result.Location);
                    }
                }
            }
        }

        public void Render()
        {
            if (mMapEditor.State == States.Height)
                mModel.Render(mPosition, new Vector3(0, 0, 1), new Vector3(mSize));
            else if (mMapEditor.State == States.Object)
                mModel.Render(mPosition, mOrientation, mScale);
        }

    }
}
