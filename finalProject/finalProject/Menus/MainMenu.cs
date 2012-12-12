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
    class MainMenu : GameMenu
    {
        ModelDrawer mDebugDrawer;
        Game mOwnerGame;

        public MainMenu(Game game, ModelDrawer debugModelDrawer)
        {
            mDebugDrawer = debugModelDrawer;
            mOwnerGame = game;

            Sprite titleSprite = new Sprite("title");
            int height = (int)(Game1.Graphics.PreferredBackBufferHeight * .25);
            int width = (int)((float)titleSprite.Width / (float)titleSprite.Height * (float)height);
            GraphicItem title = new GraphicItem(
                new Microsoft.Xna.Framework.Rectangle(
                    Game1.Graphics.PreferredBackBufferWidth / 2 - width / 2,
                    0,
                    width,
                    height
                ),
                titleSprite
            );

            Sprite playSprite = new Sprite("play");
            height = (int)(Game1.Graphics.PreferredBackBufferHeight * .15);
            width = (int)((float)playSprite.Width / (float)playSprite.Height * (float)height);
            int y = (int)(Game1.Graphics.PreferredBackBufferHeight * .2);
            Button play = new Button(
                new Microsoft.Xna.Framework.Rectangle(
                    Game1.Graphics.PreferredBackBufferWidth / 2 - width / 2,
                    y + height,
                    width,
                    height
                ),
                playSprite,
                new Button.ButtonAction(StartGame)
            );

            Sprite quitSprite = new Sprite("options");
            height = (int)(Game1.Graphics.PreferredBackBufferHeight * .15);
            width = (int)((float)quitSprite.Width / (float)quitSprite.Height * (float)height);
            y = (int)(Game1.Graphics.PreferredBackBufferHeight * .5);
            Button quit = new Button(
                new Microsoft.Xna.Framework.Rectangle(
                    Game1.Graphics.PreferredBackBufferWidth / 2 - width / 2,
                    y + height,
                    width,
                    height
                ),
                quitSprite,
                new Button.ButtonAction(Quit)
            );

            Add(title);
            Add(play);
            Add(quit);
        }

        private void StartGame(GameConstructLibrary.Menu.Button button)
        {
            InputAction.IsMouseLocked = true;
            Game1.PopState();

            GameWorld world = new GameWorld(mDebugDrawer);
            world.AddLevelFromFile("tree", new Vector3(0, 0, 0), Quaternion.Identity, new Vector3(8.0f, 0.01f, 8.0f));
            Game1.PushState(world);

        }

        private void Quit(Button button)
        {
            mOwnerGame.Exit();
        }

        public override void Render()
        {
            base.Render();
        }
    }
}
