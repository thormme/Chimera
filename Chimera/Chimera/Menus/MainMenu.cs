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
    class MainMenu : GameMenu
    {
        ModelDrawer mDebugDrawer;
        Game mOwnerGame;

        public MainMenu(Game game, ModelDrawer debugModelDrawer)
        {
            mDebugDrawer = debugModelDrawer;
            mOwnerGame = game;

            Sprite titleSprite = new Sprite("chimera");
            int height = (int)(GraphicsManager.Device.PresentationParameters.BackBufferHeight * .25);
            int width = (int)((float)titleSprite.Width / (float)titleSprite.Height * (float)height);
            int y = (int)(GraphicsManager.Device.PresentationParameters.BackBufferHeight * .1);
            GraphicItem title = new GraphicItem(
                new Microsoft.Xna.Framework.Rectangle(
                    GraphicsManager.Device.PresentationParameters.BackBufferWidth / 2 - width / 2,
                    y,
                    width,
                    height
                ),
                titleSprite
            );

            Sprite playSprite = new Sprite("play");
            height = (int)(GraphicsManager.Device.PresentationParameters.BackBufferHeight * .15);
            width = (int)((float)playSprite.Width / (float)playSprite.Height * (float)height);
            y = (int)(GraphicsManager.Device.PresentationParameters.BackBufferHeight * .3);
            Button play = new Button(
                new Microsoft.Xna.Framework.Rectangle(
                    GraphicsManager.Device.PresentationParameters.BackBufferWidth / 2 - width / 2,
                    y + height,
                    width,
                    height
                ),
                playSprite,
                new Button.ButtonAction(StartGame)
            );

            Sprite instructionsSprite = new Sprite("instructions");
            height = (int)(GraphicsManager.Device.PresentationParameters.BackBufferHeight * .12);
            width = (int)((float)instructionsSprite.Width / (float)instructionsSprite.Height * (float)height);
            y = (int)(GraphicsManager.Device.PresentationParameters.BackBufferHeight * .5);
            Button instructions = new Button(
                new Microsoft.Xna.Framework.Rectangle(
                    GraphicsManager.Device.PresentationParameters.BackBufferWidth / 2 - width / 2,
                    y + height,
                    width,
                    height
                ),
                instructionsSprite,
                new Button.ButtonAction(Instructions)
            );

            Sprite quitSprite = new Sprite("quit");
            height = (int)(GraphicsManager.Device.PresentationParameters.BackBufferHeight * .14);
            width = (int)((float)quitSprite.Width / (float)quitSprite.Height * (float)height);
            y = (int)(GraphicsManager.Device.PresentationParameters.BackBufferHeight * .64);
            Button quit = new Button(
                new Microsoft.Xna.Framework.Rectangle(
                    GraphicsManager.Device.PresentationParameters.BackBufferWidth / 2 - width / 2,
                    y + height,
                    width,
                    height
                ),
                quitSprite,
                new Button.ButtonAction(Quit)
            );

            Add(title);
            Add(play);
            Add(instructions);
            Add(quit);
        }

        private void StartGame(Button button)
        {
            InputAction.IsMouseLocked = true;
            ChimeraGame.PopState();

            GameWorld world = new GameWorld(mDebugDrawer);
            world.AddLevelFromFile("tree", new Vector3(0, 0, 0), Quaternion.Identity, new Vector3(8.0f, 0.01f, 8.0f));
            ChimeraGame.PushState(world);

        }

        private void Instructions(Button button)
        {
            ChimeraGame.PopState();
            ChimeraGame.PushState(new StoryMenu(mOwnerGame));
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
