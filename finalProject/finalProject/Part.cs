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

namespace finalProject
{
    /// <summary>
    /// Represents a part of a creature. Can have active and passive effects.
    /// </summary>
    abstract public class Part : PhysicsObject
    {
        private Creature mCreature;
        private bool mAttached;

        public Part(Creature creature, Renderable renderable, Entity entity)
            : base(renderable, entity)
        {
            mCreature = creature;
            Game1.World.Add(this);
        }

        virtual public bool Attached
        {
            get;
            set;
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
    }
}
