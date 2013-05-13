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
using BEPUphysics.Collidables;

namespace GameConstructLibrary
{
    public class DummyObject
    {
        private static UInt32 mNextObjectID = 1;

        /// <summary>
        /// The name of the object type in the editor.
        /// </summary>
        private string mEditorType { get; set; }
        public string EditorType
        {
            get
            {
                return mEditorType;
            }
            set
            {
                mEditorType = value;
                ParameterChanged();
            }
        }

        /// <summary>
        /// The in game object type this will be create.
        /// </summary>
        private string mType { get; set; }
        public string Type
        {
            get
            {
                return mType;
            }
            set
            {
                mType = value;
                ParameterChanged();
            }
        }

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
                ParameterChanged();
            }
        }

        private string[] mParameters { get; set; }
        public string[] Parameters
        {
            get
            {
                return mParameters;
            }
            set
            {
                mParameters = value;
                ParameterChanged();
            }
        }

        private Vector3 mPosition { get; set; }
        public Vector3 Position
        {
            get
            {
                return mPosition;
            }
            set
            {
                bool changed = (Position - value).LengthSquared() > .0001;
                mPosition = value;
                if (changed)
                {
                    ParameterChanged();
                }
            }
        }

        private Quaternion mRotation { get; set; }
        public Quaternion Rotation
        {
            get
            {
                return mRotation;
            }
            set
            {
                mRotation = value;
            }
        }

        private Vector3 mScale { get; set; }
        public Vector3 Scale
        {
            get
            {
                return mScale;
            }
            set
            {
                mScale = value;
                ParameterChanged();
            }
        }

        private float mHeight { get; set; }
        public float Height
        {
            get
            {
                return mHeight;
            }
            set
            {
                mHeight = value;
                ParameterChanged();
            }
        }

        private bool mFloating { get; set; }
        public bool Floating
        {
            get
            {
                return mFloating;
            }
            set
            {
                mFloating = value;
                ParameterChanged();
            }
        }

        [XmlIgnore]
        private Vector3 mYawPitchRoll = Vector3.Zero;
        [XmlIgnore]
        public Vector3 YawPitchRoll
        {
            get
            {
                return mYawPitchRoll;
            }
            set
            {
                if (Math.Abs(value.X - mYawPitchRoll.X) > 0.0001 ||
                    Math.Abs(value.Y - mYawPitchRoll.Y) > 0.0001 ||
                    Math.Abs(value.Z - mYawPitchRoll.Z) > 0.0001)
                {
                    mRotation = Quaternion.CreateFromYawPitchRoll(value.X, value.Y, value.Z);
                    mYawPitchRoll = value;
                }
            }
        }
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
        [XmlIgnore]
        public bool IsHighlighted { get; set; }
        [XmlIgnore]
        private StaticMesh mPhysicsStaticMesh;
        [XmlIgnore]
        private StaticMesh mPhysicsMesh
        {
            get
            {
                if (mPhysicsStaticMesh == null)
                {
                    Vector3[] vertices;
                    int[] indices;
                    CollisionMeshManager.LookupMesh(Model, out vertices, out indices);

                    mPhysicsStaticMesh = new StaticMesh(
                        vertices,
                        indices,
                        new AffineTransform(Scale, Rotation, Position));
                }
                return mPhysicsStaticMesh;
            }
        }

        public DummyObject()
        {
            IsHighlighted = false;
        }

        public DummyObject(DummyObject copy)
        {
            Type = copy.Type;
            Model = copy.Model;
            Parameters = copy.Parameters;
            Position = copy.Position;
            YawPitchRoll = copy.YawPitchRoll;
            Scale = copy.Scale;
            Rotation = copy.Rotation;
            Height = copy.Height;
            Floating = copy.Floating;
            IsHighlighted = false;
        }

        public void ApplyRotation(Vector3 yawPitchRoll)
        {
            Quaternion newRotation = Quaternion.CreateFromYawPitchRoll(yawPitchRoll.X, yawPitchRoll.Y, yawPitchRoll.Z);

            mRotation = Quaternion.Concatenate(mRotation, newRotation);
            mYawPitchRoll = Utility.Utils.GetYawPitchRollFromQuaternion(mRotation);
            ParameterChanged();
        }

        public void ApplyRotation(Vector3 yawPitchRoll, Vector3 centerOfRotation)
        {
            Quaternion newRotation = Quaternion.CreateFromYawPitchRoll(yawPitchRoll.X, yawPitchRoll.Y, yawPitchRoll.Z);

            mRotation = Quaternion.Concatenate(mRotation, newRotation);
            mYawPitchRoll = Utility.Utils.GetYawPitchRollFromQuaternion(mRotation);

            Vector3 offset = Position - centerOfRotation;
            float distance = offset.Length();
            if (distance > 0.0001f)
            {
                Position = centerOfRotation + Vector3.Normalize(Vector3.Transform(offset, newRotation)) * distance;
            }

            ParameterChanged();
        }

        public void ApplyScale(Vector3 scale)
        {
            mScale *= scale;
            ParameterChanged();
        }

        private void ParameterChanged()
        {
            mPhysicsStaticMesh = null;
        }

        public bool RayCast(Ray ray, float maximumLength, out RayHit rayHit)
        {
            return mPhysicsMesh.RayCast(ray, maximumLength, out rayHit);
        }

        public void Draw()
        {
            Vector3 finalPosition = new Vector3(Position.X, Position.Y + Height, Position.Z);
            if (IsHighlighted)
            {
                mDrawableModel.Render(finalPosition, Matrix.CreateFromQuaternion(mRotation), Scale, Color.Red, 0.5f, false);
                IsHighlighted = false;
            }
            else
            {
                mDrawableModel.Render(finalPosition, Matrix.CreateFromQuaternion(mRotation), Scale, false);
            }
        }

    }
}
