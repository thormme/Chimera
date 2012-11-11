using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GraphicsLibrary;

namespace GameConstructLibrary
{
    /// <summary>
    /// Interface for all movable game objects.
    /// </summary>
    public interface IGameObject
    {
        Vector3 Position
        {
            get;
            set;
        }

        Matrix XNAOrientationMatrix
        {
            get;
            set;
        }

        float Scale
        {
            get;
            set;
        }

        void Render();
    }
}
