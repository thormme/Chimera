using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GraphicsLibrary;
using BEPUphysics.Collidables.MobileCollidables;
using BEPUphysics.CollisionShapes.ConvexShapes;
using GameConstructLibrary;
using BEPUphysics.CollisionRuleManagement;
using BEPUphysics.Entities.Prefabs;

namespace finalProject
{
    /// <summary>
    /// A radial sensor used to identify PhysicsObjects near a point, like on a creature.
    /// </summary>
    public class RadialSensor : PhysicsObject
    {
        protected List<Creature> mCollidingCreatures;
        public List<Creature>  CollidingCreatures
        {
            get
            {
                return mCollidingCreatures;
            }
        }

        /// <summary>
        /// Constructs the RadialSensor as a sphere.
        /// </summary>
        /// <param name="radius">
        /// The radius of the RadialSensor's sphere.
        /// </param>
        public RadialSensor(float radius)
            : base(null, new Sphere(new Vector3(0), radius))
        {
            this.Entity.CollisionInformation.CollisionRules.Personal = CollisionRule.NoSolver;
            mCollidingCreatures = new List<Creature>();
        }

        /// <summary>
        /// Stores the list of PhysicsObjects, and also a list of Creatures.
        /// </summary>
        /// <param name="objects">
        /// The list of colliding PhysicsObjects.
        /// </param>
        public virtual void Update(GameTime time)
        {
            mCollidingCreatures.Clear();

            foreach (IGameObject i in mCollidingObjects)
            {
                if (i as Creature != null)
                {
                    mCollidingCreatures.Add(i as Creature);
                    //System.Console.WriteLine(i);
                }
            }
            //System.Console.WriteLine("done");

            mCollidingCreatures.Sort(new ClosestPhysicsObject(Position));
        }
    }
}
