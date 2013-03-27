using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameConstructLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using GameConstructLibrary.Menu;
using GraphicsLibrary;

namespace Chimera.Menus
{
    public class PauseState : GameMenu
    {
        private Game mOwnerGame;

        public PauseState(Game ownerGame)
        {
            mOwnerGame = ownerGame;
            InputAction.IsMouseLocked = false;
            /*
            int width = Game1.Graphics.PreferredBackBufferWidth;
            int height = Game1.Graphics.PreferredBackBufferHeight;
            int buttonHeight = (int)(0.1f * height);
            int buttonWidth = (int)(2 * buttonHeight);
            */

            Sprite pausedSprite = new Sprite("paused");
            int height = (int)(GraphicsManager.Device.PresentationParameters.BackBufferHeight * .25);
            int width = (int)((float)pausedSprite.Width / (float)pausedSprite.Height * (float)height);
            int y = (int)(GraphicsManager.Device.PresentationParameters.BackBufferHeight * .1);
            GraphicItem paused = new GraphicItem(
                new Microsoft.Xna.Framework.Rectangle(
                    GraphicsManager.Device.PresentationParameters.BackBufferWidth / 2 - width / 2,
                    y,
                    width,
                    height
                ),
                pausedSprite
            );

            Sprite resumeSprite = new Sprite("resume");
            height = (int)(GraphicsManager.Device.PresentationParameters.BackBufferHeight * .12);
            width = (int)((float)resumeSprite.Width / (float)resumeSprite.Height * (float)height);
            y = (int)(GraphicsManager.Device.PresentationParameters.BackBufferHeight * .32);
            Button resume = new Button(
                new Microsoft.Xna.Framework.Rectangle(
                    GraphicsManager.Device.PresentationParameters.BackBufferWidth / 2 - width / 2,
                    y + height,
                    width,
                    height
                ),
                resumeSprite,
                new Button.ButtonAction(Resume)
            );

            Sprite checkpointSprite = new Sprite("checkpoint");
            height = (int)(GraphicsManager.Device.PresentationParameters.BackBufferHeight * .15);
            width = (int)((float)checkpointSprite.Width / (float)checkpointSprite.Height * (float)height);
            y = (int)(GraphicsManager.Device.PresentationParameters.BackBufferHeight * .47);
            Button checkpoint = new Button(
                new Microsoft.Xna.Framework.Rectangle(
                    GraphicsManager.Device.PresentationParameters.BackBufferWidth / 2 - width / 2,
                    y + height,
                    width,
                    height
                ),
                checkpointSprite,
                new Button.ButtonAction(Checkpoint)
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
                new Button.ButtonAction(Menu)
            );

            Add(paused);
            Add(resume);
            Add(checkpoint);
            Add(quit);

            /*
            Add(
                new Button(
                    new Rectangle(width / 2 - buttonWidth / 2, height / 2 - buttonHeight, buttonWidth, buttonHeight),
                    new Sprite("blueButton"),
                    new Button.ButtonAction(Resume)
                )
            );
            Add(
                new Button(
                    new Rectangle(width / 2 - buttonWidth / 2, height / 2, buttonWidth, buttonHeight),
                    new Sprite("yellowButton"),
                    new Button.ButtonAction(Menu)
                )
            );
            Add(
                new Button(
                    new Rectangle(width / 2 - buttonWidth / 2, height / 2 + buttonHeight, buttonWidth, buttonHeight),
                    new Sprite("redButton"),
                    new Button.ButtonAction(Quit)
                )
            );
             */
        }

        private void Resume(Button button)
        {
            InputAction.IsMouseLocked = true;
            ChimeraGame.PopState();
        }

        private void Checkpoint(Button button)
        {
            InputAction.IsMouseLocked = true;
            ChimeraGame.Player.Die();
            ChimeraGame.PopState();
        }

        private void Menu(Button button)
        {
            (mOwnerGame as ChimeraGame).ExitToMenu();
        }

        public override void Update(GameTime gameTime)
        {
            
            base.Update(gameTime);
        }
    }
}
