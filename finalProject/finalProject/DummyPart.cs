using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using BEPUphysics.Entities.Prefabs;
using GraphicsLibrary;

namespace finalProject
{
    class DummyPart : Part
    {
        private const float JumpSpeed = 4.0f;
        private const double CoolDownTime = 2.0f;
        private double mCoolDownTimer;
    
        public DummyPart()
            : base(
            new InanimateModel("sphere"),
            new Creature.PartBone[] { 
                Creature.PartBone.ArmLeft1, 
                Creature.PartBone.LegFrontLeft1, 
                Creature.PartBone.L_Index1 
            }, 
            1,
            new Vector3(),
            Matrix.CreateFromQuaternion(new Quaternion()),
            new Vector3(0.25f, 0.25f, 0.25f)
            )
        {
            mCoolDownTimer = -1.0f;
        }

        public override void Update(GameTime time)
        {
            mCoolDownTimer -= time.ElapsedGameTime.TotalSeconds;
        }

        public override void Use(Vector3 direction)
        {
            if (mCoolDownTimer < 0.0f)
            {
                Creature.Entity.LinearVelocity = Vector3.Normalize(direction) * JumpSpeed;
                Creature.Jump();
                mCoolDownTimer = CoolDownTime;
            }
        }
    }
}
