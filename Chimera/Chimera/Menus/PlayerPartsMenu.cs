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
    class PlayerPartsMenu : GameMenu
    {

        Game mOwnerGame;
        private PlayerCreature mPlayer;
        private int mSlot;
        private GraphicItem mMenuSlot;

        public PlayerPartsMenu(Game game, PlayerCreature creature, int slot, GraphicItem menuSlot)
        {
            mOwnerGame = game;
            mPlayer = creature;
            mSlot = slot;
            mMenuSlot = menuSlot;

            Sprite backgroundSprite = new Sprite("red");
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


            y = (int)(GraphicsManager.Device.PresentationParameters.BackBufferHeight / 2);
            foreach (Type partType in creature.Info.Parts)
            {
                Type part = partType;
                string[] fullName = partType.ToString().Split('.');
                string name = fullName[fullName.Length - 1];
                Sprite sprite = new Sprite(name + "Icon");
                width = (int)(GraphicsManager.Device.PresentationParameters.BackBufferWidth) / 8;
                height = (int)(GraphicsManager.Device.PresentationParameters.BackBufferWidth / 8);
                x += (int)(GraphicsManager.Device.PresentationParameters.BackBufferWidth) / 8;
                Button button = new Button(
                new Microsoft.Xna.Framework.Rectangle(
                        x,
                        y,
                        width,
                        height
                    ),
                    sprite,
                    new Button.ButtonAction((Button b) => CreatePart(part, b))
                );
                Add(button);

                //Console.WriteLine(name);
            }
            
        }

        private void CreatePart(Type part, Button button)
        {
            if (mPlayer.PartAttachments[mSlot] != null)
            {
                mPlayer.RemovePart(mPlayer.PartAttachments[mSlot].Part);
            }
            mPlayer.AddPart(Activator.CreateInstance(part) as Part, mSlot);

            string[] fullName = part.ToString().Split('.');
            string name = fullName[fullName.Length - 1];
            Sprite sprite = new Sprite(name + "Icon");
            mMenuSlot.Sprite = sprite;
            
            ChimeraGame.PopState();

        }

        public override void Render()
        {
            base.Render();
        }
    }
}
