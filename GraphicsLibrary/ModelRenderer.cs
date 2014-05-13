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

        #region Structures

        public class ModelParameters : RendererBase.RendererParameters
        {
            public bool Spaghettify { get; set; }
            public Vector3 WormholePosition { get; set; }
            public float MaxWormholeDistance { get; set; }
        }

        #endregion

        #region Public Interface

        public ModelRenderer(Model model)
        {
            mModel = model;
            ConstructBoundingSphere();

            mUIConfigurer = null;
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

        protected override void WithShadowsConfigurer(AnimationUtilities.SkinnedEffect effect, RendererParameters instance, object[] optionalParameters)
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

        protected override void NoShadeConfigurer(AnimationUtilities.SkinnedEffect effect, RendererBase.RendererParameters instance, object[] optionalParameters)
        {
            effect.CurrentTechnique = effect.Techniques["NoShade"];
        }
        
        protected override void DrawGeometry(Matrix view, Matrix projection, object[] optionalParameters, EffectConfigurer effectConfigurer, RendererParameters instance)
        {
            ModelParameters modelInstance = instance as ModelParameters;

            if (modelInstance != null && modelInstance.TryCull && GraphicsManager.ViewBoundingFrustum.Contains(modelInstance.BoundingBox) == ContainmentType.Disjoint)
            {
                return;
            }

            foreach (ModelMesh mesh in mModel.Meshes)
            {
                foreach (AnimationUtilities.SkinnedEffect effect in mesh.Effects)
                {
                    effect.World = modelInstance.World;
                    effect.View       = view;
                    effect.Projection = projection;

                    effect.Parameters["xOverlayColor"].SetValue(modelInstance.OverlayColor.ToVector3());
                    effect.Parameters["xOverlayColorWeight"].SetValue(modelInstance.OverlayWeight);
                    effect.Parameters["xTextureOffset"].SetValue(modelInstance.TextureAnimationOffset);

                    effect.Parameters["xIsBeingSpaghettified"].SetValue(modelInstance.Spaghettify);
                    effect.Parameters["xWormholePosition"].SetValue(modelInstance.WormholePosition);
                    effect.Parameters["xMaxWormholeDistance"].SetValue(modelInstance.MaxWormholeDistance);
                    effect.Parameters["xModelWormholeDistance"].SetValue((modelInstance.WormholePosition - modelInstance.World.Translation).Length());

                    effectConfigurer(effect, modelInstance, optionalParameters);
                }
                mesh.Draw();
            }
        }

        #endregion
    }
}
