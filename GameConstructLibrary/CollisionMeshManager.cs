using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using GraphicsLibrary;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using BEPUphysics.DataStructures;
using BEPUphysics.CollisionShapes;

namespace GameConstructLibrary
{
    public static class CollisionMeshManager
    {
        static private string mBASE_DIRECTORY = DirectoryManager.GetRoot() + "Chimera/ChimeraContent/";
        static private string mModelDirectory = mBASE_DIRECTORY + "models/";

        static private Dictionary<string, InstancedMeshShape> mUniqueMeshLibrary = new Dictionary<string, InstancedMeshShape>();
        static private Dictionary<string, Model> mUniqueMeshModelLibrary = new Dictionary<string, Model>();

        static private void LoadMeshIntoDatabase(ContentManager content, String directoryName, String meshName)
        {
            Model input;

            string meshAndDirectoryName = directoryName + "/" + meshName;

            FileInfo meshFile = new FileInfo(content.RootDirectory + "\\" + "models/" + meshAndDirectoryName + "_collision" + ".xnb");
            FileInfo modelFile = new FileInfo(content.RootDirectory + "\\" + "models/" + meshAndDirectoryName + ".xnb");
            if (meshFile.Exists)
            {
                input = content.Load<Model>("models/" + meshAndDirectoryName + "_collision");
            }
            else if (modelFile.Exists)
            {
                input = content.Load<Model>("models/" + meshAndDirectoryName);
            }
            else
            {
                throw new FileNotFoundException("Could not find '" + meshAndDirectoryName + "' models/collision or models directory in content.");
            }

            Vector3[] staticTriangleVertices;
            int[] staticTriangleIndices;
            Vector3 scale = new Vector3();
            Vector3 translate = new Vector3();
            Quaternion rotate = new Quaternion();
            input.Root.Transform.Decompose(out scale, out rotate, out translate);
            TriangleMesh.GetVerticesAndIndicesFromModel(input, out staticTriangleVertices, out staticTriangleIndices);

            Matrix inverseRotation = Matrix.Invert(Matrix.CreateFromQuaternion(rotate));

            for (int i = 0; i < staticTriangleVertices.Length; i++)
            {
                staticTriangleVertices[i] -= translate;
                Vector3.Transform(staticTriangleVertices[i], inverseRotation);
                staticTriangleVertices[i] /= scale;
            }

            if (mUniqueMeshLibrary.ContainsKey(meshName))
            {
                throw new Exception("Duplicate model key: " + meshName);
            }
            mUniqueMeshLibrary.Add(meshName, new InstancedMeshShape(staticTriangleVertices, staticTriangleIndices));

            if (mUniqueMeshModelLibrary.ContainsKey(meshName))
            {
                throw new Exception("Duplicate model key: " + meshName);
            }
            mUniqueMeshModelLibrary.Add(meshName, input);
        }

        /// <summary>
        /// Loads all meshes from content/models/collision/ into memory.
        /// </summary>
        /// <param name="content">Global game content manager.</param>
        static public void LoadContent(ContentManager content)
        {
            // Load models.
            DirectoryInfo dir = new DirectoryInfo(content.RootDirectory + "\\" + "models");
            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException("Could not find models/ directory in content.");
            }

            DirectoryInfo[] subDirs = dir.GetDirectories();
            foreach (DirectoryInfo subDir in subDirs)
            {
                FileInfo[] files = subDir.GetFiles("*.*x*");
                foreach (FileInfo file in files)
                {
                    string meshName = Path.GetFileNameWithoutExtension(file.Name);
                    try
                    {
                        LoadMeshIntoDatabase(content, subDir.Name, meshName);
                    }
                    catch (Exception e)
                    {
                    }
                }
            }
        }

        /// <summary>
        /// Retrieves static mesh from database.  Throws KeyNotFoundException if meshName does not exist in database.
        /// </summary>
        /// <param name="meshName">Name of the mesh.</param>
        /// <returns>A mesh to be used in static collidables.</returns>
        static public InstancedMeshShape LookupStaticMesh(string meshName)
        {
            InstancedMeshShape result;
            if (mUniqueMeshLibrary.TryGetValue(meshName, out result))
            {
                return result;
            }
            throw new KeyNotFoundException("Unable to find mesh key: " + meshName);
        }

        /// <summary>
        /// Retrieves mesh vertices and indices from database.  Throws KeyNotFoundException if meshName does not exist in database.
        /// </summary>
        /// <param name="meshName">Name of the mesh.</param>
        /// <param name="vertices">Outputs the vertices contained by the mesh.</param>
        /// <param name="indices">Array of indices used by the mesh's triangles.</param>
        static public void LookupMesh(string meshName, out Vector3[] vertices, out int[] indices)
        {
            Model result;
            if (mUniqueMeshModelLibrary.TryGetValue(meshName, out result))
            {
                TriangleMesh.GetVerticesAndIndicesFromModel(result, out vertices, out indices);
                return;
            }
            throw new KeyNotFoundException("Unable to find mesh key: " + meshName);
        }
    }
}
