﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BEPUphysics.CollisionShapes.ConvexShapes;
using GameConstructLibrary;
using BEPUphysics.Entities.Prefabs;
using GraphicsLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BEPUphysics.EntityStateManagement;
using BEPUphysics.Entities;
using BEPUphysics.Collidables.MobileCollidables;
using BEPUphysics.Collidables;
using BEPUphysics.NarrowPhaseSystems.Pairs;

namespace finalProject
{
    /// <summary>
    /// The creature representing the player character.
    /// </summary>
    class PlayerCreature : Creature
    {
        private const float mPlayerRadius = 1.0f;

        public Camera PlayerCamera
        {
            get
            {
                return (mController as PlayerController).mCamera;
            }
        }

        private const float mSneak = 10.0f;
        public override float Sneak
        {
            get
            {
                return mSneak;
            }
        }

        public override bool Incapacitated
        {
            get
            {
                return false;
            }
        }

        public PlayerCreature(Viewport viewPort, Vector3 position)
            : base(position, new InanimateModel("box"), new Box(new Vector3(0), 1.0f, 1.0f, 1.0f, 1.0f), new RadialSensor(2.0f), new PlayerController(viewPort))
        {
            Scale = new Vector3(1.0f);
        }

        public override void Damage(int damage)
        {
            while (damage-- > 0)
            {
                if (mParts.Count() == 0)
                {
                    // die?
                    return;
                }

                mParts.Remove(mParts[Rand.rand.Next(mParts.Count())]);
            }
        }
        
        /// <summary>
        /// Adds a part to the PlayerCreature. The part chosen is from the closest incapacitated animal within the radial sensor.
        /// </summary>
        public void FindAndAddPart()
        {
            foreach (Creature cur in mSensor.CollidingCreatures)
            {
                if (cur.Incapacitated)
                {
                    AddPart(cur.Parts[0]);
                    return;
                }
            }
        }
    }
}
