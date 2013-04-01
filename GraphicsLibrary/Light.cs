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
            set
            { 
                mPosition = value;
                mDirection = mGaze - mPosition;
            }
        }
        private Vector3 mPosition;

        public Vector3 Gaze
        {
            get { return mGaze; }
            set { mGaze = value; }
        }
        private Vector3 mGaze;

        public Vector3 Direction
        {
            get { return mDirection; }
        }
        private Vector3 mDirection;

        public float PositionTheta
        {
            get { return mPositionTheta; }
            set
            {
                mPositionTheta = value;
                Position = Vector3.Normalize(new Vector3(
                    (float)Math.Sin((double)PositionTheta) * (float)Math.Cos((double)PositionPhi),
                    (float)Math.Sin((double)PositionTheta) * (float)Math.Sin((double)PositionPhi),
                    (float)Math.Cos((double)PositionTheta)));
            }
        }
        private float mPositionTheta = 0.0f;

        public float PositionPhi
        {
            get { return mPositionPhi; }
            set
            {
                mPositionPhi = value;
                Position = Vector3.Normalize(new Vector3(
                    (float)Math.Sin((double)PositionTheta) * (float)Math.Cos((double)PositionPhi),
                    (float)Math.Sin((double)PositionTheta) * (float)Math.Sin((double)PositionPhi),
                    (float)Math.Cos((double)PositionTheta)));
            }
        }
        private float mPositionPhi = 0.0f;

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

        public Light(Vector3 position, Vector3 gaze, Vector3 ambientColor, Vector3 diffuseColor, Vector3 specularColor)
        {
            mGaze = gaze;
            Position = position;
            mPositionPhi = (float)Math.Atan(Position.Y / Position.X);
            mPositionTheta = (float)Math.Acos(Position.Z / Position.Length());
            mAmbientColor = ambientColor;
            mDiffuseColor = diffuseColor;
            mSpecularColor = specularColor;
        }
    }
}
