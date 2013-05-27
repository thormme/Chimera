using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameConstructLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GraphicsLibrary
{
    public class TerrainRenderer : RendererBase
    {
        #region Constants

        const int MAX_TEXTURE_LAYERS = 5;

        static string[] LAYER_TEXTURE_NAMES = new string[] { "Texture", "RedTexture", "GreenTexture", "BlueTexture", "AlphaTexture" };

        #endregion

        #region Private Variables

        private TerrainHeightMap mHeightMap;
        private TerrainTexture   mTexture;
        private AnimationUtilities.SkinnedEffect mEffect;

        #endregion

        #region Public Properties

        public TerrainHeightMap HeightMap { get { return mHeightMap; } }
        public TerrainTexture Texture { get { return mTexture; } }

        #endregion

        #region Structures

        public class TerrainParameters : RendererParameters
        {
            public BoundingBox[,] BoundingBoxes { get; set; }
            public TerrainRenderable.CursorShape DrawCursor { get; set; }
            public Vector3 CursorPosition { get; set; }
            public float CursorInnerRadius { get; set; }
            public float CursorOuterRadius { get; set; }
            public Vector4 TextureMask { get; set; }
        }

        #endregion

        #region Public Interface

        public TerrainRenderer(TerrainHeightMap heightMap, TerrainTexture texture, AnimationUtilities.SkinnedEffect effect)
        {
            mHeightMap = heightMap;
            mTexture   = texture;
            mEffect    = effect;

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

            effect.Parameters["xPickingIndex"].SetValue(new Vector4(0,0,0,0));
        }

        protected override void DrawGeometry(Matrix view, Matrix projection, object[] optionalParameters, EffectConfigurer effectConfigurer, RendererParameters instance)
        {
            TerrainParameters terrainInstance = instance as TerrainParameters;

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

            if (terrainInstance.DrawCursor != TerrainRenderable.CursorShape.NONE)
            {
                mEffect.Parameters["xCursorPosition"].SetValue(terrainInstance.CursorPosition);
                mEffect.Parameters["xCursorInnerRadius"].SetValue(terrainInstance.CursorInnerRadius);
                mEffect.Parameters["xCursorOuterRadius"].SetValue(terrainInstance.CursorOuterRadius);
            }

            mEffect.Parameters["xTextureMask"].SetValue(terrainInstance.TextureMask);

            effectConfigurer(mEffect, instance, optionalParameters);
            
            for (int chunkRowIndex = 0; chunkRowIndex < mHeightMap.NumChunksVertical; ++chunkRowIndex)
            {
                for (int chunkColIndex = 0; chunkColIndex < mHeightMap.NumChunksHorizontal; ++chunkColIndex )
                {
                    if (terrainInstance.TryCull && GraphicsManager.ViewBoundingFrustum.Contains((instance as TerrainParameters).BoundingBoxes[chunkRowIndex, chunkColIndex]) == ContainmentType.Disjoint)
                    {
                        // Hey, it looks like you.
                        // Just tried to draw me.
                        // But I'm not on screen.
                        // So cull me, maybe?
                        continue;
                    }

                    DrawTerrainChunk(chunkRowIndex, chunkColIndex, -1);
                }
            }

            for (int sideIndex = 0; sideIndex < 4; ++sideIndex)
            {
                DrawTerrainChunk(-1, -1, sideIndex);
            }

            GraphicsManager.Device.SetVertexBuffer(null);
        }

        private void DrawTerrainChunk(int rowIndex, int colIndex, int sideIndex)
        {
            VertexBuffer vertexBuffer = null;
            IndexBuffer indexBuffer = null;
            if (sideIndex < 0)
            {
                mEffect.Parameters["AlphaMap"].SetValue(mTexture.TextureBuffers[rowIndex, colIndex]);

                for (int i = 0; i < MAX_TEXTURE_LAYERS; ++i)
                {
                    string detailTextureName = mTexture.DetailTextureNames[rowIndex, colIndex, i];
                    Vector2 uvOffset = mTexture.DetailTextureUVOffset[rowIndex, colIndex, i];
                    Vector2 uvScale = mTexture.DetailTextureUVScale[rowIndex, colIndex, i];

                    if (detailTextureName != null)
                    {
                        mEffect.Parameters[LAYER_TEXTURE_NAMES[i]].SetValue(AssetLibrary.LookupTexture(detailTextureName));

                        mEffect.Parameters[LAYER_TEXTURE_NAMES[i] + "_uvOffset"].SetValue(uvOffset);
                        mEffect.Parameters[LAYER_TEXTURE_NAMES[i] + "_uvScale"].SetValue(uvScale);
                    }
                }

                vertexBuffer = mHeightMap.VertexBuffers[rowIndex, colIndex];
                indexBuffer = mHeightMap.IndexBuffers[rowIndex, colIndex];
            }
            else
            {
                vertexBuffer = mHeightMap.EdgeVertexBuffers[sideIndex];
                indexBuffer = mHeightMap.EdgeIndexBuffers[sideIndex];
            }

            GraphicsManager.Device.SetVertexBuffer(vertexBuffer);
            GraphicsManager.Device.Indices = indexBuffer;

            mEffect.CurrentTechnique.Passes[0].Apply();

            GraphicsManager.Device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, vertexBuffer.VertexCount, 0, indexBuffer.IndexCount / 3);
        }

        #endregion
    }
}
