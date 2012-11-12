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

        private InputAction mForward;
        private InputAction mBackward;
        private InputAction mLeft;
        private InputAction mRight;
        private InputAction mRollLeft;
        private InputAction mRollRight;

        private MouseState mMouse;
        private Vector2 mMouseClick;
        private MouseButtonInputAction mLeftClick;
        private MouseButtonInputAction mRightClick;
        private MouseButtonInputAction mLeftHold;
        private MouseButtonInputAction mRightHold;
        private MouseButtonInputAction mLeftReleased;
        private MouseButtonInputAction mRightReleased;

        private Viewport mView;
        private Space mSpace;
        private bool mPicking;

        private Camera mCamera;
        private Vector3 mPosition;
        private Vector2 mMovement;
        private Vector3 mOrientation;

        public MapEntity(Camera camera, Viewport view, Space space)
        {

            mForward = new KeyInputAction(0, InputAction.ButtonAction.Down, Keys.W);
            mBackward = new KeyInputAction(0, InputAction.ButtonAction.Down, Keys.S);
            mLeft = new KeyInputAction(0, InputAction.ButtonAction.Down, Keys.A);
            mRight = new KeyInputAction(0, InputAction.ButtonAction.Down, Keys.D);
            mRollLeft = new KeyInputAction(0, InputAction.ButtonAction.Down, Keys.Q);
            mRollRight = new KeyInputAction(0, InputAction.ButtonAction.Down, Keys.E);

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
            mMovement = new Vector2(0.0f, 0.0f);
            mOrientation = new Vector3(0.0f, 0.0f, 0.0f);

        }

        public void Update(GameTime gameTime)
        {
            InputAction.Update();
            mMouse = Mouse.GetState();

            UpdatePosition();
            UpdateOrientation();
            UpdatePicking();

            mCamera.MoveForward(speed * mMovement.Y * gameTime.ElapsedGameTime.Milliseconds);
            mCamera.MoveRight(speed * mMovement.X * gameTime.ElapsedGameTime.Milliseconds);
            mCamera.RotatePitch(sensitivity * mOrientation.Y * gameTime.ElapsedGameTime.Milliseconds);
            mCamera.RotateYaw(sensitivity * mOrientation.X * gameTime.ElapsedGameTime.Milliseconds);


        }

        private void UpdatePosition()
        {
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
                mOrientation.Z = MathHelper.ToRadians((float)(mRollRight.Degree - mRollLeft.Degree) * sensitivity); // roll
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
            }
        }

    }
}
