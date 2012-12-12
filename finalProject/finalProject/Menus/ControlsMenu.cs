﻿using System;
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
    class ControlsMenu : GameMenu
    {
        ModelDrawer mDebugDrawer;
        Game mOwnerGame;

        public ControlsMenu(Game game, ModelDrawer debugModelDrawer)
        {
            mDebugDrawer = debugModelDrawer;
            mOwnerGame = game;

            Sprite backgroundSprite = new Sprite("blue");
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

            Sprite storySprite = new Sprite("controls");
            height = (int)(Game1.Graphics.PreferredBackBufferHeight);
            width = (int)((float)storySprite.Width / (float)storySprite.Height * (float)height);
            y = 0;
            GraphicItem controls = new GraphicItem(
                new Microsoft.Xna.Framework.Rectangle(
                    Game1.Graphics.PreferredBackBufferWidth / 2 - width / 2,
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
            Game1.PopState();
            Game1.PushState(new MainMenu(mOwnerGame, mDebugDrawer));
        }

        public override void Render()
        {
            base.Render();
        }
    }
}
