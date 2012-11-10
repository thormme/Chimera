using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using System.Reflection;
using System.Xml.Serialization;
using GameConstructLibrary;

namespace MapEditor
{

    class LevelManager
    {

        private StreamWriter writer;
        private StreamReader reader;

        private XmlRootAttribute root;

        public LevelManager()
        {
            root = new XmlRootAttribute();
            root.ElementName = "object";
            root.IsNullable = true;
        }

        public void Save(string file, List<DummyObject> objects)
        {
            Console.WriteLine(objects.Count());
            writer = new StreamWriter(DirectoryManager.GetRoot() + "finalProject/finalProjectContent/levels/" + file);
            foreach (DummyObject obj in objects)
            {
                XmlSerializer serializer = new XmlSerializer(obj.GetType(), root);
                serializer.Serialize(writer, obj);
            }
            writer.Close();
        }

        public List<DummyObject> Load(string file)
        {
            List<DummyObject> objects = new List<DummyObject>();

            reader = new StreamReader(DirectoryManager.GetRoot() + "finalProject/finalProjectContent/levels/" + file);
            XmlSerializer deserializer = new XmlSerializer(typeof(List<DummyObject>), root);

            objects = (List<DummyObject>)deserializer.Deserialize(reader);

            reader.Close();
            return objects;
        }
    }
}
