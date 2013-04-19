using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GraphicsLibrary
{
    public class ModelRenderer : RendererBase
    {
        #region Private Variables

        protected Model mModel;
        protected BoundingSphere mBoundingSphere;
        
        #endregion

        #region Public Properties

        public Model Model { get { return mModel; } }

        public BoundingSphere BoundingSphere { get { return mBoundingSphere; } }

        #endregion

        #region Public Interface

        public ModelRenderer(Model model)
        {
            mModel = model;
            ConstructBoundingSphere();
        }

        protected virtual void ConstructBoundingSphere()
        {
            Vector3 modelCenter = Vector3.Zero;
            foreach (ModelMesh mesh in mModel.Meshes)
            {
                modelCenter += mesh.BoundingSphere.Center;
            }

            modelCenter /= mModel.Meshes.Count;

            float modelRadius = 0;
            foreach (ModelMesh mesh in mModel.Meshes)
            {
                float meshRadius = (mesh.BoundingSphere.Center - modelCenter).Length() + mesh.BoundingSphere.Radius;

                modelRadius = Math.Max(modelRadius, meshRadius);
            }

            mBoundingSphere = new BoundingSphere(modelCenter, modelRadius);
        }

        #endregion

        #region Instance Render Helper Methods

        protected override void NormalDepthConfigurer(AnimationUtilities.SkinnedEffect effect, RendererParameters instance, object[] optionalParameters)
        {
            effect.CurrentTechnique = effect.Techniques["NormalDepthShade"];
        }

        protected override void PickingConfigurer(AnimationUtilities.SkinnedEffect effect, RendererBase.RendererParameters instance, object[] optionalParameters)
        {
            effect.CurrentTechnique = effect.Techniques["PickingShade"];

            Vector4 indexColor = new Vector4((float)(instance.ObjectID << 8 >> 24) / 255, (float)(instance.ObjectID << 16 >> 24) / 255, (float)(instance.ObjectID << 24 >> 24) / 255, 1f);
            effect.Parameters["xPickingIndex"].SetValue(indexColor);
        }

        protected override void ShadowMapConfigurer(AnimationUtilities.SkinnedEffect effect, RendererBase.RendererParameters instance, object[] optionalParameters)
        {
            Matrix lightView = (optionalParameters[0] as Matrix?).Value;
            Matrix lightProjection = (optionalParameters[1] as Matrix?).Value;

            effect.CurrentTechnique = effect.Techniques["ShadowCast"];

            effect.Parameters["xLightView"].SetValue(lightView);
            effect.Parameters["xLightProjection"].SetValue(lightProjection);
        }

        protected override void ShadowsConfigurer(AnimationUtilities.SkinnedEffect effect, RendererParameters instance, object[] optionalParameters)
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

        protected override void DrawGeometry(Matrix view, Matrix projection, object[] optionalParameters, EffectConfigurer effectConfigurer, RendererParameters instance)
        {
            if (effectConfigurer != ShadowMapConfigurer && GraphicsManager.ViewBoundingFrustum.Contains(instance.BoundingBox) == ContainmentType.Disjoint)
            {
                return;
            }

            foreach (ModelMesh mesh in mModel.Meshes)
            {
                foreach (AnimationUtilities.SkinnedEffect effect in mesh.Effects)
                {
                    effect.World      = instance.World;
                    effect.View       = view;
                    effect.Projection = projection;

                    effect.Parameters["xOverlayColor"].SetValue(instance.OverlayColor.ToVector3());
                    effect.Parameters["xOverlayColorWeight"].SetValue(instance.OverlayWeight);
                    effect.Parameters["xTextureOffset"].SetValue(instance.TextureAnimationOffset);

                    effectConfigurer(effect, instance, optionalParameters);
                }
                mesh.Draw();
            }
        }

        #endregion
    }
}
