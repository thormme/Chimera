using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GraphicsLibrary
{
    public abstract class RendererBase
    {
        #region Member Variables

        protected Queue<RendererParameters> mInstances = new Queue<RendererParameters>();

        #endregion

        #region Structures

        public class RendererParameters
        {
            public string Name { get; set; }
            public Matrix World { get; set; }
            public Guid Guid { get; set; }
            public UInt32 ObjectID { get; set; }
            public BoundingBox BoundingBox { get; set; }
            public Color OverlayColor { get; set; }
            public float OverlayWeight { get; set; }
            public Vector2 TextureAnimationOffset { get; set; }
            public bool TryCull { get; set; }
        }

        #endregion

        #region Public Interface

        public void AddInstance(RendererParameters instance)
        {
            mInstances.Enqueue(instance);
        }

        public void ClearAllInstances()
        {
            mInstances.Clear();
        }

        public void RenderAllInstancesNormalDepth(Matrix view, Matrix projection)
        {
            foreach (RendererParameters instance in mInstances)
            {
                RenderNormalDepth(view, projection, instance);
            }
        }

        public void RenderAllInstancesShadowMap(Matrix lightView, Matrix lightProjection)
        {
            foreach (RendererParameters instance in mInstances)
            {
                RenderShadowMap(lightView, lightProjection, instance);
            }
        }

        public void RenderAllInstancesWithoutShadows(Matrix view, Matrix projection, Light light)
        {
            foreach (RendererParameters instance in mInstances)
            {
                RenderWithoutShadows(view, projection, light, instance);
            }
        }

        public void RenderAllInstancesWithShadows(CascadedShadowMap shadowMap, Matrix view, Matrix projection, Light light)
        {
            foreach (RendererParameters instance in mInstances)
            {
                RenderWithShadows(shadowMap, view, projection, light, instance);
            }
        }

        public void RenderAllInstancesPicking(Matrix view, Matrix projection)
        {
            foreach (RendererParameters instance in mInstances)
            {
                RenderPicking(view, projection, instance);
            }
        }

        #endregion

        #region Instance Render Methods

        protected void RenderNormalDepth(Matrix view, Matrix projection, RendererParameters instance)
        {
            EffectConfigurer normalDepthConfigurer = NormalDepthConfigurer;
            DrawGeometry(view, projection, null, normalDepthConfigurer, instance);
        }

        protected void RenderShadowMap(Matrix lightView, Matrix lightProjection, RendererParameters instance)
        {
            instance.TryCull = false;
            EffectConfigurer shadowMapConfigurer = ShadowMapConfigurer;
            DrawGeometry(Matrix.Identity, Matrix.Identity, new object[] { (Matrix?)lightView, (Matrix?)lightProjection }, shadowMapConfigurer, instance);
        }

        protected void RenderWithoutShadows(Matrix view, Matrix projection, Light light, RendererParameters instance)
        {
            EffectConfigurer withoutShadowsConfigurer = WithoutShadowsConfigurer;
            DrawGeometry(view, projection, new object[] { light }, withoutShadowsConfigurer, instance);
        }

        protected void RenderWithShadows(CascadedShadowMap shadowMap, Matrix view, Matrix projection, Light light, RendererParameters instance)
        {
            EffectConfigurer shadowsConfigurer = ShadowsConfigurer;
            DrawGeometry(view, projection, new object[] { shadowMap, light }, shadowsConfigurer, instance);
        }

        protected void RenderPicking(Matrix view, Matrix projection, RendererParameters instance)
        {
            EffectConfigurer pickingConfigurer = PickingConfigurer;
            DrawGeometry(view, projection, null, pickingConfigurer, instance);
        }

        #endregion

        #region Instance Render Helper Methods

        protected abstract void NormalDepthConfigurer(AnimationUtilities.SkinnedEffect effect, RendererParameters instance, object[] optionalParameters);
        protected abstract void ShadowMapConfigurer(AnimationUtilities.SkinnedEffect effect, RendererParameters instance, object[] optionalParameters);
        protected abstract void ShadowsConfigurer(AnimationUtilities.SkinnedEffect effect, RendererParameters instance, object[] optionalParameters);
        protected abstract void WithoutShadowsConfigurer(AnimationUtilities.SkinnedEffect effect, RendererParameters instance, object[] optionalParameters);
        protected abstract void PickingConfigurer(AnimationUtilities.SkinnedEffect effect, RendererParameters instance, object[] optionalParameters);

        protected abstract void DrawGeometry(Matrix view, Matrix projection, object[] optionalParameters, EffectConfigurer effectConfigurer, RendererParameters instance);

        #endregion

        protected delegate void EffectConfigurer(AnimationUtilities.SkinnedEffect effect, RendererParameters instance, object[] optionalParameters);
    }
}
