using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GraphicsLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameConstructLibrary
{
    public class FPSCamera : ICamera
    {
        #region Camera Basis

        /// <summary>
        /// Up vector in world space of player.
        /// </summary>
        public Vector3 Up
        {
            get
            {
                return mUp;
            }
            set
            {
                mUp = value;
            }
        }
        private Vector3 mUp = Vector3.Up;

                /// <summary>
        /// Direction camera is facing in world space.
        /// </summary>
        public Vector3 Forward
        {
            get
            {
                return mForward;
            }
        }
        private Vector3 mForward = Vector3.Forward;

        public Vector3 GetForward()
        {
            return Forward;
        }

        /// <summary>
        /// Direction perpendicular to forward and up in world space.
        /// </summary>
        public Vector3 Right
        {
            get
            {
                return mRight;
            }
        }
        private Vector3 mRight = Vector3.Right;

        #endregion

        #region User Configuration

        /// <summary>
        /// Flip direction of horizontal rotation.  False: Positive Y rotates up.  True: Positive Y rotates down.
        /// </summary>
        public bool InvertVertical
        {
            get
            {
                return mInvertVertical;
            }
            set
            {
                mInvertVertical = value;
            }
        }
        private bool mInvertVertical = false;

        /// <summary>
        /// Flip direction of horizontal rotation.  False: Positive X rotates right.  True: Positive X rotates left.
        /// </summary>
        public bool InvertHorizontal
        {
            get
            {
                return mInvertHorizontal;
            }
            set
            {
                mInvertHorizontal = value;
            }
        }
        private bool mInvertHorizontal = false;

        #endregion

        #region Camera Properties

        /// <summary>
        /// Position of camera in world space.
        /// </summary>
        public Vector3 Position
        {
            get
            {
                return mPosition;
            }
            set
            {
                mPosition = value;
            }
        }
        private Vector3 mPosition;

        public Vector3 GetPosition()
        {
            return Position;
        }

        public Vector3 Target
        {
            get
            {
                return mTarget;
            }
            set
            {
                mTarget = value;
                mForward = mTarget - mPosition;
                mForward.Normalize();

                mRight = Vector3.Cross(Up, Forward);
                mRight.Normalize();
            }
        }
        private Vector3 mTarget;

        #endregion 

        #region View Properties

        /// <summary>
        /// Transformation of vector in to camera space.
        /// </summary>
        public Matrix ViewTransform
        {
            get
            {
                return Matrix.CreateLookAt(Position, Position + Forward, Up);
            }
        }
        private Matrix mViewTransform;

        public Matrix GetViewTransform()
        {
            return ViewTransform;
        }

        #endregion

        #region Perspective Properties

        private bool mDirtyProjection = true;

        /// <summary>
        /// Orthographic projection in to view frustum.
        /// </summary>
        public Matrix ProjectionTransform
        {
            get
            {
                if (mDirtyProjection)
                {
                    mProjectionTransform = Matrix.CreatePerspectiveFieldOfView(mFieldOfView, mAspectRatio, mNearPlaneDistance, mFarPlaneDistance);
                    mDirtyProjection = false;
                }
                return mProjectionTransform;
            }
        }
        private Matrix mProjectionTransform;

        public Matrix GetProjectionTransform()
        {
            return ProjectionTransform;
        }

        /// <summary>
        /// Screen aspect ratio.
        /// </summary>
        public float AspectRatio
        {
            get
            {
                return mAspectRatio;
            }
            set
            {
                mAspectRatio = value;
                mDirtyProjection = true;
            }
        }
        private float mAspectRatio;

        /// <summary>
        /// Vertical field of view in degrees.
        /// </summary>
        public float FieldOfView
        {
            get
            {
                return mFieldOfView;
            }
            set
            {
                mFieldOfView = value;
                mDirtyProjection = true;
            }
        }
        private float mFieldOfView = MathHelper.PiOver4;

        public Viewport Viewport
        {
            set
            {
                this.AspectRatio = (float)value.Width / (float)value.Height;
            }
        }

        /// <summary>
        /// Distance of near plane in front of camera.
        /// </summary>
        public float NearPlaneDistance
        {
            get
            {
                return mNearPlaneDistance;
            }
            set
            {
                mNearPlaneDistance = value;
                mDirtyProjection = true;
            }
        }
        private float mNearPlaneDistance = 1.0f;

        /// <summary>
        /// Distance of far plane in front of camera.
        /// </summary>
        public float FarPlaneDistance
        {
            get
            {
                return mFarPlaneDistance;
            }
            set
            {
                mFarPlaneDistance = value;
                mDirtyProjection = true;
            }
        }
        private float mFarPlaneDistance = 2000.0f;

        public float GetFarPlaneDistance()
        {
            return FarPlaneDistance;
        }

        public float GetNearPlaneDistance()
        {
            return NearPlaneDistance;
        }

        public void SetFarPlaneDistance(float value)
        {
            FarPlaneDistance = value;
        }

        #endregion

        #region Public Methods

        public FPSCamera(Viewport viewport)
        {
            this.Viewport = viewport;
        }

        /// <summary>
        /// Modifies DesiredPositionLocal by rotating it around the target by yaw, pitch, roll.
        /// </summary>
        /// <param name="yaw"></param>
        /// <param name="pitch"></param>
        /// <param name="roll"></param>
        public void RotateAroundSelf(float yaw, float pitch, float roll)
        {
            if (mInvertHorizontal == true)
            {
                yaw *= -1.0f;
            }

            if (mInvertVertical == true)
            {
                pitch *= -1.0f;
            }

            // Apply rotations.
            Matrix yawRotation = Matrix.CreateFromAxisAngle(Up, yaw);
            mForward = Vector3.Transform(Forward, yawRotation);
            mRight   = Vector3.Transform(Right, yawRotation);

            Matrix rollRotation = Matrix.CreateFromAxisAngle(Forward, roll);
            mRight = Vector3.Transform(Right, rollRotation);

            Matrix pitchRotation = Matrix.CreateFromAxisAngle(Right, pitch);
            mForward = Vector3.Transform(Forward, pitchRotation);
        }

        public void Move(float forwardDistance, float rightDistance, float upDistance)
        {
            mPosition += (forwardDistance * Forward + rightDistance * Right + upDistance * Up);
        }

        #endregion
    }
}