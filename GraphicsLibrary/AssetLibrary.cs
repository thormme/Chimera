using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using GameConstructLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GraphicsLibrary
{
    static public class AssetLibrary
    {
        #region Constants

        static private char[] mDelimeterChars = { ' ' };

        #endregion

        #region Private Libraries

        static private Effect mConfigurableShader;

        static public Effect PostProcessShader { get { return mPostProcessShader; } }
        static private Effect mPostProcessShader;

        static public AnimationUtilities.SkinnedEffect VertexBufferShader { get { return mVertexBufferShader; } }
        static private AnimationUtilities.SkinnedEffect mVertexBufferShader;

        static private Dictionary<string, ModelRenderer> mUniqueModelLibrary = new Dictionary<string, ModelRenderer>();
        static private Dictionary<string, TerrainRenderer> mUniqueTerrainLibrary = new Dictionary<string, TerrainRenderer>();
        static private Dictionary<string, Texture2D> mUniqueTextureLibrary = new Dictionary<string, Texture2D>();
        static private Dictionary<string, TexturedParticles> mUniqueParticleLibrary = new Dictionary<string, TexturedParticles>();

        static private WaterRenderer mWaterRenderer;
        static private SkyBoxRenderer mSkyBoxRenderer;

        #endregion

        #region Public Properties

        static public Dictionary<string, ModelRenderer> ModelLibrary { get { return mUniqueModelLibrary; } }

        static public Dictionary<string, Texture2D> TextureLibrary { get { return mUniqueTextureLibrary; } }

        #endregion

        #region Add Asset Methods

        static public void AddModel(string modelName, ModelRenderer model)
        {
            if (mUniqueModelLibrary.ContainsKey(modelName))
            {
                throw new Exception("Duplicate model key: " + modelName);
            }
            mUniqueModelLibrary.Add(modelName, model);
        }

        static public void AddTerrain(FileInfo level, TerrainHeightMap heightMap, TerrainTexture texture)
        {
            if (mUniqueTerrainLibrary.ContainsKey(level.Name))
            {
                mUniqueTerrainLibrary.Remove(level.Name);
            }

            mUniqueTerrainLibrary.Add(level.Name, new TerrainRenderer(heightMap, texture, mVertexBufferShader));
        }

        static public void UpdateTerrain(FileInfo savePath, ref string levelName)
        {
            if (!mUniqueTerrainLibrary.ContainsKey(savePath.Name))
            {
                mUniqueTerrainLibrary.Add(savePath.Name, mUniqueTerrainLibrary[levelName]);
                levelName = savePath.Name;
            }
        }

        #endregion

        #region Lookup Asset Methods

        static public RendererBase LookupRenderer(string rendererName)
        {
            if (rendererName == "WATER_RENDERER")
            {
                return mWaterRenderer;
            }

            if (rendererName == "SKY_BOX_RENDERER")
            {
                return mSkyBoxRenderer;
            }

            if (mUniqueModelLibrary.ContainsKey(rendererName))
            {
                return mUniqueModelLibrary[rendererName];
            }

            if (mUniqueTerrainLibrary.ContainsKey(rendererName))
            {
                return mUniqueTerrainLibrary[rendererName];
            }

            throw new Exception("Unable to locate " + rendererName + " in Renderer library.");
        }

        static public ModelRenderer LookupModel(string modelName)
        {
            ModelRenderer result;
            if (mUniqueModelLibrary.TryGetValue(modelName, out result))
            {
                return result;
            }
            throw new KeyNotFoundException("Unable to find model key: " + modelName);
        }

        static public TerrainRenderer LookupTerrain(string terrainName)
        {
            TerrainRenderer result;
            if (mUniqueTerrainLibrary.TryGetValue(terrainName, out result))
            {
                return result;
            }
            throw new KeyNotFoundException("Unable to find terrain key: " + terrainName);
        }

        static public TerrainHeightMap LookupTerrainHeightMap(string terrainName)
        {
            return LookupTerrain(terrainName).HeightMap;
        }

        static public TerrainTexture LookupTerrainTexture(string terrainName)
        {
            return LookupTerrain(terrainName).Texture;
        }

        static public AnimationUtilities.SkinningData LookupModelSkinningData(string modelName)
        {
            AnimateModelRenderer renderer = LookupModel(modelName) as AnimateModelRenderer;
            return renderer.SkinningData;
        }

        static public Matrix LookupTweakedBoneOrientation(string modelName, string boneName)
        {
            ModelRenderer model;
            if (!mUniqueModelLibrary.TryGetValue(modelName, out model) || !(model is AnimateModelRenderer))
            {
                throw new Exception(modelName + " does not contain any bone orientation tweaks");
            }

            Dictionary<string, Matrix> boneTweakLibrary = (model as AnimateModelRenderer).BoneTweakLibrary;

            Matrix localBoneOrientation;
            if (!boneTweakLibrary.TryGetValue(boneName, out localBoneOrientation))
            {
                throw new Exception(modelName + " does not contain bone: " + boneName);
            }

            return localBoneOrientation;
        }
        
        static public Texture2D LookupSprite(string spriteName)
        {
            Texture2D result;
            if (mUniqueTextureLibrary.TryGetValue(spriteName, out result))
            {
                return result;
            }
            throw new KeyNotFoundException("Unable to find sprite key: " + spriteName);
        }

        static public TexturedParticles LookupParticles(string particleSystemName)
        {
            TexturedParticles result;
            if (mUniqueParticleLibrary.TryGetValue(particleSystemName, out result))
            {
                return result;
            }
            throw new KeyNotFoundException("Unable to find particle key: " + particleSystemName);
        }

        #endregion

        #region Asset Loading Methods

        static public void LoadContent(ContentManager content)
        {
            LoadShaders(content);
            LoadModels(content);
            LoadSprites(content);
            LoadParticles(content);
            CreateWater();
            CreateSkyBox();
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
                        string modelName = Path.GetFileNameWithoutExtension(file.Name);
                        bool isSkinned = false;

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

                                    isSkinned = true;
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

                        ModelRenderer renderer = isSkinned ? new AnimateModelRenderer(input) : new ModelRenderer(input);
                        AddModel(modelName, renderer);
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

                    (LookupModel(Path.GetFileNameWithoutExtension(file.Name)) as AnimateModelRenderer).BoneTweakLibrary = tweakLibrary;
                }
            }
        }

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

        static private void LoadParticles(ContentManager content)
        {
            //mParticleManager = new ParticleSystemManager();

            //for (int i = 0; i < 1; ++i)
            //{
            //    TexturedParticles newParticleSystem = new TexturedParticles(null, "puff");
            //    newParticleSystem.AutoInitialize(mDevice, content, mSpriteBatch);

            //    mParticleManager.AddParticleSystem(newParticleSystem);
            //    mUniqueParticleLibrary.Add("puff", newParticleSystem);
            //}
        }

        static private void CreateWater()
        {
            mWaterRenderer = new WaterRenderer(new Vector2(2, 2), mVertexBufferShader);
        }

        static private void CreateSkyBox()
        {
            mSkyBoxRenderer = new SkyBoxRenderer(mVertexBufferShader);
        }

        #endregion

        #region Asset Loading Helper Methods

        static private void ParseTweakFile(TextReader textReader, out string boneName, out Matrix tweakMatrix)
        {
            // Get bone name.
            boneName = textReader.ReadLine();

            // Get first row of matrix.
            string[] row1Parts = textReader.ReadLine().Split(mDelimeterChars, 4);

            float M11 = float.Parse(row1Parts[0]);
            float M12 = float.Parse(row1Parts[1]);
            float M13 = float.Parse(row1Parts[2]);
            float M14 = float.Parse(row1Parts[3]);

            // Get second row of matrix.
            string[] row2Parts = textReader.ReadLine().Split(mDelimeterChars, 4);

            float M21 = float.Parse(row2Parts[0]);
            float M22 = float.Parse(row2Parts[1]);
            float M23 = float.Parse(row2Parts[2]);
            float M24 = float.Parse(row2Parts[3]);

            // Get third row of matrix.
            string[] row3Parts = textReader.ReadLine().Split(mDelimeterChars, 4);

            float M31 = float.Parse(row3Parts[0]);
            float M32 = float.Parse(row3Parts[1]);
            float M33 = float.Parse(row3Parts[2]);
            float M34 = float.Parse(row3Parts[3]);

            // Get fourth row of matrix.
            string[] row4Parts = textReader.ReadLine().Split(mDelimeterChars, 4);

            float M41 = float.Parse(row4Parts[0]);
            float M42 = float.Parse(row4Parts[1]);
            float M43 = float.Parse(row4Parts[2]);
            float M44 = float.Parse(row4Parts[3]);

            // Skip blank line.
            textReader.ReadLine();

            tweakMatrix = new Matrix(M11, M12, M13, M14, M21, M22, M23, M24, M31, M32, M33, M34, M41, M42, M43, M44);
        }

        #endregion
    }
}
