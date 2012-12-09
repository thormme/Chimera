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
            if (Mouse.GetState().X > Bounds.X && Mouse.GetState().X < Bounds.X + Bounds.Width &&
                Mouse.GetState().Y > Bounds.Y && Mouse.GetState().Y < Bounds.X + Bounds.Height &&
                Mouse.GetState().LeftButton == ButtonState.Pressed)
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
            mSprite.Render(Bounds);
        }

        public override void OnSelect()
        {
        }

        public override void OnDeselect()
        {
        }
    }
}
