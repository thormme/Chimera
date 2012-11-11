using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GraphicsLibrary;

namespace GameConstructLibrary
{
    public class PlayerController : Controller
    {
        #region
        private const float mCameraRotation = MathHelper.PiOver2;

        private Camera mCamera;

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
        #endregion

        public PlayerController() 
        {
            mMoveUp = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Down, Microsoft.Xna.Framework.Input.Keys.W);
            mMoveDown = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Down, Microsoft.Xna.Framework.Input.Keys.S);
            mMoveLeft = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Down, Microsoft.Xna.Framework.Input.Keys.A);
            mMoveRight = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Down, Microsoft.Xna.Framework.Input.Keys.D);
            mCameraUp = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Down, Microsoft.Xna.Framework.Input.Keys.Up);
            mCameraDown = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Down, Microsoft.Xna.Framework.Input.Keys.Down);
            mCameraLeft = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Down, Microsoft.Xna.Framework.Input.Keys.Left);
            mCameraRight = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Down, Microsoft.Xna.Framework.Input.Keys.Right);
            mPart1 = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Pressed, Microsoft.Xna.Framework.Input.Keys.D1);
            mPart2 = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Pressed, Microsoft.Xna.Framework.Input.Keys.D2);
            mPart3 = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Pressed, Microsoft.Xna.Framework.Input.Keys.D3);
            mJump = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Pressed, Microsoft.Xna.Framework.Input.Keys.Space);
            mAdd = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Pressed, Microsoft.Xna.Framework.Input.Keys.LeftShift);
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
        }

        public override void Update(GameTime time, List<Creature> collidingCreatures)
        {
            Vector2 moveDirection = new Vector2(0.0f, 0.0f);
            float rotation = mCameraRotation * time.ElapsedGameTime.Milliseconds / 1000;

            #region
            if (mCameraUp.Active)
            {
                mCamera.RotatePitch(rotation);
            }
            if (mCameraDown.Active)
            {
                mCamera.RotatePitch(-rotation);
            }
            if (mCameraRight.Active)
            {
                mCamera.RotateYaw(rotation);
            }
            if (mCameraLeft.Active)
            {
                mCamera.RotateYaw(-rotation);
            }
            #endregion

            mCreature.Forward = mCamera.Target;

            #region
            if (mMoveUp.Active)
            {
                moveDirection.Y += 1.0f;
            }
            if (mMoveDown.Active)
            {
                moveDirection.Y -= 1.0f;
            }
            if (mMoveRight.Active)
            {
                moveDirection.X += 1.0f;
            }
            if (mMoveLeft.Active)
            {
                moveDirection.X -= 1.0f;
            }
            #endregion

            mCreature.Move(moveDirection);

            #region
            if (mPart1.Active)
            {
                mCreature.UsePart(1, mCamera.Target);
            }
            if (mPart2.Active)
            {
                mCreature.UsePart(2, mCamera.Target);
            }
            if (mPart3.Active)
            {
                mCreature.UsePart(3, mCamera.Target);
            }

            if (mJump.Active)
            {
                mCreature.Jump();
            }
            if (mAdd.Active)
            {
                if (mCreature as PlayerCreature != null)
                {
                    (mCreature as PlayerCreature).AddPart();
                }
            }
            #endregion
        }
    }
}
