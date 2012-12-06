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
        DistanceLimit mRopeLimit;
        bool mReachedDestination;

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
            mReachedDestination = false;
        }

        public override void Update(GameTime time)
        {
            base.Update(time);
            if ((Entity.Position - mOwner.Entity.Position).Length() > 300)
            {

            }
            /*
            if (mRopeLimit != null) // You have a connection
            {
                Ray ray = new Ray(mOwner.Entity.Position, Entity.Position - mOwner.Entity.Position);
                List<RayCastResult> results = new List<RayCastResult>();
                mOwner.World.Space.RayCast(ray, (Entity.Position - mOwner.Entity.Position).Length(), results);
                foreach (RayCastResult result in results)
                {
                    if (result.HitObject as Collidable == Entity.CollisionInformation)
                    {
                        if ((result.HitData.Location - mOwner.Entity.Position).Length() <= 5.0f)
                        {
                            mReachedDestination = true;
                        }
                        else if (!mReachedDestination)
                        {
                            mRopeLimit.MaximumLength -= 10.0f * (float)time.ElapsedGameTime.TotalSeconds;
                        }
                    }
                }
            }
             */
        }

        public override void Render()
        {
            //base.Render();
            Vector3 offset = Entity.Position - mOwner.Entity.Position;
            offset.Normalize();
            mTongueGraphic.Render(mOwner.Entity.Position + offset * (Entity.Position - mOwner.Entity.Position).Length(), 
                                  offset, 
                                  new Vector3(0.2f, 0.05f, (Entity.Position - mOwner.Entity.Position).Length()));
        }

        public override void InitialCollisionDetected(EntityCollidable sender, Collidable other, CollidablePairHandler collisionPair)
        {
            base.InitialCollisionDetected(sender, other, collisionPair);
            // If hit physics object link projectile to it then grapple to projectile, otherwise set projectile to kinematic and grapple to projectile
            if (other.Tag is CharacterSynchronizer)
            {
                StickToEntity((other.Tag as CharacterSynchronizer).body);
            }
            CreateRope();
        }

        private void ReleaseTongue()
        {

            if (mRopeLimit != null)
            {
                mReachedDestination = false;
                mOwner.World.Space.Remove(mRopeLimit);
            }

        }

        
        protected override void Hit(IGameObject gameObject)
        {

        }

        private void CreateRope()
        {
            Entity.AngularMomentum = Vector3.Zero;
            Entity.LinearMomentum = Vector3.Zero;
            Entity.BecomeKinematic();
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
                base.World = value;
                if (World == null)
                {
                    ReleaseTongue();
                }
            }
        }

    }
}
