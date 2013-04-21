using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphicsLibrary
{
    public class UIModelRenderer : ModelRenderer
    {
        #region Public Interface

        public UIModelRenderer(Model model) : base(model)
        {
            mNormalDepthConfigurer = null;
            mShadowMapConfigurer = null;
            mWithShadowsConfigurer = null;
            mWithoutShadowsConfigurer = null;

            mUIConfigurer = UIConfigurer;
        }

        #endregion

        #region Instance Render Helper Methods

        protected override void UIConfigurer(AnimationUtilities.SkinnedEffect effect, RendererBase.RendererParameters instance, object[] optionalParameters)
        {
            NoShadeConfigurer(effect, instance, optionalParameters);
        }

        #endregion
    }
}
