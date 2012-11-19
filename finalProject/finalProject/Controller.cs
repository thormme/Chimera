﻿#region Using Statements

using System.Collections.Generic;
using Microsoft.Xna.Framework;

#endregion

namespace finalProject
{
    /// <summary>
    /// Abstract class used to define the behavior of a creature.
    /// </summary>
    abstract public class Controller
    {
        #region Fields

        protected Creature mCreature;

        #endregion

        #region Public Methods

        public Controller() {}

        /// <summary>
        /// Sets the creature this controller will control.
        /// </summary>
        /// <param name="creature">The creature to control.</param>
        public virtual void SetCreature(Creature creature)
        {
            mCreature = creature;
        }

        /// <summary>
        /// Called every frame. Defines the behavior of the creature based on the nearby creatures.
        /// </summary>
        /// <param name="time">The game time.</param>
        /// <param name="collidingCreatures">The nearby creatures, in order of distance from the creature.</param>
        abstract public void Update(GameTime time, List<Creature> collidingCreatures);

        #endregion
    }
}
