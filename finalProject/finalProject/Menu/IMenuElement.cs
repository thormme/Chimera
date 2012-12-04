using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace finalProject.Menu
{
    interface IMenuElement
    {
        Vector2 Position { get; set; }
        Vector2 Size { get; set; }

        MenuState Menu;

        void Update(GameTime gameTime);
        void Render(SpriteBatch spriteBatch);
    }
}
