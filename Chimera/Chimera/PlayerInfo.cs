using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chimera
{
    public class PlayerInfo
    {

        public List<Type> Parts
        {
            get
            {
                return mAvailableParts;
            }
            private set
            {
                mAvailableParts = value;
            }
        }
        private List<Type> mAvailableParts = new List<Type>();

        public PlayerInfo()
        {

        }

        public PlayerInfo(List<Type> availableParts)
        {
            mAvailableParts = availableParts;
        }

        public void AddPart(Type partType)
        {
            if (!mAvailableParts.Contains(partType))
            {
                mAvailableParts.Add(partType);
            }
        }

        public void RemovePart(Type partType)
        {
            mAvailableParts.Remove(partType);
        }

    }
}
