using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GraphicsLibrary;
using Microsoft.Xna.Framework;
using BEPUphysics.Collidables;
using BEPUphysics.MathExtensions;
using BEPUphysics.Entities;
using BEPUphysics.Collidables.MobileCollidables;
using BEPUphysics.CollisionShapes;

namespace GameConstructLibrary
{
    public class PhysicsProp : PhysicsObject
    {
        private static Entity CreateEntity(String modelName, Vector3 translation, Quaternion orientation, Vector3 scale, float mass)
        {
            Vector3[] vertices;
            int[] indices;
            CollisionMeshManager.LookupMesh(modelName, out vertices, out indices);
            return new Entity(new MobileMeshCollidable(new MobileMeshShape(vertices, indices, new AffineTransform(scale, orientation, translation), MobileMeshSolidity.Solid)), mass);
        }

        public PhysicsProp(String modelName, Vector3 translation, Quaternion orientation, Vector3 scale, float mass)
            : base(new InanimateModel(modelName), CreateEntity(modelName, translation, orientation, scale, mass))
        {
            Position = translation;
            XNAOrientationMatrix = Matrix.CreateFromQuaternion(orientation);
            Scale = scale;
        }
    }
}
