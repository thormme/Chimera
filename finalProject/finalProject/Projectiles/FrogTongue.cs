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

namespace finalProject.Projectiles
{
    public class FrogTongue : Projectile
    {

        DistanceLimit mRopeLimit;
        PhysicsObject mAnchorObject;
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
            Entity.IsAffectedByGravity = false;
            mReachedDestination = false;
        }

        public override void Update(GameTime time)
        {
            base.Update(time);
            if (mRopeLimit != null) // You have a connection
            {
                Ray ray = new Ray(mOwner.Entity.Position, mAnchorObject.Entity.Position - mOwner.Entity.Position);
                List<RayCastResult> results = new List<RayCastResult>();
                mOwner.World.Space.RayCast(ray, (mAnchorObject.Entity.Position - mOwner.Entity.Position).Length(), results);
                foreach (RayCastResult result in results)
                {
                    if (result.HitObject as Collidable == mAnchorObject.Entity.CollisionInformation)
                    {
                        Console.WriteLine((result.HitData.Location - mOwner.Entity.Position).Length());
                        if ((result.HitData.Location - mOwner.Entity.Position).Length() <= 5.0f)
                        {
                            mReachedDestination = true;
                        }
                        else if (!mReachedDestination)
                        {
                            Console.WriteLine("here");
                            mRopeLimit.MaximumLength -= 20.0f * (float)time.ElapsedGameTime.TotalSeconds;
                        }
                    }
                }
            }
        }

        private void ReleaseTongue()
        {
            if (mRopeLimit != null)
            {
                mReachedDestination = false;
                mOwner.World.Space.Remove(mRopeLimit);
                mRopeLimit = null;
                Unstick();
            }
        }

        protected override void Hit(IGameObject gameObject)
        {
            PhysicsObject anchor = (gameObject as PhysicsObject);
            if (anchor != null)
            {
                mAnchorObject = anchor;
                CheckHits = false;
                StickToObject(mAnchorObject);
                mRopeLimit = new DistanceLimit(mOwner.Entity, mAnchorObject.Entity, mOwner.Entity.Position, mAnchorObject.Entity.Position, 0.0f, (mAnchorObject.Entity.Position - mOwner.Entity.Position).Length());
                mRopeLimit.Bounciness = 0.8f;
                mOwner.World.Space.Add(mRopeLimit);
            }
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
