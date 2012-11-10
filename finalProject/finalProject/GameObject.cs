using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GraphicsLibrary;

namespace finalProject
{
    interface GameObject
    {
        public Vector3 Position
        {
            get;
            set;
        }

        public Matrix Orientation
        {
            get;
            set;
        }

        public float Scale
        {
            get;
            set;
        }

        public void Render();
    }
}
