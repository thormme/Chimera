using Microsoft.Xna.Framework;

namespace GraphicsLibrary
{
    class RenderableDefinition
    {
        private string mName;
        public string Name
        {
            get { return mName; }
        }

        private bool mIsModel;
        public bool IsModel
        {
            get { return mIsModel; }
        }

        private bool mIsSkinned;
        public bool IsSkinned
        {
            get { return mIsSkinned; }
        }

        private Matrix mWorldTransform;
        public Matrix WorldTransform
        {
            get { return mWorldTransform; }
        }

        private Matrix[] mBoneTransforms;
        public Matrix[] BoneTransforms
        {
            get { return mBoneTransforms; }
        }

        public Vector2 AnimationRate
        {
            get { return mAnimationRate; }
            set { mAnimationRate = value; }
        }
        private Vector2 mAnimationRate;

        public Vector3 OverlayColor
        {
            get { return mOverlayColor; }
        }
        private Vector3 mOverlayColor;

        public float OverlayColorWeight
        {
            get { return mOverlayColorWeight; }
        }
        private float mOverlayColorWeight;

        public RenderableDefinition(string name, bool isModel, bool isSkinned, Matrix worldTransform, Matrix[] boneTransforms, Color overlayColor, float overlayColorWeight, Vector2 animationRate)
        {
            mName           = name;
            mIsModel        = isModel;
            mIsSkinned      = isSkinned;
            mWorldTransform = worldTransform;
            mBoneTransforms = boneTransforms;
            mOverlayColor = overlayColor.ToVector3();
            mOverlayColorWeight = overlayColorWeight;
            mAnimationRate = animationRate;
        }
    }
}
