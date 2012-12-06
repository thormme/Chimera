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

        public bool CheckHits { get; set; }
        private Entity Stick;
        private WeldJoint mStickConstraint;
        private float mOriginalMass;

        protected Actor mOwner;
        protected Vector3 mProjectileImpulse;
        protected float mSpeed;
        protected float mForce;

        public Projectile(Renderable renderable, Entity entity, Actor owner, Vector3 direction, float speed, Vector3 scale) :
            base(renderable, entity)
        {
            direction.Normalize();

            CheckHits = true;
            Stick = null;

            mOwner = owner;
            mProjectileImpulse = new Vector3(direction.X * speed, direction.Y * speed, direction.Z * speed);
            mSpeed = speed;
            mOriginalMass = Entity.Mass;
            
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
            Stick = entity;
            Position = Stick.Position;
            Entity.AngularMomentum = Vector3.Zero;
            Entity.LinearMomentum = Vector3.Zero;
            mStickConstraint = new WeldJoint(Stick, Entity);
            Entity.Mass = 0.001f;
            World.Space.Add(mStickConstraint);
            CollisionRules.AddRule(Entity, Stick, CollisionRule.NoBroadPhase);
        }

        protected void Unstick()
        {
            if (Stick != null)
            {
                CollisionRules.RemoveRule(Entity, Stick);
                Entity.Mass = mOriginalMass;
                World.Space.Remove(mStickConstraint);
                Stick = null;
            }
        }

    }
}
