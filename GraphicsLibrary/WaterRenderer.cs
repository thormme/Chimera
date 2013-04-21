using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GraphicsLibrary
{
    public class WaterRenderer : RendererBase
    {
        #region Constants

        const float SEA_WIDTH  = 6000.0f;
        const float SEA_HEIGHT = 6000.0f;
        const float TEXTURE_TILE_SCALE = 80.0f;

        #endregion

        #region Private Variables

        private VertexPositionNormalTexture[] mVertices = null;
        private int[] mIndices = null;

        private VertexBuffer mVertexBuffer;
        private IndexBuffer  mIndexBuffer;
        private Vector2      mResolution;

        private AnimationUtilities.SkinnedEffect mEffect;

        #endregion

        #region Structures

        public class WaterParameters : RendererParameters
        {
            public string TextureName { get; set; }
            public float SeaLevel { get; set; }
        }

        #endregion

        #region Public Interface

        public WaterRenderer(Vector2 resolution, AnimationUtilities.SkinnedEffect effect)
        {
            mEffect = effect;
            mResolution = resolution;

            ResizeVertices();
            ResizeIndices();
            FillBuffers();

            mUIConfigurer = null;
        }

        #endregion

        #region Instance Render Helper Methods

        protected override void NormalDepthConfigurer(AnimationUtilities.SkinnedEffect effect, RendererParameters instance, object[] optionalParameters)
        {
            effect.CurrentTechnique = effect.Techniques["NormalDepthShade"];
        }

        protected override void ShadowMapConfigurer(AnimationUtilities.SkinnedEffect effect, RendererBase.RendererParameters instance, object[] optionalParameters)
        {
            Matrix lightView = (optionalParameters[0] as Matrix?).Value;
            Matrix lightProjection = (optionalParameters[1] as Matrix?).Value;

            effect.CurrentTechnique = effect.Techniques["ShadowCast"];

            effect.Parameters["xLightView"].SetValue(lightView);
            effect.Parameters["xLightProjection"].SetValue(lightProjection);
        }

        protected override void WithShadowsConfigurer(AnimationUtilities.SkinnedEffect effect, RendererParameters instance, object[] optionalParameters)
        {
            CascadedShadowMap shadowMap = optionalParameters[0] as CascadedShadowMap;
            Light light = optionalParameters[1] as Light;

            effect.CurrentTechnique = effect.Techniques["CelShadeWithShadows"];

            effect.Parameters["ShadowMap"].SetValue(shadowMap.Buffer);

            effect.Parameters["xLightView"].SetValue(shadowMap.LightView);
            effect.Parameters["xLightProjections"].SetValue(shadowMap.LightProjections);

            effect.Parameters["xVisualizeCascades"].SetValue(shadowMap.VisualizeCascades);
            effect.Parameters["xCascadeCount"].SetValue(shadowMap.CascadeCount);
            effect.Parameters["xCascadeBufferBounds"].SetValue(shadowMap.CascadeBounds);
            effect.Parameters["xCascadeColors"].SetValue(shadowMap.CascadeColors);

            effect.Parameters["xDirLightDirection"].SetValue(light.Direction);
            effect.Parameters["xDirLightDiffuseColor"].SetValue(light.DiffuseColor);
            effect.Parameters["xDirLightSpecularColor"].SetValue(light.SpecularColor);
            effect.Parameters["xDirLightAmbientColor"].SetValue(light.AmbientColor);

            effect.SpecularColor = new Vector3(0.25f);
            effect.SpecularPower = 16;
        }

        protected override void WithoutShadowsConfigurer(AnimationUtilities.SkinnedEffect effect, RendererParameters instance, object[] optionalParameters)
        {
            Light light = optionalParameters[0] as Light;

            effect.CurrentTechnique = effect.Techniques["CelShadeWithoutShadows"];

            effect.Parameters["xDirLightDirection"].SetValue(light.Direction);
            effect.Parameters["xDirLightDiffuseColor"].SetValue(light.DiffuseColor);
            effect.Parameters["xDirLightSpecularColor"].SetValue(light.SpecularColor);
            effect.Parameters["xDirLightAmbientColor"].SetValue(light.AmbientColor);

            effect.SpecularColor = new Vector3(0.25f);
            effect.SpecularPower = 16;
        }

        protected override void NoShadeConfigurer(AnimationUtilities.SkinnedEffect effect, RendererBase.RendererParameters instance, object[] optionalParameters)
        {
            effect.CurrentTechnique = effect.Techniques["NoShade"];
        }

        protected override void PickingConfigurer(AnimationUtilities.SkinnedEffect effect, RendererParameters instance, object[] optionalParameters)
        {
            effect.CurrentTechnique = effect.Techniques["PickingShade"];
        }

        protected override void DrawGeometry(Matrix view, Matrix projection, object[] optionalParameters, EffectConfigurer effectConfigurer, RendererParameters instance)
        {
            effectConfigurer(mEffect, instance, optionalParameters);

            WaterParameters waterInstance = instance as WaterParameters;

            GraphicsManager.Device.SamplerStates[0] = SamplerState.LinearWrap;

            mEffect.World = Matrix.CreateScale(1.0f, waterInstance.SeaLevel, 1.0f) * waterInstance.World;
            mEffect.View = view;
            mEffect.Projection = projection;

            mEffect.Parameters["xOverlayColor"].SetValue(waterInstance.OverlayColor.ToVector3());
            mEffect.Parameters["xOverlayColorWeight"].SetValue(waterInstance.OverlayWeight);
            mEffect.Parameters["xTextureOffset"].SetValue(waterInstance.TextureAnimationOffset);
            mEffect.Texture = AssetLibrary.LookupTexture(waterInstance.TextureName);

            GraphicsManager.Device.SetVertexBuffer(mVertexBuffer);
            GraphicsManager.Device.Indices = mIndexBuffer;

            mEffect.CurrentTechnique.Passes[0].Apply();

            GraphicsManager.Device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, mVertexBuffer.VertexCount, 0, mIndexBuffer.IndexCount / 3);
        }
        
        #endregion

        #region VertexBuffer and IndexBuffer Creation

        private void ResizeVertices()
        {
            Vector2 quadResolution = mResolution - Vector2.One;

            mVertices = new VertexPositionNormalTexture[(int)mResolution.X * (int)mResolution.Y];

            float quadWidth  = SEA_WIDTH  / (float)quadResolution.X;
            float quadHeight = SEA_HEIGHT / (float)quadResolution.Y;

            for (int y = 0; y < mResolution.Y; ++y)
            {
                for (int x = 0; x < mResolution.X; ++x)
                {
                    VertexPositionNormalTexture vertex = new VertexPositionNormalTexture();
                    vertex.Position = new Vector3(-SEA_WIDTH / 2.0f + (float)x * quadWidth, 1.0f, -SEA_HEIGHT / 2.0f + (float)y * quadHeight);
                    vertex.Normal = Vector3.Up;
                    vertex.TextureCoordinate = new Vector2((float)x / quadResolution.X * TEXTURE_TILE_SCALE, (float)y / (float)quadResolution.Y * TEXTURE_TILE_SCALE);

                    mVertices[x + y * (int)mResolution.X] = vertex;
                }
            }
        }
        
        private void ResizeIndices()
        {
            Vector2 quadResolution = mResolution - Vector2.One;

            mIndices = new int[6 * (int)quadResolution.X * (int)quadResolution.Y];

            int indicesIndex = 0;
            for (int y = 0; y < quadResolution.Y; ++y)
            {
                for (int x = 0; x < quadResolution.X; ++x)
                {
                    int TLIndex = x +      y      * (int)mResolution.X;
                    int TRIndex = x + 1 +  y      * (int)mResolution.X;
                    int BRIndex = x + 1 + (y + 1) * (int)mResolution.X;
                    int BLIndex = x +     (y + 1) * (int)mResolution.X;

                    mIndices[indicesIndex++] = TLIndex;
                    mIndices[indicesIndex++] = TRIndex;
                    mIndices[indicesIndex++] = BRIndex;

                    mIndices[indicesIndex++] = TLIndex;
                    mIndices[indicesIndex++] = BRIndex;
                    mIndices[indicesIndex++] = BLIndex;
                }
            }
        }

        private void FillBuffers()
        {
            if (mVertexBuffer == null || mVertexBuffer.VertexCount != mVertices.Length)
            {
                mVertexBuffer = new VertexBuffer(GraphicsManager.Device, VertexPositionNormalTexture.VertexDeclaration, mVertices.Length, BufferUsage.None);
            }
            mVertexBuffer.SetData(mVertices);

            if (mIndexBuffer == null || mIndexBuffer.IndexCount != mIndices.Length)
            {
                mIndexBuffer = new IndexBuffer(GraphicsManager.Device, typeof(int), mIndices.Length, BufferUsage.None);
            }
            mIndexBuffer.SetData(mIndices);
        }

        #endregion
    }
}
