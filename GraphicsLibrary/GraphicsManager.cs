using AnimationUtilities;
using GameConstructLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;

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

        static public AnimationUtilities.SkinnedEffect TerrainShader
        {
            get { return mTerrainShader; }
        }
        static private AnimationUtilities.SkinnedEffect mTerrainShader;

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
        static private RenderTarget2D[] mTerrainTextureComposites;

        static private Texture2D mSkyTexture;

        static private ICamera mCamera;
        static public ICamera Camera
        {
            get { return mCamera; }
        }

        static private Vector3 mMinSceneExtent = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
        static private Vector3 mMaxSceneExtent = new Vector3(float.MinValue, float.MinValue, float.MinValue);

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

        /// <summary>
        /// Renders scene in debug mode.
        /// </summary>
        static public bool DebugVisualization
        {
            get { return mDebugVisualization; }
            set { mDebugVisualization = value; }
        }
        static private bool mDebugVisualization = false;

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

        #endregion

        #region Public Methods

        /// <summary>
        /// Loads all models from content/models/ and shader file in to memory.
        /// </summary>
        /// <param name="content">Global game content manager.</param>
        static public void LoadContent(ContentManager content, GraphicsDevice device, SpriteBatch spriteBatch)
        {
            mDevice = device;
            var pp = device.PresentationParameters;

            mSpriteBatch = spriteBatch;

            // Load shaders.
            mConfigurableShader = content.Load<Effect>("shaders/ConfigurableShader");
            mPostProcessShader = content.Load<Effect>("shaders/PostProcessing");
            mTerrainShader = new AnimationUtilities.SkinnedEffect(mConfigurableShader);

            // Create buffers.
            mShadowMap = new CascadedShadowMap(device, 2048);

            mSceneBuffer = new RenderTarget2D(device, pp.BackBufferWidth, pp.BackBufferHeight, false, SurfaceFormat.Color, DepthFormat.Depth24);
            mNormalDepthBuffer = new RenderTarget2D(device, pp.BackBufferWidth, pp.BackBufferHeight, false, SurfaceFormat.Color, DepthFormat.Depth24);
            mOutlineBuffer = new RenderTarget2D(device, pp.BackBufferWidth, pp.BackBufferHeight, false, SurfaceFormat.Color, DepthFormat.Depth24);
            mCompositeBuffer = new RenderTarget2D(device, pp.BackBufferWidth, pp.BackBufferHeight, false, SurfaceFormat.Color, DepthFormat.Depth24);
            mTerrainTextureComposites = new RenderTarget2D[] { new RenderTarget2D(device, 1024, 1024),
                                                               new RenderTarget2D(device, 1024, 1024) };

            mDirectionalLight = new Light(
                new Vector3(-0.3333333f, -1.0f, 0.33333333f), // Direction
                new Vector3(0.05333332f, 0.09882354f, 0.1819608f), // Ambient Color
                new Vector3(1, 0.9607844f, 0.8078432f),            // Diffuse Color
                new Vector3(1, 0.9607844f, 0.8078432f)             // Specular Color
                );

            mSkyTexture = content.Load<Texture2D>("textures/sprites/sky");

            // Load models.
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

            // Load terrain.
            dir = new DirectoryInfo(content.RootDirectory + "\\" + "levels/maps/");
            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException("Could not find levels/maps directory in content.");
            }

            FileInfo[] terrainFiles = dir.GetFiles("*");
            foreach (FileInfo file in terrainFiles)
            {
                string terrainName = Path.GetFileNameWithoutExtension(file.Name);

                if (terrainName.Contains("_texture"))
                {
                    continue;
                }

                Texture2D terrain = content.Load<Texture2D>("levels/maps/" + terrainName);
                TerrainHeightMap heightMap = new TerrainHeightMap(terrain, mDevice);

                FileInfo[] textureFiles = dir.GetFiles(terrainName + "_texture.*");
                Texture2D terrainTexture = null;
                if (textureFiles.Length > 0)
                {
                    string inputTextureName = Path.GetFileNameWithoutExtension(textureFiles[0].Name);
                    terrainTexture = content.Load<Texture2D>("levels/maps/" + inputTextureName);
                }

                TerrainDescription newTerrain = new TerrainDescription(
                    heightMap, 
                    new List<Texture2D>(), 
                    new List<string>()
                    );

                newTerrain.TextureNames.Add("default_terrain_detail");
                newTerrain.AlphaMaps.Add(terrainTexture);

                if (mUniqueTerrainLibrary.ContainsKey(terrainName))
                {
                    throw new Exception("Duplicate terrain key: " + terrainName);
                }
                mUniqueTerrainLibrary.Add(terrainName, newTerrain);
            }

            // Load textures.
            dir = new DirectoryInfo(content.RootDirectory + "\\" + "textures/maps/");
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
        /// Updates Projection and View matrices to current view space.
        /// </summary>
        /// <param name="camera">View space camera.</param>
        static public void Update(ICamera camera, GameTime gameTime)
        {
            mTimeElapsed = (float)gameTime.TotalGameTime.TotalSeconds;

            mCamera = camera;
            mView = mCamera.GetViewTransform();
            mProjection = mCamera.GetProjectionTransform();
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

            // Composite Terrain Texture
            foreach (RenderableDefinition renderable in mRenderQueue)
            {
                if (!renderable.IsModel)
                {
                    CompositeTerrainTexture(renderable.Name);
                    continue;
                }
            }

            RasterizerState cull = new RasterizerState();
            cull.CullMode = CullMode.CullCounterClockwiseFace;

            mDevice.RasterizerState = cull;

            if (CastingShadows && mCamera != null)
            {
                mShadowMap.GenerateCascades(mCamera, mDirectionalLight, new BoundingBox(mMinSceneExtent, mMaxSceneExtent));
                mShadowMap.WriteShadowsToBuffer(mRenderQueue);

                mMinSceneExtent = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
                mMaxSceneExtent = new Vector3(float.MinValue, float.MinValue, float.MinValue);
            }

            if (CelShading != CelShaded.None)
            {
                // Draw Normal Depth Map.
                mDevice.SetRenderTarget(mNormalDepthBuffer);
                mDevice.Clear(Color.Black);

                foreach (RenderableDefinition renderable in mRenderQueue)
                {
                    if (mOutlining == Outlines.AnimateModels && renderable.IsSkinned ||
                        mOutlining == Outlines.All)
                    {
                        DrawRenderableDefinition(renderable, false, false, true, false);
                    }
                }

                // Draw Scene to Texture.
                mDevice.SetRenderTarget(mSceneBuffer);
                mDevice.Clear(Color.CornflowerBlue);

                foreach (RenderableDefinition renderable in mRenderQueue)
                {
                    DrawRenderableDefinition(renderable, false, false, false, false);
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
                        DrawRenderableDefinition(mTransparentQueue[i], false, false, false, true);
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
                    DrawRenderableDefinition(renderable, false, false, false, false);
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
            TerrainDescription result;
            if (mUniqueTerrainLibrary.TryGetValue(terrainName, out result))
            {
                return result.Terrain;
            }
            throw new KeyNotFoundException("Unable to find terrain key: " + terrainName);
        }

        /// <summary>
        /// Retrieves texture of terrain from database.  Throws KeyNotFoundException if terrainName does not exist in database.
        /// </summary>
        static public List<Texture2D> LookupTerrainAlphaMaps(string terrainName)
        {
            TerrainDescription result;
            if (mUniqueTerrainLibrary.TryGetValue(terrainName, out result))
            {
                return result.AlphaMaps;
            }
            throw new KeyNotFoundException("Unable to find texture for terrain key: " + terrainName);
        }

        /// <summary>
        /// Retrieves names of textures applied to terrain from database.  Throws KeyNotFoundException if terrainName does not exist in database.
        /// </summary>
        static public List<string> LookupTerrainTextureNames(string terrainName)
        {
            TerrainDescription result;
            if (mUniqueTerrainLibrary.TryGetValue(terrainName, out result))
            {
                return result.TextureNames;
            }
            throw new KeyNotFoundException("Unable to find texture names for terrain key: " + terrainName);
        }

        /// <summary>
        /// Retrieves composite terrain texture.
        /// </summary>
        static public Texture2D LookupTerrainTexture(string terrainName)
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
                mRenderQueue.Enqueue(new RenderableDefinition(modelName, true, true, worldTransforms, boneTransforms, overlayColor, overlayColorWeight, Vector2.Zero));
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
                Matrix[] emptyTransforms = new Matrix[1];
                emptyTransforms[0] = Matrix.Identity;
                mRenderQueue.Enqueue(new RenderableDefinition(modelName, true, false, worldTransforms, emptyTransforms, overlayColor, overlayColorWeight, Vector2.Zero));

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
                Matrix[] emptyTransforms = new Matrix[1];
                emptyTransforms[0] = Matrix.Identity;
                Vector3 translation;
                Quaternion rotation;
                Vector3 scale;

                worldTransforms.Decompose(out scale, out rotation, out translation);

                mTransparentQueue.Add(new RenderableDefinition(modelName, true, false, worldTransforms, emptyTransforms, overlayColor, overlayColorWeight, animationRate));
            }
            else
            {
                throw new Exception("Unable to render transparent model " + modelName + " before calling BeginRendering() or after FinishRendering().\n");
            }
        }

        /// <summary>
        /// Renders terrain.
        /// </summary>
        /// <param name="terrainName">Name of terrain stored in database.</param>
        /// <param name="worldTransforms">Position, orientation, and scale of terrain in world space.</param>
        static public void RenderTerrain(string terrainName, Matrix worldTransforms, BoundingBox boundingBox, Color overlayColor, float overlayColorWeight)
        {
            if (mCanRender)
            {
                mRenderQueue.Enqueue(new RenderableDefinition(terrainName, false, false, worldTransforms, null, overlayColor, overlayColorWeight, Vector2.Zero));
                UpdateSceneExtents(boundingBox, worldTransforms);
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

        public static BoundingBox BuildTerrainBoundingBox(string terrainName)
        {
            Vector3 minExtent = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            Vector3 maxExtent = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            TerrainHeightMap terrain = LookupTerrain(terrainName).Terrain;

            BoundVertexBuffer(ref minExtent, ref maxExtent, terrain.VertexBuffer, terrain.NumVertices);

            return new BoundingBox(minExtent, maxExtent);
        }

        #endregion

        ///////////////////////////// Internal functions /////////////////////////////

        #region Helper Methods

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
        static private void DrawRenderableDefinition(RenderableDefinition renderable, bool isShadow, bool hiResShadow, bool isOutline, bool hasTransparency)
        {
            if (!renderable.IsModel)
            {
                // Render Terrain.
                DrawTerrain(renderable.Name, renderable.WorldTransform, isShadow, hiResShadow, isOutline, renderable.OverlayColor, renderable.OverlayColorWeight);
            }
            else
            {
                // Render Inanimate or Animate Model.
                DrawModel(renderable.Name, renderable.BoneTransforms, renderable.WorldTransform, renderable.IsSkinned, isShadow, hiResShadow, isOutline, hasTransparency, renderable.AnimationRate, renderable.OverlayColor, renderable.OverlayColorWeight);
            }
        }

        /// <summary>
        /// Draws all meshes within model to current rendertarget.  Applies rigged model transforms if necesarry.
        /// </summary>
        static private void DrawModel(string modelName, Matrix[] boneTransforms, Matrix worldTransforms, bool isSkinned, bool isShadow, bool hiResShadow, bool isOutline, bool hasTransparency, Vector2 animationRate, Vector3 overlayColor, float overlayColorWeight)
        {
            Model model = LookupModel(modelName);

            foreach (ModelMesh mesh in model.Meshes)
            {
                string techniqueName;
                if (isOutline)
                {
                    techniqueName = (isSkinned) ? "SkinnedNormalDepthShade" : "NormalDepthShade";
                }
                else if (CelShading == CelShaded.Models || CelShading == CelShaded.All)
                {
                    techniqueName = (isSkinned) ? "SkinnedCelShade" : (hasTransparency) ? "NoShade" : "CelShade";
                }
                else if (CelShading == CelShaded.AnimateModels)
                {
                    techniqueName = (isSkinned) ? "SkinnedCelShade" : (hasTransparency) ? "NoShade" : "Phong";
                }
                else
                {
                    techniqueName = (isSkinned) ? "SkinnedPhong" : (hasTransparency) ? "NoShade" : "Phong";
                }

                DrawMesh(mesh, techniqueName, boneTransforms, worldTransforms, animationRate, overlayColor, overlayColorWeight);
            }
        }

        /// <summary>
        /// Draws current Terrain to render target using appropriate effects.
        /// </summary>
        static private void DrawTerrain(string terrainName, Matrix worldTransforms, bool isShadow, bool hiResShadow, bool isOutline, Vector3 overlayColor, float overlayColorWeight)
        {
            TerrainDescription heightmap = LookupTerrain(terrainName);

            string techniqueName = (isOutline) ? "NormalDepthShade" : ((CelShading == CelShaded.Terrain || CelShading == CelShaded.All) ? "CelShade" : "Phong");
            DrawTerrainHeightMap(heightmap, techniqueName, worldTransforms, overlayColor, overlayColorWeight);
        }

        /// <summary>
        /// Sets all effects for current mesh and renders to screen.
        /// </summary>
        static private void DrawMesh(ModelMesh mesh, string techniqueName, Matrix[] boneTransforms, Matrix worldTransforms, Vector2 animationRate, Vector3 overlayColor, float overlayColorWeight)
        {
            foreach (AnimationUtilities.SkinnedEffect effect in mesh.Effects)
            {
                effect.CurrentTechnique = effect.Techniques[techniqueName];

                if (CastingShadows)
                {
                    effect.Parameters["ShadowMap"].SetValue(mShadowMap.Buffer);
                }

                effect.SetBoneTransforms(boneTransforms);

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

                effect.Parameters["xOverlayColor"].SetValue(overlayColor);
                effect.Parameters["xOverlayColorWeight"].SetValue(overlayColorWeight);

                effect.Parameters["xNumShadowBands"].SetValue(techniqueName.Contains("Skinned") ? 2.0f : 3.0f);

                effect.SpecularColor = new Vector3(0.25f);
                effect.SpecularPower = 16;

                effect.World = worldTransforms;

                if (animationRate != Vector2.Zero)
                {
                    mTimeElapsed %= 1.0f;
                    animationRate *= mTimeElapsed;
                    animationRate.X %= 1.0f;
                    animationRate.Y %= 1.0f;
                }
                effect.Parameters["xTextureOffset"].SetValue(animationRate);
            }
            mesh.Draw();
        }

        static private void CompositeTerrainTexture(string name)
        {
            string[] textureParameters = { "RedTexture", "GreenTexture", "BlueTexture" };
            TerrainDescription terrain = LookupTerrain(name);

            mTerrainShader.CurrentTechnique = mTerrainShader.Techniques["CompositeTerrainTexture"];

            int i = 0;
            for (; i < terrain.TextureNames.Count; i += 3)
            {
                mDevice.SetRenderTarget(mTerrainTextureComposites[((i / 3) + 1) % 2]);
                mDevice.Clear(Color.White);

                mTerrainShader.Texture = mTerrainTextureComposites[(i / 3) % 2];

                if (terrain.AlphaMaps.Count > i / 3)
                {
                    mTerrainShader.Parameters["AlphaMap"].SetValue(terrain.AlphaMaps[i / 3]);
                }

                if (terrain.TextureNames.Count > i)
                {
                    mTerrainShader.Parameters[textureParameters[i % 3]].SetValue(LookupSprite(terrain.TextureNames[i]));
                }

                if (terrain.TextureNames.Count > i + 1)
                {
                    mTerrainShader.Parameters[textureParameters[(i + 1) % 3]].SetValue(LookupSprite(terrain.TextureNames[i + 1]));
                }

                if (terrain.TextureNames.Count > i + 2)
                {
                    mTerrainShader.Parameters[textureParameters[(i + 2) % 3]].SetValue(LookupSprite(terrain.TextureNames[i + 2]));
                }

                mSpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null, mTerrainShader);
                mSpriteBatch.Draw(mTerrainTextureComposites[(i / 3) % 2], new Rectangle(0, 0, mTerrainTextureComposites[(i / 3) % 2].Width, mTerrainTextureComposites[(i / 3) % 2].Height), Color.White);
                mSpriteBatch.End();
            }

            terrain.Texture = mTerrainTextureComposites[(i / 3) % 2];
        }

        /// <summary>
        /// Sets effect for current terrain and renders to screen.
        /// </summary>
        static private void DrawTerrainHeightMap(TerrainDescription heightMap, string techniqueName, Matrix worldTransforms, Vector3 overlayColor, float overlayColorWeight)
        {

            mDevice.Indices = heightMap.Terrain.IndexBuffer;
            mDevice.SetVertexBuffer(heightMap.Terrain.VertexBuffer);

            mTerrainShader.Texture = heightMap.Texture;

            if (CastingShadows)
            {
                mTerrainShader.Parameters["ShadowMap"].SetValue(mShadowMap.Buffer);
            }

            mTerrainShader.Parameters["xVisualizeCascades"].SetValue(mShadowMap.VisualizeCascades);
            mTerrainShader.Parameters["xCascadeCount"].SetValue(mShadowMap.CascadeCount);
            mTerrainShader.Parameters["xLightView"].SetValue(mShadowMap.LightView);
            mTerrainShader.Parameters["xLightProjections"].SetValue(mShadowMap.LightProjections);
            mTerrainShader.Parameters["xCascadeBufferBounds"].SetValue(mShadowMap.CascadeBounds);
            mTerrainShader.Parameters["xCascadeColors"].SetValue(mShadowMap.CascadeColors);

            mTerrainShader.View = mView;
            mTerrainShader.Projection = mProjection;

            TerrainShader.LightView = mShadowMap.LightView;
            TerrainShader.LightProjection = mShadowMap.LightProjection;

            mTerrainShader.Parameters["xDirLightDirection"].SetValue(mDirectionalLight.Direction);
            mTerrainShader.Parameters["xDirLightDiffuseColor"].SetValue(mDirectionalLight.DiffuseColor);
            mTerrainShader.Parameters["xDirLightSpecularColor"].SetValue(mDirectionalLight.SpecularColor);
            mTerrainShader.Parameters["xDirLightAmbientColor"].SetValue(mDirectionalLight.AmbientColor);

            mTerrainShader.Parameters["xOverlayColor"].SetValue(overlayColor);
            mTerrainShader.Parameters["xOverlayColorWeight"].SetValue(overlayColorWeight);

            mTerrainShader.SpecularColor = new Vector3(0.25f);
            mTerrainShader.SpecularPower = 16;

            mTerrainShader.World = worldTransforms;

            mTerrainShader.CurrentTechnique = mTerrainShader.Techniques[techniqueName];
            mTerrainShader.CurrentTechnique.Passes[0].Apply();

            mDevice.DrawIndexedPrimitives(
                PrimitiveType.TriangleList,
                0,
                0,
                heightMap.Terrain.NumVertices,
                0,
                heightMap.Terrain.NumIndices / 3);
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
            mMinSceneExtent = Vector3.Min(mMinSceneExtent, Vector3.Transform(boundingBox.Min, worldTransform));
            mMaxSceneExtent = Vector3.Max(mMaxSceneExtent, Vector3.Transform(boundingBox.Max, worldTransform));
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
