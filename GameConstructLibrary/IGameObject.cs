using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GraphicsLibrary;

namespace GameConstructLibrary
{
    /// <summary>
    /// Interface for all game objects.
    /// </summary>
    public interface IGameObject
    {
        Vector3 Position
        {
            get;
        }

        Matrix XNAOrientationMatrix
        {
            get;
        }

        float Scale
        {
            get;
        }

        void Render();
    }
}
