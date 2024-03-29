﻿using System;
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
    class GoalsMenu : GameMenu
    {
        Game mOwnerGame;

        public GoalsMenu(Game game)
        {
            mOwnerGame = game;

            Sprite backgroundSprite = new Sprite("yellow");
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

            Sprite storySprite = new Sprite("goals");
            height = (int)(GraphicsManager.Device.PresentationParameters.BackBufferHeight);
            width = (int)((float)storySprite.Width / (float)storySprite.Height * (float)height);
            y = 0;
            GraphicItem goals = new GraphicItem(
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
            Add(goals);
        }

        private void Continue(Button button)
        {
            ChimeraGame.PopState();
            ChimeraGame.PushState(new ControlsMenu(mOwnerGame));
        }

        public override void Render()
        {
            base.Render();
        }
    }
}
