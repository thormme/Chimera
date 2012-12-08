using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GraphicsLibrary
{
    /// <summary>
    /// Texture that is drawn in screen space.
    /// </summary>
    public class Sprite
    {
        private string mName;

        /// <summary>
        /// Instantiate new sprite with name of texture in content/textures/.
        /// </summary>
        public Sprite(string spriteName)
        {
            mName = spriteName;
        }

        /// <summary>
        /// Draw sprite to screen at screenposition with overlay color.
        /// </summary>
        /// <param name="screenPosition">Pixel position of top left of sprite.</param>
        /// <param name="width">Number of pixels wide to draw the sprite.</param>
        /// <param name="height">Number of pixels tall to draw the sprite.</param>
        public void Render(Vector2 screenPosition, int width, int height)
        {
            Render(new Rectangle((int)screenPosition.X, (int)screenPosition.Y, width, height), new Color(1.0f, 1.0f, 1.0f, 1.0f), 0.0f);
        }

        /// <summary>
        /// Draw sprite to screen at screenposition.
        /// </summary>
        /// <param name="screenPosition">Pixel position of top left of sprite.</param>
        /// <param name="width">Number of pixels wide to draw the sprite.</param>
        /// <param name="height">Number of pixels tall to draw the sprite.</param>
        /// <param name="blendColor">Color to overlay on top of sprite.</param>
        /// <param name="blendColorWeight">Percentage of sprite color to come from blendColor</param>
        public void Render(Vector2 screenPosition, int width, int height, Color blendColor, float blendColorWeight)
        {
            Render(new Rectangle((int)screenPosition.X, (int)screenPosition.Y, width, height), blendColor, blendColorWeight);
        }

        /// <summary>
        /// Draw sprite to screen occupying screenSpace.
        /// </summary>
        /// <param name="screenSpace">Portion of screen occupied by sprite.</param>
        public void Render(Rectangle screenSpace)
        {
            Render(screenSpace, new Color(1.0f, 1.0f, 1.0f, 1.0f), 0.0f);
        }

        /// <summary>
        /// Draw sprite to screen occupying screenSpace with overlay color.
        /// </summary>
        /// <param name="screenSpace">Portion of screen occupied by sprite.</param>
        /// <param name="blendColor">Color to overlay on top of sprite.</param>
        /// <param name="blendColorWeight">Percentage of sprite color to come from blendColor</param>
        public void Render(Rectangle screenSpace, Color blendColor, float blendColorWeight)
        {
            GraphicsManager.RenderSprite(mName, screenSpace, blendColor, blendColorWeight);
        }
    }
}
