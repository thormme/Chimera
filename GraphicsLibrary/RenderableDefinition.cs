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

        public RenderableDefinition(string name, bool isModel, bool isSkinned, Matrix worldTransform, Matrix[] boneTransforms)
        {
            mName           = name;
            mIsModel        = isModel;
            mIsSkinned      = isSkinned;
            mWorldTransform = worldTransform;
            mBoneTransforms = boneTransforms;
        }
    }
}
