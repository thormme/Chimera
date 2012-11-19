using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MapEditor
{
    public class DummyObject
    {

        public string Type { get; set; }
        public string Model { get; set; }
        public string[] Parameters { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Orientation { get; set; }
        public Vector3 Scale { get; set; }

        public DummyObject()
        {

        }

        public DummyObject(DummyObject copy)
        {
            Type = copy.Type;
            Model = copy.Model;
            Parameters = copy.Parameters;
            Position = new Vector3(copy.Position.X, copy.Position.Y, copy.Position.Z);
            Orientation = new Vector3(copy.Orientation.X, copy.Orientation.Y, copy.Orientation.Z);
            Scale = new Vector3(copy.Scale.X, copy.Scale.Y, copy.Scale.Z);
        }

    }
}
