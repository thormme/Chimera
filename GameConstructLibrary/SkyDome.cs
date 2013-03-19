using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GraphicsLibrary;

namespace GameConstructLibrary
{
    public class SkyDome : IGameObject
    {
        public Vector3 Position
        {
            get;
            private set;
        }

        public Vector3 Scale
        {
            get;
            set;
        }

        public World World
        {
            protected get;
            set;
        }

        public Matrix XNAOrientationMatrix
        {
            get
            {
                return Matrix.CreateTranslation(Position) * Matrix.CreateScale(Scale.X);
            }
        }

        private InanimateModel mModel;

        public SkyDome(string skyTexture)
        {
            mModel = new InanimateModel("skyDome");

            Position = new Vector3(1, 1, 1);
            //Scale = new Vector3(GraphicsManager.BoundingBoxHypotenuse * 2.5f);
        }

        public void Render()
        {
            //Scale = new Vector3(GraphicsManager.BoundingBoxHypotenuse * 2.5f / 40.0f);
            Matrix scaleMatrix = Matrix.CreateScale(Scale);
            mModel.Render(scaleMatrix);
        }
    }
}
