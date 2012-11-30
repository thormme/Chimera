#region Using Statements

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using GraphicsLibrary;
using GameConstructLibrary;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BEPUphysics.Entities.Prefabs;
using FinalProject;

#endregion

namespace finalProject
{
    /// <summary>
    /// Parses player input and controls PlayerCreature.
    /// </summary>
    public class PlayerController : Controller
    {
        #region Fields

        private const float rotationRate = MathHelper.PiOver2; // Number of radians per second the camera can turn.
        private const int NumParts = 10;

        public ChaseCamera mCamera;

        private GamePadThumbStickInputAction mMoveForward;
        private GamePadThumbStickInputAction mMoveRight;

        private KeyInputAction mMoveForwardKey;
        private KeyInputAction mMoveBackwardKey;
        private KeyInputAction mMoveRightKey;
        private KeyInputAction mMoveLeftKey;

        private GamePadThumbStickInputAction mLookForward;
        private GamePadThumbStickInputAction mLookRight;

        private KeyInputAction mLookForwardKey;
        private KeyInputAction mLookBackwardKey;
        private KeyInputAction mLookRightKey;
        private KeyInputAction mLookLeftKey;

        private MouseMovementInputAction mLookForwardMouse;
        private MouseMovementInputAction mLookRightMouse;

        private GamePadButtonInputAction mPressPart1;
        private GamePadButtonInputAction mPressPart2;
        private GamePadButtonInputAction mPressPart3;
        private GamePadButtonInputAction mReleasePart1;
        private GamePadButtonInputAction mReleasePart2;
        private GamePadButtonInputAction mReleasePart3;
        private GamePadButtonInputAction mPressJump;

        private List<KeyInputAction> mUse;
        private List<KeyInputAction> mFinishUse;

        private KeyInputAction mJumpKey;

        private KeyInputAction mStealKey;

        private KeyInputAction mPressIncBonePitch;
        private KeyInputAction mPressDecBonePitch;

        private KeyInputAction mPressIncBoneYaw;
        private KeyInputAction mPressDecBoneYaw;

        private KeyInputAction mPressIncBoneRoll;
        private KeyInputAction mPressDecBoneRoll;

        private KeyInputAction mPressIncBoneIndex;
        private KeyInputAction mPressDecBoneIndex;

        private KeyInputAction mSaveBoneTransforms;

        #endregion

        #region Public Methods

        public PlayerController(Viewport viewPort) 
        {
            mCamera = new ChaseCamera(viewPort);
            mCamera.DesiredPositionLocal = new Vector3(0.0f, 2.0f, 5.0f);
            mCamera.LookAtLocal = new Vector3(0.0f, 0.75f, 0.0f);
            mCamera.TargetDirection = Vector3.Forward;
            mCamera.MaxRopeLengthSquared = mCamera.DesiredPositionLocal.LengthSquared();
            mCamera.MinRopeLengthSquared = mCamera.MaxRopeLengthSquared * 0.75f;

            mMoveForward  = new GamePadThumbStickInputAction(PlayerIndex.One, InputAction.ButtonAction.Down, InputAction.GamePadThumbStick.Left, InputAction.GamePadThumbStickAxis.Y, GamePadDeadZone.Circular, -0.2f, 0.2f);
            mMoveRight    = new GamePadThumbStickInputAction(PlayerIndex.One, InputAction.ButtonAction.Down, InputAction.GamePadThumbStick.Left, InputAction.GamePadThumbStickAxis.X, GamePadDeadZone.Circular, -0.2f, 0.2f);
            
            mLookForward  = new GamePadThumbStickInputAction(PlayerIndex.One, InputAction.ButtonAction.Down, InputAction.GamePadThumbStick.Right, InputAction.GamePadThumbStickAxis.Y, GamePadDeadZone.Circular, -0.2f, 0.2f);
            mLookRight    = new GamePadThumbStickInputAction(PlayerIndex.One, InputAction.ButtonAction.Down, InputAction.GamePadThumbStick.Right, InputAction.GamePadThumbStickAxis.X, GamePadDeadZone.Circular, -0.2f, 0.2f);
            
            mMoveForwardKey  = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Down, Keys.W);
            mMoveBackwardKey = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Down, Keys.S);
            mMoveRightKey    = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Down, Keys.D);
            mMoveLeftKey     = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Down, Keys.A);

            mLookForwardMouse  = new MouseMovementInputAction(PlayerIndex.One, InputAction.ButtonAction.Down, InputAction.MouseAxis.Y, 4.0f, 0.5f, 1.0f);
            mLookRightMouse    = new MouseMovementInputAction(PlayerIndex.One, InputAction.ButtonAction.Down, InputAction.MouseAxis.X, 4.0f, 0.5f, 1.0f);

            mLookForwardKey = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Down, Keys.Up);
            mLookBackwardKey = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Down, Keys.Down);
            mLookRightKey = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Down, Keys.Right);
            mLookLeftKey = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Down, Keys.Left);

            mPressPart1 = new GamePadButtonInputAction(PlayerIndex.One, InputAction.ButtonAction.Pressed, Buttons.X);
            mPressPart2 = new GamePadButtonInputAction(PlayerIndex.One, InputAction.ButtonAction.Pressed, Buttons.Y);
            mPressPart3 = new GamePadButtonInputAction(PlayerIndex.One, InputAction.ButtonAction.Pressed, Buttons.B);
            mReleasePart1 = new GamePadButtonInputAction(PlayerIndex.One, InputAction.ButtonAction.Released, Buttons.X);
            mReleasePart2 = new GamePadButtonInputAction(PlayerIndex.One, InputAction.ButtonAction.Released, Buttons.Y);
            mReleasePart3 = new GamePadButtonInputAction(PlayerIndex.One, InputAction.ButtonAction.Released, Buttons.B);
            mPressJump = new GamePadButtonInputAction(PlayerIndex.One, InputAction.ButtonAction.Pressed, Buttons.A);

            mUse = new List<KeyInputAction>(NumParts);
            mUse.Add(new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Pressed, Keys.D1));
            mUse.Add(new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Pressed, Keys.D2));
            mUse.Add(new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Pressed, Keys.D3));
            mUse.Add(new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Pressed, Keys.D4));
            mUse.Add(new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Pressed, Keys.D5));
            mUse.Add(new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Pressed, Keys.D6));
            mUse.Add(new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Pressed, Keys.D7));
            mUse.Add(new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Pressed, Keys.D8));
            mUse.Add(new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Pressed, Keys.D9));
            mUse.Add(new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Pressed, Keys.D0));

            mFinishUse = new List<KeyInputAction>(NumParts);
            mFinishUse.Add(new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Released, Keys.D1));
            mFinishUse.Add(new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Released, Keys.D2));
            mFinishUse.Add(new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Released, Keys.D3));
            mFinishUse.Add(new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Released, Keys.D4));
            mFinishUse.Add(new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Released, Keys.D5));
            mFinishUse.Add(new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Released, Keys.D6));
            mFinishUse.Add(new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Released, Keys.D7));
            mFinishUse.Add(new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Released, Keys.D8));
            mFinishUse.Add(new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Released, Keys.D9));
            mFinishUse.Add(new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Released, Keys.D0)); 

            mJumpKey = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Down, Keys.Space);

            mStealKey = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Down, Keys.LeftShift);

            mPressDecBoneYaw = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Down, Keys.T);
            mPressIncBoneYaw = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Down, Keys.Y);
            mPressDecBonePitch = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Down, Keys.U);
            mPressIncBonePitch = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Down, Keys.I);
            mPressIncBoneRoll = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Down, Keys.O);
            mPressDecBoneRoll = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Down, Keys.P);
            mPressIncBoneIndex = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Pressed, Keys.OemPlus);
            mPressDecBoneIndex = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Pressed, Keys.OemMinus);

            mSaveBoneTransforms = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Pressed, Keys.OemQuestion);
        }

        ~PlayerController()
        {
            mMoveForward.Destroy();
            mMoveRight.Destroy();

            mMoveForwardKey.Destroy();
            mMoveBackwardKey.Destroy();
            mMoveRightKey.Destroy();
            mMoveLeftKey.Destroy();

            mLookForward.Destroy();
            mLookRight.Destroy();

            mLookForwardMouse.Destroy();
            mLookRightMouse.Destroy();

            mLookForwardKey.Destroy();
            mLookBackwardKey.Destroy();
            mLookRightKey.Destroy();
            mLookLeftKey.Destroy();

            mPressPart1.Destroy();
            mPressPart2.Destroy();
            mPressPart3.Destroy();
            mPressJump.Destroy();

            mStealKey.Destroy();
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

        /// <summary>
        /// Assign creature to camera.
        /// </summary>
        /// <param name="creature"></param>
        public override void SetCreature(Creature creature)
        {
            base.SetCreature(creature);
            mCamera.TargetBody = mCreature;
        }
        #endregion

        #region Helper Methods

        /// <summary>
        /// Move creature in direction player requests.
        /// </summary>
        private void MoveCreature(GameTime gameTime)
        {
            float elapsedTime = (float)gameTime.ElapsedGameTime.Milliseconds / 1000.0f;

            // Parse movement input.
            bool moveForwardActive  = mMoveForward.Active;
            float moveForwardDegree = mMoveForward.Degree;
            if (moveForwardActive == false)
            {
                moveForwardActive = mMoveForwardKey.Active || mMoveBackwardKey.Active;
                moveForwardDegree = mMoveForwardKey.Degree - mMoveBackwardKey.Degree;
            }

            bool moveRightActive  = mMoveRight.Active;
            float moveRightDegree = mMoveRight.Degree;
            if (moveRightActive == false)
            {
                moveRightActive = mMoveRightKey.Active || mMoveLeftKey.Active;
                moveRightDegree = mMoveRightKey.Degree - mMoveLeftKey.Degree;
            }

            // Moving player.
            Vector2 walkDirection = Vector2.Zero;
            if (moveForwardActive || moveRightActive)
            {
                (mCreature as PlayerCreature).Stance = Stance.Walking;

                Vector3 forward = mCamera.Forward;
                forward.Y = 0.0f;
                forward.Normalize();

                Vector3 right = mCamera.Right;

                walkDirection += moveForwardDegree * new Vector2(forward.X, forward.Z);
                walkDirection -= moveRightDegree * new Vector2(right.X, right.Z);
            }
            else
            {
                (mCreature as PlayerCreature).Stance = Stance.Standing;
            }

            // Parse look input.
            bool lookForwardActive = mLookForward.Active;
            float lookForwardDegree = mLookForward.Degree;
            if (lookForwardActive == false)
            {
                lookForwardActive = mLookForwardKey.Active || mLookBackwardKey.Active;
                lookForwardDegree = mLookForwardKey.Degree - mLookBackwardKey.Degree;
            }

            bool lookRightActive = mLookRight.Active;
            float lookRightDegree = mLookRight.Degree;
            if (lookRightActive == false)
            {
                lookRightActive = mLookRightKey.Active || mLookLeftKey.Active;
                lookRightDegree = mLookRightKey.Degree - mLookLeftKey.Degree;
            }

            // Rotating camera.
            if (lookForwardActive || lookRightActive)
            {
                mCamera.RotateAroundTarget(elapsedTime * rotationRate * lookRightDegree, elapsedTime * rotationRate * lookForwardDegree, 0.0f);
            }

            if (mCamera.TrackTarget)
            {
                float ropeLength = mCamera.Rope.LengthSquared();
                if (ropeLength > mCamera.MaxRopeLengthSquared && walkDirection != Vector2.Zero)
                {
                    Vector3 walkDirection3D = new Vector3(walkDirection.X, 0.0f, walkDirection.Y);
                    float theta = Vector3.Dot(walkDirection3D, mCamera.Forward);
                    if (theta < 0.9f)
                    {
                        float direction = (Vector3.Cross(walkDirection3D, mCamera.Forward).Y < 0) ? -1.0f : 1.0f;
                        mCamera.RotateAroundTarget(elapsedTime * rotationRate * direction, 0.0f, 0.0f);
                    }
                }
                else if (ropeLength < mCamera.MinRopeLengthSquared && walkDirection != Vector2.Zero)
                {
                    Vector3 walkDirection3D = new Vector3(walkDirection.X, 0.0f, walkDirection.Y);
                    float theta = Vector3.Dot(walkDirection3D, mCamera.Forward);
                    if (theta > -0.9f)
                    {
                        float direction = (Vector3.Cross(walkDirection3D, mCamera.Forward).Y < 0) ? -1.0f : 1.0f;
                        mCamera.RotateAroundTarget(elapsedTime * 4.0f * rotationRate * direction, 0.0f, 0.0f);
                    }
                }
            }

            if (mCreature.CharacterController.supportData.SupportObject == null)
            {
                // In the air.
                (mCreature as PlayerCreature).Stance = Stance.Jumping;
            }

            mCreature.Move(walkDirection);
        }

        /// <summary>
        /// If player has requested action perform it.
        /// </summary>
        private void PerformActions()
        {
            if (mPressJump.Active || mJumpKey.Active)
            {
                mCreature.Jump();
            }

            for(int i = 0; i < NumParts; ++i)
            {
                if (mUse[i].Active)
                {
                    if (mStealKey.Active && mCreature is PlayerCreature)
                    {
                        (mCreature as PlayerCreature).FindAndAddPart(i);
                    }
                    else
                    {
                        mCreature.UsePart(i, mCamera.Forward);
                    }
                }
                else if (mFinishUse[i].Active)
                {
                    mCreature.FinishUsingPart(i, mCamera.Forward);
                }
            }

            if (mSaveBoneTransforms.Active)
            {
                mCreature.WriteBoneTransforms();
            }

            if (mPressIncBoneRoll.Active)
            {
                Matrix rotation = Matrix.CreateFromAxisAngle(mCreature.BoneForward, MathHelper.Pi / 50.0f);
                mCreature.BoneRotations *= rotation;
                mCreature.BoneRight = Vector3.Transform(mCreature.BoneRight, rotation);
                mCreature.BoneUp = Vector3.Transform(mCreature.BoneUp, rotation);
            }
            else if (mPressDecBoneRoll.Active)
            {
                Matrix rotation = Matrix.CreateFromAxisAngle(mCreature.BoneForward, -MathHelper.Pi / 50.0f);
                mCreature.BoneRotations *= rotation;
                mCreature.BoneRight = Vector3.Transform(mCreature.BoneRight, rotation);
                mCreature.BoneUp = Vector3.Transform(mCreature.BoneUp, rotation);
            }

            if (mPressIncBoneYaw.Active)
            {
                Matrix rotation = Matrix.CreateFromAxisAngle(mCreature.BoneUp, MathHelper.Pi / 50.0f);
                mCreature.BoneRotations *= rotation;
                mCreature.BoneRight = Vector3.Transform(mCreature.BoneRight, rotation);
                mCreature.BoneForward = Vector3.Transform(mCreature.BoneForward, rotation);
            }
            else if (mPressDecBoneYaw.Active)
            {
                Matrix rotation = Matrix.CreateFromAxisAngle(mCreature.BoneUp, -MathHelper.Pi / 50.0f);
                mCreature.BoneRotations *= rotation;
                mCreature.BoneRight = Vector3.Transform(mCreature.BoneRight, rotation);
                mCreature.BoneForward = Vector3.Transform(mCreature.BoneForward, rotation);
            }

            if (mPressIncBonePitch.Active)
            {
                Matrix rotation = Matrix.CreateFromAxisAngle(mCreature.BoneRight, MathHelper.Pi / 50.0f);
                mCreature.BoneRotations *= rotation;
                mCreature.BoneUp = Vector3.Transform(mCreature.BoneUp, rotation);
                mCreature.BoneForward = Vector3.Transform(mCreature.BoneForward, rotation);
            }
            else if (mPressDecBonePitch.Active)
            {
                Matrix rotation = Matrix.CreateFromAxisAngle(mCreature.BoneRight, -MathHelper.Pi / 50.0f);
                mCreature.BoneRotations *= rotation;
                mCreature.BoneUp = Vector3.Transform(mCreature.BoneUp, rotation);
                mCreature.BoneForward = Vector3.Transform(mCreature.BoneForward, rotation);
            }

            if (mPressIncBoneIndex.Active)
            {
                mCreature.BoneIndex++;
            }
            else if (mPressDecBoneIndex.Active)
            {
                mCreature.BoneIndex--;
            }
        }

        /// <summary>
        /// Update camera to follow new creature configuration.
        /// </summary>
        private void UpdateCamera(GameTime gameTime)
        {
            mCamera.TargetPosition = mCreature.Position;
            mCamera.Up = Vector3.Up;

            mCamera.Update(gameTime);
        }

        #endregion
    }
}
