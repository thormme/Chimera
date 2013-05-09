using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GraphicsLibrary
{
    public class SkyBoxRenderer : RendererBase
    {
        #region Constants

        const float Scale = 30000.0f;
   
        #endregion

        #region Private Variables

        private VertexBuffer mVertexBuffer;
        private IndexBuffer mIndexBuffer;

        private AnimationUtilities.SkinnedEffect mEffect;

        #endregion

        #region Structures

        public class SkyBoxParameters : RendererParameters
        {
            public string TextureName { get; set; }
        }

        #endregion

        #region Public Interface

        public SkyBoxRenderer(AnimationUtilities.SkinnedEffect effect)
        {
            mEffect = effect;

            CreateBuffers();

            mNormalDepthConfigurer = null;
            mShadowMapConfigurer = null;
            mPickingConfigurer = null;
            mUIConfigurer = null;
            mOverlayConfigurer = null;
        }

        #endregion

        #region Instance Render Helper Methods

        protected override void WithShadowsConfigurer(AnimationUtilities.SkinnedEffect effect, RendererParameters instance, object[] optionalParameters)
        {
            WithoutShadowsConfigurer(effect, instance, optionalParameters);
        }

        protected override void WithoutShadowsConfigurer(AnimationUtilities.SkinnedEffect effect, RendererParameters instance, object[] optionalParameters)
        {
            effect.CurrentTechnique = effect.Techniques["NoShade"];
        }

        protected override void NoShadeConfigurer(AnimationUtilities.SkinnedEffect effect, RendererBase.RendererParameters instance, object[] optionalParameters)
        {
            WithoutShadowsConfigurer(effect, instance, optionalParameters);
        }

        protected override void DrawGeometry(Matrix view, Matrix projection, object[] optionalParameters, EffectConfigurer effectConfigurer, RendererParameters instance)
        {
            effectConfigurer(mEffect, instance, optionalParameters);

            GraphicsManager.Device.SamplerStates[0] = SamplerState.PointClamp;

            SkyBoxParameters skyBoxInstance = instance as SkyBoxParameters;

            GraphicsManager.Device.SamplerStates[0] = SamplerState.LinearWrap;

            Vector3 sceneScale = new Vector3(Scale, Scale, Scale);
            Vector3 centerOffset = new Vector3(0, sceneScale.Y * 0.5f, 0);

            mEffect.World      = Matrix.CreateScale(sceneScale) * skyBoxInstance.World * Matrix.CreateTranslation(-centerOffset);
            mEffect.View       = view;
            mEffect.Projection = Matrix.CreatePerspectiveFieldOfView(GraphicsManager.Camera.FieldOfView, GraphicsManager.Camera.AspectRatio, GraphicsManager.Camera.GetNearPlaneDistance(), Scale); ;

            mEffect.Parameters["xOverlayColor"].SetValue(skyBoxInstance.OverlayColor.ToVector3());
            mEffect.Parameters["xOverlayColorWeight"].SetValue(skyBoxInstance.OverlayWeight);
            mEffect.Parameters["xTextureOffset"].SetValue(skyBoxInstance.TextureAnimationOffset);
            
            mEffect.Texture = AssetLibrary.LookupTexture(skyBoxInstance.TextureName);

            GraphicsManager.Device.SetVertexBuffer(mVertexBuffer);
            GraphicsManager.Device.Indices = mIndexBuffer;

            mEffect.CurrentTechnique.Passes[0].Apply();

            GraphicsManager.Device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, mVertexBuffer.VertexCount, 0, mIndexBuffer.IndexCount / 3);
        }

        #endregion

        #region VertexBuffer and IndexBuffer Creation

        private void CreateBuffers()
        {
            const int bottomLength = 2, rightLength = 2, numSides = 6;
            const float uLength = 1.0f / 4.0f, vLength = 1.0f / 3.0f;

            float[] sideUOrigin = { uLength, uLength, 2 * uLength, 3 * uLength, 0.0f, uLength };
            float[] sideVOrigin = { 0.0f, vLength, vLength, vLength, vLength, 2 * vLength };
            Vector3[] sideTopLeft = { new Vector3(-0.5f, 1.0f, 0.5f), new Vector3(-0.5f, 1.0f, -0.5f), new Vector3(0.5f, 1.0f, -0.5f), new Vector3(0.5f, 1.0f, 0.5f), new Vector3(-0.5f, 1.0f, 0.5f), new Vector3(-0.5f, 0.0f, -0.5f) };
            Vector3[] sideRight = { new Vector3(1.0f, 0.0f, 0.0f), new Vector3(1.0f, 0.0f, 0.0f), new Vector3(0.0f, 0.0f, 1.0f), new Vector3(-1.0f, 0.0f, 0.0f), new Vector3(0.0f, 0.0f, -1.0f), new Vector3(1.0f, 0.0f, 0.0f) };
            Vector3[] sideBottom = { new Vector3(0.0f, 0.0f, -1.0f), new Vector3(0.0f, -1.0f, 0.0f), new Vector3(0.0f, -1.0f, 0.0f), new Vector3(0.0f, -1.0f, 0.0f), new Vector3(0.0f, -1.0f, 0.0f), new Vector3(0.0f, 0.0f, 1.0f) };
            Vector3[] sideNormal = { new Vector3(0, -1, 0), new Vector3(0, 0, 1), new Vector3(-1, 0, 0), new Vector3(0, 0, -1), new Vector3(1, 0, 0), new Vector3(0, 1, 0) };

            VertexPositionNormalTexture[] skyBoxVertices = new VertexPositionNormalTexture[numSides * bottomLength * rightLength];

            int sideIndex;
            for (sideIndex = 0; sideIndex < 6; ++sideIndex)
            {
                for (int bottom = 0; bottom <= 1; ++bottom)
                {
                    for (int right = 0; right <= 1; ++right)
                    {
                        VertexPositionNormalTexture vertex = new VertexPositionNormalTexture();
                        vertex.Position = sideTopLeft[sideIndex] + bottom * sideBottom[sideIndex] + right * sideRight[sideIndex];
                        vertex.Normal = sideNormal[sideIndex];
                        vertex.TextureCoordinate = new Vector2(sideUOrigin[sideIndex] + right * uLength, sideVOrigin[sideIndex] + bottom * vLength);

                        skyBoxVertices[sideIndex * bottomLength * rightLength + bottom * bottomLength + right] = vertex;
                    }
                }
            }

            mVertexBuffer = new VertexBuffer(GraphicsManager.Device, VertexPositionNormalTexture.VertexDeclaration, skyBoxVertices.Length, BufferUsage.WriteOnly);
            mVertexBuffer.SetData(skyBoxVertices);

            const int numQuadVertices = 6;
            int[] skyBoxIndices = new int[numSides * numQuadVertices];

            int count = 0;
            for (sideIndex = 0; sideIndex < numSides; ++sideIndex)
            {
                int topLeftIndex = sideIndex * bottomLength * rightLength;
                int topRightIndex = topLeftIndex + 1;
                int bottomLeftIndex = topRightIndex + 1;
                int bottomRightIndex = bottomLeftIndex + 1;

                skyBoxIndices[count++] = topLeftIndex;
                skyBoxIndices[count++] = topRightIndex;
                skyBoxIndices[count++] = bottomRightIndex;

                skyBoxIndices[count++] = topLeftIndex;
                skyBoxIndices[count++] = bottomRightIndex;
                skyBoxIndices[count++] = bottomLeftIndex;
            }

            mIndexBuffer = new IndexBuffer(GraphicsManager.Device, typeof(int), skyBoxIndices.Length, BufferUsage.WriteOnly);
            mIndexBuffer.SetData(skyBoxIndices);
        }

        #endregion
    }
}
