﻿using System;
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
        public static void Save(string file, List<DummyObject> objects)
        {
            XmlRootAttribute root = new XmlRootAttribute();
            root.ElementName = "object";
            root.IsNullable = true;

            StreamWriter writer = new StreamWriter(DirectoryManager.GetRoot() + "finalProject/finalProjectContent/levels/" + file);

            XmlSerializer serializer = new XmlSerializer(typeof(List<DummyObject>), root);
            serializer.Serialize(writer, objects);
            writer.Close();
        }

        public static List<DummyObject> Load(string file)
        {
            XmlRootAttribute root = new XmlRootAttribute();
            root.ElementName = "object";
            root.IsNullable = true;

            List<DummyObject> objects = new List<DummyObject>();

            StreamReader reader = new StreamReader("Content\\" + "levels/" + file);
            XmlSerializer deserializer = new XmlSerializer(typeof(List<DummyObject>), root);

            objects = (List<DummyObject>)deserializer.Deserialize(reader);

            reader.Close();
            return objects;
        }
    }
}
