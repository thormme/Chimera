using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameConstructLibrary;
using Microsoft.Xna.Framework;

namespace finalProject.Menu
{
    public class MenuState : GameState
    {
        public List<IMenuElement> MenuElements = new List<IMenuElement>();
        private List<IMenuElement> mUncommittedMenuElementAdditions = new List<IMenuElement>();
        private List<IMenuElement> mUncommittedMenuElementRemovals = new List<IMenuElement>();

        public MenuState()
        {

        }

        public override void Update(GameTime gameTime)
        {
            foreach (IMenuElement menuElement in MenuElements)
            {
                menuElement.Update(gameTime);
            }
        }

        public override void Render()
        {
            foreach (IMenuElement menuElement in MenuElements)
            {
                menuElement.Render();
            }
        }

        private void CommitMenuElementChanges()
        {
            foreach (IMenuElement menuElement in mUncommittedMenuElementAdditions)
            {
                MenuElements.Add(menuElement);
            }
            foreach (IMenuElement menuElement in mUncommittedMenuElementRemovals)
            {
                MenuElements.Remove(menuElement);
            }
        }
    }
}
