using System;
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
        static private TerrainDescription mLastLoadedTerrainDescription = null;
        static private List<DummyObject> mLastLoadedObjectList = null;

        #endregion

        #region Read / Write Level Files.

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePathAndName"></param>
        /// <returns></returns>
        static public TerrainHeightMap LoadHeightMapFromFile(FileInfo filePathAndName)
        {
            if (!filePathAndName.Name.Contains(".lvl"))
            {
                filePathAndName = new FileInfo(filePathAndName.Name + ".lvl");
            }

            if (mLastLoadedLevelName == null || filePathAndName.Name != mLastLoadedLevelName)
            {
                LoadLevelFromFile(filePathAndName);
            }

            return mLastLoadedTerrainDescription.Terrain;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePathAndName"></param>
        /// <returns></returns>
        static public TerrainTexture LoadTextureFromFile(FileInfo filePathAndName)
        {
            if (!filePathAndName.Name.Contains(".lvl"))
            {
                filePathAndName = new FileInfo(filePathAndName.Name + ".lvl");
            }

            if (mLastLoadedLevelName == null || filePathAndName.Name != mLastLoadedLevelName)
            {
                LoadLevelFromFile(filePathAndName);
            }

            return mLastLoadedTerrainDescription.Texture;
        }

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePathAndName"></param>
        static public void SaveLevelToFile(FileInfo filePathAndName)
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

            string[] entryNames = { "AlphaMap.png", "DetailTextures.ctl", "HeightMap.png", "Objects.col" };
            MemoryStream[] entryStreams = { mLastLoadedTerrainDescription.Texture.ExportTextureToStream(), mLastLoadedTerrainDescription.Texture.ExportDetailListToStream(), mLastLoadedTerrainDescription.Terrain.ExportToStream(), CreateObjectStream() };

            zipStream.SetLevel(5);

            for (int i = 0; i < entryNames.Length; ++i)
            {
                ZipEntry newEntry = new ZipEntry(entryNames[i]);
                newEntry.DateTime = DateTime.Now;

                zipStream.PutNextEntry(newEntry);

                StreamUtils.Copy(entryStreams[i], zipStream, new byte[4096]);
                zipStream.CloseEntry();
            }

            zipStream.IsStreamOwner = true;
            zipStream.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="heightMapWidth"></param>
        /// <param name="heightMapHeight"></param>
        /// <param name="alphaMapWidth"></param>
        /// <param name="alphaMapHeight"></param>
        /// <param name="chunkRows"></param>
        /// <param name="chunkCols"></param>
        /// <param name="defaultTexture"></param>
        /// <returns></returns>
        static public string GenerateBlankLevel(int heightMapWidth, int heightMapHeight, int alphaMapWidth, int alphaMapHeight, int chunkRows, int chunkCols, string defaultTexture)
        {
            Color[] heightMapBuffer = new Color[heightMapHeight * heightMapWidth];
            for (int i = 0; i < heightMapBuffer.Length; ++i)
            {
                heightMapBuffer[i] = new Color(0, 255, 0, 0);
            }
            Texture2D heightMapTexture = new Texture2D(GraphicsManager.Device, heightMapWidth, heightMapHeight);
            heightMapTexture.SetData(heightMapBuffer);

            Color[] alphaMapBuffer = new Color[alphaMapHeight * alphaMapWidth];
            for (int i = 0; i < alphaMapBuffer.Length; ++i)
            {
                alphaMapBuffer[i] = new Color(0, 0, 0, 0);
            }
            Texture2D alphaMapTexture = new Texture2D(GraphicsManager.Device, alphaMapWidth, alphaMapHeight);
            alphaMapTexture.SetData(alphaMapBuffer);

            string[, ,] detailTextureNames = new string[chunkRows, chunkCols, 5];
            Vector2[, ,] detailTextureUVOffsets = new Vector2[chunkRows, chunkCols, 5];
            Vector2[, ,] detailTextureUVScales = new Vector2[chunkRows, chunkCols, 5];

            for (int row = 0; row < chunkRows; ++row)
            {
                for (int col = 0; col < chunkCols; ++col)
                {
                    detailTextureNames[row, col, 0] = defaultTexture;
                    for (int index = 0; index < 5; ++index)
                    {
                        detailTextureUVOffsets[row, col, index] = Vector2.Zero;
                        detailTextureUVScales[row, col, index] = Vector2.One;
                    }
                }
            }

            TerrainHeightMap heightMap = new TerrainHeightMap(heightMapTexture, chunkRows, chunkCols, GraphicsManager.Device);
            TerrainTexture texture = new TerrainTexture(alphaMapTexture, detailTextureNames, detailTextureUVOffsets, detailTextureUVScales, chunkRows, chunkCols, GraphicsManager.Device);

            mLastLoadedTerrainDescription = new TerrainDescription(heightMap, texture);

            string name = "PROCEDURALLY_GENERATED_DEFAULT_LEVEL.lvl";
            mLastLoadedLevelName = name;

            return name;
        }

        /// <summary>
        /// 
        /// </summary>
        static public void Clear()
        {
            mLastLoadedLevelName = null;
            mLastLoadedObjectList = null;
            mLastLoadedTerrainDescription = null;
        }

        #endregion

        #region Helper Functions

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePathAndName"></param>
        static private void LoadLevelFromFile(FileInfo filePathAndName)
        {
            ZipFile zipFile = null;
            try
            {
                int numChunks = 9;

                FileStream fileStream = File.OpenRead(filePathAndName.FullName);
                zipFile = new ZipFile(fileStream);

                Texture2D alphaMap = null, heightMap = null;
                string[, ,] detailTextureNames = null;
                Vector2[, ,] detailTextureUVOffsets = null;
                Vector2[, ,] detailTextureUVScales = null;

                mLastLoadedObjectList = new List<DummyObject>();

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

                    if (entryFileName.Contains("AlphaMap"))
                    {
                        alphaMap = Texture2D.FromStream(GraphicsManager.Device, memoryStream);
                    }
                    else if (entryFileName.Contains("HeightMap"))
                    {
                        heightMap = Texture2D.FromStream(GraphicsManager.Device, memoryStream);
                    }
                    else if (entryFileName.Contains("DetailTextures"))
                    {
                        LoadDetailTextureNames(memoryStream, out detailTextureNames, out detailTextureUVOffsets, out detailTextureUVScales);
                    }
                    else if (entryFileName.Contains("Objects"))
                    {
                        mLastLoadedObjectList = LoadDummyObjects(memoryStream);
                    }

                    zipStream.Close();
                }

                TerrainHeightMap terrainHeightMap = new TerrainHeightMap(heightMap, numChunks, numChunks, GraphicsManager.Device);
                TerrainTexture texture = new TerrainTexture(alphaMap, detailTextureNames, detailTextureUVOffsets, detailTextureUVScales, numChunks, numChunks, GraphicsManager.Device);

                mLastLoadedTerrainDescription = new TerrainDescription(terrainHeightMap, texture);
                mLastLoadedLevelName = filePathAndName.Name;
            }
            finally
            {
                if (zipFile != null)
                {
                    zipFile.IsStreamOwner = true;
                    zipFile.Close();
                }

                int numDummies = mLastLoadedObjectList.Count;

                bool containsPlayer = false, containsGoal = false;

                for (int i = 0; i < numDummies; ++i)
                {
                    if (mLastLoadedObjectList[i].Type == "Chimera.PlayerCreature, Chimera")
                    {
                        containsPlayer = true;
                    }

                    if (mLastLoadedObjectList[i].Type == "Chimera.GoalPoint, Chimera")
                    {
                        containsGoal = true;
                    }
                }

                if (containsPlayer == false)
                {
                    DummyObject player = new DummyObject();
                    player.Type = "Chimera.PlayerCreature, Chimera";
                    player.Model = "playerBean";
                    player.Position = new Vector3(0.0f, mLastLoadedTerrainDescription.Terrain.HeightAt(0.0f, 0.0f), 0.0f);
                    player.YawPitchRoll = Vector3.Zero;
                    player.Scale = Vector3.Zero;
                    player.RotationAxis = Vector3.Zero;
                    player.RotationAngle = 0.0f;
                    player.Height = 0.0f;

                    mLastLoadedObjectList.Add(player);
                }

                if (containsGoal == false)
                {
                    DummyObject goal = new DummyObject();
                    goal.Type = "Chimera.GoalPoint, Chimera";
                    goal.Model = "tractorBeam";
                    goal.Parameters = new string[2] { null, "PenguinLimbs" };
                    goal.Position = new Vector3(50.0f, mLastLoadedTerrainDescription.Terrain.HeightAt(50.0f, 50.0f), 50.0f);
                    goal.YawPitchRoll = Vector3.Zero;
                    goal.Scale = new Vector3(0.125f, 100f, 0.125f);
                    goal.RotationAxis = Vector3.Zero;
                    goal.RotationAngle = 0.0f;
                    goal.Height = 0.0f;

                    mLastLoadedObjectList.Add(goal);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileStream"></param>
        /// <returns></returns>
        static private void LoadDetailTextureNames(MemoryStream fileStream, out string[, ,] detailTextureNames, out Vector2[, ,] detailTextureUVOffset, out Vector2[, ,] detailTextureUVScale)
        {
            using (StreamReader reader = new StreamReader(fileStream))
            {
                List<int> size = ParseStringForIntegers(reader.ReadLine(), ' ');
                int numChunksRow = size[0];
                int numChunksCol = size[1];

                detailTextureNames = new string[numChunksRow, numChunksCol, 5];
                detailTextureUVOffset = new Vector2[numChunksRow, numChunksCol, 5];
                detailTextureUVScale = new Vector2[numChunksRow, numChunksCol, 5];

                string intString = reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    List<int> chunkDetails = ParseStringForIntegers(intString, ' ');

                    for (int textureNameIndex = 0; textureNameIndex < 5; textureNameIndex++)
                    {
                        string line = reader.ReadLine();
                        string[] parsedLine = line.Split(' ');

                        string textureName = parsedLine[0];

                        if (textureName != "NULLTEXTURE")
                        {
                            float uOffset = float.Parse(parsedLine[1]), vOffset = float.Parse(parsedLine[2]);
                            float uScale = float.Parse(parsedLine[3]), vScale = float.Parse(parsedLine[4]);

                            detailTextureNames[chunkDetails[0], chunkDetails[1], textureNameIndex] = textureName;
                            detailTextureUVOffset[chunkDetails[0], chunkDetails[1], textureNameIndex] = new Vector2(uOffset, vOffset);
                            detailTextureUVScale[chunkDetails[0], chunkDetails[1], textureNameIndex] = new Vector2(uScale, vScale);
                        }
                    }

                    intString = reader.ReadLine();
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

        static private MemoryStream CreateObjectStream()
        {
            XmlRootAttribute root = new XmlRootAttribute();
            root.ElementName = "object";
            root.IsNullable = true;

            using (var ms = new MemoryStream())
            {
                StreamWriter writer = new StreamWriter(ms);

                XmlSerializer serializer = new XmlSerializer(typeof(List<DummyObject>), root);
                serializer.Serialize(writer, mLastLoadedObjectList);
                writer.Close();

                return new MemoryStream(ms.ToArray(), false);
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

        #endregion
    }
}
