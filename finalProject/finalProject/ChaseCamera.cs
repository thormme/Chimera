using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using BEPUphysics.Collidables;
using GraphicsLibrary;
using finalProject;
using GameConstructLibrary;

namespace FinalProject
{
    /// <summary>
    /// 
    /// </summary>
    public class ChaseCamera : ICamera
    {
        #region Target Properties

        /// <summary>
        /// Position in world space of player.
        /// </summary>
        public Vector3 TargetPosition
        {
            get
            {
                return mTargetPosition;
            }
            set
            {
                mTargetPosition = value;
            }
        }
        private Vector3 mTargetPosition;

        /// <summary>
        /// Forward vector in world space of player.
        /// </summary>
        public Vector3 TargetDirection
        {
            get
            {
                return mTargetDirection;
            }
            set
            {
                mTargetDirection = value;
            }
        }
        private Vector3 mTargetDirection;

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
        private Vector3 mUp;

        /// <summary>
        /// Physical representation of target.
        /// </summary>
        public Creature TargetBody
        {
            get
            {
                return mTargetBody;
            }
            set
            {
                mTargetBody = value;
            }
        }
        private Creature mTargetBody;

        #endregion

        #region Desired Camera Properties

        /// <summary>
        /// Maximum distance that camera can be from the player before being dragged along.
        /// </summary>
        public float MaxRopeLengthSquared
        {
            get
            {
                return mMaxRopeLengthSquared;
            }
            set
            {
                mMaxRopeLengthSquared = value;
            }
        }
        private float mMaxRopeLengthSquared = 81.0f;

        public float MinRopeLengthSquared
        {
            get
            {
                return mMinRopeLengthSquared;
            }
            set
            {
                mMinRopeLengthSquared = value;
            }
        }
        private float mMinRopeLengthSquared = 46.0f;

        /// <summary>
        /// Desired position of camera in target's local space.
        /// </summary>
        public Vector3 DesiredPositionLocal
        {
            get
            {
                return mDesiredPositionLocal;
            }
            set
            {
                mDesiredPositionLocal = value;
            }
        }
        private Vector3 mDesiredPositionLocal = new Vector3(0.0f, 2.0f, 3.0f);

        /// <summary>
        /// Used to restore camera position when moving player.
        /// </summary>

        /// <summary>
        /// Desired position of camera in world space.
        /// </summary>
        public Vector3 DesiredPosition
        {
            get
            {
                UpdatePositions();
                return mDesiredPosition;
            }
        }
        private Vector3 mDesiredPosition;

        /// <summary>
        /// Position of where the camera is looking in target's local space.
        /// </summary>
        public Vector3 LookAtLocal
        {
            get
            {
                return mLookAtLocal;
            }
            set
            {
                mLookAtLocal = value;
            }
        }
        private Vector3 mLookAtLocal = new Vector3(0.0f, 1.0f, 0.0f);

        /// <summary>
        /// Position of where the camera is looking in world space.
        /// </summary>
        public Vector3 LookAt
        {
            get
            {
                UpdatePositions();
                return mLookAt;
            }
        }
        private Vector3 mLookAt;

        #endregion

        #region Camera Properties

        /// <summary>
        /// Sets whether or not camera will automatically follow the target.
        /// </summary>
        public bool TrackTarget
        {
            get
            {
                return mTrackTarget;
            }
            set
            {
                mTrackTarget = value;
            }
        }
        private bool mTrackTarget = true;

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

        /// <summary>
        /// Position of camera in world space.
        /// </summary>
        public Vector3 Position
        {
            get
            {
                return mPosition;
            }
        }
        private Vector3 mPosition;

        /// <summary>
        /// Velocity of camera in world space.
        /// </summary>
        public Vector3 Velocity
        {
            get
            {
                return mVelocity;
            }
        }
        private Vector3 mVelocity;

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
        private Vector3 mForward;

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
        private Vector3 mRight;

        /// <summary>
        /// Minimum number of degrees camera can be from the up vector.
        /// </summary>
        public float MinPitchAngle
        {
            get
            {
                return mMinPitchAngle;
            }
        }
        private float mMinPitchAngle = (float)Math.Cos(MathHelper.ToRadians(-10.0f));

        #endregion

        #region Camera Physics Properties

        /// <summary>
        /// The stiffer the camera spring the closer the camera stays to the target.
        /// </summary>
        public float Stiffness
        {
            get
            {
                return mStiffness;
            }
            set
            {
                mStiffness = value;
            }
        }
        private float mStiffness = 5000.0f;

        /// <summary>
        /// Large enough values keep spring from oscillating infinitely.
        /// </summary>
        public float Damping
        {
            get
            {
                return mDamping;
            }
            set
            {
                mDamping = value;
            }
        }
        private float mDamping = 1000.0f;

        /// <summary>
        /// Mass of the camera's body, the heavier the camera the stiffer the spring must be.
        /// </summary>
        public float Mass
        {
            get
            {
                return mMass;
            }
            set
            {
                mMass = value;
            }
        }
        private float mMass = 10.0f;

        /// <summary>
        /// Rope attaching camera to target.
        /// </summary>
        public Vector3 Rope
        {
            get
            {
                return mRope;
            }
        }
        private Vector3 mRope;

        #endregion

        #region View Properties

        /// <summary>
        /// Transformation of vector in to camera space.
        /// </summary>
        public Matrix ViewTransform
        {
            get
            {
                return mViewTransform;
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
        private float mFarPlaneDistance = 1000.0f;

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

        public ChaseCamera(Viewport viewport)
        {
            this.mAspectRatio = (float)viewport.Width / (float)viewport.Height;
        }

        /// <summary>
        /// Move camera based on physics simulation.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            UpdatePositions();

            float timeElapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Calculate Spring Force.
            Vector3 strech = mPosition - mDesiredPosition;
            Vector3 force = -mStiffness * strech - mDamping * mVelocity;

            // Apply Acceleration.
            Vector3 acceleration = force / mMass;
            mVelocity += acceleration * timeElapsed;

            // Apply Velocity.
            mPosition += mVelocity * timeElapsed;

            // Calculate basis vectors.
            Vector3 fullForward = mLookAt - mPosition;
            mForward = Vector3.Normalize(fullForward);
            mRight = Vector3.Normalize(Vector3.Cross(mUp, mForward));

            // Check for intersection with the scene.  Move camera forward if need be.
            List<BEPUphysics.RayCastResult> results = new List<BEPUphysics.RayCastResult>();
            float cameraDistance = fullForward.Length();
            mTargetBody.World.Space.RayCast(new Ray(mLookAt, -fullForward), cameraDistance, results);

            foreach (BEPUphysics.RayCastResult result in results)
            {
                if (result.HitObject as Collidable != mTargetBody.CharacterController.Body.CollisionInformation &&
                    !(result.HitObject.Tag is RadialSensor) &&
                    !(result.HitObject.Tag is CharacterSynchronizer))
                {
                    Vector3 shortenedForward = mLookAt - result.HitData.Location;
                    float distance = shortenedForward.Length();
                    if (0.9f * distance < cameraDistance)
                    {
                        mPosition = mLookAt - 0.9f * distance * mForward;
                        break;
                    }
                }
            }

            // Calculate new rope.
            mRope = mPosition - mTargetPosition;

            mViewTransform = Matrix.CreateLookAt(mPosition, mLookAt, mUp);
        }

        /// <summary>
        /// Modifies DesiredPositionLocal by rotating it around the target by yaw, pitch, roll.
        /// </summary>
        /// <param name="yaw"></param>
        /// <param name="pitch"></param>
        /// <param name="roll"></param>
        public void RotateAroundTarget(float yaw, float pitch, float roll)
        {
            if (mInvertHorizontal == true)
            {
                yaw *= -1.0f;
            }

            if (mInvertVertical == true)
            {
                pitch *= -1.0f;
            }

            // Rotate pitch.
            Matrix pitchRotation = Matrix.CreateFromAxisAngle(Vector3.Normalize(Vector3.Cross(mUp, mDesiredPositionLocal)), pitch);
            Vector3 newDesiredPosition = Vector3.Transform(mDesiredPositionLocal, pitchRotation);

            float newAngle = Vector3.Dot(Vector3.Normalize(newDesiredPosition), mUp);
            if (Math.Abs(newAngle) >= mMinPitchAngle)
            {
                newDesiredPosition = mDesiredPositionLocal;
            }

            // Rotate yaw.
            Matrix yawRotation = Matrix.CreateFromAxisAngle(mUp, yaw);
            newDesiredPosition = Vector3.Transform(newDesiredPosition, yawRotation);

            // Rotate roll.
            Matrix rollRotation = Matrix.CreateFromAxisAngle(mForward, roll);
            newDesiredPosition = Vector3.Transform(newDesiredPosition, rollRotation);

            // Apply rotations.
            mDesiredPositionLocal = newDesiredPosition;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Projects local space positions in to world space.
        /// </summary>
        private void UpdatePositions()
        {
            Matrix projection = Matrix.Identity;
            projection.Forward = mTargetDirection;
            projection.Up = mUp;
            projection.Right = Vector3.Cross(mUp, mTargetDirection);

            mDesiredPosition = mTargetPosition + Vector3.TransformNormal(mDesiredPositionLocal, projection);
            mLookAt = mTargetPosition + Vector3.TransformNormal(mLookAtLocal, projection);
        }

        #endregion
    }
}
