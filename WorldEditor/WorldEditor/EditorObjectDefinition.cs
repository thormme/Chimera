using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using GameConstructLibrary;
using Microsoft.Xna.Framework;

namespace WorldEditor
{
    class EditorObjectDefinition
    {
        public string EditorType
        {
            get;
            protected set;
        }
        public string GameType
        {
            get;
            protected set;
        }
        public string Model
        {
            get;
            protected set;
        }
        public string[] Parameters
        {
            get;
            protected set;
        }

        public EditorObjectDefinition(FileInfo file)
        {
            string[] data = System.IO.File.ReadAllLines(file.FullName);

            string editorType = Path.GetFileNameWithoutExtension(file.Name);
            string gameType = data[0];
            string model = data[1];

            List<string> parameters = new List<string>();
            if (data.Length - 2 > 0)
            {
                for (int count = 0; count < data.Length - 2; count++)
                {
                    parameters.Add(data[count + 2]);
                }
            }

            EditorType = editorType;
            GameType = gameType;
            Model = model;
            Parameters = parameters.ToArray();
        }

        public EditorObjectDefinition(string editorType, string gameType, string model, string[] parameters)
        {
            EditorType = editorType;
            GameType = gameType;
            Model = model;
            Parameters = parameters.ToArray();
        }

        public DummyObject CreateDummyObject()
        {
            DummyObject tempObject = new DummyObject();

            tempObject.Type = GameType;
            tempObject.Model = Model;

            tempObject.Parameters = Parameters.ToArray();

            tempObject.Position = Vector3.Zero;
            tempObject.YawPitchRoll = Vector3.Up;
            tempObject.Scale = Vector3.One;
            tempObject.Height = 0.0f;

            return tempObject;
        }
    }
}
