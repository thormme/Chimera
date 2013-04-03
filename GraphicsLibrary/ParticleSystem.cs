using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphicsLibrary
{
    public class ParticleSystem
    {
        private string mTextureName;
        private Matrix mWorldTransform = Matrix.Identity;

        public Vector3 Position
        {
            get { return mPosition; }
            set
            { 
                mPosition = value;
                mWorldTransform = Matrix.CreateTranslation(mPosition);
            }
        }
        private Vector3 mPosition;

        public ParticleSystem(string textureName, Vector3 position)
        {
            mTextureName = textureName;
            Position = position;
        }

        public void Render()
        {
            //GraphicsManager.RenderParticles(mTextureName, mWorldTransform);
        }
    }
}
