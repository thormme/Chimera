using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using BEPUphysics.Entities;
using BEPUphysics.MathExtensions;
using GraphicsLibrary;
using BEPUphysics.CollisionShapes;

namespace GameConstructLibrary
{
    /// <summary>
    /// Base class for all game objects which will be affected by the physics simulation.
    /// </summary>
    public class PhysicsObject : IMobileObject, IEntityOwner
    {
        public PhysicsObject(Renderable renderable, Entity entity)
        {
            mRenderable = renderable;
            Scale = new Vector3(1.0f);
            PhysicsEntity = entity;
            PhysicsEntity.Tag = this;
        }

        protected Renderable mRenderable;

        public Matrix XNAOrientationMatrix
        {
            get
            {
                return new Matrix(
                    PhysicsEntity.OrientationMatrix.M11, PhysicsEntity.OrientationMatrix.M12, PhysicsEntity.OrientationMatrix.M13, 1.0f,
                    PhysicsEntity.OrientationMatrix.M21, PhysicsEntity.OrientationMatrix.M22, PhysicsEntity.OrientationMatrix.M23, 1.0f,
                    PhysicsEntity.OrientationMatrix.M31, PhysicsEntity.OrientationMatrix.M32, PhysicsEntity.OrientationMatrix.M33, 1.0f,
                    1.0f, 1.0f, 1.0f, 1.0f
                    );
            }
            set
            {
                PhysicsEntity.OrientationMatrix = new Matrix3X3(
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

        /// <summary>
        /// Scales the model.
        /// // TODO: Investigate scaling the physics object
        /// </summary>
        public Vector3 Scale
        {
            get;
            set;
        }

        public Entity PhysicsEntity
        {
            get;
            protected set;
        }

        public virtual void Render()
        {
            mRenderable.Render(PhysicsEntity.WorldTransform);
        }

        public Vector3 Position
        {
            get
            {
                return PhysicsEntity.Position;
            }
            set
            {
                PhysicsEntity.Position = value;
            }
        }
    }
}
