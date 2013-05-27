using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GraphicsLibrary
{
    public class InanimateModel : Renderable
    {
        public UInt32 ObjectID;

        public InanimateModel(string modelName)
            : base(modelName, typeof(ModelRenderer))
        {
        }

        private bool mSpaghettify = false;
        public bool Spaghettify
        {
            get { return mSpaghettify; }
            set { mSpaghettify = value; }
        }

        private Vector3 mWormholePosition = Vector3.Zero;
        public Vector3 WormholePosition
        {
            get { return mWormholePosition; }
            set { mWormholePosition = value; }
        }

        private float mMaxWormholeDistance = 0.0f;
        public float MaxWormholeDistance
        {
            get { return mMaxWormholeDistance; }
            set { mMaxWormholeDistance = value; }
        }

        protected override void AlertAssetLibrary() { }

        protected override void Draw(Matrix worldTransform, Color overlayColor, float overlayColorWeight, bool tryCull)
        {
            
            if (Name == "rock1dark_collision")
            {
                overlayColor = Color.Beige;
                overlayColorWeight = 1.0f;
            }

            ModelRenderer.ModelParameters parameters = new ModelRenderer.ModelParameters();
            parameters.BoundingBox = BoundingBox;
            parameters.Spaghettify = mSpaghettify;
            parameters.MaxWormholeDistance = mMaxWormholeDistance;
            parameters.Name = Name;
            parameters.ObjectID = ObjectID;
            parameters.OverlayColor = overlayColor;
            parameters.OverlayWeight = overlayColorWeight;
            parameters.TryCull = tryCull;
            parameters.World = worldTransform;
            parameters.WormholePosition = mWormholePosition;

            GraphicsManager.EnqueueRenderable(parameters, RendererType);
        }
    }
}
