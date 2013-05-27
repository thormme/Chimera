using BEPUphysics.CollisionRuleManagement;
using BEPUphysics.Entities.Prefabs;
using GameConstructLibrary;
using GraphicsLibrary;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Chimera
{
    public class Wormhole : Actor
    {
        #region Constants

        private const float Diameter = 20.0f;
        private const float Length = 40.0f;

        #endregion

        #region Fields

        public static CollisionGroup WormholeGroup = new CollisionGroup();

        ScrollingTransparentModel mModel;

        protected List<Creature> mCollidingCreatures;
        public List<Creature> CollidingCreatures
        {
            get
            {
                return mCollidingCreatures;
            }
        }

        private Vector3 mCenterPosition;

        #endregion

        public Wormhole(Vector3 position, Quaternion orientation, Vector3 scale)
            : base(null, new Box(position, Diameter, Length, Diameter))
        {
            Entity.Orientation = orientation;
            Entity.Position += Up * 5.0f;
            Entity.CollisionInformation.CollisionRules.Group = WormholeGroup;

            mCenterPosition = Position - Up * Length / 2.0f;

            Scale = scale;
            mModel = new ScrollingTransparentModel("wormhole", new Vector2(MathHelper.TwoPi * 0.75f));

            mCollidingCreatures = new List<Creature>();
        }

        public Wormhole(string modelName, Vector3 translation, Quaternion orientation, Vector3 scale, string[] extraParameters)
            : this(translation, orientation, scale)
        {
        }

        public override void Update(GameTime gameTime)
        {
            mModel.Update(gameTime);

            foreach (Creature creature in mCollidingCreatures)
            {
                creature.EscapeWormhole();
            }
            mCollidingCreatures.Clear();

            foreach (IGameObject gameObject in CollidingObjects)
            {
                if (gameObject is Creature)
                {
                    Creature creature = gameObject as Creature;
                    if (creature != null && !creature.Position.Equals(Position))
                    {
                        creature.Spaghettify(mCenterPosition, Up, 1.0f, Length);
                        mCollidingCreatures.Add(creature);
                    }
                }
            }
        }

        public override void Render()
        {
            BoundingBox scaledBoundingBox = Entity.CollisionInformation.BoundingBox;

            mModel.Render(mCenterPosition + Up * 5.0f, XNAOrientationMatrix, Scale, false);
        }
    }
}
