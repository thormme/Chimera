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
    public class MapEditorEntity
    {

        private Vector3 scale = new Vector3(2.0f, 0.25f, 2.0f);

        private const float speed = 0.1f;
        private const float sensitivity = 0.1f;
        private const float length = 2000.0f;

        private KeyInputAction mForward;
        private KeyInputAction mBackward;
        private KeyInputAction mLeft;
        private KeyInputAction mRight;
        private KeyInputAction mDown;
        private KeyInputAction mUp;

        private MouseState mMouse;
        private Vector2 mMouseClick;
        private MouseButtonInputAction mRightPressed;
        private MouseButtonInputAction mRightHold;
        private MouseButtonInputAction mRightReleased;

        private Vector3 mMovement;
        private Vector3 mDirection;

        private InanimateModel mModel;
        private Vector3 mPosition;
        private Vector3 mScale;
        private Vector3 mOrientation;

        public MapEditorEntity()
        {

            mForward = new KeyInputAction(0, InputAction.ButtonAction.Down, Keys.W);
            mBackward = new KeyInputAction(0, InputAction.ButtonAction.Down, Keys.S);
            mLeft = new KeyInputAction(0, InputAction.ButtonAction.Down, Keys.A);
            mRight = new KeyInputAction(0, InputAction.ButtonAction.Down, Keys.D);
            mDown = new KeyInputAction(0, InputAction.ButtonAction.Down, Keys.Q);
            mUp = new KeyInputAction(0, InputAction.ButtonAction.Down, Keys.E);

            mMouseClick = new Vector2(0.0f, 0.0f);

            mRightPressed = new MouseButtonInputAction(0, InputAction.ButtonAction.Pressed, MouseButtonInputAction.MouseButton.Right);
            mRightHold = new MouseButtonInputAction(0, InputAction.ButtonAction.Down, MouseButtonInputAction.MouseButton.Right);
            mRightReleased = new MouseButtonInputAction(0, InputAction.ButtonAction.Released, MouseButtonInputAction.MouseButton.Right);

            mMovement = new Vector3(0.0f, 0.0f, 0.0f);
            mDirection = new Vector3(0.0f, 0.0f, 0.0f);

            mModel = new InanimateModel("editor");
            mPosition = new Vector3(0.0f, 0.0f, 0.0f);
            mScale = new Vector3(1.0f, 1.0f, 1.0f);
            mOrientation = new Vector3(0.0f, 0.0f, 1.0f);

        }

        public void Update(GameTime gameTime)
        {
            InputAction.Update();
            mMouse = Mouse.GetState();

            UpdatePosition();
            UpdateOrientation();
            UpdatePicking();

            GameMapEditor.Camera.Move(speed * mMovement.Y * gameTime.ElapsedGameTime.Milliseconds, speed * mMovement.X * gameTime.ElapsedGameTime.Milliseconds, 0.0f);
            GameMapEditor.Camera.RotateAroundSelf(sensitivity * mDirection.X * gameTime.ElapsedGameTime.Milliseconds, sensitivity * mDirection.Y * gameTime.ElapsedGameTime.Milliseconds, 0.0f);
            //GameMapEditor.Camera.MoveRight(speed * mMovement.X * gameTime.ElapsedGameTime.Milliseconds);
            //GameMapEditor.Camera.RotatePitch(sensitivity * mDirection.Y * gameTime.ElapsedGameTime.Milliseconds);
            //GameMapEditor.Camera.RotateYaw(sensitivity * mDirection.X * gameTime.ElapsedGameTime.Milliseconds);

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
            Vector3 nearWorld = GameMapEditor.Viewport.Unproject(nearScreen, GameMapEditor.Camera.ProjectionTransform, GameMapEditor.Camera.ViewTransform, Matrix.Identity);
            Vector3 farWorld = GameMapEditor.Viewport.Unproject(farScreen, GameMapEditor.Camera.ProjectionTransform, GameMapEditor.Camera.ViewTransform, Matrix.Identity);
            Vector3 projectionDirection = (farWorld - nearWorld);
            projectionDirection.Normalize();
            Ray ray = new Ray(GameMapEditor.Camera.Position, projectionDirection);
            RayHit result;
            
            if (GameMapEditor.Map.TerrainPhysics.StaticCollidable.RayCast(ray, length, out result))
            {

                GameMapEditor.Placeable = true;
                GameMapEditor.Position = mPosition;


                if (GameMapEditor.CurrentState == States.Height)
                {
                    HeightEditorDialog tempDialog = GameMapEditor.Dialog as HeightEditorDialog;
                    mModel = new InanimateModel("editor");
                    mPosition = result.Location;
                    mScale = new Vector3(tempDialog.GetScale());
                    mOrientation = new Vector3(0.0f, 0.0f, 1.0f);
                }
                else if (GameMapEditor.CurrentState == States.Object)
                {
                    ObjectEditorDialog tempDialog = GameMapEditor.Dialog as ObjectEditorDialog;
                    DummyObject tempObject = new DummyObject();
                    if (tempDialog.GetObject(out tempObject))
                    {
                        mModel = new InanimateModel(tempObject.Model);
                        mPosition = new Vector3(result.Location.X, result.Location.Y + tempObject.Height * scale.Y, result.Location.Z);
                        mScale = tempObject.Scale;
                        mOrientation = tempObject.Orientation;
                    }
                }
            }
            else
            {
                GameMapEditor.Placeable = false;
                GameMapEditor.Position = new Vector3(0.0f, 0.0f, 0.0f);
            }

            if (!GameMapEditor.EditMode) mPosition = new Vector3(0.0f, 10000.0f, 0.0f);

        }

        public void Render()
        {
            mModel.Render(mPosition, mOrientation, mScale);
        }

    }
}
