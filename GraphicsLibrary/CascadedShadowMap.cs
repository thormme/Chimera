using GameConstructLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphicsLibrary
{
    public class CascadedShadowMap
    {
        #region Constants

        private const int MAX_BUFFER_WIDTH  = 4096;
        private const int MAX_BUFFER_HEIGHT = 4096;
        private const int MAX_CASCADE_COUNT = 4;

        #endregion

        #region Shadow Buffer Variables

        public RenderTarget2D Buffer
        {
            get { return mShadowBuffer; }
        }
        private RenderTarget2D mShadowBuffer;

        public int CascadeCount
        {
            get { return mCascadeCount; }
            set { ResizeShadowBuffer(value, mCascadeResolution); }
        }
        private int mCascadeCount = 0;

        public int CascadeResolution
        {
            get { return mCascadeResolution; }
            set { ResizeShadowBuffer(mCascadeCount, value); }
        }
        private int mCascadeResolution = 0;

        public Vector4[] CascadeBounds
        {
            get
            {
                Vector4[] cascadeBounds = new Vector4[mCascadeCount];
                for (int iCascadeCount = 0; iCascadeCount < mCascadeCount; iCascadeCount++)
                {
                    cascadeBounds[iCascadeCount] = mCascadeContainer[iCascadeCount].BufferBounds;
                }
                return cascadeBounds;
            }
        }

        public Vector4[] CascadeColors
        {
            get
            {
                Vector4[] cascadeColor = new Vector4[mCascadeCount];
                for (int iCascadeCount = 0; iCascadeCount < mCascadeCount; iCascadeCount++)
                {
                    Color color = mCascadeContainer[iCascadeCount].Color;
                    cascadeColor[iCascadeCount] = new Vector4((float)color.R / 255.0f, (float)color.G / 255.0f, (float)color.B / 255.0f, (float)color.A / 255.0f);
                }
                return cascadeColor;
            }
        }

        private List<Cascade> mCascadeContainer = new List<Cascade>();

        #endregion

        #region Private Shadow Buffer Methods

        private void ResizeShadowBuffer(int cascadeCount, int cascadeResolution)
        {
            if (cascadeResolution > MAX_BUFFER_HEIGHT || cascadeCount * cascadeResolution > MAX_BUFFER_WIDTH || cascadeCount < 0)
            {
                string resizeErrorMessage = cascadeCount == mCascadeCount ? "Cascade Resolution is too high" : "Cascade Count is too high";
                throw new Exception(resizeErrorMessage);
            }

            if (cascadeCount != mCascadeCount)
            {
                mCascadeCount = cascadeCount;
                ResizeCascadeContainer();
            }
            
            mCascadeResolution = cascadeResolution;

            mShadowBuffer = new RenderTarget2D(
                mGraphicsDevice,                      // Hardware abstraction to render from.
                mCascadeCount * mCascadeResolution,   // Buffer width.
                mCascadeResolution,                   // Buffer height.
                false,                                // No MipMapping.
                SurfaceFormat.Single,                 // Single precision floating point values in buffer.
                DepthFormat.Depth24Stencil8,          // Precisions of accompanying depth buffer.
                0,                                    // No Multisampling (0x MSAA).
                RenderTargetUsage.DiscardContents     // Clear buffer upon binding to graphics device.
                );
        }

        private void ResizeCascadeContainer()
        {
            int[] nearPercentages = {  -1, -10, -30, -60  };
            int[] farPercentages  = { -10, -30, -60, -100 };
            Color[] colorBands = { 
                                      new Color(1.0f, 0.0f, 0.0f, 1.0f), 
                                      new Color(0.0f, 1.0f, 0.0f, 1.0f), 
                                      new Color(0.0f, 0.0f, 1.0f, 1.0f), 
                                      new Color(1.0f, 1.0f, 0.0f, 1.0f) 
                                  };

            mCascadeContainer.Clear();
            for (int iCascadeCount = 0; iCascadeCount < mCascadeCount; iCascadeCount++)
            {
                mCascadeContainer.Add(new Cascade(nearPercentages[iCascadeCount], farPercentages[iCascadeCount], colorBands[iCascadeCount]));
            }
        }

        private List<Vector3> SplitViewFrustum(float viewSpaceNearSplit, float viewSpaceFarSplit, Matrix viewTransform, Matrix projectionTransform)
        {
            Vector4 clipSpaceNearVector = Vector4.Transform(new Vector3(0.0f, 0.0f, viewSpaceNearSplit), projectionTransform);
            float clipSpaceNearSplit = clipSpaceNearVector.W != 0.0f ? clipSpaceNearVector.Z / clipSpaceNearVector.W : 0.0f;

            Vector4 clipSpaceFarVector = Vector4.Transform(new Vector3(0.0f, 0.0f, viewSpaceFarSplit), projectionTransform);
            float clipSpaceFarSplit = clipSpaceFarVector.W != 0.0f ? clipSpaceFarVector.Z / clipSpaceFarVector.W : 0.0f;

            Matrix inverseViewProjection = Matrix.Invert(viewTransform * projectionTransform);
            Vector3[] clipCorners = new BoundingBox(new Vector3(-1.0f, -1.0f, clipSpaceNearSplit), new Vector3(1.0f, 1.0f, clipSpaceFarSplit)).GetCorners();

            List<Vector3> transformedCorners = new List<Vector3>();
            foreach (Vector3 corner in clipCorners)
            {
                Vector4 transformedCorner = Vector4.Transform(corner, inverseViewProjection);
                transformedCorner /= transformedCorner.W;

                transformedCorners.Add(new Vector3(transformedCorner.X, transformedCorner.Y, transformedCorner.Z));
            }

            return transformedCorners;
        }

        #endregion

        #region External System References

        public GraphicsDevice GraphicsDevice
        {
            get { return mGraphicsDevice; }
            set { mGraphicsDevice = value; }
        }
        private GraphicsDevice mGraphicsDevice;

        #endregion

        #region Light Transform Variables

        public Matrix LightView
        {
            get { return mLightView; }
        }
        private Matrix mLightView;

        public Matrix LightProjection
        {
            get { return mLightProjection; }
        }
        private Matrix mLightProjection;

        public Matrix[] LightProjections
        {
            get
            {
                Matrix[] lightProjections = new Matrix[mCascadeCount];
                for (int iCascadeCount = 0; iCascadeCount < mCascadeCount; iCascadeCount++)
                {
                    lightProjections[iCascadeCount] = mCascadeContainer[iCascadeCount].ProjectionTransform;
                }
                return lightProjections;
            }
        }

        #endregion

        #region Public Interface

        public CascadedShadowMap(GraphicsDevice graphicsDevice, int cascadeCount, int cascadeResolution)
        {
            GraphicsDevice = graphicsDevice;
            ResizeShadowBuffer(cascadeCount, cascadeResolution);
        }

        public void GenerateCascades(ICamera camera, Light light, BoundingBox sceneAABB)
        {
            //Matrix lightRotation = Matrix.CreateLookAt(Vector3.Zero, -light.Direction, Vector3.Up);

            //BoundingFrustum viewFrustum = new BoundingFrustum(camera.GetViewTransform() * camera.GetProjectionTransform());
            //Vector3[] viewFrustumCorners = viewFrustum.GetCorners();
            //Matrix viewTransform = camera.GetViewTransform();
            //Vector3.Transform(viewFrustumCorners, ref viewTransform, viewFrustumCorners);

            //BoundingBox viewBoundingBox = BoundingBox.CreateFromPoints(viewFrustumCorners);
            //Vector3 halfBoxSize = (viewBoundingBox.Max - viewBoundingBox.Min) / 2.0f;

            //Vector3 lightPosition = viewBoundingBox.Min + halfBoxSize;
            //lightPosition.Z = viewBoundingBox.Min.Z;
            //light.Position = Vector3.Transform(lightPosition, Matrix.Invert(lightRotation));

            mLightView = Matrix.CreateLookAt(light.Position, light.Position + light.Direction, Vector3.Up);

            Vector3[] sceneCorners = sceneAABB.GetCorners();
            Vector3.Transform(sceneCorners, ref mLightView, sceneCorners);
            BoundingBox lightSpaceSceneAABB = BoundingBox.CreateFromPoints(sceneCorners);

            mLightProjection = Matrix.CreateOrthographicOffCenter(
                lightSpaceSceneAABB.Min.X, 
                lightSpaceSceneAABB.Max.X, 
                lightSpaceSceneAABB.Min.Y, 
                lightSpaceSceneAABB.Max.Y, 
                lightSpaceSceneAABB.Min.Z, 
                lightSpaceSceneAABB.Max.Z
                );
            //float cameraViewDistance = camera.GetFarPlaneDistance() - camera.GetNearPlaneDistance();

            //for (int iCascadeCount = 0; iCascadeCount < mCascadeCount; iCascadeCount++)
            //{
            //    Cascade cascade = mCascadeContainer[iCascadeCount];

            //    float nearFrustumSplit = (float)cascade.NearPlanePercentage / 100.0f/* * cameraViewDistance*/;
            //    float farFrustumSplit = (float)cascade.FarPlanePercentage / 100.0f/* * cameraViewDistance*/;

            //    Vector3[] cascadeFrustumCorners = new Vector3[viewFrustumCorners.Length];
            //    for (int i = 0; i < 8; i++)
            //    {
            //        if (i < 4)
            //        {
            //            cascadeFrustumCorners[i] = viewFrustumCorners[i + 4] * nearFrustumSplit;
            //        }
            //        else
            //        {
            //            cascadeFrustumCorners[i] = viewFrustumCorners[i] * farFrustumSplit;
            //        }
            //    }

                //List<Vector3> cascadeCorners = SplitViewFrustum(nearFrustumSplit, farFrustumSplit, camera.GetViewTransform(), camera.GetProjectionTransform());
                //Vector3[] lightSpaceCascadeCorners = new Vector3[cascadeCorners.Count];
                //Vector3.Transform(cascadeCorners.ToArray(), ref mLightView, lightSpaceCascadeCorners);

                //Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
                //Vector3 max = new Vector3(float.MinValue, float.MinValue, float.MinValue);
                //foreach (Vector3 corner in lightSpaceCascadeCorners)
                //{
                //    min = Vector3.Min(min, corner);
                //    max = Vector3.Max(max, corner);
                //}

                //cascade.ProjectionTransform = Matrix.CreateOrthographicOffCenter(
                //    min.X,
                //    max.X,
                //    min.Y,
                //    max.Y,
                //    -lightSpaceSceneAABB.Max.Z,
                //    -lightSpaceSceneAABB.Min.Z
                //    );

                //float cascadeFraction = 1.0f / (float)mCascadeCount;
                //float bufferBorder = 3.0f / ((float)mCascadeCount * (float)mCascadeResolution);

                //cascade.BufferBounds = new Vector4(
                //    cascadeFraction * iCascadeCount + bufferBorder, 
                //    cascadeFraction * iCascadeCount + cascadeFraction - bufferBorder,
                //    0.0f,
                //    1.0f - bufferBorder);
            //}
        }

        public void WriteShadowsToBuffer(Queue<RenderableDefinition> renderables)
        {
            // Render shadow map for each subfrusta.
            mGraphicsDevice.BlendState = BlendState.Opaque;
            mGraphicsDevice.DepthStencilState = DepthStencilState.Default;
            mGraphicsDevice.SamplerStates[1] = SamplerState.PointClamp;
            mGraphicsDevice.SamplerStates[2] = SamplerState.PointClamp;

            mGraphicsDevice.SetRenderTarget(mShadowBuffer);
            mGraphicsDevice.Clear(Color.White);

            //for (int iCascadeCount = 0; iCascadeCount < mCascadeCount; iCascadeCount++)
            //{
                //Viewport cascadeViewport = new Viewport(iCascadeCount * mCascadeResolution, 0, mCascadeResolution, mCascadeResolution);

                //mGraphicsDevice.Viewport = cascadeViewport;

                foreach (RenderableDefinition renderable in renderables)
                {
                    if (renderable.IsModel)
                    {
                        WriteModelShadowToBuffer(renderable, mLightProjection);
                    }
                    else
                    {
                        WriteTerrainShadowToBuffer(renderable, mLightProjection);
                    }
                }
            //}
        }

        #endregion

        #region Private Render Helpers

        private void WriteModelShadowToBuffer(RenderableDefinition renderable, Matrix cascadeProjection)
        {
            Model model = GraphicsManager.LookupModel(renderable.Name);

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (SkinnedEffect effect in mesh.Effects)
                {
                    effect.LightView = mLightView;
                    effect.LightProjection = cascadeProjection;

                    effect.CurrentTechnique = effect.Techniques[renderable.IsSkinned ? "SkinnedShadowCast" : "ShadowCast"];

                    effect.SetBoneTransforms(renderable.BoneTransforms);

                    effect.World = renderable.WorldTransform;
                }

                mesh.Draw();
            }
        }

        private void WriteTerrainShadowToBuffer(RenderableDefinition renderable, Matrix cascadeProjection)
        {
            TerrainHeightMap terrain = GraphicsManager.LookupTerrainHeightMap(renderable.Name);

            GraphicsManager.TerrainShader.World = renderable.WorldTransform;

            GraphicsManager.TerrainShader.LightView = mLightView;
            GraphicsManager.TerrainShader.LightProjection = cascadeProjection;

            GraphicsManager.TerrainShader.CurrentTechnique = GraphicsManager.TerrainShader.Techniques["ShadowCast"];
            GraphicsManager.TerrainShader.CurrentTechnique.Passes[0].Apply();

            mGraphicsDevice.DrawIndexedPrimitives(
                PrimitiveType.TriangleList,
                0,
                0,
                terrain.NumVertices,
                0,
                terrain.NumIndices / 3);
        }

        #endregion

        internal class Cascade
        {
            public int NearPlanePercentage { get; set; }
            public int FarPlanePercentage { get; set; }
            public Matrix ProjectionTransform { get; set; }
            public Vector4 BufferBounds { get; set; }
            public Color Color { get; set; }

            public Cascade(int nearPercentage, int farPercentage, Color color)
            {
                NearPlanePercentage = nearPercentage;
                FarPlanePercentage = farPercentage;
                ProjectionTransform = Matrix.Identity;
                BufferBounds = Vector4.Zero;
                Color = color;
            }
        }
    }
}
