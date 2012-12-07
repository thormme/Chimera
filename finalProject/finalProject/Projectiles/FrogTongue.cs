using System;
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
using BEPUphysics.Collidables.MobileCollidables;
using BEPUphysics.NarrowPhaseSystems.Pairs;

namespace finalProject.Projectiles
{
    public class FrogTongue : Projectile
    {
        InanimateModel mTongueGraphic;
        DistanceLimit mRopeLimit = null;

        public FrogTongue(Actor owner, Vector3 direction)
            : base(
            new InanimateModel("box"),
            new Box(owner.Position, 0.5f, 0.5f, 0.5f, 0.5f),
            owner,
            direction,
            60.0f,
            new Vector3(0.5f)
            )
        {
            mTongueGraphic = new InanimateModel("box");
            CheckHits = false;
            Entity.IsAffectedByGravity = false;
            Entity.CollisionInformation.CollisionRules.Group = Projectile.SensorProjectileGroup;
        }

        public override void Update(GameTime time)
        {
            base.Update(time);
        }

        public override void Render()
        {
            //base.Render();
            Vector3 offset = mOwner.Entity.Position - Entity.Position;
            offset.Normalize();
            mTongueGraphic.Render(mOwner.Entity.Position, 
                                  offset, 
                                  new Vector3(0.2f, 0.05f, (Entity.Position - mOwner.Entity.Position).Length()));
        }

        public override void InitialCollisionDetected(EntityCollidable sender, Collidable other, CollidablePairHandler collisionPair)
        {
            if (mRopeLimit == null)
            {
                base.InitialCollisionDetected(sender, other, collisionPair);
                // If hit physics object link projectile to it then grapple to projectile, otherwise set projectile to kinematic and grapple to projectile
                if (other.Tag is CharacterSynchronizer)
                {
                    StickToEntity((other.Tag as CharacterSynchronizer).body);
                }
                else if (other.Tag is IEntityOwner)
                {
                    StickToEntity((other.Tag as IEntityOwner).Entity);
                }
                else
                {
                    Entity.BecomeKinematic();
                }
                CreateRope();
            }
        }

        private void ReleaseTongue()
        {
            if (mRopeLimit != null)
            {
                mOwner.World.Space.Remove(mRopeLimit);
            }
            Unstick();
        }

        
        protected override void Hit(IGameObject gameObject)
        {

        }

        private void CreateRope()
        {
            Entity.AngularMomentum = Vector3.Zero;
            Entity.LinearMomentum = Vector3.Zero;
            mRopeLimit = new DistanceLimit(mOwner.Entity, Entity, mOwner.Entity.Position, Entity.Position, 0.0f, (Entity.Position - mOwner.Entity.Position).Length());
            mRopeLimit.Bounciness = 0.8f;
            mOwner.World.Space.Add(mRopeLimit);
        }
         

        public override World World
        {
            get
            {
                return base.World;
            }
            set
            {
                if (value == null)
                {
                    ReleaseTongue();
                }
                base.World = value;
            }
        }
    }
}
