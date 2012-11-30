using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GraphicsLibrary;

namespace finalProject.Parts
{
    public class TurtleShell : Part
    {
        public TurtleShell()
            : base(
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
                        new Vector3(0.25f, 0.25f, 0.25f)
                    )
                }
            )
        { }

        public override void Update(GameTime time)
        { }

        public override void Use(Vector3 direction)
        {
            Creature.Invulnerable = true;
        }

        public override void FinishUse(Vector3 direction)
        {
            Creature.Invulnerable = false;
        }
    }
}
