using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameConstructLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GraphicsLibrary
{
    public class HeightMapRenderer : RendererBase
    {
        #region Constants

        const int MAX_TEXTURE_LAYERS = 5;

        static string[] LAYER_TEXTURE_NAMES = new string[] { "Texture", "RedTexture", "GreenTexture", "BlueTexture", "AlphaTexture" };

        #endregion

        #region Private Variables

        protected HeightMapMesh mMesh;
        protected AnimationUtilities.SkinnedEffect mEffect;

        #endregion

        #region Public Properties

        public HeightMapMesh Mesh { get { return mMesh; } }

        #endregion

        #region Structures

        public class HeightMapParameters : RendererParameters
        {
            public HeightMapRenderable.CursorShape DrawCursor { get; set; }
            public Vector3 CursorPosition { get; set; }
            public float CursorInnerRadius { get; set; }
            public float CursorOuterRadius { get; set; }
            public Vector4 TextureMask { get; set; }
        }

        #endregion

        #region Public Interface

        public HeightMapRenderer(HeightMapMesh heightMap, AnimationUtilities.SkinnedEffect effect)
        {
            mMesh = heightMap;
            mEffect = effect;

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

            effect.CurrentTechnique = effect.Techniques["TerrainCelShadeWithShadows"];

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

            effect.CurrentTechnique = effect.Techniques["TerrainCelShadeWithoutShadows"];

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

            effect.Parameters["xPickingIndex"].SetValue(new Vector4(0, 0, 0, 0));
        }

        protected override void DrawGeometry(Matrix view, Matrix projection, object[] optionalParameters, EffectConfigurer effectConfigurer, RendererParameters instance)
        {
            HeightMapParameters terrainInstance = instance as HeightMapParameters;

            if (terrainInstance.TryCull && GraphicsManager.ViewBoundingFrustum.Contains(terrainInstance.BoundingBox) == ContainmentType.Disjoint)
            {
                return;
            }

            for (int i = 0; i < 7; ++i)
            {
                GraphicsManager.Device.SamplerStates[i] = SamplerState.PointWrap;
            }
            GraphicsManager.Device.SamplerStates[2] = SamplerState.LinearClamp;

            mEffect.World = instance.World;
            mEffect.View = view;
            mEffect.Projection = projection;

            mEffect.Parameters["xOverlayColor"].SetValue(instance.OverlayColor.ToVector3());
            mEffect.Parameters["xOverlayColorWeight"].SetValue(instance.OverlayWeight);
            mEffect.Parameters["xTextureOffset"].SetValue(instance.TextureAnimationOffset);

            mEffect.Parameters["xIsBeingSpaghettified"].SetValue(false);

            mEffect.Parameters["xDrawCursor"].SetValue((int)terrainInstance.DrawCursor);

            if (terrainInstance.DrawCursor != HeightMapRenderable.CursorShape.NONE)
            {
                mEffect.Parameters["xCursorPosition"].SetValue(terrainInstance.CursorPosition);
                mEffect.Parameters["xCursorInnerRadius"].SetValue(terrainInstance.CursorInnerRadius);
                mEffect.Parameters["xCursorOuterRadius"].SetValue(terrainInstance.CursorOuterRadius);
            }

            mEffect.Parameters["xTextureMask"].SetValue(terrainInstance.TextureMask);

            effectConfigurer(mEffect, instance, optionalParameters);

            mEffect.Parameters["AlphaMap"].SetValue(mMesh.AlphaMap);

            for (int i = 0; i < mMesh.DetailTextureNames.Length; ++i)
            {
                string detailTextureName = mMesh.DetailTextureNames[i];
                Vector2 uvOffset = mMesh.DetailTextureUVOffset[i];
                Vector2 uvScale = mMesh.DetailTextureUVScale[i];

                if (detailTextureName != null)
                {
                    mEffect.Parameters[LAYER_TEXTURE_NAMES[i]].SetValue(AssetLibrary.LookupTexture(detailTextureName));

                    mEffect.Parameters[LAYER_TEXTURE_NAMES[i] + "_uvOffset"].SetValue(uvOffset);
                    mEffect.Parameters[LAYER_TEXTURE_NAMES[i] + "_uvScale"].SetValue(uvScale);
                }
            }

            VertexBuffer vertexBuffer = mMesh.VertexBuffer;
            IndexBuffer indexBuffer = mMesh.IndexBuffer;

            GraphicsManager.Device.SetVertexBuffer(vertexBuffer);
            GraphicsManager.Device.Indices = indexBuffer;

            mEffect.CurrentTechnique.Passes[0].Apply();

            GraphicsManager.Device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, vertexBuffer.VertexCount, 0, indexBuffer.IndexCount / 3);

            GraphicsManager.Device.SetVertexBuffer(null);
            GraphicsManager.Device.Indices = null;
        }

        #endregion
    }
}
