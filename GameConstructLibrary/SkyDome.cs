using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GraphicsLibrary;

namespace GameConstructLibrary
{
    public class SkyDome : IGameObject, IActor
    {
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

        public string TextureName
        {
            get
            {
                return mModel.TextureName;
            }
            set
            {
                mModel.TextureName = value;
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

        private SkyDomeRenderable mModel;

        public SkyDome(string skyTexture)
        {
            mModel = new SkyDomeRenderable(skyTexture);

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
