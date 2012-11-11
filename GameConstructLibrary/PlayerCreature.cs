using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameConstructLibrary
{
    class PlayerCreature
    {
        private const float mSneak = 10.0f;
        public float Sneak
        {
            get
            {
                return mSneak;
            }
        }

        public void OnDeath()
        {
            // TODO: respawn
        }
    }
}
