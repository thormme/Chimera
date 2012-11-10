﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BEPUphysics;
using Microsoft.Xna.Framework;

namespace finalProject
{
    // TODO: Add terrain
    // TODO: Add ability to add/remove objects during game.
    class World
    {
        List<IGameObject> mGameObjects;
        List<Actor> mActors;

        public Space mSpace;

        public World()
        {
            mGameObjects = new List<IGameObject>();
            mActors = new List<Actor>();

            mSpace = new Space();
            mSpace.ForceUpdater.Gravity = new Vector3(0, -9.81f, 0);
        }

        public void Update(GameTime gameTime)
        {
            mSpace.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

            foreach (Actor actor in mActors)
            {
                actor.Update(gameTime);
            }
        }

        public void Render()
        {
            foreach (IGameObject gameObject in mGameObjects)
            {
                gameObject.Render();
            }
        }
    }
}
