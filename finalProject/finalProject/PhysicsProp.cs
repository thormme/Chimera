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
using GameConstructLibrary;
using BEPUphysics.Entities.Prefabs;
using Microsoft.Xna.Framework.Graphics;

namespace finalProject
{
    public class PhysicsProp : PhysicsObject
    {
        Vector3 mCorrectionTranslate;

        private static Entity CreateEntity(String modelName, Vector3 translation, Quaternion orientation, Vector3 scale, float mass)
        {
            Vector3[] vertices;
            int[] indices;
            CollisionMeshManager.LookupMesh(modelName, out vertices, out indices);
            
            Vector3[] modVertices = new Vector3[vertices.Length];
            for (int i = 0; i < vertices.Length; i++)
            {
                modVertices[i] = scale * vertices[i];
            }
            Entity entity = new ConvexHull(modVertices, mass);
            return entity;
            //return new MobileMesh(vertices, indices, new AffineTransform(scale, orientation, translation), MobileMeshSolidity.Solid, mass);
        }

        public PhysicsProp(String modelName, Vector3 translation, Quaternion orientation, Vector3 scale, float mass)
            : base(new InanimateModel(modelName), CreateEntity(modelName, translation, orientation, scale, mass))
        {
            mCorrectionTranslate = Entity.Position;
            Position = translation;
            XNAOrientationMatrix = Matrix.CreateFromQuaternion(orientation);
            Scale = scale;
        }

                /// <summary>
        /// Constructor for use by the World level loading.
        /// </summary>
        /// <param name="modelName">Name of the model.</param>
        /// <param name="translation">The position.</param>
        /// <param name="orientation">The orientation.</param>
        /// <param name="scale">The amount to scale by.</param>
        /// <param name="extraParameters">Extra parameters.</param>
        public PhysicsProp(String modelName, Vector3 translation, Quaternion orientation, Vector3 scale, string[] extraParameters)
            : this(modelName, translation, orientation, scale, Convert.ToSingle(extraParameters[0]))
        {
        }

        public override void Render()
        {
            mRenderable.Render(Matrix.CreateScale(Scale) * Matrix.CreateTranslation(-mCorrectionTranslate) * XNAOrientationMatrix * Matrix.CreateTranslation(Position));
        }
    }
}
