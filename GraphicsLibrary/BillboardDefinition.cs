using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GraphicsLibrary
{
    class BillboardDefinition
    {
        public string Name
        {
            get { return mName; }
            set { mName = value; }
        }
        private string mName;

        public Vector3 Position
        {
            get { return mPosition; }
            set { mPosition = value; }
        }
        private Vector3 mPosition;

        public float Width
        {
            get { return mWidth; }
            set { mWidth = value; }
        }
        private float mWidth;

        public float Height
        {
            get { return mHeight; }
            set { mHeight = value; }
        }
        private float mHeight;

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

        public BillboardDefinition(string name, Vector3 position, float width, float height, Color blendColor, float blendColorWeight)
        {
            mName = name;
            mPosition = position;
            mWidth = width;
            mHeight = height;
            mBlendColor = blendColor;
            mBlendColorWeight = blendColorWeight;
        }
    }
}
