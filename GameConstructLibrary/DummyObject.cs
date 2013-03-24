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

        [XmlIgnoreAttribute]
        public TransformableEntity Entity;
        [XmlIgnoreAttribute]
        public Vector3 ModelOffset;

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

        void UpdateEntityParameters()
        {
            Entity.Transform = Matrix3X3.CreateFromMatrix(Matrix.CreateScale(Scale) * Matrix.CreateTranslation(-ModelOffset));
            Entity.OrientationMatrix = Matrix3X3.CreateFromMatrix(Matrix.CreateFromYawPitchRoll(YawPitchRoll.X, YawPitchRoll.Y, YawPitchRoll.Z));
            Entity.Position = Position;
        }

        /// <summary>
        /// Add the object to a physics space.
        /// Will remove previously added entity.
        /// </summary>
        /// <param name="space">The Space to add the entity to.</param>
        void AddToSpace(Space space)
        {
            if (space.Entities.Contains(Entity))
            {
                space.Remove(Entity);
            }
            Vector3[] vertices;
            int[] indices;
            CollisionMeshManager.LookupMesh(Model, out vertices, out indices);
            Vector3 localPosition;
            ConvexHullShape entity = new ConvexHullShape(vertices, out localPosition);
            ModelOffset = localPosition;
            //Entity entity = new MobileMesh(vertices, indices, new AffineTransform(Scale, Quaternion.CreateFromYawPitchRoll(YawPitchRoll.X, YawPitchRoll.Y, YawPitchRoll.Z), Position), MobileMeshSolidity.Solid);
            Entity = new TransformableEntity(Position, entity,
                Matrix3X3.CreateFromMatrix(Matrix.CreateScale(Scale) * Matrix.CreateTranslation(-localPosition)));
            Entity.OrientationMatrix = Matrix3X3.CreateFromMatrix(Matrix.CreateFromYawPitchRoll(YawPitchRoll.X, YawPitchRoll.Y, YawPitchRoll.Z));
            space.Add(Entity);
            //return entity;
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
