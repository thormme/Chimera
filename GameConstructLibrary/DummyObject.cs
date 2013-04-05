using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GraphicsLibrary;
using Utility;
using System.Xml.Serialization;
using BEPUphysics.Entities;
using BEPUphysics;
using BEPUphysics.MathExtensions;
using BEPUphysics.Entities.Prefabs;
using BEPUphysics.CollisionShapes;
using BEPUphysics.EntityStateManagement;
using BEPUphysics.CollisionShapes.ConvexShapes;

namespace GameConstructLibrary
{
    public class DummyObject
    {
        private static UInt32 mNextObjectID = 1;

        public string Type { get; set; }
        private string mModel { get; set; }
        public string Model
        {
            get
            {
                return mModel;
            }
            set
            {
                mDrawableModel = new InanimateModel(value);
                mDrawableModel.ObjectID = mNextObjectID++;
                mModel = value;
            }
        }
        public string[] Parameters { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 YawPitchRoll { get; set; }
        public Vector3 Scale { get; set; }
        public Vector3 RotationAxis { get; set; }
        public float RotationAngle { get; set; }
        public float Height { get; set; }
        public bool Floating { get; set; }

        [XmlIgnore]
        public InanimateModel mDrawableModel { get; set; }
        [XmlIgnore]
        public UInt32 ObjectID
        {
            get
            {
                return mDrawableModel.ObjectID;
            }
        }

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
            Vector3 finalPosition = new Vector3(Position.X, Position.Y + Height * Utils.WorldScale.Y, Position.Z);
            Matrix orientation = RotationAngle == 0 ? Matrix.CreateFromYawPitchRoll(YawPitchRoll.X, YawPitchRoll.Y, YawPitchRoll.Z) : Matrix.CreateFromAxisAngle(RotationAxis, RotationAngle);
            mDrawableModel.Render(finalPosition, orientation, Scale);
        }

    }
}
