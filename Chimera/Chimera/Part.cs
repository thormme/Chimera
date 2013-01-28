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

namespace Chimera
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

            /// <summary>
            /// Render the SubPart.
            /// </summary>
            /// <param name="worldTransform">The SubPart's world transform.</param>
            /// <param name="color">Color with which to modify the object.</param>
            /// <param name="weight">Amount to colorify the object. 0-none 1-full</param>
            /// <param name="scale">Whether to render using the scale associated with this SubPart.</param>
            public void Render(Matrix worldTransform, Color color, float weight, bool scale)
            {
                Matrix transform = Orientation * (scale ? Matrix.CreateScale(Scale) : Matrix.Identity) * Matrix.CreateTranslation(Position) * worldTransform;
                Renderable.Render(transform, color, weight);
            }

        }

        private Creature mCreature;

        public Sprite Icon
        {
            get;
            protected set;
        }
        protected Sprite mSprite;

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

        public bool CanAnimate
        {
            get { return mCanAnimate; }
            protected set { mCanAnimate = value; }
        }
        protected bool mCanAnimate = true;

        /// <summary>
        /// Constructs a new Part.
        /// </summary>
        /// <param name="subParts">The array of SubParts this part contains.</param>
        /// <param name="raisesBody">Whether this part should raise the associated Creature off of the ground.</param>
        /// <param name="partSprite">The GUI sprite used to indicate this Part.</param>
        public Part(SubPart[] subParts, bool raisesBody, Sprite partSprite)
        {
            SubParts = subParts;
            Height = raisesBody ? 1.0f : 0.0f;
            mSprite = partSprite;
        }

        /// <summary>
        /// The Creature which currently owns this part.
        /// If not owned may be null.
        /// </summary>
        public virtual Creature Creature
        {
            protected get
            {
                return mCreature;
            }
            set
            {
                if (mCreature != null) 
                {
                    Reset();
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

        /// <summary>
        /// Render the GUI element associated with this Part.
        /// </summary>
        /// <param name="bounds">The bounds that the sprite should be rendered within.</param>
        public virtual void RenderSprite(Rectangle bounds)
        {
            mSprite.Render(bounds);
        }

        /// <summary>
        /// Tries to set the animation being played by parts.  Will fail if part state is not appropriate.
        /// </summary>
        /// <param name="animationName">The name of the animation to be played must be in a file named model_animation.</param>
        /// <param name="loop">Whether the animation will loop.</param>
        /// <param name="playOnCreature">Whether to attempt to play the animation on the associated creature.</param>
        public virtual void TryPlayAnimation(string animationName, bool loop, bool playOnCreature)
        {
            if (CanAnimate)
            {
                PlayAnimation(animationName, loop, playOnCreature);
            }
        }

        /// <summary>
        /// Sets the animation being played by parts.
        /// </summary>
        /// <param name="animationName">The name of the animation to be played must be in a file named model_animation.</param>
        /// <param name="loop">Whether the animation will loop.</param>
        /// <param name="playOnCreature">Whether to attempt to play the animation on the associated creature.</param>
        protected virtual void PlayAnimation(string animationName, bool loop, bool playOnCreature)
        {
            if (playOnCreature)
            {
                mCreature.TryPlayAnimation(animationName, loop);
            }
            foreach (SubPart subPart in SubParts)
            {
                if (subPart.Renderable is AnimateModel)
                {
                    (subPart.Renderable as AnimateModel).PlayAnimation(animationName, loop);
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
        /// Resets internal state.
        /// Called when the Creature is set.
        /// </summary>
        abstract public void Reset();

        /// <summary>
        /// Cancels part use without resetting internal state.
        /// Called when the creature is put into a state where it cannot use parts.
        /// </summary>
        abstract public void Cancel();

        public virtual void InitialCollisionDetected(EntityCollidable sender, Collidable other, CollidablePairHandler collisionPair) { }
    }
}
