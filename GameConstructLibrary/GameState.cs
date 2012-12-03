using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameConstructLibrary
{
    public abstract class GameState
    {
        public abstract void Update(GameTime gameTime);
        public abstract void Render();
    }
}
