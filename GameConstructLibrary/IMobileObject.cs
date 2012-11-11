using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameConstructLibrary
{
    /// <summary>
    /// Interface for all movable game objects.
    /// </summary>
    public interface IMobileObject : IGameObject
    {
        new Vector3 Position
        {
            get;
            set;
        }

        new Matrix XNAOrientationMatrix
        {
            get;
            set;
        }

        new float Scale
        {
            get;
            set;
        }

        new void Render();
    }
}
