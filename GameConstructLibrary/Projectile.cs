using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GraphicsLibrary;
using BEPUphysics.Entities;
using Microsoft.Xna.Framework;
using BEPUphysics.CollisionRuleManagement;

namespace GameConstructLibrary
{
    public class Projectile : Actor
    {

        public static CollisionGroup ProjectileGroup = new CollisionGroup();

        public bool CheckHits { get; set; }

        protected Actor mOwner;
        protected Vector3 mProjectileImpulse;
        protected float mSpeed;
        protected float mForce;

        public Projectile(Renderable renderable, Entity entity, Actor owner, Vector3 direction, float speed, Vector3 scale) :
            base(renderable, entity)
        {
            direction.Normalize();

            CheckHits = true;

            mOwner = owner;
            mProjectileImpulse = new Vector3(direction.X * speed, 0.0f, direction.Z * speed);
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
            World.Remove(this);
        }

    }
}
