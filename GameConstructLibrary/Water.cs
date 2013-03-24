using GraphicsLibrary;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameConstructLibrary
{
    public class Water : IGameObject, IActor
    {
        #region Public Properties

        public float SeaLevel
        {
            get { return mModel.SeaLevel; }
            set { mModel.SeaLevel = value; }
        }

        public Vector2 Resolution
        {
            get { return mModel.Resolution; }
            set { mModel.Resolution = value; }
        }

        public string TextureName
        {
            get { return mModel.TextureName; }
            set { mModel.TextureName = value; }
        }

        private Vector3 mPosition;
        public Vector3 Position
        {
            get { return mPosition; }
            set
            {
                mPosition = value;
                XNAOrientationMatrix = Matrix.CreateTranslation(mPosition);
            }
        }
        
        public World World
        {
            protected get;
            set;
        }

        public Matrix XNAOrientationMatrix
        {
            get;
            private set;
        }

        public Vector3 Scale
        {
            get;
            private set;
        }

        #endregion

        #region Private Variables

        private WaterRenderable mModel;

        #endregion

        public Water(string waterTexture, float seaLevel, Vector2 vertexResolution)
        {
            mModel = new WaterRenderable(waterTexture, seaLevel, vertexResolution);

            Position = new Vector3(1, 1, 1);
            Scale = new Vector3(1);
            XNAOrientationMatrix = Matrix.Identity;
        }

        public void Render()
        {
            mModel.Render(XNAOrientationMatrix);
        }

        public void Update(GameTime gameTime)
        {
        }
    }
}
