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
using finalProject.Projectiles;
using BEPUphysics.BroadPhaseEntries;

namespace finalProject.Parts
{
    class FrogHead : CooldownPart
    {

        private const float tongueLength = 500.0f;
        private FrogTongue mTongue;

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
            base.Update(time);
        }

        protected override void UseCooldown(Microsoft.Xna.Framework.Vector3 direction)
        {
            Ray ray = new Ray(Creature.Entity.Position, new Vector3(direction.X, 0.0f, direction.Z));
            RayCastResult result;
            Func<BroadPhaseEntry, bool> filter = (bfe) => (bfe.CollisionRules.Group != Sensor.SensorGroup);
            if (Creature.World.Space.RayCast(ray, filter, out result))
            {
                mTongue = new FrogTongue(Creature, direction, result.HitData.Location);
                Creature.World.Add(mTongue);
            }

        }

        

        public override void FinishUse(Vector3 direction)
        {
            if (Creature != null && mTongue != null)
            {
                Creature.World.Remove(mTongue);
            }
            mTongue = null;
        }

        public override void Reset()
        {
            FinishUse(Vector3.Zero);
        }

    }
}
