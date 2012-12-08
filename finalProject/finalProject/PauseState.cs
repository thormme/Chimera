﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameConstructLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace finalProject
{
    public class PauseState : IGameState
    {
        int count = 100;
        public PauseState()
        {
        }

        public void Update(GameTime gameTime)
        {
            count--;
            if (count < 0)
            {
                Game1.PopState();
            }
        }

        public void Render()
        {
            
        }
    }
}
