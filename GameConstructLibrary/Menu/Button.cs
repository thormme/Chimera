using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using GraphicsLibrary;

namespace GameConstructLibrary.Menu
{
    public class Button : SelectableItem
    {
        public delegate void ButtonAction(Button button);

        protected static InputAction mUseButton = new CombinedInputAction(
            new InputAction[]
            {
                new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Pressed, Microsoft.Xna.Framework.Input.Keys.Enter),
                new GamePadButtonInputAction(PlayerIndex.One, InputAction.ButtonAction.Pressed, Microsoft.Xna.Framework.Input.Buttons.A)
            },
            InputAction.ButtonAction.Down
        );

        protected static InputAction mUseMouse = new CombinedInputAction(
            new InputAction[]
            {
                new MouseButtonInputAction(PlayerIndex.One, InputAction.ButtonAction.Released, InputAction.MouseButton.Left)
            },
            InputAction.ButtonAction.Down
        );

        public Rectangle Bounds;
        Sprite mSprite;
        ButtonAction mAction;

        public Button(Rectangle bounds, Sprite sprite, ButtonAction action)
        {
            Bounds = bounds;
            mSprite = sprite;
            mAction = action;
        }

        public override void Update(GameTime gameTime)
        {
            // Check that mouse is within button.
            if ((Mouse.GetState().X > Bounds.X && Mouse.GetState().X < Bounds.X + Bounds.Width &&
                Mouse.GetState().Y > Bounds.Y && Mouse.GetState().Y < Bounds.X + Bounds.Height &&
                mUseMouse.Active) ||
                (mUseButton.Active && Selected))
            {
                Use();
            }
        }

        public void Use()
        {
            mAction(this);
        }

        public override void Render()
        {
            if (!Selected)
            {
                mSprite.Render(Bounds);
            }
            else
            {
                mSprite.Render(Bounds, Color.Yellow, 1.0f);
            }
        }

        public override void OnSelect()
        {
        }

        public override void OnDeselect()
        {
        }
    }
}
