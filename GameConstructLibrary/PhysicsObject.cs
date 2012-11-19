using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using BEPUphysics.Entities;
using BEPUphysics.MathExtensions;
using GraphicsLibrary;
using BEPUphysics.CollisionShapes;
using BEPUphysics.Collidables.MobileCollidables;
using BEPUphysics.Collidables;
using BEPUphysics.NarrowPhaseSystems.Pairs;

namespace GameConstructLibrary
{
    /// <summary>
    /// Base class for all game objects which will be affected by the physics simulation.
    /// </summary>
    public class PhysicsObject : IMobileObject, IEntityOwner
    {
        protected List<IGameObject> mCollidingObjects;

        public PhysicsObject(Renderable renderable, Entity entity)
        {
            mRenderable = renderable;
            Scale = new Vector3(1.0f);
            Entity = entity;
            Entity.Tag = this;
            Entity.CollisionInformation.Tag = this;
            mCollidingObjects = new List<IGameObject>();
            Entity.CollisionInformation.Events.InitialCollisionDetected += InitialCollisionDetected;
            Entity.CollisionInformation.Events.CollisionEnded += CollisionEnded;
        }

        protected Renderable mRenderable;

        public Matrix XNAOrientationMatrix
        {
            get
            {
                return new Matrix(
                    Entity.OrientationMatrix.M11, Entity.OrientationMatrix.M12, Entity.OrientationMatrix.M13, 1.0f,
                    Entity.OrientationMatrix.M21, Entity.OrientationMatrix.M22, Entity.OrientationMatrix.M23, 1.0f,
                    Entity.OrientationMatrix.M31, Entity.OrientationMatrix.M32, Entity.OrientationMatrix.M33, 1.0f,
                    1.0f, 1.0f, 1.0f, 1.0f
                    );
            }
            set
            {
                Entity.OrientationMatrix = new Matrix3X3(
                    value.M11, value.M12, value.M13,
                    value.M21, value.M22, value.M23,
                    value.M31, value.M32, value.M33
                    );
            }
        }

        public Vector3 Forward
        {
            get
            {
                return XNAOrientationMatrix.Forward;
            }
            set
            {
                Matrix temp = XNAOrientationMatrix;
                temp.Forward = value;
                XNAOrientationMatrix = temp;
            }
        }

        public Vector3 Right
        {
            get
            {
                return XNAOrientationMatrix.Right;
            }
            set
            {
                Matrix temp = XNAOrientationMatrix;
                temp.Right = value;
                XNAOrientationMatrix = temp;
            }
        }

        public Vector3 Up
        {
            get
            {
                return XNAOrientationMatrix.Up;
            }
            set
            {
                Matrix temp = XNAOrientationMatrix;
                temp.Up = value;
                XNAOrientationMatrix = temp;
            }
        }

        /// <summary>
        /// Scales the model.
        /// // TODO: Investigate scaling the physics object
        /// </summary>
        public Vector3 Scale
        {
            get;
            set;
        }

        public Entity Entity
        {
            get;
            protected set;
        }

        public virtual void Render()
        {
            if (mRenderable != null)
            {
                mRenderable.Render(Entity.Position, XNAOrientationMatrix.Forward, Scale);
            }
        }

        public Vector3 Position
        {
            get
            {
                return Entity.Position;
            }
            set
            {
                Entity.Position = value;
            }
        }

        public virtual void InitialCollisionDetected(EntityCollidable sender, Collidable other, CollidablePairHandler collisionPair)
        {
            mCollidingObjects.Add(other.Tag as IGameObject);
        }

        public void CollisionEnded(EntityCollidable sender, Collidable other, CollidablePairHandler collisionPair)
        {
            mCollidingObjects.Remove(other.Tag as IGameObject);
        }
    }
}
