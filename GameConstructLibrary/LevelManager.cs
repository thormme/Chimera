using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using System.Reflection;
using System.Xml.Serialization;
using GraphicsLibrary;

namespace GameConstructLibrary
{
    /// <summary>
    /// Saves and Loads object data for a level.
    /// </summary>
    public static class LevelManager
    {
        public static void Save(string fileName, List<DummyObject> objects)
        {
            XmlRootAttribute root = new XmlRootAttribute();
            root.ElementName = "object";
            root.IsNullable = true;

            StreamWriter writer = new StreamWriter(DirectoryManager.GetRoot() + "Chimera/ChimeraContent/levels/" + fileName);

            XmlSerializer serializer = new XmlSerializer(typeof(List<DummyObject>), root);
            serializer.Serialize(writer, objects);
            writer.Close();
        }

        public static List<DummyObject> Load(string fileName)
        {
            XmlRootAttribute root = new XmlRootAttribute();
            root.ElementName = "object";
            root.IsNullable = true;

            List<DummyObject> objects = new List<DummyObject>();

            StreamReader reader = new StreamReader("Content\\" + "levels/" + fileName);
            XmlSerializer deserializer = new XmlSerializer(typeof(List<DummyObject>), root);

            objects = (List<DummyObject>)deserializer.Deserialize(reader);

            reader.Close();
            return objects;
        }

        /// <summary>
        /// Check whether a level exists.
        /// </summary>
        /// <param name="file">The level name.</param>
        /// <returns>True if the level exists.</returns>
        public static bool Exists(string file)
        {
            FileInfo meshFile = new FileInfo("Content\\" + "levels/" + file);
            return meshFile.Exists;
        }
    }
}
