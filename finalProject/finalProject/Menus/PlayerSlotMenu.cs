using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameConstructLibrary.Menu;
using Microsoft.Xna.Framework;
using BEPUphysicsDrawer.Models;
using GraphicsLibrary;
using GameConstructLibrary;
using System.Reflection;

namespace finalProject.Menus
{
    class PlayerSlotMenu : GameMenu
    {

        Game mOwnerGame;
        private PlayerCreature mPlayer;

        public PlayerSlotMenu(Game game, PlayerCreature creature)
        {
            InputAction.IsMouseLocked = false;

            mOwnerGame = game;
            mPlayer = creature;

            Sprite backgroundSprite = new Sprite("blue");
            int width = (int)(ChimeraGame.Graphics.PreferredBackBufferWidth);
            int height = (int)(ChimeraGame.Graphics.PreferredBackBufferHeight);
            int x = 0;
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

            Add(background);

            x = 0;
            y = (int)(ChimeraGame.Graphics.PreferredBackBufferHeight / 2);

            width = (int)(ChimeraGame.Graphics.PreferredBackBufferWidth) / 8;
            height = (int)(ChimeraGame.Graphics.PreferredBackBufferWidth) / 8;

            x += (int)(ChimeraGame.Graphics.PreferredBackBufferWidth) / 6;
            Sprite redSprite = new Sprite("redButton");
            Button redButton = new Button(
            new Microsoft.Xna.Framework.Rectangle(
                    x,
                    y,
                    width,
                    height
                ),
                redSprite,
                new Button.ButtonAction((Button b) => CreatePart(0, b))
            );
            Add(redButton);

            x += (int)(ChimeraGame.Graphics.PreferredBackBufferWidth) / 6;
            Sprite blueSprite = new Sprite("blueButton");
            Button blueButton = new Button(
            new Microsoft.Xna.Framework.Rectangle(
                    x,
                    y,
                    width,
                    height
                ),
                blueSprite,
                new Button.ButtonAction((Button b) => CreatePart(1, b))
            );
            Add(blueButton);

            x += (int)(ChimeraGame.Graphics.PreferredBackBufferWidth) / 6;
            Sprite yellowSprite = new Sprite("yellowButton");
            Button yellowButton = new Button(
            new Microsoft.Xna.Framework.Rectangle(
                    x,
                    y,
                    width,
                    height
                ),
                yellowSprite,
                new Button.ButtonAction((Button b) => CreatePart(2, b))
            );
            Add(yellowButton);

            Sprite doneSprite = new Sprite("check");
            x += (int)(ChimeraGame.Graphics.PreferredBackBufferWidth) / 6;
            Button exitButton = new Button(
            new Microsoft.Xna.Framework.Rectangle(
                    x,
                    y,
                    width,
                    height
                ),
                doneSprite,
                new Button.ButtonAction(Done)
            );
            Add(exitButton);

        }

        private void CreatePart(int slot, Button button)
        {
            ChimeraGame.PushState(new PlayerPartsMenu(mOwnerGame, mPlayer, slot));
        }

        private void Done(Button button)
        {
            InputAction.IsMouseLocked = true;
            ChimeraGame.PopState();
        }

        public override void Render()
        {
            base.Render();
        }
    }
}
