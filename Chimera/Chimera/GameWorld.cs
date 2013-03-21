using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameConstructLibrary;
using BEPUphysicsDrawer.Models;
using Microsoft.Xna.Framework;
using BEPUphysics;
using BEPUphysics.BroadPhaseEntries;
using BEPUphysics.Entities;
using BEPUphysics.Entities.Prefabs;
using BEPUphysics.CollisionRuleManagement;

namespace Chimera
{
    public class GameWorld : World
    {
        public PlayerCreature Player = null;
        public GoalPoint Goal = null;

        public GameWorld(ModelDrawer debugModelDrawer) :
            base(debugModelDrawer)
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            Space.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

            foreach (IActor actor in mActors)
            {
                if (ChimeraGame.Camera != null)
                {
                    // Don't update actors which are far from the camera.
                    if (actor is IGameObject)
                    {
                        if (actor is SkyDome && Player != null)
                        {
                            (actor as SkyDome).Position = Player.Position;
                        }

                        if (actor is PlayerCreature)
                        {
                            actor.Update(gameTime);
                        }
                        else if (((actor as IGameObject).Position - ChimeraGame.Camera.Position).Length() < 400)
                        {
                            actor.Update(gameTime);
                        }
                    }
                    else
                    {
                        actor.Update(gameTime);
                    }
                }
            }

            CommitChanges();
        }

        public override void Render()
        {
            //if (mCameraEntity != null)
            //{
            if (ChimeraGame.Camera != null)
            {
                foreach (IGameObject gameObject in mGameObjects)
                {
                    // Don't render Creatures that are far away.
                    if (gameObject is Creature)
                    {
                        if ((gameObject.Position - ChimeraGame.Camera.Position).Length() < 200.0f)
                        {
                            if (Vector3.Dot(ChimeraGame.Camera.Forward, gameObject.Position - ChimeraGame.Camera.Position) >= 0)
                            {
                                gameObject.Render();
                            }
                        }
                    }
                    else
                    {
                        gameObject.Render();
                    }

                }
            }
        }

        protected override TerrainPhysics LoadTerrain(string mapName, Vector3 position, Quaternion orientation, Vector3 scale)
        {
            TerrainPhysics tp = base.LoadTerrain(mapName, position, orientation, scale);

            Vector3 min = tp.StaticCollidable.BoundingBox.Min;
            Vector3 max = tp.StaticCollidable.BoundingBox.Max;
            min.Y -= 200f;
            max.Y += 200f;

            List<Box> bounds = new List<Box>();
            Add(new InvisibleWall(new Vector3(min.X - 10, 0, 0), 20, max.Y - min.Y + 40, max.Z - min.Z + 40));
            Add(new DeathZone(new Vector3(0, min.Y - 10, 0), max.X - min.X + 40, 20, max.Z - min.Z + 40));
            Add(new InvisibleWall(new Vector3(0, 0, min.Z - 10), max.X - min.X + 40, max.Y - min.Y + 40, 20));
            Add(new InvisibleWall(new Vector3(max.X + 10, 0, 0), 20, max.Y - min.Y + 40, max.Z - min.Z + 40));
            Add(new InvisibleWall(new Vector3(0, max.Y + 10, 0), max.X - min.X + 40, 20, max.Z - min.Z + 40));
            Add(new InvisibleWall(new Vector3(0, 0, max.Z + 10), max.X - min.X + 40, max.Y - min.Y + 40, 20));

            return tp;
        }

        protected override void CheckSpecialObject(object obj)
        {
            if (obj is PlayerCreature)
            {
                Player = (obj as PlayerCreature);
                ChimeraGame.Player = (obj as PlayerCreature);
                /*
                mCameraEntity = new Sphere(Game1.Camera.GetPosition(), 1.0f);
                mCameraEntity.CollisionInformation.CollisionRules.Personal = CollisionRule.NoSolver;
                mCameraEntity.Tag = mCameraEntity;
                Space.Add(mCameraEntity);*/
                 
            }
            else if (obj is GoalPoint)
            {
                Goal = (obj as GoalPoint);
            }
        }
    }
}
