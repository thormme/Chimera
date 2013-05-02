﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using GraphicsLibrary;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Xml.Serialization;

namespace GameConstructLibrary
{
    static public class LevelFileLoader
    {
        #region Previously Loaded Level Content

        static private string mLastLoadedLevelName = null;
        //static private TerrainDescription mLastLoadedTerrainDescription = null;
        static private List<DummyObject> mLastLoadedObjectList = null;

        #endregion

        #region Read / Write Level Files.

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePathAndName"></param>
        static public Tuple<Level, List<DummyObject>> LoadLevelFromFile(FileInfo filePathAndName)
        {
            Tuple<Level, List<DummyObject>> result;

            ZipFile zipFile = null;
            try
            {
                FileStream fileStream = File.OpenRead(filePathAndName.FullName);
                zipFile = new ZipFile(fileStream);

                Level level = new Level(filePathAndName.Name);

                Dictionary<Vector3, Texture2D> levelHeightMaps = new Dictionary<Vector3, Texture2D>();
                Dictionary<Vector3, Texture2D> levelAlphaMaps = new Dictionary<Vector3, Texture2D>();
                Dictionary<Vector3, string[]> levelTextureNames = new Dictionary<Vector3, string[]>();
                Dictionary<Vector3, Vector2[]> levelUVOffset = new Dictionary<Vector3, Vector2[]>();
                Dictionary<Vector3, Vector2[]> levelUVScale = new Dictionary<Vector3, Vector2[]>();

                HashSet<Vector3> coordinates = new HashSet<Vector3>();

                List<DummyObject> dummies = new List<DummyObject>();

                foreach (ZipEntry zipEntry in zipFile)
                {
                    if (!zipEntry.IsFile)
                    {
                        continue;
                    }

                    string entryFileName = Path.GetFileName(zipEntry.Name);

                    Stream zipStream = zipFile.GetInputStream(zipEntry);
                    MemoryStream memoryStream = new MemoryStream();
                    zipStream.CopyTo(memoryStream);
                    memoryStream.Seek(0, SeekOrigin.Begin);

                    if (entryFileName.Contains("Objects"))
                    {
                        dummies = LoadDummyObjects(memoryStream);
                        continue;
                    }

                    string[] nameSegments = entryFileName.Split(new char[] { '_' });
                    Vector3 coordinate = new Vector3(float.Parse(nameSegments[1]), float.Parse(nameSegments[2]), float.Parse(nameSegments[3]));

                    if (!coordinates.Contains(coordinate))
                    {
                        coordinates.Add(coordinate);
                    }

                    if (entryFileName.Contains("AlphaMap"))
                    {
                        levelAlphaMaps.Add(coordinate, Texture2D.FromStream(GraphicsManager.Device, memoryStream));
                    }
                    else if (entryFileName.Contains("HeightMap"))
                    {
                        levelHeightMaps.Add(coordinate, Texture2D.FromStream(GraphicsManager.Device, memoryStream));
                    }
                    else if (entryFileName.Contains("DetailTextures"))
                    {
                        string[] textureNames;
                        Vector2[] uvOffsets;
                        Vector2[] uvScales;
                        LoadDetailTextureNames(memoryStream, out textureNames, out uvOffsets, out uvScales);

                        levelTextureNames.Add(coordinate, textureNames);
                        levelUVOffset.Add(coordinate, uvOffsets);
                        levelUVScale.Add(coordinate, uvScales);
                    }
                    else if (entryFileName.Contains("Objects"))
                    {
                        mLastLoadedObjectList = LoadDummyObjects(memoryStream);
                    }

                    zipStream.Close();
                }

                foreach (Vector3 coordinate in coordinates)
                {
                    AddNewLevelBlock(level, coordinate, levelHeightMaps[coordinate], levelAlphaMaps[coordinate], levelTextureNames[coordinate], levelUVOffset[coordinate], levelUVScale[coordinate]);
                }

                result = new Tuple<Level, List<DummyObject>>(level, dummies);
            }
            finally
            {
                if (zipFile != null)
                {
                    zipFile.IsStreamOwner = true;
                    zipFile.Close();
                }
            }

            return result;
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="filePathAndName"></param>
        ///// <returns></returns>
        //static public TerrainHeightMap LoadHeightMapFromFile(FileInfo filePathAndName)
        //{
        //    if (!filePathAndName.Name.Contains(".lvl"))
        //    {
        //        filePathAndName = new FileInfo(filePathAndName.Name + ".lvl");
        //    }

        //    if (mLastLoadedLevelName == null || filePathAndName.Name != mLastLoadedLevelName)
        //    {
        //        LoadLevelFromFile(filePathAndName);
        //    }

        //    return mLastLoadedTerrainDescription.Terrain;
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="filePathAndName"></param>
        ///// <returns></returns>
        //static public TerrainTexture LoadTextureFromFile(FileInfo filePathAndName)
        //{
        //    if (!filePathAndName.Name.Contains(".lvl"))
        //    {
        //        filePathAndName = new FileInfo(filePathAndName.Name + ".lvl");
        //    }

        //    if (mLastLoadedLevelName == null || filePathAndName.Name != mLastLoadedLevelName)
        //    {
        //        LoadLevelFromFile(filePathAndName);
        //    }

        //    return mLastLoadedTerrainDescription.Texture;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePathAndName"></param>
        /// <returns></returns>
        static public List<DummyObject> LoadObjectsFromFile(FileInfo filePathAndName)
        {
            if (!filePathAndName.Name.Contains(".lvl"))
            {
                filePathAndName = new FileInfo(filePathAndName.Name + ".lvl");
            }

            if (mLastLoadedLevelName == null || filePathAndName.Name != mLastLoadedLevelName)
            {
                LoadLevelFromFile(filePathAndName);
            }

            return mLastLoadedObjectList;
        }

        static public void WriteLevel(Level level, List<DummyObject> objects, FileInfo filePathAndName)
        {
            if (!filePathAndName.Name.Contains(".lvl"))
            {
                filePathAndName = new FileInfo(filePathAndName.Name + ".lvl");
            }

            if (File.Exists(filePathAndName.FullName))
            {
                File.Delete(filePathAndName.FullName);
            }

            FileStream output = File.Create(filePathAndName.FullName);
            ZipOutputStream zipStream = new ZipOutputStream(output);

            List<Tuple<string, MemoryStream>> serializedLevel = level.Serialize();
            serializedLevel.Add(SerializedDummies(objects));

            zipStream.SetLevel(5);

            foreach (Tuple<string, MemoryStream> file in serializedLevel)
            {
                ZipEntry newEntry = new ZipEntry(file.Item1);
                newEntry.DateTime = DateTime.Now;

                zipStream.PutNextEntry(newEntry);

                StreamUtils.Copy(file.Item2, zipStream, new byte[4096]);
                zipStream.CloseEntry();
            }

            zipStream.IsStreamOwner = true;
            zipStream.Close();
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="heightMapWidth"></param>
        ///// <param name="heightMapHeight"></param>
        ///// <param name="alphaMapWidth"></param>
        ///// <param name="alphaMapHeight"></param>
        ///// <param name="chunkRows"></param>
        ///// <param name="chunkCols"></param>
        ///// <param name="defaultTexture"></param>
        ///// <returns></returns>
        //static public string GenerateBlankLevel(int heightMapWidth, int heightMapHeight, int alphaMapWidth, int alphaMapHeight, int chunkRows, int chunkCols, string defaultTexture, out List<DummyObject> dummyObjectList)
        //{
        //    mLastLoadedObjectList = new List<DummyObject>();
        //    dummyObjectList = mLastLoadedObjectList;

        //    Color[] heightMapBuffer = new Color[heightMapHeight * heightMapWidth];
        //    for (int i = 0; i < heightMapBuffer.Length; ++i)
        //    {
        //        heightMapBuffer[i] = new Color(0, 0, 255, 0);
        //    }
        //    Texture2D heightMapTexture = new Texture2D(GraphicsManager.Device, heightMapWidth, heightMapHeight);
        //    heightMapTexture.SetData(heightMapBuffer);

        //    Color[] alphaMapBuffer = new Color[alphaMapHeight * alphaMapWidth];
        //    for (int i = 0; i < alphaMapBuffer.Length; ++i)
        //    {
        //        alphaMapBuffer[i] = new Color(0, 0, 0, 0);
        //    }
        //    Texture2D alphaMapTexture = new Texture2D(GraphicsManager.Device, alphaMapWidth, alphaMapHeight);
        //    alphaMapTexture.SetData(alphaMapBuffer);

        //    string[, ,] detailTextureNames = new string[chunkRows, chunkCols, 5];
        //    Vector2[, ,] detailTextureUVOffsets = new Vector2[chunkRows, chunkCols, 5];
        //    Vector2[, ,] detailTextureUVScales = new Vector2[chunkRows, chunkCols, 5];

        //    for (int row = 0; row < chunkRows; ++row)
        //    {
        //        for (int col = 0; col < chunkCols; ++col)
        //        {
        //            detailTextureNames[row, col, 0] = defaultTexture;
        //            for (int index = 0; index < 5; ++index)
        //            {
        //                detailTextureUVOffsets[row, col, index] = Vector2.Zero;
        //                detailTextureUVScales[row, col, index] = Vector2.One;
        //            }
        //        }
        //    }

        //    TerrainHeightMap heightMap = new TerrainHeightMap(heightMapTexture, chunkRows, chunkCols, GraphicsManager.Device);
        //    TerrainTexture texture = new TerrainTexture(alphaMapTexture, detailTextureNames, detailTextureUVOffsets, detailTextureUVScales, chunkRows, chunkCols, GraphicsManager.Device);

        //    mLastLoadedTerrainDescription = new TerrainDescription(heightMap, texture);

        //    string name = "PROCEDURALLY_GENERATED_DEFAULT_LEVEL.lvl";
        //    mLastLoadedLevelName = name;

        //    return name;
        //}

        static public Level GenerateNewLevel(string levelName, string defaultTexture)
        {
            Level level = new Level(levelName);

            GenerateBlankLevelBlock(level, Vector3.Zero, defaultTexture);

            return level;
        }

        static public bool GenerateBlankLevelBlock(Level level, Vector3 coordinate, string defaultTextureName)
        {
            if (level.Contains(coordinate))
            {
                return false;
            }

            string[] detailTextureNames = new string[5] { defaultTextureName, null, null, null, null };
            Vector2[] uvOffsets = new Vector2[5] { Vector2.Zero, Vector2.Zero, Vector2.Zero, Vector2.Zero, Vector2.Zero };
            Vector2[] uvScales = new Vector2[5] { Vector2.One, Vector2.One, Vector2.One, Vector2.One, Vector2.One };

            Color[] blankHeightMap = new Color[HeightMapMesh.NUM_VERT_VERTICES * HeightMapMesh.NUM_HORIZ_VERTICES];
            for (int i = 0; i < blankHeightMap.Length; i++)
            {
                blankHeightMap[i] = new Color(0, 0, 0, 0);
            }
            Texture2D blankHeightMapTexture = new Texture2D(GraphicsManager.Device, HeightMapMesh.NUM_HORIZ_VERTICES, HeightMapMesh.NUM_VERT_VERTICES);

            Color[] blankAlphaMap = new Color[HeightMapMesh.NUM_VERT_VERTICES * HeightMapMesh.NUM_HORIZ_VERTICES * HeightMapMesh.NUM_VERT_TEXELS_PER_QUAD * HeightMapMesh.NUM_HORIZ_TEXELS_PER_QUAD];
            for (int i = 0; i < blankAlphaMap.Length; i++)
            {
                blankAlphaMap[i] = new Color(0, 0, 0, 0);
            }
            Texture2D blankAlphaMapTexture = new Texture2D(GraphicsManager.Device, HeightMapMesh.NUM_HORIZ_VERTICES * HeightMapMesh.NUM_HORIZ_TEXELS_PER_QUAD, HeightMapMesh.NUM_VERT_VERTICES * HeightMapMesh.NUM_VERT_TEXELS_PER_QUAD);

            return AddNewLevelBlock(level, coordinate, blankHeightMapTexture, blankAlphaMapTexture, detailTextureNames, uvOffsets, uvScales);
        }

        static public bool AddNewLevelBlock(Level level, Vector3 coordinate, Texture2D heightMap, Texture2D alphaMap, string[] detailTextureNames, Vector2[] uvOffsets, Vector2[] uvScales)
        {
            if (level.Contains(coordinate))
            {
                return false;
            }

            Color[] heightColors = new Color[heightMap.Width * heightMap.Height];
            heightMap.GetData(heightColors);

            float[,] heights = new float[heightMap.Height, heightMap.Width];
            for (int i = 0; i < heightColors.Length; i++)
            {
                heights[i / HeightMapMesh.NUM_HORIZ_VERTICES, i % HeightMapMesh.NUM_HORIZ_VERTICES] = ConvertColorToFloat(heightColors[i]);
            }

            HeightMapMesh heightMapMesh = new HeightMapMesh(heights, alphaMap, detailTextureNames, uvOffsets, uvScales);

            AssetLibrary.AddHeightMap(level.Name + coordinate.ToString(), heightMapMesh);
            level.AddNewBlock(coordinate);
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        static public void Clear()
        {
            mLastLoadedLevelName = null;
            mLastLoadedObjectList = null;
            //mLastLoadedTerrainDescription = null;
        }

        #endregion

        #region Helper Functions

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileStream"></param>
        /// <returns></returns>
        static private void LoadDetailTextureNames(MemoryStream fileStream, out string[] detailTextureNames, out Vector2[] detailTextureUVOffset, out Vector2[] detailTextureUVScale)
        {
            using (StreamReader reader = new StreamReader(fileStream))
            {
                detailTextureNames = new string[5];
                detailTextureUVOffset = new Vector2[5];
                detailTextureUVScale = new Vector2[5];

                while (!reader.EndOfStream)
                {
                    for (int textureNameIndex = 0; textureNameIndex < 5; textureNameIndex++)
                    {
                        string line = reader.ReadLine();
                        string[] parsedLine = line.Split(' ');

                        string textureName = parsedLine[0];

                        if (textureName != "NULLTEXTURE")
                        {
                            float uOffset = float.Parse(parsedLine[1]), vOffset = float.Parse(parsedLine[2]);
                            float uScale = float.Parse(parsedLine[3]), vScale = float.Parse(parsedLine[4]);

                            detailTextureNames[textureNameIndex] = textureName;
                            detailTextureUVOffset[textureNameIndex] = new Vector2(uOffset, vOffset);
                            detailTextureUVScale[textureNameIndex] = new Vector2(uScale, vScale);
                        }
                    }
                }

                reader.Close();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileStream"></param>
        /// <returns></returns>
        static private List<DummyObject> LoadDummyObjects(MemoryStream fileStream)
        {
            List<DummyObject> dummyObjects = new List<DummyObject>();

            XmlRootAttribute root = new XmlRootAttribute();
            root.ElementName = "object";
            root.IsNullable = true;

            using (StreamReader reader = new StreamReader(fileStream))
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(List<DummyObject>), root);

                dummyObjects = (List<DummyObject>)deserializer.Deserialize(reader);

                reader.Close();
            }

            return dummyObjects;
        }

        static private Tuple<string, MemoryStream> SerializedDummies(List<DummyObject> dummies)
        {
            XmlRootAttribute root = new XmlRootAttribute();
            root.ElementName = "object";
            root.IsNullable = true;

            using (var ms = new MemoryStream())
            {
                StreamWriter writer = new StreamWriter(ms);

                XmlSerializer serializer = new XmlSerializer(typeof(List<DummyObject>), root);
                serializer.Serialize(writer, dummies);
                writer.Close();

                return new Tuple<string, MemoryStream>("Objects.col", new MemoryStream(ms.ToArray(), false));
            }
        }

        /// <summary>
        /// parses line for integers separated by .
        /// </summary>
        static private List<int> ParseStringForIntegers(string line, char separator)
        {
            List<int> result = new List<int>();

            string[] intStrings = line.Split(separator);
            foreach (string num in intStrings)
            {
                int value;
                if (Int32.TryParse(num, out value))
                {
                    result.Add(value);
                }
            }

            return result;
        }

        static private float ConvertColorToFloat(Color heightColor)
        {
            return heightColor.R / 255.0f;
        }

        #endregion
    }
}
