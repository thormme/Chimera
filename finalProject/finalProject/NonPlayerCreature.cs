﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GraphicsLibrary;
using BEPUphysics.CollisionShapes;
using BEPUphysics.Entities;
using BEPUphysics.Collidables.MobileCollidables;
using BEPUphysics.Collidables;
using BEPUphysics.NarrowPhaseSystems.Pairs;

namespace finalProject
{
    /// <summary>
    /// Abstract class used for making NPCs.
    /// </summary>
    public abstract class NonPlayerCreature : Creature
    {
        private float mSneak;
        public override float Sneak
        {
            get
            {
                return mSneak;
            }
        }

        public NonPlayerCreature(
            Vector3 position,
            float sensitivityRadius,
            Controller controller,
            Renderable renderable,
            Entity entity,
            float visionAngle,
            float listeningSensitivity,
            float sneak,
            Part part
            )
            : base(position, renderable, entity, new SensitiveSensor(sensitivityRadius, visionAngle, listeningSensitivity), controller)
        {
            mSneak = sneak;
            mController = controller;
            Game1.World.Add(part);
            AddPart(part);
            //RemakeEntity();
        }

        public override void InitialCollisionDetected(EntityCollidable sender, Collidable other, CollidablePairHandler collisionPair)
        {
            System.Console.WriteLine("NonPlayerCreature");
        }
    }
}
