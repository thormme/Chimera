using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GraphicsLibrary;
using BEPUphysics.Entities;
using Microsoft.Xna.Framework;
using BEPUphysics.CollisionRuleManagement;
using BEPUphysics.Constraints.TwoEntity.Joints;
using BEPUphysics.Constraints.TwoEntity.JointLimits;
using BEPUphysics.Constraints.SolverGroups;

namespace GameConstructLibrary
{
    public class Projectile : Actor
    {

        public static CollisionGroup ProjectileGroup = new CollisionGroup();
        public static CollisionGroup SensorProjectileGroup = new CollisionGroup();

        public bool CheckHits { get; set; }
        public Entity StuckEntity;
        private WeldJoint mStickConstraint;

        protected Actor mOwner;
        protected Vector3 mProjectileImpulse;
        protected float mSpeed;
        protected float mForce;

        public Projectile(Renderable renderable, Entity entity, Actor owner, Vector3 direction, float speed, Vector3 scale) :
            base(renderable, entity)
        {
            if (direction.Length() != 0.0f)
            {
                direction.Normalize();
            }

            CheckHits = true;
            StuckEntity = null;

            mOwner = owner;
            mProjectileImpulse = new Vector3(direction.X * speed, direction.Y * speed, direction.Z * speed);
            mSpeed = speed;
            
            Entity.Position = owner.Position;
            XNAOrientationMatrix = Matrix.CreateLookAt(owner.Position, direction, owner.Up);
            Scale = scale;

            CollisionRules.AddRule(Entity, owner.Entity, CollisionRule.NoBroadPhase);
            Entity.CollisionInformation.CollisionRules.Group = ProjectileGroup;

            Entity.PositionUpdateMode = BEPUphysics.PositionUpdating.PositionUpdateMode.Continuous;
            
            Entity.ApplyLinearImpulse(ref mProjectileImpulse);

        }

        public override void Update(Microsoft.Xna.Framework.GameTime time)
        {
            if (CheckHits)
            {
                foreach (IGameObject gameObject in CollidingObjects)
                {
                    Hit(gameObject);
                }
            }

        }

        protected virtual void Hit(IGameObject gameObject)
        {
        }

        protected void StickToEntity(Entity entity)
        {
            StuckEntity = entity;
            Entity.AngularMomentum = Vector3.Zero;
            Entity.LinearMomentum = Vector3.Zero;
            mStickConstraint = new WeldJoint(StuckEntity, Entity);
            World.Space.Add(mStickConstraint);
            CollisionRules.AddRule(Entity, StuckEntity, CollisionRule.NoBroadPhase);
        }

        protected void Unstick()
        {
            if (StuckEntity != null)
            {
                CollisionRules.RemoveRule(Entity, StuckEntity);
                World.Space.Remove(mStickConstraint);
                StuckEntity = null;
            }
        }

    }
}
