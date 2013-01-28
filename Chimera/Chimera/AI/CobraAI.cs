using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameConstructLibrary;
using Chimera.Creatures;

namespace Chimera.AI
{
    class CobraAI : RangedAI
    {
        protected override List<Creature> FilterCreatures(List<Creature> collidingCreatures)
        {
            Type type = typeof(Cobra);
            List<Creature> noLikeType = new List<Creature>();
            foreach (Creature creature in collidingCreatures)
            {
                if (!(creature.GetType() == type))
                {
                    noLikeType.Add(creature);
                }
            }
            return noLikeType;
        }

        protected override int ChoosePartSlot()
        {
            return Rand.rand.Next(mCreature.PartAttachments.Count);
        }
    }
}
