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
        public Vector3 YawPitchRoll { get; set; }
        public Vector3 Scale { get; set; }
        public Vector3 RotationAxis { get; set; }
        public float RotationAngle { get; set; }
        public float Height { get; set; }
        public bool Floating { get; set; }

        public DummyObject()
        {
            RotationAngle = 0;
        }

        public DummyObject(DummyObject copy)
        {
            Type = copy.Type;
            Model = copy.Model;
            Parameters = copy.Parameters;
            Position = copy.Position;
            YawPitchRoll = copy.YawPitchRoll;
            Scale = copy.Scale;
            Height = copy.Height;
            Floating = copy.Floating;
        }

        public void Draw()
        {
            InanimateModel tempModel = new InanimateModel(Model);
            Vector3 finalPosition = new Vector3(Position.X, Position.Y + Height * Utils.WorldScale.Y, Position.Z);
            Matrix orientation = RotationAngle == 0 ? Matrix.CreateFromYawPitchRoll(YawPitchRoll.X, YawPitchRoll.Y, YawPitchRoll.Z) : Matrix.CreateFromAxisAngle(RotationAxis, RotationAngle);
            tempModel.Render(finalPosition, orientation, Scale);
        }

    }
}
