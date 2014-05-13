using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BEPUphysics.BroadPhaseEntries;
using GraphicsLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameConstructLibrary
{
    public class StaticCamera : ICamera
    {
        #region Constants

        private const float MIN_PITCH_ANGLE = 0.94f;
        private const float PREFERRED_DISTANCE_FROM_TARGET = 20.0f;

        #endregion

        #region Fields

        private bool mProjectionDirty = false;

        #endregion

        #region Properties

        private Vector3 mPreferredTargetPosition = Vector3.Zero;
        public Vector3 PreferredTargetPosition
        {
            get { return mPreferredTargetPosition; }
            set
            {
                mPreferredTargetPosition = value;
                if (mTargetPosition == Vector3.Zero)
                {
                    mTargetPosition = mPreferredTargetPosition;
                }
                mPosition = mPreferredTargetPosition - mForward * PREFERRED_DISTANCE_FROM_TARGET;
            }
        }

        private Vector3 mTargetForward = Vector3.Forward;
        public Vector3 TargetForward
        {
            get { return mTargetForward; }
            set { mTargetForward = value; }
        }

        private Vector3 mTargetPosition = Vector3.Zero;
        public Vector3 TargetPosition
        {
            get { return mTargetPosition; }
        }

        private Vector3 mPosition = Vector3.Zero;
        public Vector3 Position
        {
            get { return mPosition; }
            set { mPosition = value; }
        }

        private Vector3 mForward = Vector3.Transform(Vector3.Forward, Matrix.CreateRotationX(3.0f * -MathHelper.PiOver4 / 4.0f));
        public Vector3 Forward
        {
            get { return mForward; }
            set { mForward = value; }
        }

        private Vector3 mRight = Vector3.Right;
        public Vector3 Right
        {
            get { return mRight; }
            set { mRight = value; }
        }

        private Vector3 mUp = Vector3.Up;
        public Vector3 Up
        {
            get { return mUp; }
            set { mUp = value; }
        }

        private float mAspectRatio = 1.0f;
        public float AspectRatio
        {
            get { return mAspectRatio; }
            set
            {
                mAspectRatio = value;
                mProjectionDirty = true;
            }
        }

        public Viewport Viewport
        {
            set { this.AspectRatio = (float)value.Width / (float)value.Height; }
        }

        private float mFieldOfView = MathHelper.PiOver4;
        public float FieldOfView
        {
            get { return mFieldOfView; }
            set
            {
                mFieldOfView = value;
                mProjectionDirty = true;
            }
        }

        private Matrix mView = Matrix.Identity;
        public Matrix View
        {
            get { return mView; }
        }

        private Matrix mProjection = Matrix.Identity;
        public Matrix Projection
        {
            get
            {
                if (mProjectionDirty)
                {
                    mProjection = Matrix.CreatePerspectiveFieldOfView(mFieldOfView, mAspectRatio, mNearPlaneDistance, mFarPlaneDistance);
                    mProjectionDirty = false;
                }
                return mProjection;
            }
        }

        private float mFarPlaneDistance = 1000.0f;
        public float FarPlaneDistance
        {
            get { return mFarPlaneDistance; }
            set
            { 
                mFarPlaneDistance = value;
                mProjectionDirty = true;
            }
        }

        private float mNearPlaneDistance = 1.0f;
        public float NearPlaneDistance
        {
            get { return mNearPlaneDistance; }
            set
            {
                mNearPlaneDistance = value;
                mProjectionDirty = true;
            }
        }

        private bool mInvertHorizontal = false;
        public bool InvertHorizontal
        {
            get { return mInvertHorizontal; }
            set { mInvertHorizontal = false; }
        }

        private bool mInvertVertical = false;
        public bool InvertVertical
        {
            get { return mInvertVertical; }
            set { mInvertVertical = false; }
        }

        private bool mTrackTarget = false;
        public bool TrackTarget
        {
            get { return mTrackTarget; }
            set { mTrackTarget = value; }
        }

        private World mWorld = null;
        public World World
        {
            get { return mWorld; }
            set { mWorld = value; }
        }

        private Vector3 mVelocity = Vector3.Zero;

        #endregion

        #region Public Methods

        public StaticCamera(Viewport viewport)
        {
            this.Viewport = viewport;
        }

        public void Update(GameTime gameTime)
        {
            Vector3 preferredTarget = mPreferredTargetPosition + mTargetForward * 3.0f + mUp * 2.0f;
            Vector3 acceleration = preferredTarget - mTargetPosition;
            float length = acceleration.Length();

            if (acceleration != Vector3.Zero)
            {
                acceleration.Normalize();
            }

            if (length < 0.3f)
            {
                acceleration = Vector3.Zero;
                mVelocity = Vector3.Zero;
            }


            //mVelocity += acceleration * 0.1f;

            //mTargetPosition += acceleration * 1.0f;
            mTargetPosition = preferredTarget;

            /*BEPUphysics.RayCastResult result = new BEPUphysics.RayCastResult();
            Func<BroadPhaseEntry, bool> filter = (bfe) => (
                !(bfe.Tag is Sensor) && 
                !(bfe.Tag is CharacterSynchronizer) && 
                !(bfe.Tag is InvisibleWall)
            );*/

            float targetDistance = PREFERRED_DISTANCE_FROM_TARGET;

            // Zoom camera to avoid occluders.
            /*if (World != null)
            {
                if(World.Space.RayCast(new Ray(mTargetPosition, -mForward), PREFERRED_DISTANCE_FROM_TARGET, filter, out result))
                {
                    targetDistance = result.HitData.T;
                }
            }*/

            mView = Matrix.CreateLookAt(mTargetPosition - mForward * targetDistance - mUp * 5.0f, mTargetPosition, mUp);
        }

        public void RotateAroundTarget(float yaw, float pitch, float roll)
        {
            mForward = RotateForwardVector(yaw, pitch, roll);
            mRight = Vector3.Normalize(Vector3.Cross(mForward, mUp));
        }

        #endregion

        #region Helper Methods

        private Vector3 RotateForwardVector(float yaw, float pitch, float roll)
        {
            if (InvertHorizontal)
            {
                yaw *= -1.0f;
            }

            if (InvertVertical)
            {
                pitch *= -1.0f;
            }

            Matrix yawRotation = Matrix.CreateFromAxisAngle(mUp, yaw);
            Matrix rollRotation = Matrix.CreateFromAxisAngle(mForward, roll);
            Matrix pitchRotation = Matrix.CreateFromAxisAngle(mRight, pitch);

            Vector3 newForward = Vector3.Transform(mForward, yawRotation * rollRotation * pitchRotation);

            float newAngle = Vector3.Dot(Vector3.Normalize(newForward), mUp);
            if (Math.Abs(newAngle) >= MIN_PITCH_ANGLE)
            {
                newForward = mForward;
            }

            return newForward;
        }

        #endregion
    }
}
