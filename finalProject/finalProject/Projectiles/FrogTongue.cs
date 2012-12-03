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

namespace finalProject.Projectiles
{
    public class FrogTongue : Projectile
    {

        Vector3 mDestination;
        DistanceLimit mRopeLimit;
        bool mReachedDestination;

        public FrogTongue(Actor owner, Vector3 direction, Vector3 position)
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
            mDestination = position;
            Entity.IsAffectedByGravity = false;
            mReachedDestination = false;
        }

        public override void Update(GameTime time)
        {
            base.Update(time);

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
                            mRopeLimit.MaximumLength -= 20.0f * (float)time.ElapsedGameTime.TotalSeconds;
                        }
                    }
                }
            }
        }

        public override void InitialCollisionDetected(BEPUphysics.Collidables.MobileCollidables.EntityCollidable sender, Collidable other, BEPUphysics.NarrowPhaseSystems.Pairs.CollidablePairHandler collisionPair)
        {
            base.InitialCollisionDetected(sender, other, collisionPair);
            // If hit physics object link projectile to it then grapple to projectile, otherwise set projectile to kinematic and grapple to projectile
            if (other.Tag is IEntityOwner)
            {
                IEntityOwner entityOwner = other as IEntityOwner;
                StickToEntity(entityOwner.Entity);
            }
            else
            {
                
            }
            CreateRope();
        }

        private void ReleaseTongue()
        {

            if (mRopeLimit != null)
            {
                Console.WriteLine("releasing tongue");

                mReachedDestination = false;
                mOwner.World.Space.Remove(mRopeLimit);
                mRopeLimit = null;
                //Unstick();
            }

        }

        
        protected override void Hit(IGameObject gameObject)
        {
            /*
            PhysicsObject anchor = (gameObject as PhysicsObject);
            if (anchor != null)
            {
                CreateRope(anchor);
            }
             */
        }

        private void CreateRope()
        {
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
