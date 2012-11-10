using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GraphicsLibrary;

namespace finalProject
{
    public interface GameObject
    {
        Vector3 Position
        {
            get;
            set;
        }

        Matrix Orientation
        {
            get;
            set;
        }

        //float Scale
        //{
        //    get;
        //    set;
        //}

        void Render();
    }
}
