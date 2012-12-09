using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GraphicsLibrary;
using BEPUphysics.Collidables.MobileCollidables;
using BEPUphysics.CollisionShapes;
using GameConstructLibrary;
using BEPUphysics.Entities;
using BEPUphysics.Collidables;
using BEPUphysics.NarrowPhaseSystems.Pairs;

namespace finalProject
{
    /// <summary>
    /// Represents a part of a creature. Can have active and passive effects.
    /// </summary>
    abstract public class Part
    {
        /// <summary>
        /// Contains different renderable pieces of Parts.
        /// </summary>
        public class SubPart
        {
            public Creature.PartBone[] PreferredBones
            {
                get;
                protected set;
            }

            public Vector3 Position
            {
                get;
                protected set;
            }

            public Matrix Orientation
            {
                get;
                protected set;
            }

            public Vector3 Scale
            {
                get;
                protected set;
            }

            public float Yaw
            {
                get
                {
                    return mYaw;
                }
                set
                {
                    mYaw = value;
                    Orientation = Matrix.CreateFromYawPitchRoll(mYaw, mPitch, mRoll);
                }
            }
            private float mYaw = 0.0f;

            public float Pitch
            {
                get
                {
                    return mPitch;
                }
                set
                {
                    mPitch = value;
                    Orientation = Matrix.CreateFromYawPitchRoll(mYaw, mPitch, mRoll);
                }
            }
            private float mPitch = 0.0f;

            public float Roll
            {
                get
                {
                    return mRoll;
                }
                set
                {
                    mRoll = value;
                    Orientation = Matrix.CreateFromYawPitchRoll(mYaw, mPitch, mRoll);
                }
            }
            private float mRoll = 0.0f;

            public Renderable Renderable;

            /// <summary>
            /// Construct a SubPart for the construction of a Part.
            /// </summary>
            /// <param name="renderable">Renderable which will be drawn attached to the Creature owning this part.</param>
            /// <param name="preferredBones">Array of PartBones in order of preference on which the renderable will be drawn.</param>
            /// <param name="position">Position offset from the bone.</param>
            /// <param name="orientation">Orientation offset from the bone.</param>
            /// <param name="scale">Scale offset from the Creature.</param>
            public SubPart(Renderable renderable, Creature.PartBone[] preferredBones, Vector3 position, Matrix orientation, Vector3 scale)
            {
                Renderable = renderable;
                PreferredBones = preferredBones;
                Position = position;
                Orientation = orientation;
                Scale = scale;
            }

            public void Render(Matrix worldTransform)
            {
                Renderable.Render(Orientation * Matrix.CreateScale(Scale) * Matrix.CreateTranslation(Position) * worldTransform);
            }

        }

        private Creature mCreature;
        private Sprite mSprite;

        public SubPart[] SubParts
        {
            get;
            protected set;
        }

        public float Height
        {
            get;
            protected set;
        }

        public Part(SubPart[] subParts, bool raisesBody, Sprite partSprite)
        {
            SubParts = subParts;
            Height = raisesBody ? 1.0f : 0.0f;
            mSprite = partSprite;
        }

        public virtual Creature Creature
        {
            protected get
            {
                return mCreature;
            }
            set
            {
                if (mCreature != null) 
                    {Reset();
                }
                mCreature = value;
            }
        }

        /// <summary>
        /// Called when the creature is damaged.
        /// </summary>
        /// <param name="damage">The amount of damage dealt.</param>
        public virtual void Damage(int damage, Creature source) { }

        /// <summary>
        /// Called by creature every frame. Used for passive effects.
        /// </summary>
        /// <param name="time">
        /// The game time.
        /// </param>
        public virtual void Update(GameTime time)
        {
            foreach (SubPart subPart in SubParts)
            {
                if (subPart.Renderable is AnimateModel)
                {
                    if (subPart.Renderable is AnimateModel)
                    {
                        (subPart.Renderable as AnimateModel).Update(time);
                    }
                }
            }
        }

        public void RenderSprite(Rectangle bounds)
        {

            Rectangle newBounds = new Rectangle(
                (int)(bounds.Left - bounds.Width / 2.0f),
                (int)(bounds.Top - bounds.Height / 2.0f),
                (int)(bounds.Width * 2.0f),
                (int)(bounds.Height * 2.0f)
            );

            mSprite.Render(newBounds);
        }

        /// <summary>
        /// Trys to set the animation being played by parts.  Will fail if part state is not appropriate.
        /// </summary>
        /// <param name="animationName">The name of the animation to be played must be in a file named model_animation.</param>
        /// <param name="isSaturated">Whether the animation will loop.</param>
        public virtual void TryPlayAnimation(string animationName, bool isSaturated)
        {
            return;
        }

        /// <summary>
        /// Sets the animation being played by parts.
        /// </summary>
        /// <param name="animationName">The name of the animation to be played must be in a file named model_animation.</param>
        /// <param name="isSaturated">Whether the animation will loop.</param>
        protected virtual void PlayAnimation(string animationName, bool isSaturated)
        {
            foreach (SubPart subPart in SubParts)
            {
                if (subPart.Renderable is AnimateModel)
                {
                    (subPart.Renderable as AnimateModel).PlayAnimation(animationName, isSaturated);
                }
            }
        }

        /// <summary>
        /// Called when the part should be used. Used for active effects.
        /// If the magnitude of the direction vector is greater than the active effect's range, nothing will happen.
        /// </summary>
        /// <param name="direction">
        /// The direction the ability will be used in.
        /// </param>
        abstract public void Use(Vector3 direction);

        /// <summary>
        /// Called when the part is finished being used. Used for active effects.
        /// If the magnitude of the direction vector is greater than the active effect's range, nothing will happen.
        /// </summary>
        /// <param name="direction">
        /// The direction the ability will be used in.
        /// </param>
        abstract public void FinishUse(Vector3 direction);

        /// <summary>
        /// Called when the Creature is set.
        /// </summary>
        abstract public void Reset();

        /// <summary>
        /// Called when the creature is put into a state where it cannot use parts.
        /// </summary>
        abstract public void Cancel();

        public virtual void InitialCollisionDetected(EntityCollidable sender, Collidable other, CollidablePairHandler collisionPair) { }
    }
}
