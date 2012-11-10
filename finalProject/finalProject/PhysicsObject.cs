using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using BEPUphysics.Entities;
using BEPUphysics.MathExtensions;
using GraphicsLibrary;

namespace finalProject
{
    public class PhysicsObject : GameObject
    {
        public PhysicsObject(Renderable renderable, Entity physicsEntity) : base(renderable)
        {
            mPhysicsEntity = physicsEntity;
        }

        protected Entity mPhysicsEntity;

        public override Vector3 Position
        {
            virtual get
            {
                return mPhysicsEntity.Position;
            }
            virtual set 
            {
                mPhysicsEntity.Position = value;
            }
        }

        public override Matrix Orientation
        {
            virtual get
            {
                return new Matrix(
                    mPhysicsEntity.OrientationMatrix.M11, mPhysicsEntity.OrientationMatrix.M12, mPhysicsEntity.OrientationMatrix.M13, 1.0f,
                    mPhysicsEntity.OrientationMatrix.M21, mPhysicsEntity.OrientationMatrix.M22, mPhysicsEntity.OrientationMatrix.M23, 1.0f,
                    mPhysicsEntity.OrientationMatrix.M31, mPhysicsEntity.OrientationMatrix.M32, mPhysicsEntity.OrientationMatrix.M33, 1.0f,
                    1.0f, 1.0f, 1.0f, 1.0f);

            }
            virtual set
            {
                mPhysicsEntity.OrientationMatrix = new Matrix3X3(
                    value.M11, value.M12, value.M13,
                    value.M21, value.M22, value.M23,
                    value.M31, value.M32, value.M33);
            }
        }

        /*public override float Scale
        {
            virtual get
            {
                return mPhysicsEntity.;

            }
            virtual set
            {
                mPhysicsEntity.Position = value;
            }
        }*/

        public Vector3 Velocity
        {
            get
            {
                return mPhysicsEntity.LinearVelocity;
            }
            set
            {
                mPhysicsEntity.LinearVelocity = value;
            }
        }

        public Vector3 LinearMomentum
        {
            get
            {
                return mPhysicsEntity.LinearMomentum;
            }
            set
            {
                mPhysicsEntity.LinearMomentum = value;
            }
        }

        public Vector3 AngularMomentum
        {
            get
            {
                return mPhysicsEntity.AngularMomentum;
            }
            set
            {
                mPhysicsEntity.AngularMomentum = value;
            }
        }

        public void applyImpulse(ref Vector3 location, ref Vector3 impulse)
        {
            mPhysicsEntity.ApplyImpulse(location, impulse);
        }
    }
}
