using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphicsLibrary
{
    public class HeightMapRenderable : Renderable
    {
        #region Public Properties

        public Vector4 LayerMask
        {
            get { return mLayerMask; }
            set { mLayerMask = value; }
        }
        private Vector4 mLayerMask = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);

        public enum CursorShape { NONE, CIRCLE, BLOCK };

        public CursorShape DrawCursor
        {
            get { return mDrawCursor; }
            set { mDrawCursor = value; }
        }
        private CursorShape mDrawCursor;

        public Vector3 CursorPosition
        {
            get { return mCursorPosition; }
            set { mCursorPosition = value; }
        }
        private Vector3 mCursorPosition;

        public float CursorInnerRadius
        {
            get { return mCursorInnerRadius; }
            set { mCursorInnerRadius = value; }
        }
        private float mCursorInnerRadius;

        public float CursorOuterRadius
        {
            get { return mCursorOuterRadius; }
            set { mCursorOuterRadius = value; }
        }
        private float mCursorOuterRadius;

        public bool Selected
        {
            get { return mSelected; }
            set { mSelected = value; }
        }
        private bool mSelected = false;

        #endregion

        public HeightMapRenderable(string heightMapName)
            : base(heightMapName, typeof(HeightMapRenderer))
        {
            BuildBoundingBox();
        }

        protected override void AlertAssetLibrary() { }

        protected override void Draw(Matrix worldTransform, Color overlayColor, float overlayColorWeight, bool tryCull)
        {
            BoundingBox transformedBoundingBox = new BoundingBox(Vector3.Transform(mBoundingBox.Min, worldTransform), Vector3.Transform(mBoundingBox.Max, worldTransform));

            HeightMapRenderer.HeightMapParameters parameters = new HeightMapRenderer.HeightMapParameters();
            parameters.BoundingBox = transformedBoundingBox;
            parameters.DrawCursor = DrawCursor;
            parameters.CursorPosition = CursorPosition;
            parameters.CursorInnerRadius = CursorInnerRadius;
            parameters.CursorOuterRadius = CursorOuterRadius;
            parameters.Name = Name;
            parameters.OverlayColor = mSelected ? Color.Red : overlayColor;
            parameters.OverlayWeight = mSelected ? 0.5f : overlayColorWeight;
            parameters.TextureMask = LayerMask;
            parameters.TryCull = tryCull;
            parameters.World = worldTransform;

            GraphicsManager.EnqueueRenderable(parameters, RendererType);

            DrawCursor = CursorShape.NONE;
            mSelected = false;
        }

        private void BuildBoundingBox()
        {
            HeightMapMesh heightMap = AssetLibrary.LookupHeightMap(Name).Mesh;

            Vector3 minExtent = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            Vector3 maxExtent = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            int vertexStride = heightMap.VertexBuffer.VertexDeclaration.VertexStride;
            int vertexBufferSize = heightMap.VertexBuffer.VertexCount * vertexStride;

            float[] vertexData = new float[vertexBufferSize / sizeof(float)];
            heightMap.VertexBuffer.GetData<float>(vertexData);

            for (int i = 0; i < vertexBufferSize / sizeof(float); i += vertexStride / sizeof(float))
            {
                Vector3 localPosition = new Vector3(vertexData[i], vertexData[i + 1], vertexData[i + 2]);

                minExtent = Vector3.Min(minExtent, localPosition);
                maxExtent = Vector3.Max(maxExtent, localPosition);
            }

            mBoundingBox= new BoundingBox(minExtent, maxExtent);
        }
    }
}
