using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameConstructLibrary;
using GraphicsLibrary;
using Microsoft.Xna.Framework;
using BEPUphysics.Entities.Prefabs;
using BEPUphysics.Constraints.TwoEntity.JointLimits;

namespace finalProject.Projectiles
{
    public class FrogTongue : Projectile
    {

        DistanceLimit mRopeLimit;

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
        }

        public override void Update(GameTime time)
        {
            if (mRopeLimit == null)
            {
                base.Update(time);
            }
            else
            {
                if (mRopeLimit.MaximumLength >= 1.0f) mRopeLimit.MaximumLength -= 10.0f * (float)time.ElapsedGameTime.TotalSeconds;
                else
                {
                    mOwner.World.Space.Remove(mRopeLimit);
                    mRopeLimit = null;
                }
            }
        }

        protected override void Hit(IGameObject gameObject)
        {
            PhysicsObject anchor = (gameObject as PhysicsObject);
            if (anchor != null)
            {
                mRopeLimit = new DistanceLimit(mOwner.Entity, anchor.Entity, mOwner.Entity.Position, anchor.Entity.Position, 0.0f, (mOwner.Entity.Position - anchor.Entity.Position).Length());
                mRopeLimit.Bounciness = 0.8f;
                mOwner.World.Space.Add(mRopeLimit);
            }
        }
    }
}
