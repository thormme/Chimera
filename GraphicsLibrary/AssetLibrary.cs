using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using GameConstructLibrary;
using GraphicsLibrary.ModelLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GraphicsLibrary
{
    static public class AssetLibrary
    {
        #region Constants

        static private Type InanimateModelKey = typeof(ModelRenderer);
        static private Type AnimateModelKey = typeof(AnimateModelRenderer);
        static private Type TransparentModelKey = typeof(TransparentModelRenderer);
        static private Type UIModelKey = typeof(UIModelRenderer);
        static private Type HeightMapKey = typeof(HeightMapRenderer);
        static private Type SkyBoxKey = typeof(SkyBoxRenderer);
        static private Type WaterKey = typeof(WaterRenderer);
        static private Type GridKey = typeof(GridRenderer);
        static private Type TextureKey = typeof(Texture2D);

        static private char[] mDelimeterChars = { ' ' };

        #endregion

        #region Private Libraries

        static private Effect mConfigurableShader;

        static public Effect PostProcessShader { get { return mPostProcessShader; } }
        static private Effect mPostProcessShader;

        static public AnimationUtilities.SkinnedEffect VertexBufferShader { get { return mVertexBufferShader; } }
        static private AnimationUtilities.SkinnedEffect mVertexBufferShader;

        static private Dictionary<Type, Dictionary<string, object>> mAssetLibrary = new Dictionary<Type, Dictionary<string, object>>();

        #endregion

        #region Public Properties

        static public Dictionary<string, object> InanimateModelLibrary { get { return mAssetLibrary[InanimateModelKey]; } }

        static public Dictionary<string, object> TextureLibrary { get { return mAssetLibrary[TextureKey]; } }

        #endregion

        #region Add Asset Methods

        static public void AddInanimateModel(string modelName, ModelRenderer model)
        {
            if (mAssetLibrary[InanimateModelKey].ContainsKey(modelName))
            {
                throw new Exception("Duplicate model key: " + modelName);
            }
            mAssetLibrary[InanimateModelKey].Add(modelName, model);
        }

        static public void AddAnimateModel(string modelName, AnimateModelRenderer model)
        {
            if (mAssetLibrary[AnimateModelKey].ContainsKey(modelName))
            {
                throw new Exception("Duplicate animate model key: " + modelName);
            }
            mAssetLibrary[AnimateModelKey].Add(modelName, model);
        }

        static public void AddTransparentModel(string modelName, TransparentModelRenderer model)
        {
            if (mAssetLibrary[TransparentModelKey].ContainsKey(modelName))
            {
                throw new Exception("Duplicate transparent model key: " + modelName);
            }
            mAssetLibrary[TransparentModelKey].Add(modelName, model);
        }

        static public void AddUIModel(string modelName, UIModelRenderer model)
        {
            if (mAssetLibrary[UIModelKey].ContainsKey(modelName))
            {
                throw new Exception("Duplicate UI model key: " + modelName);
            }
            mAssetLibrary[UIModelKey].Add(modelName, model);
        }

        static public void AddHeightMap(string heightMapName, HeightMapMesh heightMap)
        {
            if (mAssetLibrary[HeightMapKey].ContainsKey(heightMapName))
            {
                return;
            }
            mAssetLibrary[HeightMapKey].Add(heightMapName, new HeightMapRenderer(heightMap, mVertexBufferShader));
        }

        static public void AddGrid(string gridName, HeightMapMesh grid)
        {
            if (mAssetLibrary[GridKey].ContainsKey(gridName))
            {
                return;
            }
            mAssetLibrary[GridKey].Add(gridName, new GridRenderer(grid, mVertexBufferShader));
        }

        static public void AddWater(string waterName, WaterRenderer water)
        {
            if (mAssetLibrary[WaterKey].ContainsKey(waterName))
            {
                throw new Exception("Duplicate water key: " + waterName);
            }
            mAssetLibrary[WaterKey].Add(waterName, water);
        }

        static public void AddSkyBox(string skyBoxName, SkyBoxRenderer skyBox)
        {
            if (mAssetLibrary[SkyBoxKey].ContainsKey(skyBoxName))
            {
                throw new Exception("Duplicate sky box key: " + skyBoxName);
            }
            mAssetLibrary[SkyBoxKey].Add(skyBoxName, skyBox);
        }

        static public void AddTexture(string textureName, Texture2D texture)
        {
            if (mAssetLibrary[TextureKey].ContainsKey(textureName))
            {
                throw new Exception("Duplicate texture key: " + textureName);
            }
            mAssetLibrary[TextureKey].Add(textureName, texture);
        }

        #endregion

        #region Lookup Asset Methods

        static public RendererBase LookupRenderer(string rendererName, Type rendererType)
        {
            if (mAssetLibrary.ContainsKey(rendererType))
            {
                if (mAssetLibrary[rendererType].ContainsKey(rendererName))
                {
                    return mAssetLibrary[rendererType][rendererName] as RendererBase;
                }
            }
            return null;
        }

        static public ModelRenderer LookupInanimateModel(string modelName)
        {
            object result;
            if (mAssetLibrary[InanimateModelKey].TryGetValue(modelName, out result))
            {
                return result as ModelRenderer;
            }
            return null;
        }

        static public AnimateModelRenderer LookupAnimateModel(string modelName)
        {
            object result;
            if (mAssetLibrary[AnimateModelKey].TryGetValue(modelName, out result))
            {
                return result as AnimateModelRenderer;
            }
            return null;
        }

        static public TransparentModelRenderer LookupTransparentModel(string modelName)
        {
            object result;
            if (mAssetLibrary[TransparentModelKey].TryGetValue(modelName, out result))
            {
                return result as TransparentModelRenderer;
            }
            return null;
        }

        static public UIModelRenderer LookupUIModel(string modelName)
        {
            object result;
            if (mAssetLibrary[UIModelKey].TryGetValue(modelName, out result))
            {
                return result as UIModelRenderer;
            }
            return null;
        }

        static public HeightMapRenderer LookupHeightMap(string heightMapName)
        {
            object result;
            if (mAssetLibrary[HeightMapKey].TryGetValue(heightMapName, out result))
            {
                return result as HeightMapRenderer;
            }
            return null;
        }

        static public GridRenderer LookupGrid(string gridName)
        {
            object result;
            if (mAssetLibrary[GridKey].TryGetValue(gridName, out result))
            {
                return result as GridRenderer;
            }
            return null;
        }

        static public AnimationUtilities.SkinningData LookupModelSkinningData(string modelName)
        {
            return LookupAnimateModel(modelName).SkinningData;
        }

        static public Matrix LookupTweakedBoneOrientation(string modelName, string boneName)
        {
            object model;
            if (!mAssetLibrary[AnimateModelKey].TryGetValue(modelName, out model))
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
        
        static public Texture2D LookupTexture(string spriteName)
        {
            object result;
            if (mAssetLibrary[TextureKey].TryGetValue(spriteName, out result))
            {
                return result as Texture2D;
            }
            return null;
        }

        static public SkyBoxRenderer LookupSkyBox(string skyBoxName)
        {
            object result;
            if (mAssetLibrary[SkyBoxKey].TryGetValue(skyBoxName, out result))
            {
                return result as SkyBoxRenderer;
            }
            return null;
        }

        static public WaterRenderer LookupWater(string waterName)
        {
            object result;
            if (mAssetLibrary[SkyBoxKey].TryGetValue(waterName, out result))
            {
                return result as WaterRenderer;
            }
            return null;
        }

        #endregion

        #region Remove Methods

        static public void RemoveHeightMap(string heightMapName)
        {
            if (mAssetLibrary[HeightMapKey].ContainsKey(heightMapName))
            {
                mAssetLibrary[HeightMapKey][heightMapName] = null;
                mAssetLibrary[HeightMapKey].Remove(heightMapName);
            }
        }

        static public void ClearHeightMaps()
        {
            mAssetLibrary[HeightMapKey].Clear();
        }

        #endregion

        #region Asset Loading Methods

        static public void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
        {
            mAssetLibrary.Add(InanimateModelKey, new Dictionary<string, object>());
            mAssetLibrary.Add(AnimateModelKey, new Dictionary<string, object>());
            mAssetLibrary.Add(TransparentModelKey, new Dictionary<string, object>());
            mAssetLibrary.Add(UIModelKey, new Dictionary<string, object>());
            mAssetLibrary.Add(HeightMapKey, new Dictionary<string, object>());
            mAssetLibrary.Add(SkyBoxKey, new Dictionary<string, object>());
            mAssetLibrary.Add(WaterKey, new Dictionary<string, object>());
            mAssetLibrary.Add(GridKey, new Dictionary<string, object>());
            mAssetLibrary.Add(TextureKey, new Dictionary<string, object>());

            LoadShaders(content);
            LoadModels(graphicsDevice, content);
            LoadTextures(content);
            LoadWater();
            LoadSkyBox();
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

        static private void LoadModels(GraphicsDevice graphicsDevice, ContentManager content)
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

                        CustomModel input = new CustomModel(graphicsDevice, "Content/" + "models/" + subDirName + "/" + modelName);

                        foreach (var mesh in input.Meshes)
                        {
                            foreach (var part in mesh.Parts)
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

                        AddInanimateModel(modelName, new ModelRenderer(input));
                        if (input.SkinningData.AnimationClips.Count > 0)
                        {
                            AddAnimateModel(modelName, new AnimateModelRenderer(input));
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

                    LookupAnimateModel(Path.GetFileNameWithoutExtension(file.Name)).BoneTweakLibrary = tweakLibrary;
                }
            }
        }

        static private void LoadTextures(ContentManager content)
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

                AddTexture(mapName, map);
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

                AddTexture(spriteName, sprite);
            }
        }

        static private void LoadWater()
        {
            AddWater("WATER_RENDERER", new WaterRenderer(new Vector2(2, 2), mVertexBufferShader));
        }

        static private void LoadSkyBox()
        {
            AddSkyBox("SKY_BOX_RENDERER", new SkyBoxRenderer(mVertexBufferShader));
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
