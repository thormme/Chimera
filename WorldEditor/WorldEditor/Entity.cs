using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GameConstructLibrary;
using BEPUphysics;
using Microsoft.Xna.Framework.Graphics;

namespace WorldEditor
{
    public class Entity
    {

        private const float Speed = 0.5f;
        private const float Sensitivity = 0.1f;

        private Viewport mViewport = new Viewport();
        private FPSCamera mCamera = null;
        private Controls mControls = null;
        private Vector2 mDragPoint = Vector2.Zero;

        private Vector3 mMovement = Vector3.Zero;
        private Vector3 mDirection = Vector3.Zero;

        public Entity(Viewport viewport, Controls controls, FPSCamera camera)
        {
            mViewport = viewport;
            mControls = controls;
            mCamera = camera;
        }

        public void Update(GameTime gameTime)
        {
            UpdateMovement();
            UpdateDirection();
            UpdateCamera(gameTime);
        }

        private void UpdateMovement()
        {
            mMovement.X = -(float)(mControls.Right.Degree - mControls.Left.Degree);
            mMovement.Y = (float)(mControls.Forward.Degree - mControls.Backward.Degree);
        }

        private void UpdateDirection()
        {
            if (mControls.RightPressed.Active)
            {
                UpdateDragPoint();
            }
            else if (mControls.RightHold.Active)
            {
                mDirection.X = -MathHelper.ToRadians((mControls.MouseState.X - mDragPoint.X) * Sensitivity);
                mDirection.Y = MathHelper.ToRadians((mControls.MouseState.Y - mDragPoint.Y) * Sensitivity);
                UpdateDragPoint();
            }
            else if (mControls.RightReleased.Active)
            {
                mDirection = Vector3.Zero;
            }
        }

        private void UpdateCamera(GameTime gameTime)
        {
            mCamera.Move(Speed * mMovement.Y * gameTime.ElapsedGameTime.Milliseconds, Speed * mMovement.X * gameTime.ElapsedGameTime.Milliseconds, 0.0f);
            mCamera.RotateAroundSelf(Sensitivity * mDirection.X * gameTime.ElapsedGameTime.Milliseconds, Sensitivity * mDirection.Y * gameTime.ElapsedGameTime.Milliseconds, 0.0f);
        }

        private void UpdateDragPoint()
        {
            mDragPoint.X = mControls.MouseState.X;
            mDragPoint.Y = mControls.MouseState.Y;
        }

        public Tuple<Vector3, Vector3> GetPickingLocation(DummyWorld dummyWorld)
        {

            Vector3 nearScreen = new Vector3(mControls.MouseState.X, mControls.MouseState.Y, 0.0f);
            Vector3 farScreen = new Vector3(mControls.MouseState.X, mControls.MouseState.Y, 1.0f);
            Vector3 nearWorld = mViewport.Unproject(nearScreen, mCamera.ProjectionTransform, mCamera.ViewTransform, Matrix.Identity);
            Vector3 farWorld = mViewport.Unproject(farScreen, mCamera.ProjectionTransform, mCamera.ViewTransform, Matrix.Identity);
            Vector3 projectionDirection = (farWorld - nearWorld);
            projectionDirection.Normalize();
            Ray ray = new Ray(mCamera.Position, projectionDirection);
            RayHit result;

            if (dummyWorld.Terrain.StaticCollidable.RayCast(ray, 2000.0f, out result))
            {
                return new Tuple<Vector3,Vector3>(result.Location, result.Normal);
            }
            else
            {
                return null;
            }

        }

    }
}
