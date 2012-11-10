using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameConstructLibrary
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
            set { this.mPosition = value; }
        }

        private Vector3 mTarget;
        public Vector3 Target
        {
            get { return this.mTarget; }
            set { this.mTarget = value; }
        }

        public Vector3 Forward
        {
            get
            {
                Vector3 forward = mTarget - mPosition;
                forward.Normalize();
                return forward;
            }
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
            set { this.mUp = value; }
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
            set { this.mViewTransform = value; }
        }

        private Matrix  mProjectionTransform;
        public Matrix ProjectionTransform
        {
            get { return this.mProjectionTransform; }
            set { this.mProjectionTransform = value; }
        }

        /// <summary>
        /// Rotates the camera vertically.
        /// </summary>
        /// <param name="theta">Change in radians to rotate</param>
        public void RotatePitch(float theta)
        {
            mPitch = (mPitch + theta) % 360.0f;
            mUp = Vector3.Transform(
                new Vector3(0.0f, 1.0f, 0.0f), 
                Matrix.CreateFromAxisAngle(Right, MathHelper.ToRadians(mPitch)));
        }

        /// <summary>
        /// Rotates the camera horizontally.
        /// </summary>
        /// <param name="theta">Change in radians to rotate</param>
        public void RotateYaw(float theta)
        {
            mYaw = (mYaw + theta) % 360.0f;
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
            mPosition += distance * Forward;
        }

        /// <summary>
        /// Move camera to the right facing the same direction.
        /// </summary>
        /// <param name="distance">Number of units to move right.</param>
        public void MoveRight(float distance)
        {
            mPosition += distance * Right;
        }
    }
}
