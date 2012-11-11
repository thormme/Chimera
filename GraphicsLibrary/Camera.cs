using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GraphicsLibrary
{
    public class Camera
    {
        private float mAspectRatio;
        private float mPitch;
        private float mYaw;

        public Camera(Viewport viewport)
        {
            this.mPosition = new Vector3(0.0f, 0.0f, 0.0f);
            this.mTarget   = new Vector3(0.0f, 0.0f, 1.0f);

            this.mForward = mTarget - mPosition;
            this.mUp = new Vector3(0.0f, 1.0f, 0.0f);

            this.mPitch = 0.0f;
            this.mYaw   = 0.0f;

            this.mAspectRatio = (float)viewport.Width / (float)viewport.Height;
            this.mProjectionTransform = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(40.0f), this.mAspectRatio, 1.0f, 10000.0f);
        }

        private Vector3 mPosition;
        public Vector3 Position
        {
            get { return this.mPosition; }
            set
            {
                this.mPosition = value;
                this.mForward = mTarget - mPosition;
                this.mForward.Normalize();
            }
        }

        private Vector3 mTarget;
        public Vector3 Target
        {
            get { return this.mTarget; }
            set
            { 
                this.mTarget = value;
                this.mForward = mTarget - mPosition;
                this.mForward.Normalize();
            }
        }

        private Vector3 mForward;
        public Vector3 Forward
        {
            get { return mForward; }
        }

        public Vector3 Right
        {
            get
            {
                Vector3 right = Vector3.Cross(mUp, Forward);
                right.Normalize();
                return right;
            }
        }

        private Vector3 mUp;
        public Vector3 Up
        {
            get { return this.mUp; }
        }

        private Matrix  mViewTransform;
        public Matrix ViewTransform
        {
            get
            {
                mViewTransform = Matrix.CreateFromAxisAngle(Right, (MathHelper.ToRadians(mPitch))) * 
                    Matrix.CreateFromAxisAngle(mUp, (MathHelper.ToRadians(mYaw))) * 
                    Matrix.CreateLookAt(this.mPosition, this.mTarget, mUp);
                return this.mViewTransform;
            }
        }

        private Matrix  mProjectionTransform;
        public Matrix ProjectionTransform
        {
            get { return this.mProjectionTransform; }
        }

        /// <summary>
        /// Rotates the camera vertically without updating the forward or up vector.
        /// </summary>
        /// <param name="theta">Change in radians to rotate</param>
        public void PanPitch(float theta)
        {
            mPitch = (mPitch + theta) % 360.0f;
            mUp = Vector3.Transform(
                new Vector3(0.0f, 1.0f, 0.0f), 
                Matrix.CreateFromAxisAngle(Right, MathHelper.ToRadians(mPitch)));
        }

        /// <summary>
        /// Pans the camera horizontally without updating the forward or right vector.
        /// </summary>
        /// <param name="theta">Change in radians to rotate</param>
        public void PanYaw(float theta)
        {
            mYaw = (mYaw + theta) % 360.0f;
        }

        /// <summary>
        /// Rotates camera, updating forward vector and up vector.
        /// </summary>
        /// <param name="theta">Amount of rotation in degrees.</param>
        public void RotatePitch(float theta)
        {
            Matrix rotate = Matrix.CreateFromAxisAngle(Right, MathHelper.ToRadians(theta));
            this.mForward = Vector3.Transform(this.mForward, rotate);
            this.mForward.Normalize();
            this.mTarget = mPosition + mForward;
            this.mUp = Vector3.Cross(mForward, Right);
            this.mPitch = 0.0f;
        }

        public void RotateYaw(float theta)
        {
            Matrix rotate = Matrix.CreateFromAxisAngle(mUp, MathHelper.ToRadians(theta));
            this.mForward = Vector3.Transform(this.mForward, rotate);
            this.mForward.Normalize();
            this.mTarget = mPosition + mForward;
            this.mYaw = 0.0f;
        }

        /// <summary>
        /// Resets Pitch to original orientation.
        /// </summary>
        public void ResetPitch()
        {
            mPitch = 0.0f;
            mUp = new Vector3(0.0f, 1.0f, 0.0f);
        }

        // Resets Yaw to original orientation.
        public void ResetYaw()
        {
            mYaw = 0.0f;
            mUp = new Vector3(0.0f, 1.0f, 0.0f);
        }

        /// <summary>
        /// Move camera forward facing same direction.
        /// </summary>
        /// <param name="distance">Number of units to move forward.</param>
        public void MoveForward(float distance)
        {
            mPosition += distance * mForward;
            mTarget += distance * mForward;
        }

        /// <summary>
        /// Move camera to the right facing the same direction.
        /// </summary>
        /// <param name="distance">Number of units to move right.</param>
        public void MoveRight(float distance)
        {
            mPosition += distance * Right;
            mTarget += distance * Right;
        }
    }
}
