using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GraphicsLibrary
{
    class SpriteDefinition
    {
        public string Name
        {
            get { return mName; }
            set { mName = value; }
        }
        private string mName;

        public Rectangle ScreenSpace
        {
            get { return mScreenSpace; }
            set { mScreenSpace = value; }
        }
        private Rectangle mScreenSpace;

        public Color BlendColor
        {
            get { return mBlendColor; }
            set { mBlendColor = value; }
        }
        private Color mBlendColor;

        public float BlendColorWeight
        {
            get { return mBlendColorWeight; }
            set { mBlendColorWeight = value; }
        }
        private float mBlendColorWeight;

        public SpriteDefinition(string name, Rectangle screenSpace, Color blendColor, float blendColorWeight)
        {
            mName = name;
            mScreenSpace = screenSpace;
            mBlendColor = blendColor;
            mBlendColorWeight = blendColorWeight;
        }
    }
}
