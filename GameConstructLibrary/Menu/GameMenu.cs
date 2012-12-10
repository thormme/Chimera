using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameConstructLibrary.Menu
{
    /// <summary>
    /// Game Menu holding GUI elements such as buttons and images.
    /// </summary>
    public class GameMenu : IGameState
    {
        List<IMenuItem> mMenuItems = new List<IMenuItem>();

        List<IMenuItem> mUncommittedMenuItemAdditions = new List<IMenuItem>();
        List<IMenuItem> mUncommittedMenuItemRemovals = new List<IMenuItem>();

        private SelectableItem mSelectedItem = null;
        public SelectableItem SelectedItem
        {
            get
            {
                return mSelectedItem;
            }
            set
            {
                if (value != mSelectedItem)
                {
                    if (mSelectedItem != null)
                    {
                        mSelectedItem.Selected = false;
                    }
                    mSelectedItem = value;
                    mSelectedItem.Selected = true;
                }
            }
        }

        protected static InputAction mSelectPrevious = new CombinedInputAction(
            new InputAction[]
            {
                new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Pressed, Microsoft.Xna.Framework.Input.Keys.Up),
                new GamePadThumbStickInputAction(PlayerIndex.One, InputAction.ButtonAction.Pressed, InputAction.GamePadThumbStick.Left, InputAction.GamePadThumbStickAxis.Y, Microsoft.Xna.Framework.Input.GamePadDeadZone.None, -.5f, 2.0f),
                new GamePadButtonInputAction(PlayerIndex.One, InputAction.ButtonAction.Pressed, Microsoft.Xna.Framework.Input.Buttons.DPadUp)
            },
            InputAction.ButtonAction.Down
        );
        protected static InputAction mSelectNext = new CombinedInputAction(
            new InputAction[]
            {
                new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Pressed, Microsoft.Xna.Framework.Input.Keys.Down),
                new GamePadThumbStickInputAction(PlayerIndex.One, InputAction.ButtonAction.Pressed, InputAction.GamePadThumbStick.Left, InputAction.GamePadThumbStickAxis.Y, Microsoft.Xna.Framework.Input.GamePadDeadZone.None, -2.0f, .5f),
                new GamePadButtonInputAction(PlayerIndex.One, InputAction.ButtonAction.Pressed, Microsoft.Xna.Framework.Input.Buttons.DPadDown)
            },
            InputAction.ButtonAction.Down
        );

        private void RunNavigation()
        {
            if (mSelectPrevious.Active)
            {
                int currentItemIndex = mMenuItems.IndexOf(SelectedItem);
                int nextItemIndex = ((currentItemIndex - 1) % mMenuItems.Count() + mMenuItems.Count()) % mMenuItems.Count();
                while (currentItemIndex != nextItemIndex && !(mMenuItems[nextItemIndex] is SelectableItem))
                {
                    nextItemIndex = ((currentItemIndex - 1) % mMenuItems.Count() + mMenuItems.Count()) % mMenuItems.Count();
                }
                if (mMenuItems[nextItemIndex] is SelectableItem)
                {
                    SelectedItem = (mMenuItems[nextItemIndex] as SelectableItem);
                }
            }
            if (mSelectNext.Active)
            {
                int currentItemIndex = mMenuItems.IndexOf(SelectedItem);
                int nextItemIndex = (currentItemIndex + 1) % mMenuItems.Count();
                while (currentItemIndex != nextItemIndex && !(mMenuItems[nextItemIndex] is SelectableItem))
                {
                    nextItemIndex = (nextItemIndex + 1) % mMenuItems.Count();
                }
                if (mMenuItems[nextItemIndex] is SelectableItem)
                {
                    SelectedItem = (mMenuItems[nextItemIndex] as SelectableItem);
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            RunNavigation();

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
                if (SelectedItem == null && item is SelectableItem)
                {
                    SelectedItem = item as SelectableItem;
                }
                item.Menu = this;
                mMenuItems.Add(item);
            }

            foreach (IMenuItem item in mUncommittedMenuItemRemovals)
            {
                // TODO: Handle resetting the selection upon removal.
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
