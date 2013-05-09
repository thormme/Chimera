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

        protected EffectConfigurer mNormalDepthConfigurer    = null;
        protected EffectConfigurer mShadowMapConfigurer      = null;
        protected EffectConfigurer mWithoutShadowsConfigurer = null;
        protected EffectConfigurer mWithShadowsConfigurer    = null;
        protected EffectConfigurer mPickingConfigurer        = null;
        protected EffectConfigurer mNoShadeConfigurer        = null;
        protected EffectConfigurer mUIConfigurer             = null;
        protected EffectConfigurer mOverlayConfigurer        = null;

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

        public RendererBase()
        {
            mNormalDepthConfigurer    = NormalDepthConfigurer;
            mShadowMapConfigurer      = ShadowMapConfigurer;
            mWithoutShadowsConfigurer = WithoutShadowsConfigurer;
            mWithShadowsConfigurer    = WithShadowsConfigurer;
            mPickingConfigurer        = PickingConfigurer;
            mNoShadeConfigurer        = NoShadeConfigurer;
            mUIConfigurer             = UIConfigurer;
            mOverlayConfigurer        = NoShadeConfigurer;
        }

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
            if (mNormalDepthConfigurer != null)
            {
                foreach (RendererParameters instance in mInstances)
                {
                    RenderNormalDepth(view, projection, instance);
                }
            }
        }

        public void RenderAllInstancesShadowMap(Matrix lightView, Matrix lightProjection)
        {
            if (mShadowMapConfigurer != null)
            {
                foreach (RendererParameters instance in mInstances)
                {
                    RenderShadowMap(lightView, lightProjection, instance);
                }
            }
        }

        public void RenderAllInstancesWithoutShadows(Matrix view, Matrix projection, Light light)
        {
            if (mWithoutShadowsConfigurer != null)
            {
                foreach (RendererParameters instance in mInstances)
                {
                    RenderWithoutShadows(view, projection, light, instance);
                }
            }
        }

        public void RenderAllInstancesWithShadows(CascadedShadowMap shadowMap, Matrix view, Matrix projection, Light light)
        {
            if (mWithShadowsConfigurer != null)
            {
                foreach (RendererParameters instance in mInstances)
                {
                    RenderWithShadows(shadowMap, view, projection, light, instance);
                }
            }
        }

        public void RenderAllInstancesNoShading(Matrix view, Matrix projection)
        {
            if (mNoShadeConfigurer != null)
            {
                foreach (RendererParameters instance in mInstances)
                {
                    RenderNoShading(view, projection, instance);
                }
            }
        }

        public void RenderAllInstancesOverlayColor(Matrix view, Matrix projection, Color overlayColor, float overlayWeight)
        {
            if (mOverlayConfigurer != null)
            {
                foreach (RendererParameters instance in mInstances)
                {
                    instance.OverlayColor = overlayColor;
                    instance.OverlayWeight = overlayWeight;

                    RenderNoShading(view, projection, instance);
                }
            }
        }

        public void RenderAllInstancesPicking(Matrix view, Matrix projection)
        {
            if (mPickingConfigurer != null)
            {
                foreach (RendererParameters instance in mInstances)
                {
                    RenderPicking(view, projection, instance);
                }
            }
        }

        public void RenderAllInstancesUI(Matrix view, Matrix projection)
        {
            if (mUIConfigurer != null)
            {
                foreach (RendererParameters instances in mInstances)
                {
                    RenderUI(view, projection, instances);
                }
            }
        }

        #endregion

        #region Instance Render Methods

        protected void RenderNormalDepth(Matrix view, Matrix projection, RendererParameters instance)
        {
            DrawGeometry(view, projection, null, mNormalDepthConfigurer, instance);
        }

        protected void RenderShadowMap(Matrix lightView, Matrix lightProjection, RendererParameters instance)
        {
            bool oldTryCull = instance.TryCull;
            instance.TryCull = false;

            DrawGeometry(Matrix.Identity, Matrix.Identity, new object[] { (Matrix?)lightView, (Matrix?)lightProjection }, mShadowMapConfigurer, instance);

            instance.TryCull = oldTryCull;
        }

        protected void RenderWithoutShadows(Matrix view, Matrix projection, Light light, RendererParameters instance)
        {
            DrawGeometry(view, projection, new object[] { light }, mWithoutShadowsConfigurer, instance);
        }

        protected void RenderWithShadows(CascadedShadowMap shadowMap, Matrix view, Matrix projection, Light light, RendererParameters instance)
        {
            DrawGeometry(view, projection, new object[] { shadowMap, light }, mWithShadowsConfigurer, instance);
        }

        protected void RenderNoShading(Matrix view, Matrix projection, RendererParameters instance)
        {
            DrawGeometry(view, projection, null, mNoShadeConfigurer, instance);
        }

        protected void RenderPicking(Matrix view, Matrix projection, RendererParameters instance)
        {
            DrawGeometry(view, projection, null, mPickingConfigurer, instance);
        }

        protected void RenderUI(Matrix view, Matrix projection, RendererParameters instance)
        {
            DrawGeometry(view, projection, null, mUIConfigurer, instance);
        }

        #endregion

        #region Instance Render Helper Methods

        protected virtual void NormalDepthConfigurer(AnimationUtilities.SkinnedEffect effect, RendererParameters instance, object[] optionalParameters) { }
        protected virtual void ShadowMapConfigurer(AnimationUtilities.SkinnedEffect effect, RendererParameters instance, object[] optionalParameters) { }
        protected virtual void WithShadowsConfigurer(AnimationUtilities.SkinnedEffect effect, RendererParameters instance, object[] optionalParameters) { }
        protected virtual void WithoutShadowsConfigurer(AnimationUtilities.SkinnedEffect effect, RendererParameters instance, object[] optionalParameters) { }
        protected virtual void NoShadeConfigurer(AnimationUtilities.SkinnedEffect effect, RendererParameters instance, object[] optionalParameters) { }
        protected virtual void PickingConfigurer(AnimationUtilities.SkinnedEffect effect, RendererParameters instance, object[] optionalParameters) { }
        protected virtual void UIConfigurer(AnimationUtilities.SkinnedEffect effect, RendererParameters instance, object[] optionalParameters) { }

        protected abstract void DrawGeometry(Matrix view, Matrix projection, object[] optionalParameters, EffectConfigurer effectConfigurer, RendererParameters instance);

        #endregion

        protected delegate void EffectConfigurer(AnimationUtilities.SkinnedEffect effect, RendererParameters instance, object[] optionalParameters);
    }
}
