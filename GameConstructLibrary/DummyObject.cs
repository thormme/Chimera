using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GraphicsLibrary;
using Utility;

namespace GameConstructLibrary
{
    public class DummyObject
    {

        public string Type { get; set; }
        public string Model { get; set; }
        public string[] Parameters { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Orientation { get; set; }
        public Vector3 Scale { get; set; }
        public float Height { get; set; }

        public DummyObject()
        {

        }

        public DummyObject(DummyObject copy)
        {
            Type = copy.Type;
            Model = copy.Model;
            Parameters = copy.Parameters;
            Position = copy.Position;
            Orientation = copy.Orientation;
            Scale = copy.Scale;
            Height = copy.Height;
        }

        public void Draw()
        {
            InanimateModel tempModel = new InanimateModel(Model);
            tempModel.Render(
                new Vector3(Position.X, Position.Y + Height * Utils.WorldScale.Y, Position.Z),
                Matrix.CreateFromYawPitchRoll(Orientation.X, Orientation.Y, Orientation.Z),
                Scale);
        }

    }
}
