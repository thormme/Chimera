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
            get { return 4; }
        }
        private int mCascadeCount = 0;

        public int CascadeResolution
        {
            get { return mCascadeResolution; }
            set { ResizeShadowBuffer(value); }
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

        public bool VisualizeCascades
        {
            get { return mVisualizeCascades; }
            set { mVisualizeCascades = value; }
        }
        private bool mVisualizeCascades = false;

        private List<Cascade> mCascadeContainer = new List<Cascade>();

        #endregion

        #region Private Shadow Buffer Methods

        private void ResizeShadowBuffer(int cascadeResolution)
        {
            int cascadeHalfCount = 2;

            if (cascadeHalfCount * cascadeResolution > MAX_BUFFER_HEIGHT || cascadeHalfCount * cascadeResolution > MAX_BUFFER_WIDTH)
            {
                string resizeErrorMessage = "Cascade Resolution is too high";
                throw new Exception(resizeErrorMessage);
            }

            if ((cascadeHalfCount * 2) != mCascadeCount)
            {
                mCascadeCount = cascadeHalfCount * 2;
                ResizeCascadeContainer();
            }
            
            mCascadeResolution = cascadeResolution;

            mShadowBuffer = new RenderTarget2D(
                mGraphicsDevice,                        // Hardware abstraction to render from.
                mCascadeCount / 2 * mCascadeResolution, // Buffer width.
                mCascadeCount / 2 * mCascadeResolution, // Buffer height.
                false,                                  // No MipMapping.
                SurfaceFormat.Single,                   // Single precision floating point values in buffer.
                DepthFormat.Depth24Stencil8,            // Precisions of accompanying depth buffer.
                0,                                      // No Multisampling (0x MSAA).
                RenderTargetUsage.DiscardContents       // Clear buffer upon binding to graphics device.
                );
        }

        private void ResizeCascadeContainer()
        {
            float[] nearPercentages = { 0.0f / 256.0f, 1.0f / 64.0f, 1.0f / 16.0f, 1.0f / 4.0f };
            float[] farPercentages  = { 1.0f / 64.0f,  1.0f / 16.0f, 1.0f / 4.0f,  1.0f / 1.0f };
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
                    lightProjections[iCascadeCount] = mCascadeContainer[iCascadeCount].ProjectionTransform * 
                                                        mCascadeContainer[iCascadeCount].TileTransform;
                }
                return lightProjections;
            }
        }

        #endregion

        #region Public Interface

        public CascadedShadowMap(GraphicsDevice graphicsDevice, int cascadeResolution)
        {
            GraphicsDevice = graphicsDevice;
            ResizeShadowBuffer(cascadeResolution);
        }

        public void GenerateCascades(ICamera camera, Light light, BoundingBox sceneAABB)
        {
            mLightView = Matrix.CreateLookAt(camera.Position, camera.Position + light.Direction, Vector3.Up);

            Vector3[] sceneCorners = sceneAABB.GetCorners();
            Vector3.Transform(sceneCorners, ref mLightView, sceneCorners);
            BoundingBox lightSpaceSceneAABB = BoundingBox.CreateFromPoints(sceneCorners);

            float maxCameraDistance = camera.GetFarPlaneDistance() - camera.GetNearPlaneDistance();
            Vector3 cameraPosition = Vector3.Transform(camera.Position, mLightView);

            for (int iCascadeCount = 0; iCascadeCount < mCascadeCount; iCascadeCount++)
            {
                Cascade cascade = mCascadeContainer[iCascadeCount];

                if (iCascadeCount == mCascadeCount - 1)
                {
                    cascade.ProjectionTransform = Matrix.CreateOrthographicOffCenter(
                        lightSpaceSceneAABB.Min.X,
                        lightSpaceSceneAABB.Max.X,
                        lightSpaceSceneAABB.Min.Y,
                        lightSpaceSceneAABB.Max.Y,
                        -lightSpaceSceneAABB.Max.Z,
                        -lightSpaceSceneAABB.Min.Z
                    );
                }
                else
                {
                    float farCascadeDistance = maxCameraDistance * cascade.FarPlanePercentage;

                    Matrix frustumProjection = Matrix.CreatePerspectiveFieldOfView(
                        camera.FieldOfView,
                        camera.AspectRatio,
                        camera.GetNearPlaneDistance(),
                        camera.GetNearPlaneDistance() + farCascadeDistance
                        );

                    Vector3[] frustumCorners = new BoundingFrustum(camera.GetViewTransform() * frustumProjection).GetCorners();
                    Vector3.Transform(frustumCorners, ref mLightView, frustumCorners);
                    BoundingBox frustumBounds = BoundingBox.CreateFromPoints(frustumCorners);

                    cascade.ProjectionTransform = Matrix.CreateOrthographicOffCenter(
                        frustumBounds.Min.X,
                        frustumBounds.Max.X,
                        frustumBounds.Min.Y,
                        frustumBounds.Max.Y,
                        -lightSpaceSceneAABB.Max.Z,
                        -lightSpaceSceneAABB.Min.Z
                    );
                }

                int tileX = iCascadeCount % 2;
                int tileY = iCascadeCount / 2;

                float bufferBorder = 3.0f / (float)mCascadeResolution;

                cascade.BufferBounds = new Vector4(
                    0.5f * tileX + bufferBorder,
                    0.5f * tileX + 0.5f - bufferBorder,
                    0.5f * tileY + bufferBorder,
                    0.5f * tileY + 0.5f - bufferBorder
                );

                Matrix tileTransform = Matrix.Identity;
                tileTransform.M11 = 0.25f;
                tileTransform.M22 = -0.25f;
                tileTransform.Translation = new Vector3(0.25f + tileX * 0.5f, 0.25f + tileY * 0.5f, 0);
                cascade.TileTransform = tileTransform;
            }
        }

        public void WriteShadowsToBuffer(Queue<RendererBase> renderers)
        {
            // Render shadow map for each subfrusta.
            mGraphicsDevice.BlendState = BlendState.Opaque;
            mGraphicsDevice.DepthStencilState = DepthStencilState.Default;
            mGraphicsDevice.SamplerStates[1] = SamplerState.PointClamp;
            mGraphicsDevice.SamplerStates[2] = SamplerState.PointClamp;

            mGraphicsDevice.SetRenderTarget(mShadowBuffer);
            mGraphicsDevice.Clear(Color.White);

            for (int iCascadeCount = 0; iCascadeCount < mCascadeCount; iCascadeCount++)
            {
                Viewport cascadeViewport = new Viewport(
                    (iCascadeCount % 2) * mCascadeResolution,
                    (iCascadeCount / 2) * mCascadeResolution,
                    mCascadeResolution, mCascadeResolution
                );

                mGraphicsDevice.Viewport = cascadeViewport;

                foreach (RendererBase renderer in renderers)
                {
                    renderer.RenderAllInstancesShadowMap(LightView, mCascadeContainer[iCascadeCount].ProjectionTransform);
                }
            }
        }

        #endregion

        internal class Cascade
        {
            public float NearPlanePercentage { get; set; }
            public float FarPlanePercentage { get; set; }
            public Matrix ProjectionTransform { get; set; }
            public Matrix TileTransform { get; set; }
            public Vector4 BufferBounds { get; set; }
            public Color Color { get; set; }

            public Cascade(float nearPercentage, float farPercentage, Color color)
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
