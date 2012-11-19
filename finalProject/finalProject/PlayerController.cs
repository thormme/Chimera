using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GraphicsLibrary;
using GameConstructLibrary;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace finalProject
{
    public class PlayerController : Controller
    {
        #region private members
        //private const float mCameraRotation = 90.0f;
        private const float mDistFromCreature = 10.0f;
        private const float mDistFromCreatureSquared = mDistFromCreature * mDistFromCreature;

        public Camera mCamera;

        private KeyInputAction mMoveUp;
        private KeyInputAction mMoveDown;
        private KeyInputAction mMoveLeft;
        private KeyInputAction mMoveRight;
        private KeyInputAction mCameraUp;
        private KeyInputAction mCameraDown;
        private KeyInputAction mCameraLeft;
        private KeyInputAction mCameraRight;
        private KeyInputAction mPart1;
        private KeyInputAction mPart2;
        private KeyInputAction mPart3;
        private KeyInputAction mJump;
        private KeyInputAction mAdd;
        private KeyInputAction mCheat;

        private Vector3 target;
        #endregion

        public PlayerController(Viewport viewPort) 
        {
            mCamera = new Camera(viewPort);
            mMoveUp = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Down, Keys.W);
            mMoveDown = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Down, Keys.S);
            mMoveLeft = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Down, Keys.A);
            mMoveRight = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Down, Keys.D);
            mCameraUp = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Down, Keys.Up);
            mCameraDown = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Down, Keys.Down);
            mCameraLeft = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Down, Keys.Left);
            mCameraRight = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Down, Keys.Right);
            mPart1 = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Pressed, Keys.D1);
            mPart2 = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Pressed, Keys.D2);
            mPart3 = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Pressed, Keys.D3);
            mJump = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Pressed, Keys.Space);
            mAdd = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Pressed, Keys.LeftShift);
            mCheat = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Pressed, Keys.Enter);

            target = new Vector3(0.0f, -70.0f, 0.0f);
        }

        ~PlayerController()
        {
            mMoveUp.Destroy();
            mMoveDown.Destroy();
            mMoveLeft.Destroy();
            mMoveRight.Destroy();
            mCameraUp.Destroy();
            mCameraDown.Destroy();
            mCameraLeft.Destroy();
            mCameraRight.Destroy();
            mPart1.Destroy();
            mPart2.Destroy();
            mPart3.Destroy();
            mJump.Destroy();
            mAdd.Destroy();

            mCheat.Destroy();
        }

        /// <summary>
        /// Passes the player input to the creature.
        /// </summary>
        /// <param name="time">
        /// The game time.
        /// </param>
        /// <param name="collidingCreatures">
        /// The list of creatures from the creature's radial sensor.
        /// </param>
        public override void Update(GameTime time, List<Creature> collidingCreatures)
        {
            Vector2 moveDirection = new Vector2(0.0f, 0.0f);
            float time_step = (float)time.ElapsedGameTime.Milliseconds / 1000.0f;
            //float rotation = mCameraRotation * time_step;

            #region adjust camera angle
            //if (mCameraUp.Active)
            //{
            //    //mCamera.RotatePitch(rotation);
            //    //mCreature.Forward = mCamera.Forward;
            //    target += mCamera.Up;
            //}
            //if (mCameraDown.Active)
            //{
            //    //mCamera.RotatePitch(-rotation);
            //    //mCreature.Forward = mCamera.Forward;
            //    target -= mCamera.Up;
            //}
            //if (mCameraRight.Active)
            //{
            //    //mCamera.RotateYaw(rotation);
            //    //mCreature.Forward = mCamera.Forward;
            //    target += mCamera.Right;
            //}
            //if (mCameraLeft.Active)
            //{
            //    //mCamera.RotateYaw(-rotation);
            //    //mCreature.Forward = mCamera.Forward;
            //    target -= mCamera.Right;
            //}
            #endregion

            #region adjust creature position
            bool moved = false;
            if (mMoveUp.Active)
            {
                moveDirection.Y += 1.0f;
                moved = true;
            }
            if (mMoveDown.Active)
            {
                moveDirection.Y -= 1.0f;
                moved = true;
            }
            if (mMoveRight.Active)
            {
                moveDirection.X -= 1.0f;
                moved = true;
            }
            if (mMoveLeft.Active)
            {
                moveDirection.X += 1.0f;
                moved = true;
            }
            if (moved)
            {
                Vector3 moveVector = moveDirection.Y * mCamera.Forward + moveDirection.X * mCamera.Right;
                moveVector.Y = 0.0f;
                mCreature.Forward = Vector3.Normalize(moveVector);
                mCreature.Move(1.0f);
            }
            #endregion

            if (mCheat.Active)
            {
                mCreature.Entity.Position = new Vector3(0.0f);
                mCreature.Entity.LinearVelocity = new Vector3(0.0f);
            }

            moveDirection.X *= time_step;
            moveDirection.Y *= time_step;
            //mCreature.Move(moveDirection);
            mCamera.Target = mCreature.Position;
            Vector3 diffVector = mCreature.Position - mCamera.Position;
            float diff = diffVector.Length() - mDistFromCreature;
            //if (diff > 0)
            {
                mCamera.Position += Vector3.Normalize(diffVector) * diff;
            }
            //Vector3 temp = Vector3.Multiply(Vector3.Normalize(mCamera.Forward), mDistFromCreature);
            //mCamera.Position = Vector3.Subtract(mCreature.Position, temp);
            //mCamera.Position = new Vector3(0.0f, 100.0f, 1.0f);
            //mCamera.Target = target;// mCreature.Position;
            //Vector3 direction = mCreature.XNAOrientationMatrix.Forward;
            //direction.Normalize();
            //mCamera.Position = mCamera.Target - 10.0f * direction + new Vector3(0.0f, 10.0f, 0.0f);
            GraphicsManager.Update(mCamera);

            #region use/add parts, jump
            if (mPart1.Active)
            {
                mCreature.UsePart(1, mCamera.Forward);
            }
            if (mPart2.Active)
            {
                mCreature.UsePart(2, mCamera.Forward);
            }
            if (mPart3.Active)
            {
                mCreature.UsePart(3, mCamera.Forward);
            }

            if (mJump.Active)
            {
                mCreature.Jump();
            }
            if (mAdd.Active)
            {
                if (mCreature as PlayerCreature != null)
                {
                    (mCreature as PlayerCreature).FindAndAddPart();
                }
            }
            #endregion
        }
    }
}
