using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GraphicsLibrary;

namespace finalProject.Parts
{
    public class TurtleShell : MeteredPart
    {
        private bool mActive = false;

        public TurtleShell()
            : base(
                3.0f,
                3.0f,
                new Part.SubPart[] {
                    new SubPart(
                        new InanimateModel("sphere"),
                        new Creature.PartBone[] {
                            Creature.PartBone.Spine1Cap,
                            Creature.PartBone.Spine2Cap,
                            Creature.PartBone.Spine3Cap
                        },
                        new Vector3(),
                        Matrix.CreateFromQuaternion(new Quaternion()),
                        new Vector3(0.02f)
                    )
                },
            false
            )
        { }

        public override void Update(GameTime time)
        { }

        protected override void UseMeter(Vector3 direction)
        {
            if (!mActive && Creature != null)
            {
                mActive = true;
                Creature.Invulnerable = true;
                Creature.Move(Vector2.Zero);
                Creature.Controller.NoControl = true;
            }
        }

        protected override void FinishUseMeter()
        {
            if (mActive)
            {
                mActive = false;
                Creature.Invulnerable = false;
                Creature.Controller.NoControl = false;
            }
        }

        public override void Reset()
        {
            FinishUse(Vector3.Zero);
        }
    }
}
