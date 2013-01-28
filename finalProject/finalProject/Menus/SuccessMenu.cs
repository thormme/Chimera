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
    class SuccessMenu : GameMenu
    {
        ModelDrawer mDebugDrawer;
        Game mOwnerGame;

        public SuccessMenu(Game game)
        {
            mOwnerGame = game;

            Sprite backgroundSprite = new Sprite("green");
            int height = (int)(ChimeraGame.Graphics.PreferredBackBufferHeight);
            int width = (int)(ChimeraGame.Graphics.PreferredBackBufferWidth);
            int y = 0;
            GraphicItem background = new GraphicItem(
                new Microsoft.Xna.Framework.Rectangle(
                    ChimeraGame.Graphics.PreferredBackBufferWidth / 2 - width / 2,
                    y,
                    width,
                    height
                ),
                backgroundSprite
            );

            Sprite buttonSprite = new Sprite("empty");
            height = (int)(ChimeraGame.Graphics.PreferredBackBufferHeight);
            width = (int)(ChimeraGame.Graphics.PreferredBackBufferWidth);
            y = 0;
            Button button = new Button(
                new Microsoft.Xna.Framework.Rectangle(
                    ChimeraGame.Graphics.PreferredBackBufferWidth / 2 - width / 2,
                    y,
                    width,
                    height
                ),
                buttonSprite,
                new Button.ButtonAction(Continue)
            );

            Sprite successSprite = new Sprite("success");
            height = (int)(ChimeraGame.Graphics.PreferredBackBufferHeight);
            width = (int)((float)successSprite.Width / (float)successSprite.Height * (float)height);
            y = 0;
            GraphicItem success = new GraphicItem(
                new Microsoft.Xna.Framework.Rectangle(
                    ChimeraGame.Graphics.PreferredBackBufferWidth / 2 - width / 2,
                    y,
                    width,
                    height
                ),
                successSprite
            );

            Add(background);
            Add(button);
            Add(success);
        }

        private void Continue(Button button)
        {
            ChimeraGame.PopState();
            ChimeraGame.Game.ExitToMenu();
        }

        public override void Render()
        {
            base.Render();
        }
    }
}
