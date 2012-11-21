﻿#region Using Statements

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using GraphicsLibrary;
using GameConstructLibrary;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BEPUphysics.Entities.Prefabs;

#endregion

namespace finalProject
{
    /// <summary>
    /// Parses player input and controls PlayerCreature.
    /// </summary>
    public class PlayerController : Controller
    {
        #region Fields

        private const float mCameraRotation = 90.0f;
        private const float mDistFromCreature = 200.0f;

        public Camera mCamera;

        private InputAction mMoveForward;
        private InputAction mMoveRight;
        private InputAction mMoveBackward;
        private InputAction mMoveLeft;

        private InputAction mLookForward;
        private InputAction mLookBackward;
        private InputAction mLookRight;
        private InputAction mLookLeft;

        private InputAction mPressPart1;
        private InputAction mPressPart2;
        private InputAction mPressPart3;
        private InputAction mPressJump;

        private InputAction mAdd;
        private InputAction mCheat;

        #endregion

        #region Public Properties

        #endregion 

        #region Public Methods

        public PlayerController(Viewport viewPort) 
        {
            mCamera = new Camera(viewPort);
            mCamera.DesiredPositionLocal = new Vector3(0.0f, 4.0f, 10.0f);
            mCamera.LookAtLocal = new Vector3(0.0f, 1.50f, 0.0f);

            mMoveForward = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Down, Keys.W);
            mMoveBackward = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Down, Keys.S);
            mMoveRight = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Down, Keys.D);
            mMoveLeft = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Down, Keys.A);

            mLookForward = new GamePadThumbStickInputAction(PlayerIndex.One, InputAction.ButtonAction.Down, InputAction.GamePadThumbStick.Right, InputAction.GamePadThumbStickAxis.Y, GamePadDeadZone.Circular, -0.2f, 0.2f);
            mLookRight = new GamePadThumbStickInputAction(PlayerIndex.One, InputAction.ButtonAction.Down, InputAction.GamePadThumbStick.Right, InputAction.GamePadThumbStickAxis.X, GamePadDeadZone.Circular, -0.2f, 0.2f);

            mPressPart1 = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Pressed, Keys.D1);
            mPressPart2 = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Pressed, Keys.D2);
            mPressPart3 = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Pressed, Keys.D3);
            mPressJump = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Pressed, Keys.Space);

            mAdd = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Pressed, Keys.LeftShift);
            mCheat = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Pressed, Keys.Enter);
        }

        ~PlayerController()
        {
            mMoveForward.Destroy();
            mMoveRight.Destroy();
            mMoveBackward.Destroy();
            mMoveLeft.Destroy();

            mLookForward.Destroy();
            mLookRight.Destroy();

            mPressPart1.Destroy();
            mPressPart2.Destroy();
            mPressPart3.Destroy();
            mPressJump.Destroy();

            mAdd.Destroy();
            mCheat.Destroy();
        }

        /// <summary>
        /// Passes the player input to the creature.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        /// <param name="collidingCreatures">The list of creatures from the creature's radial sensor.</param>
        public override void Update(GameTime gameTime, List<Creature> collidingCreatures)
        {
            MoveCreature(gameTime);
            PerformActions();
            UpdateCamera(gameTime);
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Move creature in direction player requests.
        /// </summary>
        private void MoveCreature(GameTime gameTime)
        {
            float elapsedTime = (float)gameTime.ElapsedGameTime.Milliseconds / 1000.0f;

            if (mLookForward.Active)
            {
                Matrix rotation = Matrix.CreateFromAxisAngle(
                    Vector3.Normalize(Vector3.Cross(mCamera.Up, mCamera.DesiredPositionLocal)), 
                    -0.06f * mLookForward.Degree);
                Vector3 newDesiredPosition = Vector3.Transform(mCamera.DesiredPositionLocal, rotation);

                float newAngle = Vector3.Dot(Vector3.Normalize(newDesiredPosition), mCamera.Up);
                if (Math.Abs(newAngle) >= mCamera.MinPitchAngle)
                {
                    newDesiredPosition = mCamera.DesiredPositionLocal;
                }

                mCamera.DesiredPositionLocal = newDesiredPosition;
            }

            if (mLookRight.Active)
            {
                Matrix rotation = Matrix.CreateFromAxisAngle(mCamera.Up, -0.06f * mLookRight.Degree);
                mCamera.DesiredPositionLocal = Vector3.Transform(mCamera.DesiredPositionLocal, rotation);
            }

            Vector2 walkDirection = Vector2.Zero;
            if (mMoveForward.Active || mMoveRight.Active || mMoveLeft.Active || mMoveBackward.Active)
            {
                walkDirection = Vector2.Normalize(new Vector2(mMoveRight.Degree - mMoveLeft.Degree, mMoveBackward.Degree - mMoveForward.Degree));

                Vector3 groundForward = mCamera.Forward - Vector3.Dot(mCamera.Forward, mCreature.Up) * mCreature.Up;
                Vector2 groundForwardProjected = new Vector2(groundForward.X, groundForward.Z);

                Vector3 groundRight = mCamera.Right - Vector3.Dot(mCamera.Right, mCreature.Up) * mCreature.Up;
                Vector2 groundRightProjected = new Vector2(groundRight.X, groundRight.Z);

                walkDirection = Vector2.Normalize(-1.0f * (walkDirection.X * groundRightProjected + walkDirection.Y * groundForwardProjected));
            }
            mCreature.Move(walkDirection);
        }

        /// <summary>
        /// If player has requested action perform it.
        /// </summary>
        private void PerformActions()
        {

        }

        /// <summary>
        /// Update camera to follow new creature configuration.
        /// </summary>
        private void UpdateCamera(GameTime gameTime)
        {
            mCamera.TargetPosition = mCreature.Position;
            mCamera.TargetDirection = mCreature.Forward;
            mCamera.Up = Vector3.Up;

            mCamera.Update(gameTime);
        }

        #endregion
    }
}
