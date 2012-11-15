﻿using System;
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
            Scale = 1.0f;
            Entity = entity;
        }

        private Renderable mRenderable;

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
                return Entity.OrientationMatrix.Forward;
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
                return Entity.OrientationMatrix.Right;
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
        public float Scale
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
            mRenderable.Render(Entity.WorldTransform);
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
    }
}
