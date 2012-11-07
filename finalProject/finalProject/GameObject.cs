using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace finalProject
{
    abstract class GameObject
    {
        private IRenderable mRenderable;
        public Vector3 Position {
            get { return mRenderable.Position; }
            set { mRenderable.Position = value; }
        }

        public virtual void Render()
        {
            mRenderable.render();
        }
    }
}
