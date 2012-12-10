using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameConstructLibrary
{
    public interface IGameState
    {
        void Update(GameTime gameTime);
        void Render();
    }
}
