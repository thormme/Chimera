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

        static private Matrix mLightView;
        static private Matrix mLightProjection;
        static private Matrix mLightTransform;

        static private Effect mConfigurableShader;
        static private SkinnedEffect mTerrainShader;

        static private DirectionalLight mDirLight;
          
        static private Dictionary<string, Model> mUniqueModelLibrary = new Dictionary<string,Model>();
        static private Dictionary<string, TerrainDescription> mUniqueTerrainLibrary = new Dictionary<string, TerrainDescription>();

        static private GraphicsDevice mDevice;

        static private RenderTarget2D mShadowMap;
        static public RenderTarget2D ShadowMap
        {
            get { return mShadowMap; }
            set { mShadowMap = value; }
        }

        static private int mShadowMapLength = 2048;
        static private bool mCastingShadow = false;

        static private string mBASE_DIRECTORY = DirectoryManager.GetRoot() + "finalProject/finalProjectContent/";

        #endregion

        ///////////////////////////////// Interface //////////////////////////////////

        #region Puplic Properties

        /// <summary>
        /// Sets the render state to Cel Shading or Phong shading.
        /// </summary>
        static public bool CelShading { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Loads all models from content/models/ and shader file in to memory.
        /// </summary>
        /// <param name="content">Global game content manager.</param>
        static public void LoadContent(ContentManager content, GraphicsDevice device)
        {
            mDevice = device;

            // Load shaders.
            mConfigurableShader = content.Load<Effect>("shaders/ConfigurableShader");
            mTerrainShader = new SkinnedEffect(mConfigurableShader);
            
            // Load shadow map.
            mShadowMap = new RenderTarget2D(device, mShadowMapLength, mShadowMapLength, true, device.PresentationParameters.BackBufferFormat, DepthFormat.Depth24Stencil8);

            mDirLight = new DirectionalLight(
                mConfigurableShader.Parameters["DirLightDirection"],
                mConfigurableShader.Parameters["DirLightDiffuseColor"],
                mConfigurableShader.Parameters["DirLightSpecularColor"],
                null);

            mDirLight.Direction = new Vector3(-0.5265408f, -0.5735765f, -0.6275069f);
            mDirLight.DiffuseColor = new Vector3(1, 0.9607844f, 0.8078432f);
            mDirLight.SpecularColor = new Vector3(1, 0.9607844f, 0.8078432f);
            mDirLight.Enabled = true;

            // Load models.
            DirectoryInfo dir = new DirectoryInfo(mBASE_DIRECTORY + "models/");
            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException("Could not find models directory in content.");
            }

            FileInfo[] files = dir.GetFiles("*.fbx");
            foreach (FileInfo file in files)
            {
                string modelName = Path.GetFileNameWithoutExtension(file.Name);
                Model input = content.Load<Model>("models/" + modelName);

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
                    }
                }

                AddModel(modelName, input);
            }

            // Load terrain.
            dir = new DirectoryInfo(mBASE_DIRECTORY + "levels/maps/");
            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException("Could not find levels/maps directory in content.");
            }

            files = dir.GetFiles("*.bmp");
            foreach (FileInfo file in files)
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
        static public void Update(Camera camera)
        {
            mView = camera.ViewTransform;
            mProjection = camera.ProjectionTransform;
            
            BuildLightTransform();
        }

        /// <summary>
        /// Sets graphics state to render to shadow map.
        /// </summary>
        static public void RenderToShadowMap()
        {
            mDevice.SetRenderTarget(mShadowMap);
            mDevice.Clear(Color.White);
            mDevice.DepthStencilState = DepthStencilState.Default;
            mCastingShadow = true;
        }

        /// <summary>
        /// Sets graphics state to render to screen.
        /// </summary>
        static public void RenderToBackBuffer()
        {
            mDevice.SetRenderTarget(null);
            mDevice.Clear(Color.CornflowerBlue);
            mDevice.DepthStencilState = DepthStencilState.Default;
            mCastingShadow = false;
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
        /// Renders animated model.
        /// </summary>
        /// <param name="modelName">Name of model stored in database.</param>
        /// <param name="boneTransforms">State of rigged skeleton for current frame</param>
        /// <param name="worldTransforms">Position, orientation, and scale of model in world space.</param>
        static public void RenderSkinnedModel(string modelName, Matrix[] boneTransforms, Matrix worldTransforms)
        {
            DrawModel(modelName, boneTransforms, worldTransforms, true);
        }

        /// <summary>
        /// Renders inanimate model.
        /// </summary>
        /// <param name="modelName">Name of model stored in database.</param>
        /// <param name="worldTransforms">Position, orientation, and scale of model in world space.</param>
        static public void RenderUnskinnedModel(string modelName, Matrix worldTransforms)
        {
            Matrix[] emptyTransforms = new Matrix[1];
            emptyTransforms[0] = Matrix.Identity;
            DrawModel(modelName,emptyTransforms, worldTransforms, false);
        }

        /// <summary>
        /// Renders terrain.
        /// </summary>
        /// <param name="terrainName">Name of terrain stored in database.</param>
        /// <param name="worldTransforms">Position, orientation, and scale of terrain in world space.</param>
        static public void RenderTerrain(string terrainName, Matrix worldTransforms)
        {
            TerrainDescription heightmap = LookupTerrain(terrainName);

            mDevice.Indices = heightmap.Terrain.IndexBuffer;
            mDevice.SetVertexBuffer(heightmap.Terrain.VertexBuffer);

            if (mCastingShadow)
            {
                CastShadowTerrain(heightmap, worldTransforms);
            }
            else
            {
                if (CelShading)
                {
                    DrawTerrain(heightmap, "TerrainOutline", worldTransforms);
                }
                DrawTerrain(heightmap, (CelShading) ? "TerrainCelShade" : "TerrainPhong", worldTransforms);
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
        /// Draws all meshes within model to current rendertarget.  Applies rigged model transforms if necesarry.
        /// </summary>
        static private void DrawModel(string modelName, Matrix[] boneTransforms, Matrix worldTransforms, bool isSkinned)
        {
            Model model = LookupModel(modelName);

            foreach (ModelMesh mesh in model.Meshes)
            {
                if (mCastingShadow)
                {
                    CastShadowMesh(mesh, (isSkinned) ? "SkinnedShadowCast" : "ShadowCast", boneTransforms, worldTransforms);
                }
                else
                {
                    // Draw outline.
                    if (CelShading)
                    {
                        DrawMesh(mesh, (isSkinned) ? "SkinnedOutline" : "Outline", boneTransforms, worldTransforms);
                    }
                    DrawMesh(mesh, (isSkinned) ? ((CelShading) ? "SkinnedCelShade" : "SkinnedPhong") : ((CelShading) ? "CelShade" : "Phong"), boneTransforms, worldTransforms);
                }

        /// <summary>
            }
        }

        /// <summary>
        /// Sets all effects for current mesh and renders to shadow map.
        /// </summary>
        static private void CastShadowMesh(ModelMesh mesh, string techniqueName, Matrix[] boneTransforms, Matrix worldTransforms)
        {
            foreach (Effect effect in mesh.Effects)
            {
                EffectTechnique technique = effect.Techniques[techniqueName];

                if (technique != null)
                {
                    effect.CurrentTechnique = technique;
                }

                if (effect is SkinnedEffect)
                {
                    (effect as SkinnedEffect).SetBoneTransforms(boneTransforms);
                }
                EffectParameter parameter = effect.Parameters["lightViewProjection"];
                if (parameter != null)
                {
                    parameter.SetValue(mLightTransform);
                }
                parameter = effect.Parameters["lightWorld"];
                if (parameter != null)
                {
                    parameter.SetValue(worldTransforms);
                }
            }

            mesh.Draw();
        }

        /// <summary>
        /// Sets all effects for current mesh and renders to screen.
        /// </summary>
        static private void DrawMesh(ModelMesh mesh, string techniqueName, Matrix[] boneTransforms, Matrix worldTransforms)
        {
            foreach (Effect effect in mesh.Effects)
            {
                EffectTechnique technique = effect.Techniques[techniqueName];

                if (technique != null)
                {
                    effect.CurrentTechnique = technique;
                }

                EffectParameter parameter = effect.Parameters["xDirLightDirection"];
                if (parameter != null)
                {
                    parameter.SetValue(mDirLight.Direction);
                }
                parameter = effect.Parameters["xDirLightDiffuseColor"];
                if (parameter != null)
                {
                    parameter.SetValue(mDirLight.DiffuseColor);
                }
                parameter = effect.Parameters["xDirLightSpecularColor"];
                if (parameter != null)
                {
                    parameter.SetValue(mDirLight.SpecularColor);
                }

                if (effect is SkinnedEffect)
                {
                    (effect as SkinnedEffect).SetBoneTransforms(boneTransforms);

                    (effect as SkinnedEffect).View = mView;
                    (effect as SkinnedEffect).Projection = mProjection;

                    (effect as SkinnedEffect).SpecularColor = new Vector3(0.25f);
                    (effect as SkinnedEffect).SpecularPower = 16;

                    (effect as SkinnedEffect).World = worldTransforms;
                }
            }
            mesh.Draw();
        }

        /// <summary>
        /// Sets effect for current terrain and renders to shadow map.
        /// </summary>
        static private void CastShadowTerrain(TerrainDescription heightMap, Matrix worldTransforms)
        {
            mTerrainShader.Parameters["lightWorld"].SetValue(worldTransforms);
            mTerrainShader.Parameters["lightViewProjection"].SetValue(mLightTransform);

            mTerrainShader.CurrentTechnique = mTerrainShader.Techniques["TerrainShadowCast"];
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
        static private void DrawTerrain(TerrainDescription heightMap, string techniqueName, Matrix worldTransforms)
        {
            mTerrainShader.Texture = heightMap.TextureHigh;

            mTerrainShader.View = mView;
            mTerrainShader.Projection = mProjection;

            mTerrainShader.Parameters["xDirLightDirection"].SetValue(mDirLight.Direction);
            mTerrainShader.Parameters["xDirLightDiffuseColor"].SetValue(mDirLight.DiffuseColor);
            mTerrainShader.Parameters["xDirLightSpecularColor"].SetValue(mDirLight.SpecularColor);

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
        /// Builds out the transformation matrix for light source.
        /// </summary>
        /// <returns></returns>
        static private void BuildLightTransform()
        {
            BoundingFrustum frustum = new BoundingFrustum(mView);

            // Get the corners of the frustum
            Vector3[] frustumCornersWorld = frustum.GetCorners();

            Vector3 centroid = Vector3.Zero;
            foreach (Vector3 frustumCorner in frustumCornersWorld)
            {
                centroid += frustumCorner;
            }

            centroid /= (float)frustumCornersWorld.Length;

            float distance = Math.Abs(frustum.Near.D) + Math.Abs(frustum.Far.D);
            mLightView = Matrix.CreateLookAt(centroid - (mDirLight.Direction * distance), centroid, Vector3.Up);

            Vector3[] frustumCornersLight = new Vector3[frustumCornersWorld.Length];
            Vector3.Transform(frustumCornersWorld, ref mLightView, frustumCornersLight);

            BoundingBox box = BoundingBox.CreateFromPoints(frustumCornersLight);
            mLightProjection = Matrix.CreateOrthographicOffCenter(
                box.Min.X, box.Max.X, box.Min.Y, box.Max.Y, -box.Max.Z, -box.Min.Z);

            mLightTransform = mLightView * mProjection;
        }

        #endregion
    }
}
