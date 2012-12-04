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

        static private BoundingFrustum mCameraFrustum = new BoundingFrustum(Matrix.Identity);

        static private Matrix mLightView;
        static private Matrix mLightProjection;
        static private Matrix mLightTransform;

        static private Effect mConfigurableShader;
        static private Effect mPostProcessShader;
        static private SkinnedEffect mTerrainShader;

        static private Vector3 mLightDirection;
        static private Vector3 mLightDiffuseColor;
        static private Vector3 mLightSpecularColor;
        static private Vector3 mLightAmbientColor;

        static private Dictionary<string, Model> mUniqueModelLibrary = new Dictionary<string,Model>();
        static private Dictionary<string, TerrainDescription> mUniqueTerrainLibrary = new Dictionary<string, TerrainDescription>();
        static private Dictionary<string, Dictionary<string, Matrix>> mUniqueModelBoneLibrary = new Dictionary<string, Dictionary<string, Matrix>>();

        static private GraphicsDevice mDevice;
        static private SpriteBatch mSpriteBatch;

        static private Queue<RenderableDefinition> mRenderQueue;

        static private RenderTarget2D mShadowMap;
        static private RenderTarget2D mSceneBuffer;
        static private RenderTarget2D mNormalDepthBuffer;
        static private RenderTarget2D mOutlineBuffer;
        static private RenderTarget2D mCompositeBuffer;

        static private bool mCanRender = false;

        static private int mEdgeWidth = 1;
        static private int mEdgeIntensity = 1;
        static private int mShadowMapLength = 4096;
        static private int mShadowFarClip = 128;

        static private string mBASE_DIRECTORY = DirectoryManager.GetRoot() + "finalProject/finalProjectContent/";

        #endregion

        ///////////////////////////////// Interface //////////////////////////////////

        #region Puplic Properties

        public enum CelShaded { All, Models, Terrain, None };

        static private CelShaded mCelShading = CelShaded.All;

        /// <summary>
        /// Sets the render state to Cel Shading or Phong shading.
        /// </summary>
        static public CelShaded CelShading
        {
            get { return mCelShading; }
            set { mCelShading = value; } 
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

        /// <summary>
        /// Renders scene in debug mode.
        /// </summary>
        static public bool DebugVisualization
        {
            get { return mDebugVisualization; }
            set { mDebugVisualization = value; }
        }
        static private bool mDebugVisualization = false;

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

            mRenderQueue = new Queue<RenderableDefinition>();

            //CelShading = CelShaded.All;
            //CastingShadows = true;

            // Load shaders.
            mConfigurableShader = content.Load<Effect>("shaders/ConfigurableShader");
            mPostProcessShader = content.Load<Effect>("shaders/PostProcessing");
            mTerrainShader = new SkinnedEffect(mConfigurableShader);

            // Create buffers.
            mShadowMap = new RenderTarget2D(device, mShadowMapLength, mShadowMapLength, false, SurfaceFormat.Single, DepthFormat.Depth24);
            mSceneBuffer = new RenderTarget2D(device, pp.BackBufferWidth, pp.BackBufferHeight, false, SurfaceFormat.Color, DepthFormat.Depth24);
            mNormalDepthBuffer = new RenderTarget2D(device, pp.BackBufferWidth, pp.BackBufferHeight, false, SurfaceFormat.Color, DepthFormat.Depth24);
            mOutlineBuffer = new RenderTarget2D(device, pp.BackBufferWidth, pp.BackBufferHeight, false, SurfaceFormat.Color, DepthFormat.Depth24);
            mCompositeBuffer = new RenderTarget2D(device, pp.BackBufferWidth, pp.BackBufferHeight, false, SurfaceFormat.Color, DepthFormat.Depth24);

            mLightDiffuseColor = new Vector3(1, 0.9607844f, 0.8078432f);
            mLightSpecularColor = new Vector3(1, 0.9607844f, 0.8078432f);
            mLightAmbientColor = new Vector3(0.05333332f, 0.09882354f, 0.1819608f);
            mLightDirection = new Vector3(-0.3333333f, 0.6666667f, 0.6666667f);

            // Load models.
            DirectoryInfo dir = new DirectoryInfo(mBASE_DIRECTORY + "models/");
            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException("Could not find models directory in content.");
            }

            DirectoryInfo[] subDirs = dir.GetDirectories();
            foreach (DirectoryInfo subDir in subDirs)
            {
                string subDirName = subDir.Name;

                FileInfo[] files = subDir.GetFiles("*.*x");
                foreach (FileInfo file in files)
                {
                    string modelName = Path.GetFileNameWithoutExtension(file.Name);
                    Model input = content.Load<Model>("models/" + subDirName + "/" + modelName);

                    foreach (ModelMesh mesh in input.Meshes)
                    {
                        foreach (ModelMeshPart part in mesh.MeshParts)
                        {
                            Microsoft.Xna.Framework.Graphics.SkinnedEffect skinnedEffect = part.Effect as Microsoft.Xna.Framework.Graphics.SkinnedEffect;
                            if (skinnedEffect != null)
                            {
                                SkinnedEffect newEffect = new SkinnedEffect(mConfigurableShader);
                                newEffect.CopyFromSkinnedEffect(skinnedEffect);

                                part.Effect = newEffect;
                            }
                            else
                            {
                                Microsoft.Xna.Framework.Graphics.BasicEffect basicEffect = part.Effect as Microsoft.Xna.Framework.Graphics.BasicEffect;
                                if (basicEffect != null)
                                {
                                    SkinnedEffect newEffect = new SkinnedEffect(mConfigurableShader);
                                    newEffect.CopyFromBasicEffect(basicEffect);

                                    part.Effect = newEffect;
                                }
                            }
                        }
                    }

                    AddModel(modelName, input);
                }

                // Parse Bone Orientation Tweak File and store in library.
                FileInfo[] tweakOrientationFiles = subDir.GetFiles("*.tweak");
                foreach (FileInfo file in tweakOrientationFiles)
                {
                    Dictionary<string, Matrix> tweakLibrary = new Dictionary<string, Matrix>();
                    TextReader tr = new StreamReader(file.DirectoryName + "\\" + file.Name);

                    int count = int.Parse(tr.ReadLine());
                    string blank = tr.ReadLine();

                    char[] delimeterChars = {' '};

                    for (int i = 0; i < count; ++i)
                    {
                        // Get bone name.
                        string boneName = tr.ReadLine();

                        // Get first row of matrix.
                        string[] row1Parts = tr.ReadLine().Split(delimeterChars, 4);

                        float M11 = float.Parse(row1Parts[0]);
                        float M12 = float.Parse(row1Parts[1]);
                        float M13 = float.Parse(row1Parts[2]);
                        float M14 = float.Parse(row1Parts[3]);

                        // Get second row of matrix.
                        string[] row2Parts = tr.ReadLine().Split(delimeterChars, 4);

                        float M21 = float.Parse(row2Parts[0]);
                        float M22 = float.Parse(row2Parts[1]);
                        float M23 = float.Parse(row2Parts[2]);
                        float M24 = float.Parse(row2Parts[3]);

                        // Get third row of matrix.
                        string[] row3Parts = tr.ReadLine().Split(delimeterChars, 4);

                        float M31 = float.Parse(row3Parts[0]);
                        float M32 = float.Parse(row3Parts[1]);
                        float M33 = float.Parse(row3Parts[2]);
                        float M34 = float.Parse(row3Parts[3]);

                        // Get fourth row of matrix.
                        string[] row4Parts = tr.ReadLine().Split(delimeterChars, 4);

                        float M41 = float.Parse(row4Parts[0]);
                        float M42 = float.Parse(row4Parts[1]);
                        float M43 = float.Parse(row4Parts[2]);
                        float M44 = float.Parse(row4Parts[3]);

                        blank = tr.ReadLine();

                        Matrix tweakMatrix = new Matrix(M11, M12, M13, M14, M21, M22, M23, M24, M31, M32, M33, M34, M41, M42, M43, M44);
                        tweakLibrary.Add(boneName, tweakMatrix);
                    }
                    tr.Close();

                    mUniqueModelBoneLibrary.Add(Path.GetFileNameWithoutExtension(file.Name), tweakLibrary);
                }
            }

            // Load terrain.
            dir = new DirectoryInfo(mBASE_DIRECTORY + "levels/maps/");
            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException("Could not find levels/maps directory in content.");
            }

            FileInfo[] terrainFiles = dir.GetFiles("*.bmp");
            foreach (FileInfo file in terrainFiles)
            {
                string terrainName = Path.GetFileNameWithoutExtension(file.Name);
                TerrainHeightMap heightMap = new TerrainHeightMap(terrainName, mDevice);

                List<Texture2D> textures = new List<Texture2D>();
                List<float> heights = new List<float>();

                string[] textureNames = { "low", "medium", "high" };
                float[] textureHeights = { 0.33f, 0.66f, 1.0f };

                int index = 0;
                foreach (string name in textureNames)
                {
                    FileInfo[] textureFiles = dir.GetFiles(terrainName + "_texture_" + name + ".*");
                    if (textureFiles.Length > 0)
                    {
                        string inputTextureName = Path.GetFileNameWithoutExtension(textureFiles[0].Name);
                        Texture2D inputTexture = content.Load<Texture2D>("levels/maps/" + inputTextureName);
                        textures.Add(inputTexture);
                        heights.Add(textureHeights[index]);
                    }

                    ++index;
                }

                TerrainDescription newTerrain = new TerrainDescription(heightMap, textures, heights);
                if (mUniqueTerrainLibrary.ContainsKey(terrainName))
                {
                    throw new Exception("Duplicate model key: " + terrainName);
                }
                mUniqueTerrainLibrary.Add(terrainName, newTerrain);
            }
        }

        /// <summary>
        /// Updates Projection and View matrices to current view space.
        /// </summary>
        /// <param name="camera">View space camera.</param>
        static public void Update(ICamera camera)
        {
            mView = camera.GetViewTransform();
            
            mProjection = camera.GetProjectionTransform();

            float oldFarPlane = camera.GetFarPlaneDistance();
            camera.SetFarPlaneDistance(mShadowFarClip);

            mCameraFrustum.Matrix = mView * camera.GetProjectionTransform();

            camera.SetFarPlaneDistance(oldFarPlane);

            BuildLightTransform();
        }

        /// <summary>
        /// All Renderables drawn between BeginRendering and FinishRendering will be drawn to the screen following FinishRendering.
        /// </summary>
        static public void BeginRendering()
        {
            mRenderQueue.Clear();
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

            if (CastingShadows)
            {
                // Draw Shadow Map.
                mDevice.SetRenderTarget(mShadowMap);
                mDevice.Clear(Color.White);

                foreach (RenderableDefinition renderable in mRenderQueue)
                {
                    DrawRenderableDefinition(renderable, true, false);
                }
            }

            if (CelShading != CelShaded.None)
            {
                // Draw Normal Depth Map.
                mDevice.SetRenderTarget(mNormalDepthBuffer);
                mDevice.Clear(Color.Black);

                foreach (RenderableDefinition renderable in mRenderQueue)
                {
                    DrawRenderableDefinition(renderable, false, true);
                }

                // Draw Scene to Texture.
                mDevice.SetRenderTarget(mSceneBuffer);
                mDevice.Clear(Color.CornflowerBlue);

                foreach (RenderableDefinition renderable in mRenderQueue)
                {
                    DrawRenderableDefinition(renderable, false, false);
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
                    mSpriteBatch.Draw(mShadowMap, new Rectangle(0, 0, mSceneBuffer.Width / 2, mSceneBuffer.Height / 2), Color.White);
                    mSpriteBatch.End();

                    mSpriteBatch.Begin(0, BlendState.Opaque, SamplerState.PointClamp, null, null);
                    mSpriteBatch.Draw(mNormalDepthBuffer, new Rectangle(mSceneBuffer.Width / 2, 0, mSceneBuffer.Width / 2, mSceneBuffer.Height / 2), Color.White);
                    mSpriteBatch.End();

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
                    DrawRenderableDefinition(renderable, false, false);
                }
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
        static public void RenderSkinnedModel(string modelName, Matrix[] boneTransforms, Matrix worldTransforms)
        {
            if (mCanRender)
            {
                mRenderQueue.Enqueue(new RenderableDefinition(modelName, true, true, worldTransforms, boneTransforms));
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
        static public void RenderUnskinnedModel(string modelName, Matrix worldTransforms)
        {
            if (mCanRender)
            {
                Matrix[] emptyTransforms = new Matrix[1];
                emptyTransforms[0] = Matrix.Identity;
                mRenderQueue.Enqueue(new RenderableDefinition(modelName, true, false, worldTransforms, emptyTransforms));
            }
            else
            {
                throw new Exception("Unable to render inanimate model " + modelName + " before calling BeginRendering() or after FinishRendering().\n");
            }
        }

        /// <summary>
        /// Renders terrain.
        /// </summary>
        /// <param name="terrainName">Name of terrain stored in database.</param>
        /// <param name="worldTransforms">Position, orientation, and scale of terrain in world space.</param>
        static public void RenderTerrain(string terrainName, Matrix worldTransforms)
        {
            if (mCanRender)
            {
                mRenderQueue.Enqueue(new RenderableDefinition(terrainName, false, false, worldTransforms, null));
            }
            else
            {
                throw new Exception("Unable to render terrain " + terrainName + " before calling BeginRendering() or after FinishRendering().\n");
            }
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
        static private Model LookupModel(string modelName)
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
        /// 
        /// </summary>
        /// <param name="renderable"></param>
        static private void DrawRenderableDefinition(RenderableDefinition renderable, bool isShadow, bool isOutline)
        {
            if (!renderable.IsModel)
            {
                // Render Terrain.
                DrawTerrain(renderable.Name, renderable.WorldTransform, isShadow, isOutline);
            }
            else
            {
                // Render Inanimate or Animate Model.
                DrawModel(renderable.Name, renderable.BoneTransforms, renderable.WorldTransform, renderable.IsSkinned, isShadow, isOutline);
            }
        }

        /// <summary>
        /// Draws all meshes within model to current rendertarget.  Applies rigged model transforms if necesarry.
        /// </summary>
        static private void DrawModel(string modelName, Matrix[] boneTransforms, Matrix worldTransforms, bool isSkinned, bool isShadow, bool isOutline)
        {
            Model model = LookupModel(modelName);

            foreach (ModelMesh mesh in model.Meshes)
            {
                if (isShadow)
                {
                    CastShadowMesh(mesh, (isSkinned) ? "SkinnedShadowCast" : "ShadowCast", boneTransforms, worldTransforms);
                }
                else
                {
                    string techniqueName;
                    if (isOutline)
                    {
                        techniqueName = (isSkinned) ? "SkinnedNormalDepthShade" : "NormalDepthShade";
                    }
                    else if (CelShading == CelShaded.Models || CelShading == CelShaded.All)
                    {
                        techniqueName = (isSkinned) ? "SkinnedCelShade" : "CelShade";
                    }
                    else
                    {
                        techniqueName = (isSkinned) ? "SkinnedPhong" : "Phong";
                    }
                    DrawMesh(mesh, techniqueName, boneTransforms, worldTransforms);
                }
            }
        }

        /// <summary>
        /// Draws current Terrain to render target using appropriate effects.
        /// </summary>
        static private void DrawTerrain(string terrainName, Matrix worldTransforms, bool isShadow, bool isOutline)
        {
            TerrainDescription heightmap = LookupTerrain(terrainName);

            mDevice.Indices = heightmap.Terrain.IndexBuffer;
            mDevice.SetVertexBuffer(heightmap.Terrain.VertexBuffer);

            if (isShadow)
            {
                CastShadowTerrain(heightmap, worldTransforms);
            }
            else
            {
                string techniqueName = (isOutline) ? "NormalDepthShade" : ((CelShading == CelShaded.Terrain || CelShading == CelShaded.All) ? "TerrainCelShade" : "Phong");
                DrawTerrainHeightMap(heightmap, techniqueName, worldTransforms);
            }
        }

        /// <summary>
        /// Sets all effects for current mesh and renders to shadow map.
        /// </summary>
        static private void CastShadowMesh(ModelMesh mesh, string techniqueName, Matrix[] boneTransforms, Matrix worldTransforms)
        {
            foreach (SkinnedEffect effect in mesh.Effects)
            {
                effect.LightView = mLightView;
                effect.LightProjection = mLightProjection;

                effect.CurrentTechnique = effect.Techniques[techniqueName];

                effect.SetBoneTransforms(boneTransforms);

                effect.World = worldTransforms;
            }

            mesh.Draw();
        }

        /// <summary>
        /// Sets all effects for current mesh and renders to screen.
        /// </summary>
        static private void DrawMesh(ModelMesh mesh, string techniqueName, Matrix[] boneTransforms, Matrix worldTransforms)
        {
            foreach (SkinnedEffect effect in mesh.Effects)
            {
                effect.CurrentTechnique = effect.Techniques[techniqueName];

                if (CastingShadows)
                {
                    effect.Parameters["ShadowMap"].SetValue(mShadowMap);
                }

                effect.SetBoneTransforms(boneTransforms);

                effect.View = mView;
                effect.Projection = mProjection;

                effect.LightView = mLightView;
                effect.LightProjection = mLightProjection;

                effect.Parameters["xDirLightDirection"].SetValue(mLightDirection);
                effect.Parameters["xDirLightDiffuseColor"].SetValue(mLightDiffuseColor);
                effect.Parameters["xDirLightSpecularColor"].SetValue(mLightSpecularColor);
                effect.Parameters["xDirLightAmbientColor"].SetValue(mLightAmbientColor);

                effect.SpecularColor = new Vector3(0.25f);
                effect.SpecularPower = 16;

                effect.World = worldTransforms;
            }
            mesh.Draw();
        }

        /// <summary>
        /// Sets effect for current terrain and renders to shadow map.
        /// </summary>
        static private void CastShadowTerrain(TerrainDescription heightMap, Matrix worldTransforms)
        {
            mTerrainShader.World = worldTransforms;

            mTerrainShader.LightView = mLightView;
            mTerrainShader.LightProjection = mLightProjection;

            mTerrainShader.CurrentTechnique = mTerrainShader.Techniques["ShadowCast"];
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
        /// Sets effect for current terrain and renders to screen.
        /// </summary>
        static private void DrawTerrainHeightMap(TerrainDescription heightMap, string techniqueName, Matrix worldTransforms)
        {
            mTerrainShader.Texture = heightMap.TextureHigh;

            if (CastingShadows)
            {
                mTerrainShader.Parameters["ShadowMap"].SetValue(mShadowMap);
            }

            mTerrainShader.View = mView;
            mTerrainShader.Projection = mProjection;

            mTerrainShader.LightView = mLightView;
            mTerrainShader.LightProjection = mLightProjection;

            mTerrainShader.Parameters["xDirLightDirection"].SetValue(mLightDirection);
            mTerrainShader.Parameters["xDirLightDiffuseColor"].SetValue(mLightDiffuseColor);
            mTerrainShader.Parameters["xDirLightSpecularColor"].SetValue(mLightSpecularColor);
            mTerrainShader.Parameters["xDirLightAmbientColor"].SetValue(mLightAmbientColor);

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
        /// Builds out the transformation matrix for light source.
        /// </summary>
        /// <returns></returns>
        static private void BuildLightTransform()
        {
            Matrix lightRotation = Matrix.CreateLookAt(Vector3.Zero, -mLightDirection, Vector3.Up);

            // Get the corners of the frustum
            Vector3[] frustumCorners = mCameraFrustum.GetCorners();

            // Transform corners of frustum in to light space.
            for (int i = 0; i < frustumCorners.Length; ++i )
            {
                frustumCorners[i] = Vector3.Transform(frustumCorners[i], lightRotation);
            }

            BoundingBox lightBox = BoundingBox.CreateFromPoints(frustumCorners);

            Vector3 boxSize = lightBox.Max - lightBox.Min;
            Vector3 halfBoxSize = boxSize * 0.5f;

            Vector3 lightPosition = lightBox.Min + halfBoxSize;
            lightPosition.Z = lightBox.Min.Z;

            lightPosition = Vector3.Transform(lightPosition, Matrix.Invert(lightRotation));

            mLightView = Matrix.CreateLookAt(lightPosition, lightPosition - mLightDirection, Vector3.Up);
            //mView = mLightView;

            mLightProjection = Matrix.CreateOrthographic(boxSize.X, boxSize.Y, -boxSize.Z, boxSize.Z);
            //mProjection = mLightProjection;

            mLightTransform = mLightView * mProjection;
        }

        #endregion
    }
}
