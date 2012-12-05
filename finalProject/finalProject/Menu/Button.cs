using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace finalProject.Menu
{
    public class Button : IMenuElement
    {
        public Button(Vector2 position, Vector2 size)
        {
            Position = position;
            Size = size;
        }

        public void Update(GameTime gameTime)
        {
            if (Mouse.GetState().X >= Position.X && Mouse.GetState().X <= (Position + Size).X
                && Mouse.GetState().Y >= Position.Y && Mouse.GetState().Y <= (Position + Size).Y)
            {

            }
        }

        public void Render(SpriteBatch spriteBatch)
        {
            //spriteBatch.DrawString(spriteBatch., Text);
        }

        public Vector2 Position
        {
            get;
            set;
        }

        public Vector2 Size
        {
            get;
            set;
        }
    }
}
