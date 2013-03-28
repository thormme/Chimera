using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace XModelNormalUnifier
{
    public static class XModelManager
    {
        private const string PositionStartPoint = "Mesh mesh_";
        private const string NormalStartPoint = "MeshNormals normals {";

        private static Dictionary<int, Vector3> mIndexPositionDictionary = new Dictionary<int, Vector3>();
        private static Dictionary<Vector3, Vector3> mPositionNormalDictionary = new Dictionary<Vector3,Vector3>();
        private static List<string> mBufferBeforeNormals = new List<string>();
        private static List<string> mBufferAfterNormals = new List<string>();
        private static List<string> mBufferNormals = new List<string>();

        public static void LoadModel(string fileNameAndPath)
        {
            Reset();

            StreamReader reader = new StreamReader(fileNameAndPath);

            ParseUntilRegEx(reader, PositionStartPoint, mBufferBeforeNormals);

            string line;
            int numVertices = ParseCount(reader, mBufferBeforeNormals);
            for (int vertexCount = 0; vertexCount < numVertices; ++vertexCount)
            {
                line = reader.ReadLine();
                mBufferBeforeNormals.Add(line);

                Vector3 position = ParseStringForVector(line);
                mIndexPositionDictionary.Add(vertexCount, position);
            }

            ParseUntilRegEx(reader, NormalStartPoint, mBufferBeforeNormals);

            int normalCount;
            int numNormals = ParseCount(reader, mBufferBeforeNormals);
            for (normalCount = 0; normalCount < numNormals; ++normalCount)
            {
                line = reader.ReadLine();
                Vector3 normal = ParseStringForVector(line);

                Vector3 position = mIndexPositionDictionary[normalCount];
                if (!mPositionNormalDictionary.ContainsKey(position))
                {
                    mPositionNormalDictionary.Add(position, Vector3.Zero);
                }
                mPositionNormalDictionary[position] += normal;
            }

            for (normalCount = 0; normalCount < numNormals; ++normalCount)
            {
                Vector3 unifiedNormal = Vector3.Normalize(mPositionNormalDictionary[mIndexPositionDictionary[normalCount]]);
                mBufferNormals.Add(unifiedNormal.X.ToString() + ";" + unifiedNormal.Y.ToString() + ";" + unifiedNormal.Z.ToString() + ";,");
            }

            ParseAll(reader, mBufferAfterNormals);

            reader.Close();
        }

        public static bool SaveModel(string fileNameAndPath)
        {
            if (mBufferNormals.Count == 0)
            {
                return false;
            }

            StreamWriter writer = new StreamWriter(fileNameAndPath);

            foreach (string lineBefore in mBufferBeforeNormals)
            {
                writer.WriteLine(lineBefore);
            }

            foreach (string normal in mBufferNormals)
            {
                writer.WriteLine(normal);
            }

            foreach (string lineAfter in mBufferAfterNormals)
            {
                writer.WriteLine(lineAfter);
            }

            writer.Close();

            return true;
        }

        private static void ParseUntilRegEx(StreamReader reader, string regExString, List<string> buffer)
        {
            string line;
            while(!(line = reader.ReadLine()).Contains(regExString))
            {
                buffer.Add(line);
            }
            buffer.Add(line);
        }

        private static void ParseAll(StreamReader reader, List<string> buffer)
        {
            while (!reader.EndOfStream)
            {
                buffer.Add(reader.ReadLine());
            }
        }

        private static int ParseCount(StreamReader reader, List<string> buffer)
        {
            string line = reader.ReadLine();
            buffer.Add(line);
            return Int32.Parse(line.Split(';')[0]);
        }

        private static Vector3 ParseStringForVector(string line)
        {
            string[] coordinates = line.Split(';');
            return new Vector3(float.Parse(coordinates[0]), float.Parse(coordinates[1]), float.Parse(coordinates[2]));
        }

        private static void Reset()
        {
            mBufferBeforeNormals.Clear();
            mBufferNormals.Clear();
            mBufferAfterNormals.Clear();
            mIndexPositionDictionary.Clear();
            mPositionNormalDictionary.Clear();
        }
    }
}
