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

        public SubPart[] SubParts
        {
            get;
            protected set;
        }

        public Part(SubPart[] subParts)
        {
            SubParts = subParts;
        }

        public virtual Creature Creature
        {
            protected get
            {
                return mCreature;
            }
            set
            {
                Reset();
                mCreature = value;
            }
        }

        /// <summary>
        /// Called by creature every frame. Used for passive effects.
        /// </summary>
        /// <param name="time">
        /// The game time.
        /// </param>
        abstract public void Update(GameTime time);

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
        abstract protected void Reset();
    }
}
