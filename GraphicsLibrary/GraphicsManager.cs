using GameConstructLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using GameConstructLibrary;

namespace GraphicsLibrary
{
    static public class GraphicsManager
    {
        ////////////////////////////// Internal State ////////////////////////////////

        static private Matrix mView;
        static private Matrix mProjection;

        static private Effect configurableShader;
        static private SkinnedEffect terrainShader;

        static private Dictionary<string, Model> mUniqueModelLibrary = new Dictionary<string,Model>();
        static private Dictionary<string, TerrainDescription> mUniqueTerrainLibrary = new Dictionary<string, TerrainDescription>();

        static public bool CelShading { get; set; }

        static private string BASE_DIRECTORY = DirectoryManager.GetRoot() + "finalProject/finalProjectContent/";

        ///////////////////////////////// Interface //////////////////////////////////

        /// <summary>
        /// Loads all models from content/models/ and shader file in to memory.
        /// </summary>
        /// <param name="content">Global game content manager.</param>
        static public void LoadContent(ContentManager content, GraphicsDevice device)
        {
            configurableShader = content.Load<Effect>("shaders/ConfigurableShader");
            terrainShader = new SkinnedEffect(configurableShader);

            DirectoryInfo dir = new DirectoryInfo(BASE_DIRECTORY + "models/");
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
                            SkinnedEffect newEffect = new SkinnedEffect(configurableShader);
                            newEffect.CopyFromSkinnedEffect(skinnedEffect);

                            part.Effect = newEffect;
                        }
                    }
                }

                AddModel(modelName, input);
            }

            dir = new DirectoryInfo(BASE_DIRECTORY + "levels/maps/");
            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException("Could not find levels/maps directory in content.");
            }

            files = dir.GetFiles("*.bmp");
            foreach (FileInfo file in files)
            {
                string terrainName = Path.GetFileNameWithoutExtension(file.Name);
                TerrainHeightMap heightMap = new TerrainHeightMap(terrainName, device);

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
        /// <param name="cameraUp"></param>
        /// <param name="cameraForward"></param>
        /// <param name="cameraLeft"></param>
        static public void Update(Camera camera)
        {
            mView = camera.ViewTransform;

            mProjection = camera.ProjectionTransform;
        }

        /// <summary>
        /// Looks up model associated with modelName and returns the skinningData for that model.
        /// </summary>
        /// <param name="modelName"></param>
        /// <returns></returns>
        static public SkinningData LookupModelSkinningData(string modelName)
        {
            Model model = LookupModel(modelName);
            return model.Tag as SkinningData;
        }

        /// <summary>
        /// Renders animated model.
        /// </summary>
        static public void RenderSkinnedModel(string modelName, Matrix[] boneTransforms, Matrix worldTransforms)
        {
            RenderMesh(modelName, boneTransforms, worldTransforms, true);
        }

        /// <summary>
        /// Renders inanimate model.
        /// </summary>
        static public void RenderUnskinnedModel(string modelName, Matrix worldTransforms)
        {
            Matrix[] emptyTransforms = new Matrix[1];
            emptyTransforms[0] = Matrix.Identity;
            RenderMesh(modelName,emptyTransforms, worldTransforms, false);
        }

        /// <summary>
        /// Retrieves terrain from container.  Throws KeyNotFoundException if modelName does not exist in container.
        /// </summary>
        static public TerrainDescription LookupTerrain(string terrainName)
        {
            TerrainDescription result;
            if (mUniqueTerrainLibrary.TryGetValue(terrainName, out result))
            {
                return result;
            }
            throw new KeyNotFoundException("Unable to find model key: " + terrainName);
        }

        /// <summary>
        /// Renders Terrain.
        /// </summary>
        static public void RenderTerrain(string terrainName, Matrix worldTransforms)
        {
            TerrainDescription heightmap = LookupTerrain(terrainName);

            RenderTerrain(heightmap, worldTransforms);
        }

        /// <summary>
        /// Renders Terrain.
        /// </summary>
        static public void RenderTerrain(TerrainDescription heightmap, Matrix worldTransforms)
        {
            terrainShader.Texture = heightmap.TextureHigh;

            terrainShader.View = mView;
            terrainShader.Projection = mProjection;

            terrainShader.EnableDefaultLighting();

            terrainShader.SpecularColor = new Vector3(0.25f);
            terrainShader.SpecularPower = 16;

            terrainShader.World = worldTransforms;

            GraphicsDevice device = heightmap.Terrain.Device;
            device.Indices = heightmap.Terrain.IndexBuffer;
            device.SetVertexBuffer(heightmap.Terrain.VertexBuffer);

            terrainShader.CurrentTechnique = terrainShader.Techniques["TerrainOutline"];
            terrainShader.CurrentTechnique.Passes[0].Apply();
            device.DrawIndexedPrimitives(
                PrimitiveType.TriangleList,
                0,
                0,
                heightmap.Terrain.NumVertices,
                0,
                heightmap.Terrain.NumIndices / 3);

            terrainShader.CurrentTechnique = terrainShader.Techniques["TerrainCelShade"];
            terrainShader.CurrentTechnique.Passes[0].Apply();
            device.DrawIndexedPrimitives(
                PrimitiveType.TriangleList,
                0,
                0,
                heightmap.Terrain.NumVertices,
                0,
                heightmap.Terrain.NumIndices / 3);
        }

        ///////////////////////////// Internal functions /////////////////////////////

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
        /// Retrieves model from container.  Throws KeyNotFoundException if modelName does not exist in container.
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
        /// Helper function that sets effect parameters and renders generic mesh.
        /// </summary>
        static void RenderMesh(string modelName, Matrix[] boneTransforms, Matrix worldTransforms, bool isSkinned)
        {
            Model model = LookupModel(modelName);

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (SkinnedEffect effect in mesh.Effects)
                {
                    effect.View = mView;
                    effect.Projection = mProjection;

                    effect.EnableDefaultLighting();

                    effect.SpecularColor = new Vector3(0.25f);
                    effect.SpecularPower = 16;

                    effect.World = worldTransforms;

                    if (isSkinned)
                    {
                        effect.SetBoneTransforms(boneTransforms);

                        RenderSkinnedMesh(effect, mesh);
                    }
                    else
                    {
                        RenderUnskinnedMesh(effect, mesh);
                    }
                }
            }
        }

        /// <summary>
        /// Sets technique in effect file to render skinned mesh and then renders skinned mesh.
        /// </summary>
        static private void RenderSkinnedMesh(SkinnedEffect effect, ModelMesh mesh)
        {
            if (CelShading)
            {
                effect.CurrentTechnique = effect.Techniques["SkinnedOutline"];
                mesh.Draw();

                effect.CurrentTechnique = effect.Techniques["SkinnedCelShade"];
                mesh.Draw();
            }
            else
            {
                effect.CurrentTechnique = effect.Techniques["SkinnedPhong"];
                mesh.Draw();
            }
        }

        /// <summary>
        /// Sets technique in effect file to render unskinned mesh and then renders unskinned mesh.
        /// </summary>
        static private void RenderUnskinnedMesh(SkinnedEffect effect, ModelMesh mesh)
        {
            if (CelShading)
            {
                effect.CurrentTechnique = effect.Techniques["Outline"];
                mesh.Draw();

                effect.CurrentTechnique = effect.Techniques["CelShade"];
                mesh.Draw();
            }
            else
            {
                effect.CurrentTechnique = effect.Techniques["Phong"];
                mesh.Draw();
            }
        }
    }
}
