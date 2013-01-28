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
    class PlayerPartsMenu : GameMenu
    {

        Game mOwnerGame;
        private PlayerCreature mPlayer;
        private int mSlot;

        public PlayerPartsMenu(Game game, PlayerCreature creature, int slot)
        {
            mOwnerGame = game;
            mPlayer = creature;
            mSlot = slot;

            Sprite backgroundSprite = new Sprite("red");
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


            y = (int)(ChimeraGame.Graphics.PreferredBackBufferHeight / 2);
            foreach (Type partType in creature.Info.Parts)
            {
                Type part = partType;
                string[] fullName = partType.ToString().Split('.');
                string name = fullName[fullName.Length - 1];
                Sprite sprite = new Sprite(name + "Icon");
                width = (int)(ChimeraGame.Graphics.PreferredBackBufferWidth) / 8;
                height = (int)(ChimeraGame.Graphics.PreferredBackBufferWidth / 8);
                x += (int)(ChimeraGame.Graphics.PreferredBackBufferWidth) / 8;
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

            mPlayer.AddPart(Activator.CreateInstance(part) as Part, mSlot);
            
            ChimeraGame.PopState();

        }

        public override void Render()
        {
            base.Render();
        }
    }
}
