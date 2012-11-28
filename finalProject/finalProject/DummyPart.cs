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

        public DummyPart(Vector3 position)
            : base(new InanimateModel("box"), new Box(new Vector3(0.0f), 025.0f, 025.0f, 025.0f, 1.0f))
        {
            Position = position;
            Scale = new Vector3(025.0f, 025.0f, 025.0f);
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
