using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BEPUphysics;
using Microsoft.Xna.Framework;
using GraphicsLibrary;
using BEPUphysicsDrawer.Models;
using System.Reflection;
using BEPUphysics.Collidables;
using Microsoft.Xna.Framework.Input;

namespace GameConstructLibrary
{
    /// <summary>
    /// Manages the game world. Contains all loaded GameObjects and level chunks.
    /// </summary>
    public class World : IGameState
    {
        protected List<IGameObject> mGameObjects;
        protected List<IActor> mActors;

        protected List<IGameObject> mUncommittedGameObjectAdditions;
        protected List<IGameObject> mUncommittedGameObjectRemovals;

        private ModelDrawer mDebugModelDrawer;

        public Space Space;

        public World(ModelDrawer debugModelDrawer)
        {
            mDebugModelDrawer = debugModelDrawer;
            mGameObjects = new List<IGameObject>();
            mActors = new List<IActor>();
            Space = new Space();

            mUncommittedGameObjectAdditions = new List<IGameObject>();
            mUncommittedGameObjectRemovals = new List<IGameObject>();

            Space.ForceUpdater.Gravity = new Vector3(0, -9.81f, 0);
        }

        protected void CommitChanges()
        {
            for (int i = 0; i < mUncommittedGameObjectAdditions.Count; i++)
            {
                IGameObject gameObject = mUncommittedGameObjectAdditions[i];
                mGameObjects.Add(gameObject);
                if (gameObject is IActor)
                {
                    mActors.Add(gameObject as IActor);
                }
                if (gameObject is IEntityOwner)
                {
                    Space.Add((gameObject as IEntityOwner).Entity);
                    mDebugModelDrawer.Add((gameObject as IEntityOwner).Entity);
                }
                if (gameObject is IStaticCollidableOwner)
                {
                    Space.Add((gameObject as IStaticCollidableOwner).StaticCollidable);
                    mDebugModelDrawer.Add((gameObject as IStaticCollidableOwner).StaticCollidable);
                }
                gameObject.World = this;
            }

            for (int i = 0; i < mUncommittedGameObjectRemovals.Count; i++)
            {
                IGameObject gameObject = mUncommittedGameObjectRemovals[i];
                mGameObjects.Remove(gameObject);
                if (gameObject is IActor)
                {
                    mActors.Remove(gameObject as IActor);
                }
                if (gameObject is IEntityOwner)
                {
                    Space.Remove((gameObject as IEntityOwner).Entity);
                    mDebugModelDrawer.Remove((gameObject as IEntityOwner).Entity);
                }
                else if (gameObject is IStaticCollidableOwner)
                {
                    Space.Remove((gameObject as IStaticCollidableOwner).StaticCollidable);
                    mDebugModelDrawer.Remove((gameObject as IStaticCollidableOwner).StaticCollidable);
                }
                gameObject.World = null;
            }

            mUncommittedGameObjectAdditions.Clear();
            mUncommittedGameObjectRemovals.Clear();
        }

        public void Add(IGameObject gameObject)
        {
            if (gameObject == null)
            {
                throw new Exception("Null added to World.");
            }
            if (!mUncommittedGameObjectAdditions.Contains(gameObject))
            {
                mUncommittedGameObjectAdditions.Add(gameObject);
            }
        }

        public void Remove(IGameObject gameObject)
        {
            if (gameObject == null)
            {
                throw new Exception("Null removed from World.");
            }
            if (!mUncommittedGameObjectRemovals.Contains(gameObject))
            {
                mUncommittedGameObjectRemovals.Add(gameObject);
            }
        }

        public virtual void Update(GameTime gameTime)
        {
            //try
            {
                Space.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            }
            //catch (SystemException)
            {
                // Don't update
            }

            foreach (IActor actor in mActors)
            {
                actor.Update(gameTime);
            }

            CommitChanges();
        }

        /// <summary>
        /// Renders contents of world.  MUST be called between BeginRendering and FinishRendering.
        /// </summary>
        public virtual void Render()
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
                    Quaternion objectOrientation = Quaternion.CreateFromYawPitchRoll(dummy.Orientation.X, dummy.Orientation.Y, dummy.Orientation.Z);
                    object[] parameters = new object[5];
                    parameters[0] = dummy.Model;
                    parameters[1] = Vector3.Multiply(dummy.Position, scale) + position;
                    parameters[2] = Quaternion.Concatenate(orientation, objectOrientation);
                    parameters[3] = Vector3.Multiply(dummy.Scale, scale);
                    parameters[4] = dummy.Parameters;
                    object obj = Activator.CreateInstance(type, parameters);

                    if (obj is IGameObject)
                    {
                        Add(obj as IGameObject);
                    }

                    CheckSpecialObject(obj);

                }
            }

            TerrainPhysics tf = new TerrainPhysics(mapName, position, orientation, scale);
            (tf.StaticCollidable as Terrain).Thickness = 5.0f;
            Add(tf);

            Vector3[] corners = tf.StaticCollidable.BoundingBox.GetCorners();
        }

        protected virtual void CheckSpecialObject(object obj)
        {

        }

        public void Clear()
        {
            foreach (IGameObject gameObject in mGameObjects)
            {
                Remove(gameObject);
            }
        }
    }
}
