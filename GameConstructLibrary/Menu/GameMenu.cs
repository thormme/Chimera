using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameConstructLibrary.Menu
{
    public class GameMenu : IGameState
    {
        List<IMenuItem> mMenuItems = new List<IMenuItem>();

        List<IMenuItem> mUncommittedMenuItemAdditions = new List<IMenuItem>();
        List<IMenuItem> mUncommittedMenuItemRemovals = new List<IMenuItem>();

        public void Update(GameTime gameTime)
        {
            foreach (IMenuItem item in mMenuItems)
            {
                item.Update(gameTime);
            }

            CommitChanges();
        }

        public void Render()
        {
            foreach (IMenuItem item in mMenuItems)
            {
                item.Render();
            }
        }

        private void CommitChanges()
        {
            foreach (IMenuItem item in mUncommittedMenuItemAdditions)
            {
                item.Menu = this;
                mMenuItems.Add(item);
            }

            foreach (IMenuItem item in mUncommittedMenuItemRemovals)
            {
                item.Menu = null;
                mMenuItems.Remove(item);
            }

            mUncommittedMenuItemAdditions.Clear();
            mUncommittedMenuItemRemovals.Clear();
        }

        public void Add(IMenuItem item)
        {
            if (item == null)
            {
                throw new Exception("Null added to menu.");
            }
            if (!mUncommittedMenuItemAdditions.Contains(item))
            {
                mUncommittedMenuItemAdditions.Add(item);
            }
        }

        public void Remove(IMenuItem item)
        {
            if (item == null)
            {
                throw new Exception("Null removed from menu.");
            }
            if (!mUncommittedMenuItemRemovals.Contains(item))
            {
                mUncommittedMenuItemRemovals.Add(item);
            }
        }
    }
}
