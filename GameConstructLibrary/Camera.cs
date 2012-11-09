using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameConstructLibrary
{
    public class Camera
    {
        private float mAspectRatio;

        public Camera(Viewport viewport)
        {
            this.cameraArc = 0;
            this.cameraRotation = 0;
            this.cameraDistance = 100;
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

        public Vector3 Right
        {
            get { return Vector3.Cross(Vector3.Up, mTarget); }
        }

        private float cameraRotation;
        private float cameraArc;
        private float cameraDistance;

        private Matrix  mViewTransform;
        public Matrix ViewTransform
        {
            get
            {
                //mViewTransform = Matrix.CreateLookAt(this.mPosition, this.mTarget, Vector3.Up);
                mViewTransform = Matrix.CreateTranslation(0, -40, 0) *
                                 Matrix.CreateRotationY(MathHelper.ToRadians(cameraRotation)) *
                                 Matrix.CreateRotationX(MathHelper.ToRadians(cameraArc)) *
                                 Matrix.CreateLookAt(new Vector3(0, 0, -cameraDistance),
                                              new Vector3(0, 0, 0), Vector3.Up);
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
            //Matrix transform = Matrix.CreateFromAxisAngle(Right, theta);
            //mTarget = Vector3.Transform(mTarget, transform);
            cameraArc += theta;
        }

        /// <summary>
        /// Rotates the camera horizontally.
        /// </summary>
        /// <param name="theta">Change in radians to rotate</param>
        public void RotateYaw(float theta)
        {
            //Matrix transform = Matrix.CreateRotationY(theta);
            //mTarget = Vector3.Transform(mTarget, transform);
            cameraRotation += theta;
        }

        /// <summary>
        /// Move camera forward facing same direction.
        /// </summary>
        /// <param name="distance">Number of units to move forward.</param>
        public void MoveForward(float distance)
        {
            //mPosition += distance * mTarget;
            cameraDistance += distance;
        }

        /// <summary>
        /// Move camera to the right facing the same direction.
        /// </summary>
        /// <param name="distance">Number of units to move right.</param>
        public void MoveRight(float distance)
        {
            //mPosition += distance * Right;
        }
    }
}
