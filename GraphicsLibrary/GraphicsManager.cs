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

namespace GraphicsLibrary
{
    static public class GraphicsManager
    {
        ////////////////////////////// Internal State ////////////////////////////////

        #region Fields

        static private Matrix mView;
        static private Matrix mProjection;

        static private Effect mConfigurableShader;
        static private Effect mPostProcessShader;

        static public AnimationUtilities.SkinnedEffect VertexBufferShader
        {
            get { return mVertexBufferShader; }
        }
        static private AnimationUtilities.SkinnedEffect mVertexBufferShader;

        static private Dictionary<string, Model> mUniqueModelLibrary = new Dictionary<string, Model>();
        static private Dictionary<string, TerrainDescription> mUniqueTerrainLibrary = new Dictionary<string, TerrainDescription>();
        static private Dictionary<string, Dictionary<string, Matrix>> mUniqueModelBoneLibrary = new Dictionary<string, Dictionary<string, Matrix>>();
        static private Dictionary<string, Texture2D> mUniqueTextureLibrary = new Dictionary<string, Texture2D>();
        static private Dictionary<string, Dictionary<int, Texture2D>> mUniqueAnimateTextureLibrary = new Dictionary<string, Dictionary<int, Texture2D>>();

        static private GraphicsDevice mDevice;
        static private SpriteBatch mSpriteBatch;

        static private Queue<RenderableDefinition> mRenderQueue = new Queue<RenderableDefinition>();
        static private Queue<SpriteDefinition> mSpriteQueue = new Queue<SpriteDefinition>();
        static private List<RenderableDefinition> mTransparentQueue = new List<RenderableDefinition>();

        static private Light mDirectionalLight;
        static public Light DirectionalLight
        {
            get { return mDirectionalLight; }
            set { mDirectionalLight = value; }
        }

        static private CascadedShadowMap mShadowMap;

        static private RenderTarget2D mSceneBuffer;
        static private RenderTarget2D mNormalDepthBuffer;
        static private RenderTarget2D mOutlineBuffer;
        static private RenderTarget2D mCompositeBuffer;

        static private VertexBuffer mSkyBoxVertexBuffer;
        static private IndexBuffer mSkyBoxIndexBuffer;

        static private ICamera mCamera;
        static public ICamera Camera
        {
            get { return mCamera; }
        }

        static private BoundingFrustum mBoundingFrustum;

        static private Vector3 mMinSceneExtent = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
        static private Vector3 mMaxSceneExtent = new Vector3(float.MinValue, float.MinValue, float.MinValue);

        static public float BoundingBoxHypotenuse
        {
            get
            {
                return (mMaxSceneExtent - mMinSceneExtent).Length();
            }
        }

        static private bool mCanRender = false;

        static private float mTimeElapsed = 0.0f;

        static private int mEdgeWidth = 1;
        static private int mEdgeIntensity = 1;

        static private string mBASE_DIRECTORY = DirectoryManager.GetRoot() + "Chimera/ChimeraContent/";
        static private char[] mDelimeterChars = { ' ' };

        #endregion

        ///////////////////////////////// Interface //////////////////////////////////

        #region Puplic Properties

        public enum CelShaded { All, Models, AnimateModels, Terrain, None };

        static private CelShaded mCelShading = CelShaded.All;

        /// <summary>
        /// Sets the render state to Cel Shading or Phong shading.
        /// </summary>
        static public CelShaded CelShading
        {
            get { return mCelShading; }
            set { mCelShading = value; }
        }

        public enum Outlines { All, AnimateModels, None };

        static private Outlines mOutlining = Outlines.AnimateModels;
        static public Outlines Outlining
        {
            get { return mOutlining; }
            set { mOutlining = value; }
        }

        static private bool mCastingShadows = true;

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

        static public ICamera BirdsEyeViewCamera
        {
            get { return mBirdsEyeViewCamera; }
            set { mBirdsEyeViewCamera = value; }
        }
        static private ICamera mBirdsEyeViewCamera = null;

        /// <summary>
        /// Renders scene in debug mode.
        /// </summary>
        static public bool DebugVisualization
        {
            get { return mDebugVisualization; }
            set { mDebugVisualization = value; }
        }
        static private bool mDebugVisualization = false;

        static public bool DrawBoundingBoxes
        {
            get { return mDrawBoundingBoxes; }
            set { mDrawBoundingBoxes = value; }
        }
        static private bool mDrawBoundingBoxes = false;

        static public GraphicsDevice Device
        {
            get
            {
                return mDevice;
            }
            private set
            {
                mDevice = value;
            }
        }

        static public Dictionary<string, Model> ModelLibrary
        {
            get
            {
                return mUniqueModelLibrary;
            }
            private set
            {
                mUniqueModelLibrary = value;
            }
        }

        static public Dictionary<string, Texture2D> TextureLibrary
        {
            get
            {
                return mUniqueTextureLibrary;
            }
            private set
            {
                mUniqueTextureLibrary = value;
            }
        }

        public enum CursorShape { NONE, CIRCLE, BLOCK };

        static public CursorShape DrawCursor
        {
            get { return mDrawCursor; }
            set { mDrawCursor = value; }
        }
        static private CursorShape mDrawCursor;

        static public Vector3 CursorPosition
        {
            get { return mCursorPosition; }
            set { mCursorPosition = value; }
        }
        static private Vector3 mCursorPosition;

        static public float CursorInnerRadius
        {
            get { return mCursorInnerRadius; }
            set { mCursorInnerRadius = value; }
        }
        static private float mCursorInnerRadius;

        static public float CursorOuterRadius
        {
            get { return mCursorOuterRadius; }
            set { mCursorOuterRadius = value; }
        }
        static private float mCursorOuterRadius;

        #endregion

        #region Constants

        const int MAX_TEXTURE_LAYERS = 5;

        static string[] LAYER_TEXTURE_NAMES = new string[] { "Texture", "RedTexture", "GreenTexture", "BlueTexture", "AlphaTexture" };

        #endregion

        #region Public Methods

        /// <summary>
        /// Loads all models from content/models/ and shader file in to memory.
        /// </summary>
        /// <param name="content">Global game content manager.</param>
        static public void LoadContent(ContentManager content, GraphicsDevice device, SpriteBatch spriteBatch)
        {
            mDevice = device;
            mSpriteBatch = spriteBatch;

            CreateBuffers();
            CreateLightsAndShadows();
            LoadShaders(content);
            LoadModels(content);
            LoadSprites(content);
            CreateSkyBoxBuffers();
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
            mRenderQueue.Clear();
            mSpriteQueue.Clear();
            mTransparentQueue.Clear();
            mCanRender = true;
        }

        /// <summary>
        /// Renders to the screen all Renderables drawn between BeginRendering and FinishRendering.
        /// </summary>
        static public void FinishRendering()
        {
            mDevice.BlendState = BlendState.Opaque;
            mDevice.DepthStencilState = DepthStencilState.Default;
            mDevice.SamplerStates[1] = SamplerState.PointClamp;

            RasterizerState cull = new RasterizerState();
            cull.CullMode = CullMode.CullCounterClockwiseFace;
            //cull.FillMode = FillMode.WireFrame;

            mDevice.RasterizerState = cull;

            if (CastingShadows && mCamera != null)
            {
                mShadowMap.GenerateCascades(mCamera, mDirectionalLight, new BoundingBox(mMinSceneExtent, mMaxSceneExtent));
                mShadowMap.WriteShadowsToBuffer(mRenderQueue);
            }

            if (CelShading != CelShaded.None)
            {
                // Draw Normal Depth Map.
                mDevice.SetRenderTarget(mNormalDepthBuffer);
                mDevice.Clear(Color.Black);

                bool savedDrawBoundingBoxes = mDrawBoundingBoxes;
                mDrawBoundingBoxes = false;

                foreach (RenderableDefinition renderable in mRenderQueue)
                {
                    if ((mOutlining == Outlines.AnimateModels && renderable.IsSkinned || mOutlining == Outlines.All) &&
                        !renderable.IsSkyBox)
                    {
                        DrawRenderableDefinition(renderable, false, true, false);
                    }
                }

                mDrawBoundingBoxes = savedDrawBoundingBoxes;

                // Draw Scene to Texture.
                mDevice.SetRenderTarget(mSceneBuffer);
                mDevice.Clear(Color.CornflowerBlue);

                foreach (RenderableDefinition renderable in mRenderQueue)
                {
                    DrawRenderableDefinition(renderable, false, false, renderable.IsSkyBox);
                }


                // Draw semi transparent renderables to texture.
                if (mTransparentQueue.Count > 0)
                {
                    cull = new RasterizerState();
                    cull.CullMode = CullMode.None;
                    mDevice.RasterizerState = cull;

                    mDevice.BlendState = BlendState.AlphaBlend;
                    mDevice.DepthStencilState = DepthStencilState.DepthRead;

                    RenderableComparer rc = new RenderableComparer();

                    mTransparentQueue.Sort(rc);
                    for (int i = mTransparentQueue.Count - 1; i >= 0; --i)
                    {
                        DrawRenderableDefinition(mTransparentQueue[i], false, false, true);
                    }

                    cull = new RasterizerState();
                    cull.CullMode = CullMode.CullCounterClockwiseFace;
                    mDevice.RasterizerState = cull;

                    mDevice.BlendState = BlendState.Opaque;
                    mDevice.DepthStencilState = DepthStencilState.Default;
                }

                // Draw Outline to outline texture.
                mDevice.SetRenderTarget(mOutlineBuffer);
                mDevice.Clear(Color.White);

                ApplyPostProcessing();

                mDevice.SetRenderTarget(null);
                mDevice.Clear(Color.CornflowerBlue);

                if (DebugVisualization)
                {
                    mSpriteBatch.Begin(0, BlendState.Opaque, SamplerState.PointClamp, null, null);
                    mSpriteBatch.Draw(mShadowMap.Buffer, new Rectangle(0, 0, mSceneBuffer.Width / 2, mSceneBuffer.Height / 2), Color.White);
                    mSpriteBatch.End();

                    //mSpriteBatch.Begin(0, BlendState.Opaque, SamplerState.PointClamp, null, null);
                    //mSpriteBatch.Draw(mAnimateShadowMap, new Rectangle(mSceneBuffer.Width / 2, 0, mSceneBuffer.Width / 2, mSceneBuffer.Height / 2), Color.White);
                    //mSpriteBatch.End();

                    mSpriteBatch.Begin(0, BlendState.Opaque, SamplerState.PointClamp, null, null);
                    mSpriteBatch.Draw(mOutlineBuffer, new Rectangle(0, mSceneBuffer.Height / 2, mSceneBuffer.Width / 2, mSceneBuffer.Height / 2), Color.White);
                    mSpriteBatch.End();

                    mSpriteBatch.Begin(0, BlendState.Opaque, SamplerState.PointClamp, null, null);
                    mSpriteBatch.Draw(mCompositeBuffer, new Rectangle(mSceneBuffer.Width / 2, mSceneBuffer.Height / 2, mSceneBuffer.Width / 2, mSceneBuffer.Height / 2), Color.White);
                    mSpriteBatch.End();
                }
                else
                {
                    mSpriteBatch.Begin(0, BlendState.Opaque, SamplerState.PointClamp, null, null);
                    mSpriteBatch.Draw(mCompositeBuffer, new Rectangle(0, 0, mSceneBuffer.Width, mSceneBuffer.Height), Color.White);
                    mSpriteBatch.End();
                }
            }
            else
            {
                // Draw Scene to Screen with no post-processing.
                mDevice.SetRenderTarget(null);
                mDevice.Clear(Color.CornflowerBlue);

                foreach (RenderableDefinition renderable in mRenderQueue)
                {
                    DrawRenderableDefinition(renderable, false, false, false);
                }
            }

            mDevice.SetRenderTarget(null);
            foreach (SpriteDefinition sprite in mSpriteQueue)
            {
                mSpriteBatch.Begin(0, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
                mSpriteBatch.Draw(LookupSprite(sprite.Name), sprite.ScreenSpace, Color.White);
                if (sprite.BlendColorWeight > 0.0f)
                {
                    Color overlay = new Color(sprite.BlendColor.R, sprite.BlendColor.G, sprite.BlendColor.B, sprite.BlendColorWeight);
                    mSpriteBatch.Draw(LookupSprite(sprite.Name), sprite.ScreenSpace, overlay);
                }
                mSpriteBatch.End();
            }

            mCanRender = false;
        }

        /// <summary>
        /// Looks up model associated with modelName and returns the skinningData for that model.
        /// </summary>
        /// <param name="modelName">Name of model stored in database</param>
        /// <returns>Bone transforms associated with modelName</returns>
        static public SkinningData LookupModelSkinningData(string modelName)
        {
            Model model = LookupModel(modelName);
            return model.Tag as SkinningData;
        }

        /// <summary>
        /// Retrieves terrain height map from database.  Throws KeyNotFoundException if terrainName does not exist in database.
        /// </summary>
        static public TerrainHeightMap LookupTerrainHeightMap(string terrainName)
        {
            return LookupTerrain(terrainName).Terrain;
        }

        static public TerrainTexture LookupTerrainTexture(string terrainName)
        {
            return LookupTerrain(terrainName).Texture;
        }

        /// <summary>
        /// Retrieves Tweaked orientation transform matrix for specific bone on an AnimateModel.
        /// </summary>
        static public Matrix LookupTweakedBoneOrientation(string modelName, string boneName)
        {
            Dictionary<string, Matrix> boneTweakLibrary;
            if (!mUniqueModelBoneLibrary.TryGetValue(modelName, out boneTweakLibrary))
            {
                throw new Exception(modelName + " does not contain any bone orientation tweaks");
            }

            Matrix localBoneOrientation;
            if (!boneTweakLibrary.TryGetValue(boneName, out localBoneOrientation))
            {
                throw new Exception(modelName + " does not contain bone: " + boneName);
            }

            return localBoneOrientation;
        }

        /// <summary>
        /// Renders animated model.
        /// </summary>
        /// <param name="modelName">Name of model stored in database.</param>
        /// <param name="boneTransforms">State of rigged skeleton for current frame</param>
        /// <param name="worldTransforms">Position, orientation, and scale of model in world space.</param>
        static public void RenderSkinnedModel(string modelName, Matrix[] boneTransforms, Matrix worldTransforms, BoundingBox boundingBox, Color overlayColor, float overlayColorWeight)
        {
            if (mCanRender)
            {
                RenderableDefinition skinnedModel = new RenderableDefinition(modelName, worldTransforms, overlayColor, overlayColorWeight);
                skinnedModel.IsModel = true;
                skinnedModel.IsSkinned = true;
                skinnedModel.BoneTransforms = boneTransforms;
                skinnedModel.BoundingBoxes[0, 0] = boundingBox;

                mRenderQueue.Enqueue(skinnedModel);
                UpdateSceneExtents(boundingBox, worldTransforms);
            }
            else
            {
                throw new Exception("Unable to render animate model " + modelName + " before calling BeginRendering() or after FinishRendering().\n");
            }
        }

        /// <summary>
        /// Renders inanimate model.
        /// </summary>
        /// <param name="modelName">Name of model stored in database.</param>
        /// <param name="worldTransforms">Position, orientation, and scale of model in world space.</param>
        static public void RenderUnskinnedModel(string modelName, Matrix worldTransforms, BoundingBox boundingBox, Color overlayColor, float overlayColorWeight)
        {
            if (mCanRender)
            {
                RenderableDefinition unskinnedModel = new RenderableDefinition(modelName, worldTransforms, overlayColor, overlayColorWeight);
                unskinnedModel.IsModel = true;
                unskinnedModel.BoundingBoxes[0, 0] = boundingBox;

                mRenderQueue.Enqueue(unskinnedModel);

                UpdateSceneExtents(boundingBox, worldTransforms);
            }
            else
            {
                throw new Exception("Unable to render inanimate model " + modelName + " before calling BeginRendering() or after FinishRendering().\n");
            }
        }

        /// <summary>
        /// Renders semi-transparent model.
        /// </summary>
        /// /// <param name="modelName">Name of model stored in database.</param>
        /// <param name="worldTransforms">Position, orientation, and scale of model in world space.</param>
        static public void RenderTransparentModel(string modelName, Matrix worldTransforms, BoundingBox boundingBox, Color overlayColor, float overlayColorWeight, Vector2 animationRate)
        {
            if (mCanRender)
            {
                RenderableDefinition transparentModel = new RenderableDefinition(modelName, worldTransforms, overlayColor, overlayColorWeight);
                transparentModel.IsModel = true;
                transparentModel.AnimationRate = animationRate;
                transparentModel.BoundingBoxes[0, 0] = boundingBox;

                mTransparentQueue.Add(transparentModel);
            }
            else
            {
                throw new Exception("Unable to render transparent model " + modelName + " before calling BeginRendering() or after FinishRendering().\n");
            }
        }

        static public void RenderSkyBox(string textureName, Matrix worldTransform, Color overlayColor, float overlayWeight)
        {
            if (mCanRender)
            {
                RenderableDefinition skybox = new RenderableDefinition(null, worldTransform, overlayColor, overlayWeight);
                skybox.IsSkyBox = true;
                skybox.OverlayTextureName = textureName;
                skybox.WorldTransform = Matrix.CreateScale(1600, 1600, 1600) * skybox.WorldTransform * Matrix.CreateTranslation(new Vector3(0, -700, 0));

                mRenderQueue.Enqueue(skybox);
            }
            else
            {
                throw new Exception("Unable to render skydome before calling BeginRendering() or after FinishRendering().\n");
            }
        }

        /// <summary>
        /// Renders terrain.
        /// </summary>
        /// <param name="terrainName">Name of terrain stored in database.</param>
        /// <param name="worldTransforms">Position, orientation, and scale of terrain in world space.</param>
        static public void RenderTerrain(string terrainName, Matrix worldTransforms, BoundingBox[,] boundingBoxes, Color overlayColor, float overlayColorWeight)
        {
            if (mCanRender)
            {
                RenderableDefinition terrain = new RenderableDefinition(terrainName, worldTransforms, overlayColor, overlayColorWeight);
                terrain.BoundingBoxes = boundingBoxes;

                mRenderQueue.Enqueue(terrain);
                foreach (BoundingBox boundingBox in boundingBoxes)
                {
                    UpdateSceneExtents(boundingBox, worldTransforms);
                }
            }
            else
            {
                throw new Exception("Unable to render terrain " + terrainName + " before calling BeginRendering() or after FinishRendering().\n");
            }
        }

        /// <summary>
        /// Renders sprite to the screen.  Useful for things like UI.
        /// </summary>
        /// <param name="screenSpace"></param>
        static public void RenderSprite(string spriteName, Rectangle screenSpace, Color blendColor, float blendColorWeight)
        {
            if (mCanRender)
            {
                mSpriteQueue.Enqueue(new SpriteDefinition(spriteName, screenSpace, blendColor, blendColorWeight));
            }
            else
            {
                throw new Exception("Unable to render sprite " + spriteName + " before calling BeginRendering() or after FinishRendering().\n");
            }
        }

        public static BoundingBox BuildModelBoundingBox(string modelName)
        {
            Vector3 minExtent = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            Vector3 maxExtent = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            foreach (ModelMesh mesh in LookupModel(modelName).Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    BoundVertexBuffer(ref minExtent, ref maxExtent, part.VertexBuffer, part.NumVertices);
                }
            }

            return new BoundingBox(minExtent, maxExtent);
        }

        public static BoundingBox[,] BuildTerrainBoundingBox(string terrainName)
        {
            TerrainHeightMap terrain = LookupTerrain(terrainName).Terrain;

            BoundingBox[,] terrainBoundingBoxes = new BoundingBox[terrain.NumChunksVertical, terrain.NumChunksHorizontal];

            for (int row = 0; row < terrain.NumChunksVertical; ++row)
            {
                for (int col = 0; col < terrain.NumChunksHorizontal; ++col)
                {
                    Vector3 minExtent = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
                    Vector3 maxExtent = new Vector3(float.MinValue, float.MinValue, float.MinValue);

                    BoundVertexBuffer(ref minExtent, ref maxExtent, terrain.VertexBuffers[row, col], terrain.VertexBuffers[row, col].VertexCount);

                    terrainBoundingBoxes[row, col] = new BoundingBox(minExtent, maxExtent);
                }
            }

            return terrainBoundingBoxes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="level"></param>
        public static void AddTerrain(FileInfo level, TerrainHeightMap heightMap, TerrainTexture texture)
        {
            if (mUniqueTerrainLibrary.ContainsKey(level.Name))
            {
                mUniqueTerrainLibrary.Remove(level.Name);
            }

            mUniqueTerrainLibrary.Add(level.Name, new TerrainDescription(heightMap, texture));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="savePath"></param>
        /// <param name="levelName"></param>
        public static void UpdateTerrain(FileInfo savePath, ref string levelName)
        {
            if (!mUniqueTerrainLibrary.ContainsKey(savePath.Name))
            {
                mUniqueTerrainLibrary.Add(savePath.Name, mUniqueTerrainLibrary[levelName]);
                levelName = savePath.Name;
            }
        }

        #endregion

        ///////////////////////////// Internal functions /////////////////////////////

        #region Helper Methods

        /// <summary>
        /// Instantiates rendertargets written to by graphics device.
        /// </summary>
        static private void CreateBuffers()
        {
            var pp = mDevice.PresentationParameters;

            mCompositeBuffer          = new RenderTarget2D(mDevice, pp.BackBufferWidth, pp.BackBufferHeight, false, SurfaceFormat.Color, DepthFormat.Depth24);
            mNormalDepthBuffer        = new RenderTarget2D(mDevice, pp.BackBufferWidth, pp.BackBufferHeight, false, SurfaceFormat.Color, DepthFormat.Depth24);
            mOutlineBuffer            = new RenderTarget2D(mDevice, pp.BackBufferWidth, pp.BackBufferHeight, false, SurfaceFormat.Color, DepthFormat.Depth24);
            mSceneBuffer              = new RenderTarget2D(mDevice, pp.BackBufferWidth, pp.BackBufferHeight, false, SurfaceFormat.Color, DepthFormat.Depth24);
        }

        /// <summary>
        /// Instantiates scene light sources and shadow manager.
        /// </summary>
        static private void CreateLightsAndShadows()
        {
            mDirectionalLight = new Light(
                new Vector3(-0.3333333f, -1.0f, 0.33333333f),      // Direction
                new Vector3(0.05333332f, 0.09882354f, 0.1819608f), // Ambient Color
                new Vector3(1, 0.9607844f, 0.8078432f),            // Diffuse Color
                new Vector3(1, 0.9607844f, 0.8078432f)             // Specular Color
                );

            mShadowMap = new CascadedShadowMap(mDevice, 2048);
        }

        static private void CreateSkyBoxBuffers()
        {
            const int bottomLength = 2, rightLength = 2, numSides = 6;
            const float uLength = 1.0f / 4.0f, vLength = 1.0f / 3.0f;

            float[]   sideUOrigin = { uLength,                        uLength,                         2 * uLength,                    3 * uLength,                    0.0f,                           uLength };
            float[]   sideVOrigin = { 0.0f,                           vLength,                         vLength,                        vLength,                        vLength,                        2 * vLength };
            Vector3[] sideTopLeft = { new Vector3(-0.5f, 1.0f, 0.5f), new Vector3(-0.5f, 1.0f, -0.5f), new Vector3(0.5f, 1.0f, -0.5f), new Vector3(0.5f, 1.0f, 0.5f),  new Vector3(-0.5f, 1.0f, 0.5f), new Vector3(-0.5f, 0.0f, -0.5f) };
            Vector3[] sideRight   = { new Vector3(1.0f, 0.0f, 0.0f),  new Vector3(1.0f, 0.0f, 0.0f),   new Vector3(0.0f, 0.0f, 1.0f),  new Vector3(-1.0f, 0.0f, 0.0f), new Vector3(0.0f, 0.0f, -1.0f), new Vector3(1.0f, 0.0f, 0.0f) };
            Vector3[] sideBottom  = { new Vector3(0.0f, 0.0f, -1.0f), new Vector3(0.0f, -1.0f, 0.0f),  new Vector3(0.0f, -1.0f, 0.0f), new Vector3(0.0f, -1.0f, 0.0f), new Vector3(0.0f, -1.0f, 0.0f), new Vector3(0.0f, 0.0f, 1.0f) };
            Vector3[] sideNormal = { new Vector3(0, -1, 0), new Vector3(0, 0, 1), new Vector3(-1, 0, 0), new Vector3(0, 0, -1), new Vector3(1, 0, 0), new Vector3(0, 1, 0) };

            VertexPositionNormalTexture[] skyBoxVertices = new VertexPositionNormalTexture[numSides * bottomLength * rightLength];

            int sideIndex;
            for (sideIndex = 0; sideIndex < 6; ++sideIndex)
            {
                for (int bottom = 0; bottom <= 1; ++bottom)
                {
                    for (int right = 0; right <= 1; ++right)
                    {
                        VertexPositionNormalTexture vertex = new VertexPositionNormalTexture();
                        vertex.Position = sideTopLeft[sideIndex] + bottom * sideBottom[sideIndex] + right * sideRight[sideIndex];
                        vertex.Normal = sideNormal[sideIndex];
                        vertex.TextureCoordinate = new Vector2(sideUOrigin[sideIndex] + right * uLength, sideVOrigin[sideIndex] + bottom * vLength);

                        skyBoxVertices[sideIndex * bottomLength * rightLength + bottom * bottomLength + right] = vertex;
                    }
                }
            }

            mSkyBoxVertexBuffer = new VertexBuffer(mDevice, VertexPositionNormalTexture.VertexDeclaration, skyBoxVertices.Length, BufferUsage.WriteOnly);
            mSkyBoxVertexBuffer.SetData(skyBoxVertices);

            const int numQuadVertices = 6;
            int[] skyBoxIndices = new int[numSides * numQuadVertices];

            int count = 0;
            for (sideIndex = 0; sideIndex < numSides; ++sideIndex)
            {
                int topLeftIndex = sideIndex * bottomLength * rightLength;
                int topRightIndex = topLeftIndex + 1;
                int bottomLeftIndex = topRightIndex + 1;
                int bottomRightIndex = bottomLeftIndex + 1;

                skyBoxIndices[count++] = topLeftIndex;
                skyBoxIndices[count++] = topRightIndex;
                skyBoxIndices[count++] = bottomRightIndex;

                skyBoxIndices[count++] = topLeftIndex;
                skyBoxIndices[count++] = bottomRightIndex;
                skyBoxIndices[count++] = bottomLeftIndex;
            }

            mSkyBoxIndexBuffer = new IndexBuffer(mDevice, typeof(int), skyBoxIndices.Length, BufferUsage.WriteOnly);
            mSkyBoxIndexBuffer.SetData(skyBoxIndices);
        }

        /// <summary>
        /// Reads all models in from content directory and stores them in modelLibrary.
        /// </summary>
        static private void LoadModels(ContentManager content)
        {
            DirectoryInfo dir = new DirectoryInfo(content.RootDirectory + "\\" + "models");
            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException("Could not find models directory \n" + dir.FullName + "\nin content.");
            }

            DirectoryInfo[] subDirs = dir.GetDirectories();
            foreach (DirectoryInfo subDir in subDirs)
            {
                string subDirName = subDir.Name;

                FileInfo[] files = subDir.GetFiles("*.xnb");
                foreach (FileInfo file in files)
                {
                    try
                    {
                        string textureKey = "_animate_texture_";
                        string modelName = Path.GetFileNameWithoutExtension(file.Name);
                        if (modelName.Contains(textureKey))
                        {
                            Texture2D texture = content.Load<Texture2D>("models/" + subDirName + "/" + modelName);

                            string[] nameParts = modelName.Split(new char[] { '_' });

                            Dictionary<int, Texture2D> animateTextures;
                            if (!mUniqueAnimateTextureLibrary.TryGetValue(nameParts[0], out animateTextures))
                            {
                                animateTextures = new Dictionary<int, Texture2D>();
                                mUniqueAnimateTextureLibrary.Add(nameParts[0], animateTextures);
                            }

                            animateTextures.Add(Int32.Parse(nameParts[3]), texture);
                        }
                        else
                        {
                            Model input = content.Load<Model>("models/" + subDirName + "/" + modelName);

                            foreach (ModelMesh mesh in input.Meshes)
                            {
                                foreach (ModelMeshPart part in mesh.MeshParts)
                                {
                                    Microsoft.Xna.Framework.Graphics.SkinnedEffect skinnedEffect = part.Effect as Microsoft.Xna.Framework.Graphics.SkinnedEffect;
                                    if (skinnedEffect != null)
                                    {
                                        AnimationUtilities.SkinnedEffect newEffect = new AnimationUtilities.SkinnedEffect(mConfigurableShader);
                                        newEffect.CopyFromSkinnedEffect(skinnedEffect);

                                        part.Effect = newEffect;
                                    }
                                    else
                                    {
                                        Microsoft.Xna.Framework.Graphics.BasicEffect basicEffect = part.Effect as Microsoft.Xna.Framework.Graphics.BasicEffect;
                                        if (basicEffect != null)
                                        {
                                            AnimationUtilities.SkinnedEffect newEffect = new AnimationUtilities.SkinnedEffect(mConfigurableShader);
                                            newEffect.CopyFromBasicEffect(basicEffect);

                                            part.Effect = newEffect;
                                        }
                                    }
                                }
                            }

                            AddModel(modelName, input);
                        }
                    }
                    catch { }
                }

                // Parse Bone Orientation Tweak File and store in library.
                FileInfo[] tweakOrientationFiles = subDir.GetFiles("*.tweak");
                foreach (FileInfo file in tweakOrientationFiles)
                {
                    Dictionary<string, Matrix> tweakLibrary = new Dictionary<string, Matrix>();
                    TextReader tr = new StreamReader(file.DirectoryName + "\\" + file.Name);

                    int count = int.Parse(tr.ReadLine());
                    string blank = tr.ReadLine();

                    for (int i = 0; i < count; ++i)
                    {
                        Matrix tweakMatrix;
                        string boneName;
                        ParseTweakFile(tr, out boneName, out tweakMatrix);

                        tweakLibrary.Add(boneName, tweakMatrix);
                    }
                    tr.Close();

                    mUniqueModelBoneLibrary.Add(Path.GetFileNameWithoutExtension(file.Name), tweakLibrary);
                }

                FileInfo[] tweakModifierFiles = subDir.GetFiles("*.modtweak");
                foreach (FileInfo file in tweakModifierFiles)
                {
                    TextReader tr = new StreamReader(file.DirectoryName + "\\" + file.Name);
                    int count = int.Parse(tr.ReadLine());
                    string blank = tr.ReadLine();

                    Dictionary<string, Matrix> tweakLibrary;
                    if (mUniqueModelBoneLibrary.TryGetValue(Path.GetFileNameWithoutExtension(file.Name), out tweakLibrary))
                    {
                        TextWriter tw = new StreamWriter("playerBeanPartOrientations.txt");

                        tw.WriteLine(count);
                        tw.WriteLine("");

                        for (int i = 0; i < count; ++i)
                        {
                            Matrix modifyMatrix;
                            string boneName;
                            ParseTweakFile(tr, out boneName, out modifyMatrix);

                            Matrix storedMatrix;
                            if (tweakLibrary.TryGetValue(boneName, out storedMatrix))
                            {
                                storedMatrix = modifyMatrix * storedMatrix;
                                tweakLibrary[boneName] = storedMatrix;
                            }

                            tw.WriteLine(boneName);
                            tw.WriteLine(storedMatrix.M11.ToString() + " " + storedMatrix.M12.ToString() + " " + storedMatrix.M13.ToString() + " " + storedMatrix.M14.ToString());
                            tw.WriteLine(storedMatrix.M21.ToString() + " " + storedMatrix.M22.ToString() + " " + storedMatrix.M23.ToString() + " " + storedMatrix.M24.ToString());
                            tw.WriteLine(storedMatrix.M31.ToString() + " " + storedMatrix.M32.ToString() + " " + storedMatrix.M33.ToString() + " " + storedMatrix.M34.ToString());
                            tw.WriteLine(storedMatrix.M41.ToString() + " " + storedMatrix.M42.ToString() + " " + storedMatrix.M43.ToString() + " " + storedMatrix.M44.ToString());
                            tw.WriteLine("");
                        }
                        tw.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Sets up pipeline for executing shaders.
        /// </summary>
        static private void LoadShaders(ContentManager content)
        {
            mConfigurableShader = content.Load<Effect>("shaders/ConfigurableShader");
            mPostProcessShader = content.Load<Effect>("shaders/PostProcessing");
            mVertexBufferShader = new AnimationUtilities.SkinnedEffect(mConfigurableShader);
        }

        /// <summary>
        /// Reads sprites in from content directory and stores them in spriteLibrary.
        /// </summary>
        static private void LoadSprites(ContentManager content)
        {
            DirectoryInfo dir = new DirectoryInfo(content.RootDirectory + "\\" + "textures/maps/");
            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException("Could not find textures/maps/ directory in content.");
            }

            FileInfo[] mapFiles = dir.GetFiles("*");
            foreach (FileInfo file in mapFiles)
            {
                string mapName = Path.GetFileNameWithoutExtension(file.Name);
                Texture2D map = content.Load<Texture2D>("textures/maps/" + mapName);

                if (mUniqueTextureLibrary.ContainsKey(mapName))
                {
                    throw new Exception("Duplicate map key: " + mapName);
                }
                mUniqueTextureLibrary.Add(mapName, map);
            }

            dir = new DirectoryInfo(content.RootDirectory + "\\" + "textures/sprites/");
            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException("Could not find textures/sprites/ directory in content.");
            }

            FileInfo[] spriteFiles = dir.GetFiles("*");
            foreach (FileInfo file in spriteFiles)
            {
                string spriteName = Path.GetFileNameWithoutExtension(file.Name);
                Texture2D sprite = content.Load<Texture2D>("textures/sprites/" + spriteName);

                if (mUniqueTextureLibrary.ContainsKey(spriteName))
                {
                    throw new Exception("Duplicate sprite key: " + spriteName);
                }
                mUniqueTextureLibrary.Add(spriteName, sprite);
            }
        }

        /// <summary>
        /// Inserts new model in to model container.  Throws Exception on duplicate key.
        /// </summary>
        static private void AddModel(string modelName, Model model)
        {
            if (mUniqueModelLibrary.ContainsKey(modelName))
            {
                throw new Exception("Duplicate model key: " + modelName);
            }
            mUniqueModelLibrary.Add(modelName, model);
        }

        /// <summary>
        /// Retrieves model from database.  Throws KeyNotFoundException if modelName does not exist in database.
        /// </summary>
        static public Model LookupModel(string modelName)
        {
            Model result;
            if (mUniqueModelLibrary.TryGetValue(modelName, out result))
            {
                return result;
            }
            throw new KeyNotFoundException("Unable to find model key: " + modelName);
        }

        /// <summary>
        /// Retrieves terrain from database.  Throws KeyNotFoundException if terrainName does not exist in database.
        /// </summary>
        static private TerrainDescription LookupTerrain(string terrainName)
        {
            TerrainDescription result;
            if (mUniqueTerrainLibrary.TryGetValue(terrainName, out result))
            {
                return result;
            }
            throw new KeyNotFoundException("Unable to find terrain key: " + terrainName);
        }

        /// <summary>
        /// Retrieves sprite from database.  Throws KeyNotFoundException if spriteName does not exist in database.
        /// </summary>
        /// <param name="spriteName"></param>
        /// <returns></returns>
        static public Texture2D LookupSprite(string spriteName)
        {
            Texture2D result;
            if (mUniqueTextureLibrary.TryGetValue(spriteName, out result))
            {
                return result;
            }
            throw new KeyNotFoundException("Unable to find sprite key: " + spriteName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="renderable"></param>
        static private void DrawRenderableDefinition(RenderableDefinition renderable, bool isShadow, bool isOutline, bool hasTransparency)
        {
            if (renderable.IsModel)
            {
                DrawModel(renderable, isShadow, isOutline, hasTransparency);
                return;
            }

            if (renderable.IsSkyBox)
            {
                DrawSkyBox(renderable.OverlayTextureName, renderable.WorldTransform);
                return;
            }

            DrawTerrain(renderable, isShadow, isOutline);
        }

        /// <summary>
        /// Draws all meshes within model to current rendertarget.  Applies rigged model transforms if necesarry.
        /// </summary>
        static private void DrawModel(RenderableDefinition renderable, bool isShadow, bool isOutline, bool hasTransparency)
        {
            if (!isShadow && mBoundingFrustum.Contains(renderable.BoundingBoxes[0, 0]) == ContainmentType.Disjoint)
            {
                return;
            }

            Model model = LookupModel(renderable.Name);

            foreach (ModelMesh mesh in model.Meshes)
            {
                string techniqueName;
                if (isOutline)
                {
                    techniqueName = (renderable.IsSkinned) ? "SkinnedNormalDepthShade" : "NormalDepthShade";
                }
                else if (CelShading == CelShaded.Models || CelShading == CelShaded.All)
                {
                    techniqueName = (renderable.IsSkinned) ? "SkinnedCelShade" : (hasTransparency) ? "NoShade" : "CelShade";
                }
                else if (CelShading == CelShaded.AnimateModels)
                {
                    techniqueName = (renderable.IsSkinned) ? "SkinnedCelShade" : (hasTransparency) ? "NoShade" : "Phong";
                }
                else
                {
                    techniqueName = (renderable.IsSkinned) ? "SkinnedPhong" : (hasTransparency) ? "NoShade" : "Phong";
                }

                DrawMesh(mesh, techniqueName, renderable);
            }
        }

        /// <summary>
        /// Draws current Terrain to render target using appropriate effects.
        /// </summary>
        static private void DrawTerrain(RenderableDefinition renderable, bool isShadow, bool isOutline)
        {
            TerrainDescription heightmap = LookupTerrain(renderable.Name);

            string techniqueName = (isOutline) ? "NormalDepthShade" : ((CelShading == CelShaded.Terrain || CelShading == CelShaded.All) ? "TerrainCelShade" : "TerrainPhong");
            DrawTerrainHeightMap(heightmap, techniqueName, renderable);
        }

        /// <summary>
        /// Sets all effects for current mesh and renders to screen.
        /// </summary>
        static private void DrawMesh(ModelMesh mesh, string techniqueName, RenderableDefinition renderable)
        {
            mDevice.SamplerStates[0] = SamplerState.PointClamp;

            foreach (AnimationUtilities.SkinnedEffect effect in mesh.Effects)
            {
                effect.CurrentTechnique = effect.Techniques[techniqueName];

                if (CastingShadows)
                {
                    effect.Parameters["ShadowMap"].SetValue(mShadowMap.Buffer);
                }

                if (renderable.BoneTransforms != null)
                {
                    effect.SetBoneTransforms(renderable.BoneTransforms);
                }

                if (renderable.OverlayTextureName != null)
                {
                    effect.Texture = LookupSprite(renderable.OverlayTextureName);
                }

                effect.View = mView;
                effect.Projection = mProjection;

                effect.LightView = mShadowMap.LightView;
                effect.LightProjection = mShadowMap.LightProjection;

                effect.Parameters["xVisualizeCascades"].SetValue(mShadowMap.VisualizeCascades);
                effect.Parameters["xCascadeCount"].SetValue(mShadowMap.CascadeCount);
                effect.Parameters["xLightView"].SetValue(mShadowMap.LightView);
                effect.Parameters["xLightProjections"].SetValue(mShadowMap.LightProjections);
                effect.Parameters["xCascadeBufferBounds"].SetValue(mShadowMap.CascadeBounds);
                effect.Parameters["xCascadeColors"].SetValue(mShadowMap.CascadeColors);

                effect.Parameters["xDirLightDirection"].SetValue(mDirectionalLight.Direction);
                effect.Parameters["xDirLightDiffuseColor"].SetValue(mDirectionalLight.DiffuseColor);
                effect.Parameters["xDirLightSpecularColor"].SetValue(mDirectionalLight.SpecularColor);
                effect.Parameters["xDirLightAmbientColor"].SetValue(mDirectionalLight.AmbientColor);

                effect.Parameters["xOverlayColor"].SetValue(renderable.OverlayColor);
                effect.Parameters["xOverlayColorWeight"].SetValue(renderable.OverlayColorWeight);

                effect.Parameters["xNumShadowBands"].SetValue(techniqueName.Contains("Skinned") ? 2.0f : 3.0f);

                effect.SpecularColor = new Vector3(0.25f);
                effect.SpecularPower = 16;

                effect.World = renderable.WorldTransform;

                Vector2 animationRate = renderable.AnimationRate;
                if (renderable.AnimationRate != Vector2.Zero)
                {
                    mTimeElapsed %= 1.0f;
                    animationRate *= mTimeElapsed;
                    animationRate.X %= 1.0f;
                    animationRate.Y %= 1.0f;
                }
                effect.Parameters["xTextureOffset"].SetValue(animationRate);
            }
            mesh.Draw();

            if (mDrawBoundingBoxes)
            {
                Color bBoxColor;
                if (renderable.IsSkinned)
                {
                    bBoxColor = Color.Yellow;
                }
                else if (techniqueName == "NoShade")
                {
                    bBoxColor = Color.Green;
                }
                else
                {
                    bBoxColor = Color.Red;
                }

                BoundingBoxRenderer.Render(renderable.BoundingBoxes[0, 0], mDevice, mView, mProjection, bBoxColor);
            }
        }

        /// <summary>
        /// Sets effect for current terrain and renders to screen.
        /// </summary>
        static private void DrawTerrainHeightMap(TerrainDescription heightMap, string techniqueName, RenderableDefinition renderable)
        {
            mDevice.SamplerStates[0] = SamplerState.LinearClamp;

            if (CastingShadows)
            {
                mVertexBufferShader.Parameters["ShadowMap"].SetValue(mShadowMap.Buffer);
            }

            mVertexBufferShader.Parameters["xDrawCursor"].SetValue((int)mDrawCursor);

            if (mDrawCursor != CursorShape.NONE)
            {
                mVertexBufferShader.Parameters["xCursorPosition"].SetValue(mCursorPosition);
                mVertexBufferShader.Parameters["xCursorInnerRadius"].SetValue(mCursorInnerRadius);
                mVertexBufferShader.Parameters["xCursorOuterRadius"].SetValue(mCursorOuterRadius);
            }

            mVertexBufferShader.Parameters["xVisualizeCascades"].SetValue(mShadowMap.VisualizeCascades);
            mVertexBufferShader.Parameters["xCascadeCount"].SetValue(mShadowMap.CascadeCount);
            mVertexBufferShader.Parameters["xLightView"].SetValue(mShadowMap.LightView);
            mVertexBufferShader.Parameters["xLightProjections"].SetValue(mShadowMap.LightProjections);
            mVertexBufferShader.Parameters["xCascadeBufferBounds"].SetValue(mShadowMap.CascadeBounds);
            mVertexBufferShader.Parameters["xCascadeColors"].SetValue(mShadowMap.CascadeColors);

            mVertexBufferShader.View = mView;
            mVertexBufferShader.Projection = mProjection;

            VertexBufferShader.LightView = mShadowMap.LightView;
            VertexBufferShader.LightProjection = mShadowMap.LightProjection;

            mVertexBufferShader.Parameters["xDirLightDirection"].SetValue(mDirectionalLight.Direction);
            mVertexBufferShader.Parameters["xDirLightDiffuseColor"].SetValue(mDirectionalLight.DiffuseColor);
            mVertexBufferShader.Parameters["xDirLightSpecularColor"].SetValue(mDirectionalLight.SpecularColor);
            mVertexBufferShader.Parameters["xDirLightAmbientColor"].SetValue(mDirectionalLight.AmbientColor);

            mVertexBufferShader.Parameters["xOverlayColor"].SetValue(renderable.OverlayColor);
            mVertexBufferShader.Parameters["xOverlayColorWeight"].SetValue(renderable.OverlayColorWeight);

            mVertexBufferShader.SpecularColor = new Vector3(0.25f);
            mVertexBufferShader.SpecularPower = 16;

            mVertexBufferShader.World = renderable.WorldTransform;

            mVertexBufferShader.CurrentTechnique = mVertexBufferShader.Techniques[techniqueName];

            for (int chunkCol = 0; chunkCol < heightMap.Terrain.NumChunksHorizontal; chunkCol++)
            {
                for (int chunkRow = 0; chunkRow < heightMap.Terrain.NumChunksVertical; chunkRow++)
                {
                    if (mBoundingFrustum.Contains(renderable.BoundingBoxes[chunkRow, chunkCol]) == ContainmentType.Disjoint)
                    {
                        // Hey, it looks like you.
                        // Just tried to draw me.
                        // But I'm not on screen.
                        // So cull me, maybe?
                        continue;
                    }

                    mVertexBufferShader.Parameters["AlphaMap"].SetValue(heightMap.Texture.TextureBuffers[chunkRow, chunkCol]);

                    for (int i = 0; i < MAX_TEXTURE_LAYERS; ++i)
                    {
                        string detailTextureName = heightMap.Texture.DetailTextureNames[chunkRow, chunkCol, i];

                        if (detailTextureName != null)
                        {
                            mVertexBufferShader.Parameters[LAYER_TEXTURE_NAMES[i]].SetValue(LookupSprite(detailTextureName));
                        }
                    }

                    VertexBuffer vertexBuffer = heightMap.Terrain.VertexBuffers[chunkRow, chunkCol];
                    IndexBuffer indexBuffer = heightMap.Terrain.IndexBuffers[chunkRow, chunkCol];

                    mDevice.SetVertexBuffer(vertexBuffer);
                    mDevice.Indices = indexBuffer;

                    mVertexBufferShader.CurrentTechnique.Passes[0].Apply();

                    mDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, vertexBuffer.VertexCount, 0, indexBuffer.IndexCount / 3);

                    if (mDrawBoundingBoxes)
                    {
                        BoundingBoxRenderer.Render(renderable.BoundingBoxes[chunkRow, chunkCol], mDevice, mView, mProjection, Color.Blue);
                    }
                }
            }

            for (int edgeChunkIndex = 0; edgeChunkIndex < 4; ++edgeChunkIndex)
            {
                VertexBuffer vertexBuffer = heightMap.Terrain.EdgeVertexBuffers[edgeChunkIndex];
                IndexBuffer indexBuffer = heightMap.Terrain.EdgeIndexBuffers[edgeChunkIndex];

                mDevice.SetVertexBuffer(vertexBuffer);
                mDevice.Indices = indexBuffer;

                mVertexBufferShader.CurrentTechnique.Passes[0].Apply();

                mDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, vertexBuffer.VertexCount, 0, indexBuffer.IndexCount / 3);
            }
        }

        static private void DrawSkyBox(string textureName, Matrix worldTransform)
        {
            mDevice.SamplerStates[0] = SamplerState.PointClamp;

            mVertexBufferShader.CurrentTechnique = mVertexBufferShader.Techniques["NoShade"];
            mVertexBufferShader.World      = worldTransform;
            mVertexBufferShader.View       = mView;
            mVertexBufferShader.Projection = mProjection;

            mVertexBufferShader.Texture = LookupSprite(textureName);

            mDevice.SetVertexBuffer(mSkyBoxVertexBuffer);
            mDevice.Indices = mSkyBoxIndexBuffer;

            mVertexBufferShader.CurrentTechnique.Passes[0].Apply();

            mDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, mSkyBoxVertexBuffer.VertexCount, 0, mSkyBoxIndexBuffer.IndexCount / 3);
        }

        /// <summary>
        /// Updates scene buffer with outline.
        /// </summary>
        static private void ApplyPostProcessing()
        {
            mPostProcessShader.Parameters["EdgeWidth"].SetValue(mEdgeWidth);
            mPostProcessShader.Parameters["EdgeIntensity"].SetValue(mEdgeIntensity);
            mPostProcessShader.Parameters["ScreenResolution"].SetValue(new Vector2(mSceneBuffer.Width, mSceneBuffer.Height));
            mPostProcessShader.Parameters["NormalDepthTexture"].SetValue(mNormalDepthBuffer);
            mPostProcessShader.Parameters["SceneTexture"].SetValue(mSceneBuffer);

            mPostProcessShader.CurrentTechnique = mPostProcessShader.Techniques["EdgeDetect"];

            mSpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null, mPostProcessShader);
            mSpriteBatch.Draw(mSceneBuffer, new Rectangle(0, 0, mSceneBuffer.Width, mSceneBuffer.Height), Color.White);
            mSpriteBatch.End();

            mDevice.SetRenderTarget(mCompositeBuffer);
            mDevice.Clear(Color.CornflowerBlue);

            mPostProcessShader.Parameters["OutlineTexture"].SetValue(mOutlineBuffer);
            mPostProcessShader.Parameters["SceneTexture"].SetValue(mSceneBuffer);

            mPostProcessShader.CurrentTechnique = mPostProcessShader.Techniques["Composite"];

            mSpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null, mPostProcessShader);
            mSpriteBatch.Draw(mSceneBuffer, new Rectangle(0, 0, mSceneBuffer.Width, mSceneBuffer.Height), Color.White);
            mSpriteBatch.End();
        }

        /// <summary>
        /// Reads tweak file and extracts matrices.
        /// </summary>
        /// <param name="tr">Text reader open to tweak file.</param>
        /// <param name="boneName">Name of bone associated with current matrix.</param>
        /// <param name="tweakMatrix">Tweak matrix of current bone.</param>
        static private void ParseTweakFile(TextReader tr, out string boneName, out Matrix tweakMatrix)
        {
            // Get bone name.
            boneName = tr.ReadLine();

            // Get first row of matrix.
            string[] row1Parts = tr.ReadLine().Split(mDelimeterChars, 4);

            float M11 = float.Parse(row1Parts[0]);
            float M12 = float.Parse(row1Parts[1]);
            float M13 = float.Parse(row1Parts[2]);
            float M14 = float.Parse(row1Parts[3]);

            // Get second row of matrix.
            string[] row2Parts = tr.ReadLine().Split(mDelimeterChars, 4);

            float M21 = float.Parse(row2Parts[0]);
            float M22 = float.Parse(row2Parts[1]);
            float M23 = float.Parse(row2Parts[2]);
            float M24 = float.Parse(row2Parts[3]);

            // Get third row of matrix.
            string[] row3Parts = tr.ReadLine().Split(mDelimeterChars, 4);

            float M31 = float.Parse(row3Parts[0]);
            float M32 = float.Parse(row3Parts[1]);
            float M33 = float.Parse(row3Parts[2]);
            float M34 = float.Parse(row3Parts[3]);

            // Get fourth row of matrix.
            string[] row4Parts = tr.ReadLine().Split(mDelimeterChars, 4);

            float M41 = float.Parse(row4Parts[0]);
            float M42 = float.Parse(row4Parts[1]);
            float M43 = float.Parse(row4Parts[2]);
            float M44 = float.Parse(row4Parts[3]);

            // Skip blank line.
            tr.ReadLine();

            tweakMatrix = new Matrix(M11, M12, M13, M14, M21, M22, M23, M24, M31, M32, M33, M34, M41, M42, M43, M44);
        }

        static private void BoundVertexBuffer(ref Vector3 min, ref Vector3 max, VertexBuffer vertexBuffer, int vertexCount)
        {
            int vertexStride = vertexBuffer.VertexDeclaration.VertexStride;
            int vertexBufferSize = vertexCount * vertexStride;

            float[] vertexData = new float[vertexBufferSize / sizeof(float)];
            vertexBuffer.GetData<float>(vertexData);

            for (int i = 0; i < vertexBufferSize / sizeof(float); i += vertexStride / sizeof(float))
            {
                Vector3 localPosition = new Vector3(vertexData[i], vertexData[i + 1], vertexData[i + 2]);

                min = Vector3.Min(min, localPosition);
                max = Vector3.Max(max, localPosition);
            }
        }

        static private void UpdateSceneExtents(BoundingBox boundingBox, Matrix worldTransform)
        {
            mMinSceneExtent = Vector3.Min(mMinSceneExtent, boundingBox.Min);
            mMaxSceneExtent = Vector3.Max(mMaxSceneExtent, boundingBox.Max);
        }

        #endregion
    }

    class RenderableComparer : IComparer<RenderableDefinition>
    {
        public int Compare(RenderableDefinition x, RenderableDefinition y)
        {
            float xDistance = (x.WorldTransform.Translation - GraphicsManager.Camera.Position).LengthSquared();
            float yDistance = (y.WorldTransform.Translation - GraphicsManager.Camera.Position).LengthSquared();

            return xDistance.CompareTo(yDistance);
        }
    }
}
