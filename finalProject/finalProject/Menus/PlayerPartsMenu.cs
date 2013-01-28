using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameConstructLibrary.Menu;
using Microsoft.Xna.Framework;
using BEPUphysicsDrawer.Models;
using GraphicsLibrary;
using GameConstructLibrary;

namespace finalProject.Menus
{
    class PlayerPartsMenu : GameMenu
    {
        Game mOwnerGame;

        public PlayerPartsMenu(Game game)
        {
            mOwnerGame = game;

            Sprite backgroundSprite = new Sprite("yellow");
            int height = (int)(Game1.Graphics.PreferredBackBufferHeight);
            int width = (int)(Game1.Graphics.PreferredBackBufferWidth);
            int y = 0;
            GraphicItem background = new GraphicItem(
                new Microsoft.Xna.Framework.Rectangle(
                    Game1.Graphics.PreferredBackBufferWidth / 2 - width / 2,
                    y,
                    width,
                    height
                ),
                backgroundSprite
            );

            Sprite buttonSprite = new Sprite("empty");
            height = (int)(Game1.Graphics.PreferredBackBufferHeight);
            width = (int)(Game1.Graphics.PreferredBackBufferWidth);
            y = 0;
            Button button = new Button(
                new Microsoft.Xna.Framework.Rectangle(
                    Game1.Graphics.PreferredBackBufferWidth / 2 - width / 2,
                    y,
                    width,
                    height
                ),
                buttonSprite,
                new Button.ButtonAction(Continue)
            );

            Add(background);
            Add(button);
        }

        private void Continue(Button button)
        {
            Game1.PopState();
            Game1.PushState(new ControlsMenu(mOwnerGame));
        }

        public override void Render()
        {
            base.Render();
        }
    }
}
