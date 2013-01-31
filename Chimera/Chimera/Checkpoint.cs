﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameConstructLibrary;
using GraphicsLibrary;
using BEPUphysics.Entities;
using Microsoft.Xna.Framework;
using BEPUphysics.Collidables;
using BEPUphysics.NarrowPhaseSystems.Pairs;
using BEPUphysics.BroadPhaseSystems;
using BEPUphysics.Collidables.MobileCollidables;
using BEPUphysics.Entities.Prefabs;

namespace Chimera
{
    public class Checkpoint : PhysicsObject
    {
        InanimateModel mRedCheckpoint;
        InanimateModel mBlueCheckpoint;

        ScrollingTransparentModel mRedLight;
        ScrollingTransparentModel mBlueLight;

        private bool mActiveCheckpoint = false;
        public bool ActiveCheckpoint
        {
            get { return mActiveCheckpoint; }
            set { mActiveCheckpoint = value; }
        }

        public Checkpoint(Vector3 position, Quaternion orientation, Vector3 scale)
            : base(new InanimateModel("checkpoint_red"), new Cylinder(position, 1f, scale.Length()))
        {
            Scale = new Vector3(2.0f);

            mRedCheckpoint = new InanimateModel("checkpoint_red");
            mBlueCheckpoint = new InanimateModel("checkpoint_blue");
            mRedLight = new ScrollingTransparentModel("checkpoint_light_red");
            mBlueLight = new ScrollingTransparentModel("checkpoint_light_blue");

            mRedLight.HorizontalVelocity = 0.1f;
            mBlueLight.HorizontalVelocity = 0.1f;
        }

        /// <summary>
        /// Constructor for use by the World level loading.
        /// </summary>
        /// <param name="modelName">Name of the model.</param>
        /// <param name="translation">The position.</param>
        /// <param name="orientation">The orientation.</param>
        /// <param name="scale">The amount to scale by.</param>
        /// <param name="extraParameters">Extra parameters.</param>
        public Checkpoint(String modelName, Vector3 translation, Quaternion orientation, Vector3 scale, string[] extraParameters)
            : this(translation, orientation, scale)
        {
        }

        public override void InitialCollisionDetected(EntityCollidable sender, Collidable other, CollidablePairHandler collisionPair)
        {
            if (other.Tag is CharacterSynchronizer)
            {
                CharacterSynchronizer synchronizer = (other.Tag as CharacterSynchronizer);
                if (synchronizer.body.Tag is PlayerCreature)
                {
                    Checkpoint oldCheckpoint = (synchronizer.body.Tag as PlayerCreature).Checkpoint;
                    if (oldCheckpoint != null)
                    {
                        oldCheckpoint.ActiveCheckpoint = false;
                    }

                    (synchronizer.body.Tag as PlayerCreature).SpawnOrigin = Position;
                    (synchronizer.body.Tag as PlayerCreature).Checkpoint = this;
                    mActiveCheckpoint = true;
                }
            }
        }

        public override void  Render()
        {
            if (mActiveCheckpoint)
            {
                mBlueCheckpoint.Render(Position, XNAOrientationMatrix, Scale);
                mBlueLight.Render(Position, XNAOrientationMatrix, Scale);
            }
            else
            {
                mRedCheckpoint.Render(Position, XNAOrientationMatrix, Scale);
                mRedLight.Render(Position, XNAOrientationMatrix, Scale);
            }
        }
    }
}