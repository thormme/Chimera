using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using BEPUphysics.Entities.Prefabs;
using GraphicsLibrary;
using GameConstructLibrary;
using BEPUphysics;
using BEPUphysics.Collidables.MobileCollidables;
using BEPUphysics.Constraints.TwoEntity.JointLimits;

namespace finalProject.Parts
{
    class FrogHead : CooldownPart
    {

        DistanceLimit mRopeLimit;

        public FrogHead()
            : base(
                3.0,
                new Part.SubPart[] {
                    new SubPart(
                        new InanimateModel("sphere"),
                        new Creature.PartBone[] { 
                            Creature.PartBone.HeadCenterCap,
                            Creature.PartBone.HeadLeftCap,
                            Creature.PartBone.HeadRightCap
                        },
                        new Vector3(),
                        Matrix.CreateFromYawPitchRoll(-MathHelper.PiOver2, 0, 0),
                        new Vector3(1.0f, 1.0f, 1.0f)
                    )
                }
            )
        {
            //(mRenderable as AnimateModel).PlayAnimation("Take 001");
        }

        public override void Update(Microsoft.Xna.Framework.GameTime time)
        {
            //(mRenderable as AnimateModel).Update(time);
            if (mRopeLimit != null)
            {
                if (mRopeLimit.MaximumLength >= 1.0f) mRopeLimit.MaximumLength -= 10.0f * (float)time.ElapsedGameTime.TotalSeconds;
                else
                {
                    Creature.World.Space.Remove(mRopeLimit);
                    mRopeLimit = null;
                }
                
            }

            base.Update(time);
        }

        protected override void UseCooldown(Microsoft.Xna.Framework.Vector3 direction)
        {
            Ray ray = new Ray(Creature.Position, Creature.XNAOrientationMatrix.Forward);
            RayCastResult result;
            Creature.World.Space.RayCast(ray, out result);
            EntityCollidable temp = (result.HitObject as EntityCollidable);
            if (temp != null)
            {
                mRopeLimit = new DistanceLimit(Creature.Entity, temp.Entity, Creature.Entity.Position, temp.Entity.Position, 0.0f, (Creature.Entity.Position - temp.Entity.Position).Length());
                mRopeLimit.Bounciness = 0.8f;
                Creature.World.Space.Add(mRopeLimit);
            }
        }

        public override void FinishUse(Vector3 direction)
        {
            if (mRopeLimit != null)
            {
                Creature.World.Space.Remove(mRopeLimit);
                mRopeLimit = null;
            }
        }

        protected override void Reset()
        {
            FinishUse(Vector3.Zero);
        }
    }
}
