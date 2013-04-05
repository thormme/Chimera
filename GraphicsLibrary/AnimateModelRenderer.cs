using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GraphicsLibrary
{
    public class AnimateModelRenderer : ModelRenderer
    {
        #region Private Variables

        private Dictionary<string, Matrix> mBoneTweakLibrary;

        #endregion

        #region Structures

        public class AnimateModelParameters : RendererBase.RendererParameters
        {
            public Matrix[] SkinTransforms { get; set; }
        }

        #endregion

        #region Public Properties

        public AnimationUtilities.SkinningData SkinningData { get { return mModel.Tag as AnimationUtilities.SkinningData; } }

        public Dictionary<string, Matrix> BoneTweakLibrary { get { return mBoneTweakLibrary; } set { mBoneTweakLibrary = value; } }

        #endregion

        #region Public Interface

        public AnimateModelRenderer(Model model) : base(model) { }

        #endregion

        #region Instance Render Helper Methods

        protected override void NormalDepthConfigurer(AnimationUtilities.SkinnedEffect effect, RendererParameters instance, object[] optionalParameters)
        {
            base.NormalDepthConfigurer(effect, instance, optionalParameters);

            Matrix[] skinTransforms = new Matrix[] { Matrix.Identity };
            if (instance is AnimateModelParameters)
            {
                skinTransforms = ((instance as AnimateModelParameters).SkinTransforms);
            }
            effect.SetBoneTransforms(skinTransforms);


            effect.CurrentTechnique = effect.Techniques["SkinnedNormalDepthShade"];
        }

        protected override void ShadowMapConfigurer(AnimationUtilities.SkinnedEffect effect, RendererBase.RendererParameters instance, object[] optionalParameters)
        {
            base.ShadowMapConfigurer(effect, instance, optionalParameters);

            Matrix[] skinTransforms = new Matrix[] { Matrix.Identity };
            if (instance is AnimateModelParameters)
            {
                skinTransforms = ((instance as AnimateModelParameters).SkinTransforms);
            }
            effect.SetBoneTransforms(skinTransforms);

            effect.CurrentTechnique = effect.Techniques["SkinnedShadowCast"];
        }

        protected override void ShadowsConfigurer(AnimationUtilities.SkinnedEffect effect, RendererParameters instance, object[] optionalParameters)
        {
            base.ShadowsConfigurer(effect, instance, optionalParameters);

            Matrix[] skinTransforms = new Matrix[] { Matrix.Identity };
            if (instance is AnimateModelParameters)
            {
                skinTransforms = ((instance as AnimateModelParameters).SkinTransforms);
            }
            effect.SetBoneTransforms(skinTransforms);


            effect.CurrentTechnique = effect.Techniques["SkinnedCelShadeWithShadows"];
        }

        protected override void WithoutShadowsConfigurer(AnimationUtilities.SkinnedEffect effect, RendererParameters instance, object[] optionalParameters)
        {
            base.WithoutShadowsConfigurer(effect, instance, optionalParameters);

            Matrix[] skinTransforms = new Matrix[] { Matrix.Identity };
            if (instance is AnimateModelParameters)
            {
                skinTransforms = ((instance as AnimateModelParameters).SkinTransforms);
            }
            effect.SetBoneTransforms(skinTransforms);


            effect.CurrentTechnique = effect.Techniques["SkinnedCelShadeWithoutShadows"];
        }

        

        #endregion
    }
}
