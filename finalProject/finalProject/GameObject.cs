using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GraphicsLibrary;

namespace finalProject
{
    abstract class GameObject
    {
        public GameObject(Renderable renderable)
        {
            // TODO: Complete member initialization
            this.mRenderable = renderable;
        }

        private Renderable mRenderable;
        public Vector3 Position
        {
            virtual get;
            virtual set;
        }

        public Matrix Orientation
        {
            virtual get;
            virtual set;
        }

        public float Scale
        {
            virtual get;
            virtual set;
        }

        public virtual void Render()
        {
            mRenderable.render();
        }
    }
}
