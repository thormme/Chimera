using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphicsLibrary
{
    public class ParticleDefinition
    {
        public Matrix WorldTransform
        {
            get { return mWorldTransform; }
            set { mWorldTransform = value; }
        }
        private Matrix mWorldTransform;

        public string Name
        {
            get { return mName; }
            set { mName = value; }
        }
        private string mName;

        public ParticleDefinition(string name, Matrix worldTransform)
        {
            mName = name;
            mWorldTransform = worldTransform;
        }
    }
}
