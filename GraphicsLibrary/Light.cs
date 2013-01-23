using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GraphicsLibrary
{
    public class Light
    {
        public Vector3 Position
        {
            get { return mPosition; }
            set { mPosition = value; }
        }
        private Vector3 mPosition;

        public Vector3 Direction
        {
            get { return mDirection; }
            set { mDirection = value; }
        }
        private Vector3 mDirection;

        public Vector3 Up
        {
            get { return mUp; }
            set { mUp = value; }
        }
        private Vector3 mUp;

        public Vector3 AmbientColor
        {
            get { return mAmbientColor; }
            set { mAmbientColor = value; }
        }
        private Vector3 mAmbientColor;

        public Vector3 DiffuseColor
        {
            get { return mDiffuseColor; }
            set { mDiffuseColor = value; }
        }
        private Vector3 mDiffuseColor;

        public Vector3 SpecularColor
        {
            get { return mSpecularColor; }
            set { mSpecularColor = value; }
        }
        private Vector3 mSpecularColor;

        public Light(Vector3 direction, Vector3 ambientColor, Vector3 diffuseColor, Vector3 specularColor)
        {
            mPosition = Vector3.Zero;
            mDirection = direction;
            mAmbientColor = ambientColor;
            mDiffuseColor = diffuseColor;
            mSpecularColor = specularColor;
        }
    }
}
