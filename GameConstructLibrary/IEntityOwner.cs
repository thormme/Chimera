using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BEPUphysics.Entities;

namespace GameConstructLibrary
{
    public interface IEntityOwner
    {
        Entity Entity
        {
            get;
        }
    }
}
