using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BEPUphysics;
using Microsoft.Xna.Framework;
using GameConstructLibrary;
using GraphicsLibrary;

namespace finalProject
{
    /// <summary>
    /// Manages the game world. Contains all loaded GameObjects and level chunks.
    /// </summary>
    public class World
    {
        List<IGameObject> mGameObjects;
        List<Actor> mActors;
        List<TerrainPhysics> mTerrain;

        List<IGameObject> mUncommittedGameObjectAdditions;
        List<IGameObject> mUncommittedGameObjectRemovals;
        List<TerrainPhysics> mUncommittedTerrainAdditions;
        List<TerrainPhysics> mUncommittedTerrainRemovals;

        public Space mSpace;

        public World()
        {
            mGameObjects = new List<IGameObject>();
            mActors = new List<Actor>();
            mTerrain = new List<TerrainPhysics>();
            mSpace = new Space();

            mUncommittedGameObjectAdditions = new List<IGameObject>();
            mUncommittedGameObjectRemovals = new List<IGameObject>();
            mUncommittedTerrainAdditions = new List<TerrainPhysics>();
            mUncommittedTerrainRemovals = new List<TerrainPhysics>();

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
                if (gameObject is PhysicsObject)
                {
                    mSpace.Add(gameObject as PhysicsObject);
                }
            }

            foreach (IGameObject gameObject in mUncommittedGameObjectRemovals)
            {
                mGameObjects.Remove(gameObject);
                if (gameObject is Actor)
                {
                    mActors.Remove(gameObject as Actor);
                }
                if (gameObject is PhysicsObject)
                {
                    mSpace.Remove(gameObject as PhysicsObject);
                }
            }

            foreach (TerrainPhysics terrain in mUncommittedTerrainRemovals)
            {
                mTerrain.Add(terrain);
                mSpace.Add(terrain);
            }

            foreach (TerrainPhysics terrain in mUncommittedTerrainRemovals)
            {
                mTerrain.Remove(terrain);
                mSpace.Remove(terrain);
            }

            mUncommittedGameObjectAdditions.Clear();
            mUncommittedGameObjectRemovals.Clear();
            mUncommittedTerrainAdditions.Clear();
            mUncommittedTerrainRemovals.Clear();
        }

        public void Add(IGameObject gameObject)
        {
            mUncommittedGameObjectAdditions.Add(gameObject);
        }

        public void Remove(IGameObject gameObject)
        {
            mUncommittedGameObjectRemovals.Add(gameObject);
        }

        public void Add(TerrainPhysics terrain)
        {
            mUncommittedTerrainAdditions.Add(terrain);
        }

        public void Remove(TerrainPhysics terrain)
        {
            mUncommittedTerrainRemovals.Add(terrain);
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
            foreach (TerrainPhysics terrain in mTerrain)
            {
                terrain.Render();
            }
            foreach (IGameObject gameObject in mGameObjects)
            {
                gameObject.Render();
            }
        }
    }
}
