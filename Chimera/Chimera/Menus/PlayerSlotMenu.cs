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

namespace Chimera.Menus
{
    class PlayerSlotMenu : GameMenu
    {

        Game mOwnerGame;
        private PlayerCreature mPlayer;

        private GraphicItem[] mSlots = new GraphicItem[3];

        public PlayerSlotMenu(Game game, PlayerCreature creature)
        {
            InputAction.IsMouseLocked = false;

            mOwnerGame = game;
            mPlayer = creature;

            Sprite backgroundSprite = new Sprite("blue");
            int width = (int)(GraphicsManager.Device.PresentationParameters.BackBufferWidth);
            int height = (int)(GraphicsManager.Device.PresentationParameters.BackBufferHeight);
            int x = 0;
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
            Add(background);

            x = 0;
            y = (int)(GraphicsManager.Device.PresentationParameters.BackBufferHeight / 2);

            width = (int)(GraphicsManager.Device.PresentationParameters.BackBufferWidth) / 8;
            height = (int)(GraphicsManager.Device.PresentationParameters.BackBufferWidth) / 8;

            x += (int)(GraphicsManager.Device.PresentationParameters.BackBufferWidth) / 6;
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

            Sprite slot0Sprite = mPlayer.PartAttachments[0].Part.Icon;
            mSlots[0] = new GraphicItem(
                new Microsoft.Xna.Framework.Rectangle(
                    x,
                    y,
                    width,
                    height
                ),
                slot0Sprite
            );
            Add(mSlots[0]);

            x += (int)(GraphicsManager.Device.PresentationParameters.BackBufferWidth) / 6;
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

            Sprite slot1Sprite = mPlayer.PartAttachments[1].Part.Icon;
            mSlots[1] = new GraphicItem(
                new Microsoft.Xna.Framework.Rectangle(
                    x,
                    y,
                    width,
                    height
                ),
                slot1Sprite
            );
            Add(mSlots[1]);

            x += (int)(GraphicsManager.Device.PresentationParameters.BackBufferWidth) / 6;
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

            Sprite slot2Sprite = mPlayer.PartAttachments[2].Part.Icon;
            mSlots[2] = new GraphicItem(
                new Microsoft.Xna.Framework.Rectangle(
                    x,
                    y,
                    width,
                    height
                ),
                slot2Sprite
            );
            Add(mSlots[2]);

            Sprite doneSprite = new Sprite("check");
            x += (int)(GraphicsManager.Device.PresentationParameters.BackBufferWidth) / 6;
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
            ChimeraGame.PushState(new PlayerPartsMenu(mOwnerGame, mPlayer, slot, mSlots[slot]));
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
