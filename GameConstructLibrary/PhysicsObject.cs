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
    public class PhysicsObject : Entity, IGameObject
    {
        public PhysicsObject(Renderable renderable, EntityShape shape)
            : base(shape)
        {
            mRenderable = renderable;
        }

        private Renderable mRenderable;

        public Matrix XNAOrientationMatrix
        {
            get
            {
                return new Matrix(
                    OrientationMatrix.M11, OrientationMatrix.M12, OrientationMatrix.M13, 1.0f,
                    OrientationMatrix.M21, OrientationMatrix.M22, OrientationMatrix.M23, 1.0f,
                    OrientationMatrix.M31, OrientationMatrix.M32, OrientationMatrix.M33, 1.0f,
                    1.0f, 1.0f, 1.0f, 1.0f
                    );
            }
            set
            {
                OrientationMatrix = new Matrix3X3(
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
                return OrientationMatrix.Forward;
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
                return OrientationMatrix.Right;
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

        public virtual void Render()
        {
            if (mRenderable != null)
            {
                mRenderable.Render(Position, XNAOrientationMatrix, Scale);
            }
        }
    }
}
