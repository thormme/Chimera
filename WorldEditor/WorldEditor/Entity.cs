using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GameConstructLibrary;

namespace WorldEditor
{
    public class Entity
    {

        private const float Speed = 0.5f;
        private const float Sensitivity = 0.1f;

        private FPSCamera mCamera = null;
        private Controls mControls = null;
        private Vector2 mDragPoint = Vector2.Zero;

        private Vector3 mMovement = Vector3.Zero;
        private Vector3 mDirection = Vector3.Zero;

        public Entity(Controls controls, FPSCamera camera)
        {
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
                mDirection.X = -MathHelper.ToRadians((mControls.State.X - mDragPoint.X) * Sensitivity);
                mDirection.Y = MathHelper.ToRadians((mControls.State.Y - mDragPoint.Y) * Sensitivity);
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
            mDragPoint.X = mControls.State.X;
            mDragPoint.Y = mControls.State.Y;
        }

    }
}
