﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameConstructLibrary;
using GraphicsLibrary;
using Microsoft.Xna.Framework;
using BEPUphysics.Entities.Prefabs;
using BEPUphysics.Constraints.TwoEntity.JointLimits;
using BEPUphysics.Constraints.TwoEntity.Joints;
using BEPUphysics.CollisionRuleManagement;
using BEPUphysics;
using BEPUphysics.Collidables;
using BEPUphysics.Entities;

namespace finalProject.Projectiles
{
    public class CobraVenom : Projectile
    {

        public CobraVenom(Actor owner, Vector3 direction)
            : base(
            new InanimateModel("box"),
            new Box(owner.Position, 0.5f, 0.5f, 0.5f, 0.5f),
            owner,
            direction,
            60.0f,
            new Vector3(0.5f)
            )
        {
            CheckHits = false;
        }

        public override void Update(GameTime time)
        {
            base.Update(time);

        }

        public override void InitialCollisionDetected(BEPUphysics.Collidables.MobileCollidables.EntityCollidable sender, Collidable other, BEPUphysics.NarrowPhaseSystems.Pairs.CollidablePairHandler collisionPair)
        {
            base.InitialCollisionDetected(sender, other, collisionPair);
            // If hit physics object link projectile to it then grapple to projectile, otherwise set projectile to kinematic and grapple to projectile
            if (other.Tag is CharacterSynchronizer)
            {
                // Do something to the character
            }
        }

    }
}