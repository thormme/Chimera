using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GraphicsLibrary;

namespace finalProject
{
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

        //float Scale
        //{
        //    get;
        //    set;
        //}

        void Render();
    }
}
