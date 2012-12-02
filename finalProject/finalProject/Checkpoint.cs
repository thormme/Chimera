using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameConstructLibrary;
using GraphicsLibrary;
using BEPUphysics.Entities;
using Microsoft.Xna.Framework;
using BEPUphysics.Collidables;
using BEPUphysics.NarrowPhaseSystems.Pairs;

namespace finalProject
{
    public class Checkpoint : Prop, IActor
    {
        public Checkpoint(Vector3 position, Quaternion orientation, Vector3 scale)
            : base("box", position, orientation, scale)
        {
        }

        /// <summary>
        /// Constructor for use by the World level loading.
        /// </summary>
        /// <param name="modelName">Name of the model.</param>
        /// <param name="translation">The position.</param>
        /// <param name="orientation">The orientation.</param>
        /// <param name="scale">The amount to scale by.</param>
        /// <param name="extraParameters">Extra parameters.</param>
        public Checkpoint(String modelName, Vector3 translation, Quaternion orientation, Vector3 scale, string[] extraParameters)
            : this(translation, orientation, scale)
        {
        }

        public void Update(GameTime time)
        {
            foreach (CollidablePairHandler pair in StaticCollidable.Pairs)
            {
                if (pair.BroadPhaseOverlap.EntryA.Tag is PlayerCreature)
                {
                    (pair.BroadPhaseOverlap.EntryA.Tag as PlayerCreature).SpawnOrigin = Position;
                }
                if (pair.BroadPhaseOverlap.EntryB.Tag is PlayerCreature)
                {
                    (pair.BroadPhaseOverlap.EntryB.Tag as PlayerCreature).SpawnOrigin = Position;
                }
            }
        }
    }
}
