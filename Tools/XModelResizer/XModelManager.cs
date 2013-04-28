using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace XModelResizer
{
    public static class XModelManager
    {
        private const string PositionStartPoint = "Mesh mesh_";
        private const string NormalStartPoint = "MeshNormals normals {";

        private static List<Vector3> mVertices = new List<Vector3>();
        private static List<string> mBufferBeforeVertices = new List<string>();
        private static List<string> mBufferAfterVertices = new List<string>();
        private static List<string> mBufferVertices = new List<string>();

        public static void LoadModel(string fileNameAndPath)
        {
            Reset();

            StreamReader reader = new StreamReader(fileNameAndPath);

            ParseUntilRegEx(reader, PositionStartPoint, mBufferBeforeVertices);

            string line;
            int numVertices = ParseCount(reader, mBufferBeforeVertices);
            for (int vertexCount = 0; vertexCount < numVertices; ++vertexCount)
            {
                line = reader.ReadLine();
                Vector3 vertexPosition = ParseStringForVector(line);
                mVertices.Add(vertexPosition);
            }

            ParseAll(reader, mBufferAfterVertices);

            reader.Close();
        }

        public static bool SaveModel(string fileNameAndPath, float scale)
        {
            if (mVertices.Count == 0)
            {
                return false;
            }

            Matrix scaleTransform = Matrix.CreateScale(scale);

            for (int vertexIndex = 0; vertexIndex < mVertices.Count; ++vertexIndex)
            {
                mVertices[vertexIndex] = Vector3.Transform(mVertices[vertexIndex], scaleTransform);
                mBufferVertices.Add(ParseVectorForString(mVertices[vertexIndex]) + (vertexIndex == mVertices.Count - 1 ? ";" : ","));
            }

            StreamWriter writer = new StreamWriter(fileNameAndPath);

            foreach (string lineBefore in mBufferBeforeVertices)
            {
                writer.WriteLine(lineBefore);
            }

            foreach (string normal in mBufferVertices)
            {
                writer.WriteLine(normal);
            }

            foreach (string lineAfter in mBufferAfterVertices)
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

        private static string ParseVectorForString(Vector3 vector)
        {
            return vector.X.ToString() + ";" + vector.Y.ToString() + ";" + vector.Z.ToString() + ";";
        }

        private static void Reset()
        {
            mVertices.Clear();
            mBufferBeforeVertices.Clear();
            mBufferVertices.Clear();
            mBufferAfterVertices.Clear();
        }
    }
}
