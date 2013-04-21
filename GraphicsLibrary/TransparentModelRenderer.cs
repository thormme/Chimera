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
        #region Structure

        public class TransparentModelParameters : RendererBase.RendererParameters
        {
            public Vector2 AnimationOffset { get; set; }
        }

        #endregion

        #region Public Interface

        public TransparentModelRenderer(Model model) : base(model)
        {
            mNormalDepthConfigurer = null;
            mShadowMapConfigurer = null;
            mWithShadowsConfigurer = null;
            mWithoutShadowsConfigurer = null;
        }

        #endregion

        #region Instance Render Helper Methods

        protected override void NoShadeConfigurer(AnimationUtilities.SkinnedEffect effect, RendererParameters instance, object[] optionalParameters)
        {
            effect.Parameters["xTextureOffset"].SetValue((instance as TransparentModelParameters).AnimationOffset);

            effect.CurrentTechnique = effect.Techniques["NoShade"];
        }

        #endregion
    }
}
