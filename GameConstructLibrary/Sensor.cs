using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BEPUphysics.Entities;
using BEPUphysics.CollisionRuleManagement;

namespace GameConstructLibrary
{
    public class Sensor : PhysicsObject
    {
        public static CollisionGroup SensorGroup = new CollisionGroup();

        public Sensor(Entity entity)
            : base(null, entity)
        {
            Entity.CollisionInformation.CollisionRules.Group = SensorGroup;
        }
    }
}
