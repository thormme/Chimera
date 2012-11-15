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

namespace MapEditor
{
    public class MapEntity
    {
        
        private const float speed = 0.5f;
        private const float sensitivity = 1.0f;

        private KeyInputAction mForward;
        private KeyInputAction mBackward;
        private KeyInputAction mLeft;
        private KeyInputAction mRight;
        private KeyInputAction mDown;
        private KeyInputAction mUp;

        private MouseState mMouse;
        private Vector2 mMouseClick;
        private MouseButtonInputAction mLeftClick;
        private MouseButtonInputAction mRightClick;
        private MouseButtonInputAction mLeftHold;
        private MouseButtonInputAction mRightHold;
        private MouseButtonInputAction mLeftReleased;
        private MouseButtonInputAction mRightReleased;

        private DummyLevel mLevel;
        public DummyLevel Level { get { return mLevel; } set { mLevel = value; } }
        private Viewport mView;
        private Space mSpace;
        private bool mPicking;

        private Camera mCamera;
        private Vector3 mPosition;
        private Vector3 mMovement;
        private Vector3 mOrientation;

        public MapEntity(Camera camera, Viewport view, Space space)
        {

            mForward = new KeyInputAction(0, InputAction.ButtonAction.Down, Keys.W);
            mBackward = new KeyInputAction(0, InputAction.ButtonAction.Down, Keys.S);
            mLeft = new KeyInputAction(0, InputAction.ButtonAction.Down, Keys.A);
            mRight = new KeyInputAction(0, InputAction.ButtonAction.Down, Keys.D);
            mDown = new KeyInputAction(0, InputAction.ButtonAction.Down, Keys.Q);
            mUp = new KeyInputAction(0, InputAction.ButtonAction.Down, Keys.E);

            mMouseClick = new Vector2(0.0f, 0.0f);
            mLeftClick = new MouseButtonInputAction(0, InputAction.ButtonAction.Pressed, MouseButtonInputAction.MouseButton.Left);
            mRightClick = new MouseButtonInputAction(0, InputAction.ButtonAction.Pressed, MouseButtonInputAction.MouseButton.Right);
            mLeftHold = new MouseButtonInputAction(0, InputAction.ButtonAction.Down, MouseButtonInputAction.MouseButton.Left);
            mRightHold = new MouseButtonInputAction(0, InputAction.ButtonAction.Down, MouseButtonInputAction.MouseButton.Right);
            mLeftReleased = new MouseButtonInputAction(0, InputAction.ButtonAction.Released, MouseButtonInputAction.MouseButton.Left);
            mRightReleased = new MouseButtonInputAction(0, InputAction.ButtonAction.Released, MouseButtonInputAction.MouseButton.Right);

            mView = view;
            mSpace = space;
            mPicking = false;

            mCamera = camera;
            mPosition = new Vector3(0.0f, 0.0f, 0.0f);
            mMovement = new Vector3(0.0f, 0.0f, 0.0f);
            mOrientation = new Vector3(0.0f, 0.0f, 0.0f);

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
            mCamera.RotatePitch(sensitivity * mOrientation.Y * gameTime.ElapsedGameTime.Milliseconds);
            mCamera.RotateYaw(sensitivity * mOrientation.X * gameTime.ElapsedGameTime.Milliseconds);

        }

        private void UpdatePosition()
        {
            mMovement.Z = (float)(mUp.Degree - mDown.Degree);
            mMovement.Y = (float)(mForward.Degree - mBackward.Degree);
            mMovement.X = -(float)(mRight.Degree - mLeft.Degree);
        }

        private void UpdateOrientation()
        {
            if (mRightClick.Active == true)
            {
                mMouseClick.Y = mMouse.Y;
                mMouseClick.X = mMouse.X;
            }

            if (mRightHold.Active == true)
            {
                mOrientation.Y = MathHelper.ToRadians((mMouse.Y - mMouseClick.Y) * sensitivity); // pitch
                mOrientation.X = -MathHelper.ToRadians((mMouse.X - mMouseClick.X) * sensitivity); // yaw
                mMouseClick.Y = mMouse.Y;
                mMouseClick.X = mMouse.X;
            }

            if (mRightReleased.Active == true)
            {
                mOrientation.Y = 0;
                mOrientation.X = 0;
            }
        }

        private void UpdatePicking()
        {
            if (mLeftClick.Active == true)
            {
                mPicking = true;

                Vector3 nearScreen = new Vector3(mMouse.X, mMouse.Y, 0.0f);
                Vector3 farScreen = new Vector3(mMouse.X, mMouse.Y, 1.0f);
                Vector3 nearWorld = mView.Unproject(nearScreen, mCamera.ProjectionTransform, mCamera.ViewTransform, Matrix.Identity);
                Vector3 farWorld = mView.Unproject(farScreen, mCamera.ProjectionTransform, mCamera.ViewTransform, Matrix.Identity);
                Vector3 projectionDirection = (farWorld - nearWorld);
                projectionDirection.Normalize();

                Console.WriteLine(projectionDirection.X + ", " + projectionDirection.Y + ", " + projectionDirection.Z);
                Ray ray = new Ray(mCamera.Position, projectionDirection);
                RayCastResult result;

                mSpace.RayCast(ray, out result);
                mLevel.Click(new Vector3(50, 0, 50));

            }
        }

    }
}
