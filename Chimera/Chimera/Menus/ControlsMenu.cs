using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameConstructLibrary.Menu;
using Microsoft.Xna.Framework;
using BEPUphysicsDrawer.Models;
using GraphicsLibrary;
using GameConstructLibrary;

namespace Chimera.Menus
{
    class ControlsMenu : GameMenu
    {
        Game mOwnerGame;

        public ControlsMenu(Game game)
        {
            mOwnerGame = game;

            Sprite backgroundSprite = new Sprite("blue");
            int height = (int)(GraphicsManager.Device.PresentationParameters.BackBufferHeight);
            int width = (int)(GraphicsManager.Device.PresentationParameters.BackBufferWidth);
            int y = 0;
            GraphicItem background = new GraphicItem(
                new Microsoft.Xna.Framework.Rectangle(
                    GraphicsManager.Device.PresentationParameters.BackBufferWidth / 2 - width / 2,
                    y,
                    width,
                    height
                ),
                backgroundSprite
            );

            Sprite buttonSprite = new Sprite("empty");
            height = (int)(GraphicsManager.Device.PresentationParameters.BackBufferHeight);
            width = (int)(GraphicsManager.Device.PresentationParameters.BackBufferWidth);
            y = 0;
            Button button = new Button(
                new Microsoft.Xna.Framework.Rectangle(
                    GraphicsManager.Device.PresentationParameters.BackBufferWidth / 2 - width / 2,
                    y,
                    width,
                    height
                ),
                buttonSprite,
                new Button.ButtonAction(Continue)
            );

            Sprite storySprite = new Sprite("controls");
            height = (int)(GraphicsManager.Device.PresentationParameters.BackBufferHeight);
            width = (int)((float)storySprite.Width / (float)storySprite.Height * (float)height);
            y = 0;
            GraphicItem controls = new GraphicItem(
                new Microsoft.Xna.Framework.Rectangle(
                    GraphicsManager.Device.PresentationParameters.BackBufferWidth / 2 - width / 2,
                    y,
                    width,
                    height
                ),
                storySprite
            );

            Add(background);
            Add(button);
            Add(controls);
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
