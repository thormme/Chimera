using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameConstructLibrary
{
    public class Camera
    {
        private float mAspectRatio;

        public Camera(Viewport viewport)
        {
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

        private Matrix  mViewTransform;
        public Matrix ViewTransform
        {
            get
            {
                mViewTransform = Matrix.CreateLookAt(this.mPosition, this.mTarget, Vector3.Up);
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
    }
}
