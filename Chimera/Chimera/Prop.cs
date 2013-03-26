using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using BEPUphysics.Collidables;
using GraphicsLibrary;
using Microsoft.Xna.Framework.Graphics;
using BEPUphysics.MathExtensions;
using GameConstructLibrary;

namespace Chimera
{
    /// <summary>
    /// An immovable prop with a model and mesh based collision.
    /// </summary>
   public  class Prop : IGameObject, IStaticCollidableOwner
    {
        private Renderable mRenderable;

        /// <summary>
        /// Construct a new immovable Prop.
        /// </summary>
        /// <param name="modelName">Name of the model.</param>
        /// <param name="translation">The position.</param>
        /// <param name="orientation">The orientation.</param>
        /// <param name="scale">The amount to scale by.</param>
        public Prop(String modelName, Vector3 translation, Quaternion orientation, Vector3 scale)
        {
            mRenderable = new InanimateModel(modelName);
            StaticCollidable = new InstancedMesh(
                CollisionMeshManager.LookupStaticMesh(modelName),
                new AffineTransform(scale, orientation, translation)
            );
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
        public Prop(String modelName, Vector3 translation, Quaternion orientation, Vector3 scale, string[] extraParameters)
            : this(modelName, translation, orientation, scale)
        {
        }

        public Vector3 Position
        {
            get;
            private set;
        }

        public World World
        {
            protected get;
            set;
        }

        public Matrix XNAOrientationMatrix
        {
            get;
            private set;
        }

        public Vector3 Scale
        {
            get;
            private set;
        }

        public void Render()
        {
            mRenderable.BoundingBox = StaticCollidable.BoundingBox;
            mRenderable.Render(Position, XNAOrientationMatrix, Scale);
        }

        public StaticCollidable StaticCollidable
        {
            get;
            protected set;
        }
    }
}
