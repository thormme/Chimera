using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BEPUphysics;
using Microsoft.Xna.Framework;
using GameConstructLibrary;
using GraphicsLibrary;
using BEPUphysicsDrawer.Models;

namespace finalProject
{
    /// <summary>
    /// Manages the game world. Contains all loaded GameObjects and level chunks.
    /// </summary>
    public class World
    {
        List<IGameObject> mGameObjects;
        List<Actor> mActors;

        List<IGameObject> mUncommittedGameObjectAdditions;
        List<IGameObject> mUncommittedGameObjectRemovals;

        public Space mSpace;

        public World()
        {
            mGameObjects = new List<IGameObject>();
            mActors = new List<Actor>();
            mSpace = new Space();

            mUncommittedGameObjectAdditions = new List<IGameObject>();
            mUncommittedGameObjectRemovals = new List<IGameObject>();

            mSpace.ForceUpdater.Gravity = new Vector3(0, -9.81f, 0);
        }

        private void CommitChanges()
        {
            foreach (IGameObject gameObject in mUncommittedGameObjectAdditions)
            {
                mGameObjects.Add(gameObject);
                if (gameObject is Actor)
                {
                    mActors.Add(gameObject as Actor);
                }
                if (gameObject is IEntityOwner)
                {
                    mSpace.Add((gameObject as IEntityOwner).Entity);
                    Game1.DebugModelDrawer.Add((gameObject as IEntityOwner).Entity);
                }
                else if (gameObject is IStaticCollidableOwner)
                {
                    mSpace.Add((gameObject as IStaticCollidableOwner).StaticCollidable);
                    Game1.DebugModelDrawer.Add((gameObject as IStaticCollidableOwner).StaticCollidable);
                }
            }

            foreach (IGameObject gameObject in mUncommittedGameObjectRemovals)
            {
                mGameObjects.Remove(gameObject);
                if (gameObject is Actor)
                {
                    mActors.Remove(gameObject as Actor);
                }
                if (gameObject is IEntityOwner)
                {
                    mSpace.Add((gameObject as IEntityOwner).Entity);
                    Game1.DebugModelDrawer.Add((gameObject as IEntityOwner).Entity);
                }
                else if (gameObject is IStaticCollidableOwner)
                {
                    mSpace.Add((gameObject as IStaticCollidableOwner).StaticCollidable);
                    Game1.DebugModelDrawer.Add((gameObject as IStaticCollidableOwner).StaticCollidable);
                }
            }

            mUncommittedGameObjectAdditions.Clear();
            mUncommittedGameObjectRemovals.Clear();
        }

        public void Add(IGameObject gameObject)
        {
            mUncommittedGameObjectAdditions.Add(gameObject);
        }

        public void Remove(IGameObject gameObject)
        {
            mUncommittedGameObjectRemovals.Add(gameObject);
        }

        public void Update(GameTime gameTime)
        {
            mSpace.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

            foreach (Actor actor in mActors)
            {
                actor.Update(gameTime);
            }

            CommitChanges();
        }

        public void Render()
        {
            foreach (IGameObject gameObject in mGameObjects)
            {
                gameObject.Render();
            }
        }

        public void AddLevelFromFile(String mapName)
        {
            //LevelMan
        }
    }
}
