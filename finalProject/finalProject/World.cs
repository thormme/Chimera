using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BEPUphysics;
using Microsoft.Xna.Framework;
using GameConstructLibrary;
using GraphicsLibrary;
using BEPUphysicsDrawer.Models;
using System.Reflection;

namespace finalProject
{
    /// <summary>
    /// Manages the game world. Contains all loaded GameObjects and level chunks.
    /// </summary>
    public class World
    {
        List<IGameObject> mGameObjects;
        List<IActor> mActors;

        List<IGameObject> mUncommittedGameObjectAdditions;
        List<IGameObject> mUncommittedGameObjectRemovals;

        public Space mSpace;

        public World()
        {
            mGameObjects = new List<IGameObject>();
            mActors = new List<IActor>();
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
                if (gameObject is IActor)
                {
                    mActors.Add(gameObject as IActor);
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
                if (gameObject is IActor)
                {
                    mActors.Remove(gameObject as IActor);
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

            foreach (IActor actor in mActors)
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

        public void AddLevelFromFile(String mapName, Vector3 position, Quaternion orientation, Vector3 scale)
        {
            List<DummyObject> objects = LevelManager.Load(mapName);
            foreach (DummyObject dummy in objects)
            {
                if (dummy.Type == "Root")
                {
                    continue;
                }
                Type type = Type.GetType(dummy.Type);

                if (type != null)
                {
                    Quaternion objectOrientation = Quaternion.CreateFromRotationMatrix(Matrix.CreateLookAt(new Vector3(), dummy.Orientation, new Vector3(0, 1, 0)));
                    object[] parameters = new object[5];
                    parameters[0] = dummy.Model;
                    parameters[1] = (dummy.Position * scale) + position;
                    parameters[2] = objectOrientation * orientation;
                    parameters[3] = dummy.Scale * scale;
                    parameters[4] = dummy.Parameters;
                    object obj = Activator.CreateInstance(type, parameters);

                    if (obj is IGameObject)
                    {
                        Add(obj as IGameObject);
                    }
                }
            }

            Add(new TerrainPhysics(mapName, position, orientation, scale));
        }
    }
}
