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

        public Sprite Sprite
        {
            get
            {
                return mSprite;
            }
            set
            {
                mSprite = value;
            }
        }
        protected Sprite mSprite;

        public Rectangle Bounds
        {
            get
            {
                return mBounds;
            }
            set
            {
                mBounds = value;
            }
        }
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
