using Microsoft.Xna.Framework;

namespace GraphicsLibrary
{
    public class RenderableDefinition
    {
        private string mName;
        public string Name
        {
            get { return mName; }
        }

        private bool mIsModel = false;
        public bool IsModel
        {
            get { return mIsModel; }
            set { mIsModel = value; }
        }

        private bool mIsSkinned = false;
        public bool IsSkinned
        {
            get { return mIsSkinned; }
            set { mIsSkinned = value; }
        }

        private Matrix mWorldTransform;
        public Matrix WorldTransform
        {
            get { return mWorldTransform; }
            set { mWorldTransform = value; }
        }

        private Matrix[] mBoneTransforms;
        public Matrix[] BoneTransforms
        {
            get { return mBoneTransforms; }
            set { mBoneTransforms = value; }
        }

        private Vector2 mAnimationRate = Vector2.Zero;
        public Vector2 AnimationRate
        {
            get { return mAnimationRate; }
            set { mAnimationRate = value; }
        }

        private Vector3 mOverlayColor;
        public Vector3 OverlayColor
        {
            get { return mOverlayColor; }
        }

        private float mOverlayColorWeight;
        public float OverlayColorWeight
        {
            get { return mOverlayColorWeight; }
        }

        private string mOverlayTextureName = null;
        public string OverlayTextureName
        {
            get { return mOverlayTextureName; }
            set { mOverlayTextureName = value; }
        }

        private bool mNoShading = false;
        public bool NoShading
        {
            get { return mNoShading; }
            set { mNoShading = value; }
        }

        public RenderableDefinition(string name, Matrix worldTransform, Color overlayColor, float overlayColorWeight)
        {
            mName               = name;
            mWorldTransform     = worldTransform;
            mOverlayColor       = overlayColor.ToVector3();
            mOverlayColorWeight = overlayColorWeight;

            mBoneTransforms = new Matrix[1];
            mBoneTransforms[0] = Matrix.Identity;
        }

        public void Draw(bool isShadow)
        {

        }
    }
}
