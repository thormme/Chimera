using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphicsLibrary
{
    public class GridRenderer : HeightMapRenderer
    {
        public GridRenderer(HeightMapMesh heightMap, AnimationUtilities.SkinnedEffect effect)
            : base(heightMap, effect)
        {
            mNormalDepthConfigurer = null;
            mPickingConfigurer = null;
            mShadowMapConfigurer = null;
            mUIConfigurer = null;
            mWithoutShadowsConfigurer = null;
            mWithShadowsConfigurer = null;
        }

        protected override void DrawGeometry(Microsoft.Xna.Framework.Matrix view, Microsoft.Xna.Framework.Matrix projection, object[] optionalParameters, RendererBase.EffectConfigurer effectConfigurer, RendererBase.RendererParameters instance)
        {
            mEffect.World = instance.World;
            mEffect.View = view;
            mEffect.Projection = projection;

            mEffect.Parameters["xOverlayColor"].SetValue(instance.OverlayColor.ToVector3());
            mEffect.Parameters["xOverlayColorWeight"].SetValue(instance.OverlayWeight);
            mEffect.Parameters["xTextureOffset"].SetValue(instance.TextureAnimationOffset);

            mEffect.Parameters["xIsBeingSpaghettified"].SetValue(false);

            mEffect.Texture = mMesh.AlphaMap;

            effectConfigurer(mEffect, instance, optionalParameters);

            VertexBuffer vertexBuffer = mMesh.VertexBuffer;
            IndexBuffer indexBuffer = mMesh.IndexBuffer;

            GraphicsManager.Device.SetVertexBuffer(vertexBuffer);
            GraphicsManager.Device.Indices = indexBuffer;

            mEffect.CurrentTechnique.Passes[0].Apply();

            GraphicsManager.Device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, vertexBuffer.VertexCount, 0, indexBuffer.IndexCount / 3 - 2);

            GraphicsManager.Device.SetVertexBuffer(null);
            GraphicsManager.Device.Indices = null;
        }
    }
}
