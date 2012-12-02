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

namespace finalProject.Projectiles
{
    public class FrogTongue : Projectile
    {

        DistanceLimit mRopeLimit;
        PhysicsObject mAnchorObject;

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
        }

        public override void Update(GameTime time)
        {
            base.Update(time);
            if (mRopeLimit != null) // You have a connection
            {
                if (!mAnchorObject.CollidingObjects.Contains(mOwner.Entity.Tag))
                {
                    ReleaseTongue();
                }
                else
                {
                    mRopeLimit.MaximumLength -= 20.0f * (float)time.ElapsedGameTime.TotalSeconds;
                }
            }
        }

        private void ReleaseTongue()
        {
            if (mRopeLimit != null)
            {
                Unstick();
                mOwner.World.Space.Remove(mRopeLimit);
                mRopeLimit = null;
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
