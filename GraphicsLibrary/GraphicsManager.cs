using AnimationUtilities;
using GameConstructLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Core;
using DPSF;

namespace GraphicsLibrary
{
    static public class GraphicsManager
    {
        #region Rendering Variables And Properties

        static private GraphicsDevice mDevice;
        static private SpriteBatch mSpriteBatch;

        static private RenderTarget2D mSceneBuffer;
        static private RenderTarget2D mNormalDepthBuffer;
        static private RenderTarget2D mOutlineBuffer;
        static private RenderTarget2D mCompositeBuffer;

        static private Viewport? mOverrideViewport = null;

        static public GraphicsDevice Device
        { 
            get { return mDevice; }
            private set { mDevice = value; }
        }

        static public SpriteBatch SpriteBatch
        {
            get { return mSpriteBatch; }
        }

        static public Viewport? OverrideViewport
        {
            get { return mOverrideViewport; }
            set { mOverrideViewport = value; }
        }
        
        #endregion

        #region Lighting Variables And Properties

        static private Matrix mView;
        static private Matrix mProjection;
        
        static private Light mDirectionalLight;

        static private ICamera mCamera;
        static private ICamera mBirdsEyeViewCamera = null;
        
        static private BoundingFrustum mBoundingFrustum;
        static private Vector3 mMinSceneExtent = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
        static private Vector3 mMaxSceneExtent = new Vector3(float.MinValue, float.MinValue, float.MinValue);

        static private CascadedShadowMap mShadowMap;

        static public Light DirectionalLight
        {
            get { return mDirectionalLight; }
            set { mDirectionalLight = value; }
        }

        static public ICamera Camera
        {
            get { return mCamera; }
        }

        static public ICamera BirdsEyeViewCamera
        {
            get { return mBirdsEyeViewCamera; }
            set { mBirdsEyeViewCamera = value; }
        }

        static public BoundingFrustum ViewBoundingFrustum
        {
            get { return mBoundingFrustum; }
            set { mBoundingFrustum = value; }
        }

        #endregion

        #region Render Queues

        static private Queue<RendererBase> mRenderQueue = new Queue<RendererBase>();
        static private Queue<SpriteDefinition> mSpriteQueue = new Queue<SpriteDefinition>();
        static private List<RendererBase> mTransparentRenderQueue = new List<RendererBase>();

        static private bool mCanRender = false;

        #endregion

        #region Miscellaneous Visual Effect variables

        static private float mTimeElapsed = 0.0f;

        static private float mEdgeWidth = 1.5f;
        static private float mEdgeIntensity = 1.0f;
        
        #endregion

        #region Configuration Variables And Properties

        public enum CelShaded { All, Models, AnimateModels, Terrain, None };
        public enum Outlines { All, AnimateModels, None };

        static private CelShaded mCelShading = CelShaded.All;
        static private Outlines mOutlining = Outlines.AnimateModels;
        static private bool mCastingShadows = true;
        static private bool mDebugVisualization = false;
        static private bool mDrawBoundingBoxes = false;

        /// <summary>
        /// Sets the render state to Cel Shading or Phong shading.
        /// </summary>
        static public CelShaded CelShading
        {
            get { return mCelShading; }
            set { mCelShading = value; }
        }

        static public Outlines Outlining
        {
            get { return mOutlining; }
            set { mOutlining = value; }
        }

        /// <summary>
        /// Renders scene with or without shadows.
        /// </summary>
        static public bool CastingShadows
        {
            get { return mCastingShadows; }
            set { mCastingShadows = value; }
        }

        static public bool VisualizeCascades
        {
            get { return mShadowMap.VisualizeCascades; }
            set { mShadowMap.VisualizeCascades = value; }
        }

        /// <summary>
        /// Renders scene in debug mode.
        /// </summary>
        static public bool DebugVisualization
        {
            get { return mDebugVisualization; }
            set { mDebugVisualization = value; }
        }

        static public bool DrawBoundingBoxes
        {
            get { return mDrawBoundingBoxes; }
            set { mDrawBoundingBoxes = value; }
        }

        #endregion
        
        #region Public Interface

        static public void Initialize(GraphicsDevice device, SpriteBatch spriteBatch)
        {
            mDevice = device;
            mSpriteBatch = spriteBatch;

            CreateBuffers();
            CreateLightsAndShadows();
        }

        /// <summary>
        /// Updates Projection and View matrices to current view space.
        /// </summary>
        /// <param name="camera">View space camera.</param>
        static public void Update(ICamera camera, GameTime gameTime)
        {
            mTimeElapsed = (float)gameTime.TotalGameTime.TotalSeconds;

            if (mBirdsEyeViewCamera == null)
            {
                mCamera = camera;
            }
            else
            {
                mCamera = mBirdsEyeViewCamera;
            }

            mView = mCamera.GetViewTransform();
            mProjection = mCamera.GetProjectionTransform();

            mBoundingFrustum = new BoundingFrustum(camera.GetViewTransform() * camera.GetProjectionTransform());
        }

        /// <summary>
        /// All Renderables drawn between BeginRendering and FinishRendering will be drawn to the screen following FinishRendering.
        /// </summary>
        static public void BeginRendering()
        {
            foreach (RendererBase renderer in mRenderQueue)
            {
                renderer.ClearAllInstances();
            }
            mRenderQueue.Clear();

            mSpriteQueue.Clear();

            foreach (RendererBase transparentRenderer in mTransparentRenderQueue)
            {
                transparentRenderer.ClearAllInstances();
            }
            mTransparentRenderQueue.Clear();

            //mParticleQueue.Clear();
            mCanRender = true;
        }

        /// <summary>
        /// Renders to the screen all Renderables drawn between BeginRendering and FinishRendering.
        /// </summary>
        static public void FinishRendering()
        {
            if (CastingShadows)
            {
                RenderShadowBuffer();
            }

            RenderSceneBuffer();

            if (Outlining != Outlines.None)
            {
                RenderNormalDepthBuffer();
                RenderOutlines();
            }

            CompositeScene();

            if (DebugVisualization)
            {
                PresentDebugBuffers();
            }
            else
            {
                PresentScene();
            }

            RenderGUI();

            mCanRender = false;
        }

        #endregion

        #region Renderable Enqueue Methods

        static public void EnqueueRenderable(RendererBase.RendererParameters instance)
        {
            if (!mCanRender)
            {
                throw new Exception("Unable to render " + instance.Name + " before calling BeginRendering() or after FinishRendering().\n");
            }

            RendererBase renderer = AssetLibrary.LookupRenderer(instance.Name);
            if (!mRenderQueue.Contains(renderer))
            {
                mRenderQueue.Enqueue(renderer);
            }

            renderer.AddInstance(instance);

            UpdateSceneExtents(instance);
        }

        static public void EnqueueTransparentRenderable(TransparentModelRenderer.TransparentModelParameters instance)
        {
            if (!mCanRender)
            {
                throw new Exception("Unable to render transparent renderer " + instance.Name + " before calling BeginRendering() or after FinishRendering().\n");
            }

            ModelRenderer renderer = AssetLibrary.LookupModel(instance.Name);
            if (!(renderer is TransparentModelRenderer))
            {
                renderer = new TransparentModelRenderer(renderer.Model);
            }

            if (!mTransparentRenderQueue.Contains(renderer))
            {
                mTransparentRenderQueue.Add(renderer);
            }

            renderer.AddInstance(instance);

            UpdateSceneExtents(instance);
        }

        #endregion

        #region Sprite Enqueue Methods

        /// <summary>
        /// Renders sprite to the screen.  Useful for things like UI.
        /// </summary>
        /// <param name="screenSpace"></param>
        static public void EnqueueSprite(string spriteName, Rectangle screenSpace, Color blendColor, float blendColorWeight)
        {
            if (!mCanRender)
            {
                throw new Exception("Unable to render sprite " + spriteName + " before calling BeginRendering() or after FinishRendering().\n");
            }

            mSpriteQueue.Enqueue(new SpriteDefinition(spriteName, screenSpace, blendColor, blendColorWeight));
        }

        #endregion
        
        #region Initialization Methods

        /// <summary>
        /// Instantiates rendertargets written to by graphics device.
        /// </summary>
        static public void CreateBuffers()
        {
            var viewport = mDevice.Viewport;

            mCompositeBuffer = new RenderTarget2D(mDevice, viewport.Width, viewport.Height, false, SurfaceFormat.Color, DepthFormat.Depth24);
            mNormalDepthBuffer = new RenderTarget2D(mDevice, viewport.Width, viewport.Height, false, SurfaceFormat.Color, DepthFormat.Depth24);
            mOutlineBuffer = new RenderTarget2D(mDevice, viewport.Width, viewport.Height, false, SurfaceFormat.Color, DepthFormat.Depth24);
            mSceneBuffer = new RenderTarget2D(mDevice, viewport.Width, viewport.Height, false, SurfaceFormat.Color, DepthFormat.Depth24);
        }

        /// <summary>
        /// Instantiates scene light sources and shadow manager.
        /// </summary>
        static private void CreateLightsAndShadows()
        {
            mDirectionalLight = new Light(
                Vector3.Normalize(new Vector3(0.4233333f, 1.0f, -0.5533332f)), // Direction
                Vector3.Zero,                                                  // Gaze
                new Vector3(0.05333332f, 0.09882354f, 0.1819608f),             // Ambient Color
                new Vector3(1, 0.9607844f, 0.8078432f),                        // Diffuse Color
                new Vector3(1, 0.9607844f, 0.8078432f)                         // Specular Color
                );

            mShadowMap = new CascadedShadowMap(mDevice, 2048);
        }    
        
        #endregion

        #region Render Pipeline Methods

        static private void RenderShadowBuffer()
        {
            if (mCamera != null)
            {
                mShadowMap.GenerateCascades(mCamera, mDirectionalLight, new BoundingBox(mMinSceneExtent, mMaxSceneExtent));
                mShadowMap.WriteShadowsToBuffer(mRenderQueue);
            }
        }

        static private void RenderNormalDepthBuffer()
        {
            mDevice.BlendState = BlendState.Opaque;
            mDevice.DepthStencilState = DepthStencilState.Default;
            mDevice.SamplerStates[1] = SamplerState.PointClamp;

            RasterizerState cull = new RasterizerState();
            cull.CullMode = CullMode.CullCounterClockwiseFace;

            mDevice.RasterizerState = cull;

            mDevice.SetRenderTarget(mNormalDepthBuffer);
            mDevice.Clear(Color.Black);

            foreach (RendererBase renderer in mRenderQueue)
            {
                renderer.RenderAllInstancesNormalDepth(mView, mProjection);
            }
        }

        static private void RenderSceneBuffer()
        {
            mDevice.SetRenderTarget(mSceneBuffer);
            mDevice.Clear(Color.CornflowerBlue);

            RasterizerState cull = new RasterizerState();
            cull.CullMode = CullMode.CullCounterClockwiseFace;

            mDevice.RasterizerState = cull;
            mDevice.BlendState = BlendState.Opaque;
            mDevice.DepthStencilState = DepthStencilState.Default;

            foreach (RendererBase renderer in mRenderQueue)
            {
                if (CastingShadows)
                {
                    renderer.RenderAllInstancesWithShadows(mShadowMap, mView, mProjection, mDirectionalLight);
                }
                else
                {
                    renderer.RenderAllInstancesWithoutShadows(mView, mProjection, mDirectionalLight);
                }
            }

            // Set new depth buffer and alpha handling state to draw semi-transparent objects.
            RasterizerState transparentCull = new RasterizerState();
            transparentCull.CullMode = CullMode.None;

            mDevice.RasterizerState = transparentCull;
            mDevice.BlendState = BlendState.AlphaBlend;
            mDevice.DepthStencilState = DepthStencilState.DepthRead;

            //RenderableComparer rc = new RenderableComparer();

            //mTransparentRenderQueue.Sort(rc);
            foreach (TransparentModelRenderer renderer in mTransparentRenderQueue)
            {
                renderer.RenderAllInstancesNoShading(mView, mProjection);
            }
            
            // Restore depth buffer and alpha handling state.
            mDevice.RasterizerState = cull;
            mDevice.BlendState = BlendState.Opaque;
            mDevice.DepthStencilState = DepthStencilState.Default;
        }

        static private void RenderGUI()
        {
            mDevice.SetRenderTarget(null);
            foreach (SpriteDefinition sprite in mSpriteQueue)
            {
                mSpriteBatch.Begin(0, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
                mSpriteBatch.Draw(AssetLibrary.LookupSprite(sprite.Name), sprite.ScreenSpace, Color.White);
                if (sprite.BlendColorWeight > 0.0f)
                {
                    Color overlay = new Color(sprite.BlendColor.R, sprite.BlendColor.G, sprite.BlendColor.B, sprite.BlendColorWeight);
                    mSpriteBatch.Draw(AssetLibrary.LookupSprite(sprite.Name), sprite.ScreenSpace, overlay);
                }
                mSpriteBatch.End();
            }
        }

        static private void RenderOutlines()
        {
            Effect effect = AssetLibrary.PostProcessShader;

            mDevice.SetRenderTarget(mOutlineBuffer);
            mDevice.Clear(Color.White);

            effect.CurrentTechnique = effect.Techniques["EdgeDetect"];

            effect.Parameters["NormalDepthTexture"].SetValue(mNormalDepthBuffer);
            effect.Parameters["SceneTexture"].SetValue(mSceneBuffer);

            effect.Parameters["EdgeWidth"].SetValue(mEdgeWidth);
            effect.Parameters["EdgeIntensity"].SetValue(mEdgeIntensity);
            effect.Parameters["ScreenResolution"].SetValue(new Vector2(mSceneBuffer.Width, mSceneBuffer.Height));

            mSpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null, effect);
            mSpriteBatch.Draw(mSceneBuffer, new Rectangle(0, 0, mSceneBuffer.Width, mSceneBuffer.Height), Color.White);
            mSpriteBatch.End();
        }

        static private void CompositeScene()
        {
            Effect effect = AssetLibrary.PostProcessShader;

            mDevice.SetRenderTarget(mCompositeBuffer);
            mDevice.Clear(Color.CornflowerBlue);

            effect.CurrentTechnique = effect.Techniques["Composite"];

            effect.Parameters["OutlineTexture"].SetValue(mOutlineBuffer);
            effect.Parameters["SceneTexture"].SetValue(mSceneBuffer);

            mSpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null, effect);
            mSpriteBatch.Draw(mSceneBuffer, new Rectangle(0, 0, mSceneBuffer.Width, mSceneBuffer.Height), Color.White);
            mSpriteBatch.End();

            mDevice.SetRenderTarget(mOutlineBuffer);
            mDevice.Clear(Color.Black);
        }

        static private void PresentDebugBuffers()
        {
            mDevice.SetRenderTarget(null);
            mDevice.Clear(Color.CornflowerBlue);

            Viewport viewport = mOverrideViewport == null ? mDevice.Viewport : (Viewport)mOverrideViewport;

            mSpriteBatch.Begin(0, BlendState.Opaque, SamplerState.PointClamp, null, null);
            mSpriteBatch.Draw(mShadowMap.Buffer, new Rectangle(viewport.X, viewport.Y, viewport.Width / 2, viewport.Height / 2), Color.White);
            mSpriteBatch.End();

            mSpriteBatch.Begin(0, BlendState.Opaque, SamplerState.PointClamp, null, null);
            mSpriteBatch.Draw(mNormalDepthBuffer, new Rectangle(viewport.X + viewport.Width / 2, viewport.Y, viewport.Width / 2, viewport.Height / 2), Color.White);
            mSpriteBatch.End();

            mSpriteBatch.Begin(0, BlendState.Opaque, SamplerState.PointClamp, null, null);
            mSpriteBatch.Draw(mOutlineBuffer, new Rectangle(viewport.X, viewport.Y + viewport.Height / 2, viewport.Width / 2, viewport.Height / 2), Color.White);
            mSpriteBatch.End();

            mSpriteBatch.Begin(0, BlendState.Opaque, SamplerState.PointClamp, null, null);
            mSpriteBatch.Draw(mCompositeBuffer, new Rectangle(viewport.X + viewport.Width / 2, viewport.Y + viewport.Height / 2, viewport.Width / 2, viewport.Height / 2), Color.White);
            mSpriteBatch.End();
        }

        static private void PresentScene()
        {
            mDevice.SetRenderTarget(null);
            mDevice.Clear(Color.CornflowerBlue);

            Viewport viewport = mOverrideViewport == null ? mDevice.Viewport : (Viewport)mOverrideViewport;

            mSpriteBatch.Begin(0, BlendState.Opaque, SamplerState.PointClamp, null, null);
            mSpriteBatch.Draw(mCompositeBuffer, new Rectangle(viewport.X, viewport.Y, viewport.Width, viewport.Height), Color.White);
            mSpriteBatch.End();
        }
                                      
        #endregion

        #region Scene Bounding Box Helper Methods
  
        static private void UpdateSceneExtents(RendererBase.RendererParameters instance)
        {
            if (!(instance is TerrainRenderer.TerrainParameters))
            {
                mMinSceneExtent = Vector3.Min(mMinSceneExtent, instance.BoundingBox.Min);
                mMaxSceneExtent = Vector3.Max(mMaxSceneExtent, instance.BoundingBox.Max);

                return;
            }
                
            foreach (BoundingBox boundingBox in (instance as TerrainRenderer.TerrainParameters).BoundingBoxes)
            {
                mMinSceneExtent = Vector3.Min(mMinSceneExtent, boundingBox.Min);
                mMaxSceneExtent = Vector3.Max(mMaxSceneExtent, boundingBox.Max);
            }
        }

        #endregion
    }

    //class RenderableComparer : IComparer<RenderableDefinition>
    //{
    //    public int Compare(RenderableDefinition x, RenderableDefinition y)
    //    {
    //        float xDistance = (x.WorldTransform.Translation - GraphicsManager.Camera.Position).LengthSquared();
    //        float yDistance = (y.WorldTransform.Translation - GraphicsManager.Camera.Position).LengthSquared();

    //        return xDistance.CompareTo(yDistance);
    //    }
    //}
}
