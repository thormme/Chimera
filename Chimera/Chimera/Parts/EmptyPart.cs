using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GraphicsLibrary;
using Microsoft.Xna.Framework;
using GameConstructLibrary;

namespace Chimera.Parts
{
    public class EmptyPart : Part
    {

        public EmptyPart()
            : base(
                new Part.SubPart[] {},
                false,
                new Sprite("empty")
            )
        { }


        public override void Use(Vector3 direction)
        {
            //throw new NotImplementedException();
        }

        public override void FinishUse(Vector3 direction)
        {
            //throw new NotImplementedException();
        }

        public override void Reset()
        {
            //throw new NotImplementedException();
        }

        public override void Cancel()
        {
            //throw new NotImplementedException();
        }
    }
}
