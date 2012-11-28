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
        /// <summary>
        /// The position of the object.
        /// </summary>
        Vector3 Position
        {
            get;
        }

        /// <summary>
        /// The world within which the object is contained.
        /// </summary>
        World World
        {
            set;
        }

        /// <summary>
        /// The orientation of the object, given in XNA Matrix format.
        /// </summary>
        Matrix XNAOrientationMatrix
        {
            get;
        }

        /// <summary>
        /// The scale of the object.
        /// </summary>
        Vector3 Scale
        {
            get;
        }

        /// <summary>
        /// Render the object to the screen.
        /// </summary>
        void Render();
    }
}
