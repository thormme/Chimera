﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Chimera.AI
{
    class ActivationAI : PassiveAI
    {
        protected override void UsePartUpdate(GameTime time)
        {
            if (mUsingPart)
            {
                mCreature.UsePart(ChoosePartSlot(), mUsePartDirection);
            }
        }
    }
}