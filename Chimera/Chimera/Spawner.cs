using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GameConstructLibrary;
using BEPUphysics;

namespace Chimera
{
    public class Spawner : IGameObject, IActor
    {

        public World World
        {
            get;
            set;
        }

        public Microsoft.Xna.Framework.Vector3 Position
        {
            get;
            protected set;
        }

        public Microsoft.Xna.Framework.Matrix XNAOrientationMatrix
        {
            get;
            protected set;
        }

        public Microsoft.Xna.Framework.Vector3 Scale
        {
            get;
            protected set;
        }

        private Type mCreatureType;
        private float mSpawnRadius;
        public float SpawnRadius
        {
            get
            {
                return mSpawnRadius;
            }

            protected set
            {
                mSpawnRadius = value;
            }
        }
        private float mSpawnTime;
        private float mSpawnWait;
        private List<Creature> mCreatures;
        private int mMaxSpawned;

        public Spawner(String modelName, Vector3 translation, Quaternion orientation, Vector3 scale, string[] parameters)
        {
            Position = translation;
            XNAOrientationMatrix = Matrix.Identity;
            Scale = scale;
            mSpawnRadius = scale.Y;
            mCreatureType = Type.GetType("Chimera.Creatures." + parameters[0] + ", Chimera");
            mSpawnWait = Convert.ToSingle(parameters[1]);
            mMaxSpawned = Convert.ToInt32(parameters[2]);
            Initialize();
        }

        private void Initialize()
        {
            mSpawnTime = 0.0f;
            mCreatures = new List<Creature>();
        }

        public void Update(Microsoft.Xna.Framework.GameTime time)
        {

            mSpawnTime += time.TotalGameTime.Seconds;
            CheckDead();
            CheckSpawn();
        
        }

        private void CheckDead()
        {
            List<Creature> temp = new List<Creature>();
            foreach (Creature creature in mCreatures)
            {
                if (!creature.Incapacitated)
                {
                    temp.Add(creature);
                }
            }

            if (temp.Count < mCreatures.Count)
            {
                mSpawnTime = 0.0f;
            }
            mCreatures = temp;

        }

        private void CheckSpawn()
        {

            if (mSpawnWait <= mSpawnTime && mCreatures.Count < mMaxSpawned)
            {
                mSpawnTime = 0.0f;

                float radius = ((float)Rand.rand.Next() / Int32.MaxValue) * mSpawnRadius; // Randomize a distance from center of spawner
                float dir = ((float)Rand.rand.Next() / Int32.MaxValue) * 2.0f * (float)Math.PI; // Randomize a direction between 0 and 2PI

                // See if anything inhibits spawning it in that direction
                Vector3 creaturePosition;
                
                Ray rayOut = new Ray(Position, new Vector3((float)Math.Sin(dir), 0.0f, (float)Math.Cos(dir)));
                RayCastResult resultOut;
                if (World.Space.RayCast(rayOut, radius, out resultOut)) // If it does put it at the wall then move it away
                {
                    creaturePosition = new Vector3(resultOut.HitData.Location.X - (float)Math.Sin(dir) * 0.1f * radius,
                                                   resultOut.HitData.Location.Y,
                                                   resultOut.HitData.Location.Z - (float)Math.Sin(dir) * 0.1f * radius);
                }
                else // Otherwise put it at the random radius
                {
                    creaturePosition = new Vector3(Position.X + (float)Math.Sin(dir) * radius,
                                                   Position.Y,
                                                   Position.Z + (float)Math.Cos(dir) * radius);
                }

                // Find the height the creature will spawn at
                Ray rayDown = new Ray(new Vector3(creaturePosition.X, 
                                                  creaturePosition.Y + mSpawnRadius, 
                                                  creaturePosition.Z), 
                                      new Vector3(0.0f, -1.0f, 0.0f));

                RayCastResult resultDown;
                if (World.Space.RayCast(rayDown, 2.0f * mSpawnRadius, out resultDown)) creaturePosition.Y = resultDown.HitData.Location.Y;
                else creaturePosition.Y = Position.Y;

                // Only parameter for a creature is position
                object[] parameters = new object[2];
                parameters[0] = resultDown.HitData.Location;
                parameters[1] = this;
                object obj = Activator.CreateInstance(mCreatureType, parameters);

                mCreatures.Add(obj as Creature);
                World.Add(obj as Creature);

            }
        }

        public void Render()
        {
            // Nothing to render
        }

    }
}
