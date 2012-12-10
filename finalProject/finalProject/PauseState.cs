using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameConstructLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using GameConstructLibrary.Menu;
using GraphicsLibrary;

namespace finalProject
{
    public class PauseState : GameMenu
    {
        private Game mOwnerGame;

        public PauseState(Game ownerGame)
        {
            mOwnerGame = ownerGame;

            int width = Game1.Graphics.PreferredBackBufferWidth;
            int height = Game1.Graphics.PreferredBackBufferHeight;
            int buttonHeight = (int)(0.1f * height);
            int buttonWidth = (int)(2 * buttonHeight);

            Add(
                new Button(
                    new Rectangle(width / 2 - buttonWidth / 2, height / 2 - buttonHeight, buttonWidth, buttonHeight),
                    new Sprite("blueButton"),
                    new Button.ButtonAction(Resume)
                )
            );
            Add(
                new Button(
                    new Rectangle(width / 2 - buttonWidth / 2, height / 2 + buttonHeight, buttonWidth, buttonHeight),
                    new Sprite("redButton"),
                    new Button.ButtonAction(Quit)
                )
            );
        }

        private void Resume(Button button)
        {
            Game1.PopState();
        }

        private void Quit(Button button)
        {
            mOwnerGame.Exit();
        }
    }
}
