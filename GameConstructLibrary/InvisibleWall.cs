using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using BEPUphysics.Entities.Prefabs;
using BEPUphysics.MathExtensions;
using BEPUphysics.Entities;
using BEPUphysics.CollisionRuleManagement;

namespace GameConstructLibrary
{
    public class InvisibleWall : IGameObject, IEntityOwner
    {
        public static CollisionGroup InvisibleWallGroup = new CollisionGroup();

        public Vector3 Position
        {
            get { return Entity.Position; }
        }

        public World World { get; set; }

        public Matrix XNAOrientationMatrix
        {
            get 
            {
                return Matrix3X3.ToMatrix4X4(Entity.OrientationMatrix);
            }
        }

        public Vector3 Scale
        {
            get
            {
                return Entity.CollisionInformation.BoundingBox.Max - Entity.CollisionInformation.BoundingBox.Min;
            }
        }

        public Entity Entity { get; set; }

        public InvisibleWall(Vector3 position, float width, float height, float length)
        {
            Entity = new Box(position, width, height, length);
            Entity.CollisionInformation.CollisionRules.Group = InvisibleWallGroup;
            Entity.Tag = this;
            Entity.CollisionInformation.Tag = this;
        }

        public void Render()
        {
        }
    }
}
