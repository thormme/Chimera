using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameConstructLibrary.Menu
{
    public interface IMenuItem
    {
        GameMenu Menu { get; set; }

        void Update(GameTime gameTime);
        void Render();
    }
}
