using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GraphicsLibrary;

namespace GameConstructLibrary.Menu
{
    public class GraphicItem : IMenuItem
    {
        public GameMenu Menu { get; set; }

        protected Sprite mSprite;
        protected Rectangle mBounds;

        public GraphicItem(Rectangle bounds, Sprite sprite)
        {
            mSprite = sprite;
            mBounds = bounds;
        }

        public virtual void Update(GameTime gameTime)
        {
        }

        public virtual void Render()
        {
            mSprite.Render(mBounds);
        }
    }
}
