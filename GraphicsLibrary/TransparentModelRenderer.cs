using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GraphicsLibrary
{
    public class TransparentModelRenderer : ModelRenderer
    {
        #region Private Variable

        private bool mDoNotDraw = false;

        #endregion

        #region Structure

        public class TransparentModelParameters : RendererBase.RendererParameters
        {
            public Vector2 AnimationOffset { get; set; }
        }

        #endregion

        #region Public Interface

        public TransparentModelRenderer(Model model) : base(model) { }

        public void RenderAllInstancesNoShading(Matrix view, Matrix projection)
        {
            foreach (RendererParameters instance in mInstances)
            {
                RenderNoShading(view, projection, instance);
            }
        }

        #endregion
        
        #region Instance Render Methods

        protected void RenderNoShading(Matrix view, Matrix projection, RendererParameters instance)
        {
            EffectConfigurer noShadeConfigurer = NoShadeConfigurer;
            DrawGeometry(view, projection, null, noShadeConfigurer, instance);
        }

        #endregion

        #region Instance Render Helper Methods

        protected override void NormalDepthConfigurer(AnimationUtilities.SkinnedEffect effect, RendererParameters instance, object[] optionalParameters)
        {
            mDoNotDraw = true;
        }

        protected override void ShadowMapConfigurer(AnimationUtilities.SkinnedEffect effect, RendererBase.RendererParameters instance, object[] optionalParameters)
        {
            mDoNotDraw = true;
        }

        protected override void ShadowsConfigurer(AnimationUtilities.SkinnedEffect effect, RendererParameters instance, object[] optionalParameters)
        {
            mDoNotDraw = true;
        }

        protected override void WithoutShadowsConfigurer(AnimationUtilities.SkinnedEffect effect, RendererParameters instance, object[] optionalParameters)
        {
            mDoNotDraw = true;
        }

        protected void NoShadeConfigurer(AnimationUtilities.SkinnedEffect effect, RendererParameters instance, object[] optionalParameters)
        {
            effect.Parameters["xTextureOffset"].SetValue((instance as TransparentModelParameters).AnimationOffset);

            effect.CurrentTechnique = effect.Techniques["NoShade"];
        }

        protected override void DrawGeometry(Matrix view, Matrix projection, object[] optionalParameters, RendererBase.EffectConfigurer effectConfigurer, RendererBase.RendererParameters instance)
        {
            if (!mDoNotDraw)
            {
                base.DrawGeometry(view, projection, optionalParameters, effectConfigurer, instance);
            }

            mDoNotDraw = false;
        }

        #endregion
    }
}
