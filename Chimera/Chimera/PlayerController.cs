#region Using Statements

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using GraphicsLibrary;
using GameConstructLibrary;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BEPUphysics.Entities.Prefabs;
using Chimera;

#endregion

namespace Chimera
{
    /// <summary>
    /// Parses player input and controls PlayerCreature.
    /// </summary>
    public class PlayerController : Controller
    {
        #region Fields

        private float rotationRate = MathHelper.Pi; // Number of radians per second the camera can turn.
        private const int NumParts = 13;
        private const int NumKeys = 10;
        private const int NumButtons = 3;

        public ChaseCamera mCamera;

        private GamePadThumbStickInputAction mMoveForward;
        private GamePadThumbStickInputAction mMoveRight;

        private KeyInputAction mMoveForwardKey;
        private KeyInputAction mMoveBackwardKey;
        private KeyInputAction mMoveRightKey;
        private KeyInputAction mMoveLeftKey;

        private MouseButtonInputAction mStealPressMouse;
        private MouseButtonInputAction mStealReleaseMouse;

        private GamePadThumbStickInputAction mLookForward;
        private GamePadThumbStickInputAction mLookRight;

        private MouseMovementInputAction mLookForwardMouse;
        private MouseMovementInputAction mLookRightMouse;

        private GamePadButtonInputAction mPressPart1;
        private GamePadButtonInputAction mPressPart2;
        private GamePadButtonInputAction mPressPart3;
        private GamePadButtonInputAction mReleasePart1;
        private GamePadButtonInputAction mReleasePart2;
        private GamePadButtonInputAction mReleasePart3;
        private GamePadButtonInputAction mPressJump;

        private List<InputAction> mUse;
        private List<InputAction> mFinishUse;

        private KeyInputAction mJumpKey;

        private GamePadTriggerInputAction mStealPress;
        private GamePadTriggerInputAction mStealRelease;

        private KeyInputAction mPressIncBonePitch;
        private KeyInputAction mPressDecBonePitch;

        private KeyInputAction mPressIncBoneYaw;
        private KeyInputAction mPressDecBoneYaw;

        private KeyInputAction mPressIncBoneRoll;
        private KeyInputAction mPressDecBoneRoll;

        private KeyInputAction mPressIncBoneIndex;
        private KeyInputAction mPressDecBoneIndex;

        private KeyInputAction mPressIncTransX;
        private KeyInputAction mPressDecTransX;

        private KeyInputAction mPressIncTransY;
        private KeyInputAction mPressDecTransY;

        private KeyInputAction mPressIncTransZ;
        private KeyInputAction mPressDecTransZ;

        private KeyInputAction mSaveBoneTransforms;

        private int mBoneIndex = 0;
        #endregion

        #region Public Methods

        public PlayerController(Viewport viewPort) 
        {
            mCamera = new ChaseCamera(viewPort);
            mCamera.DesiredPositionLocal = new Vector3(0.0f, 2.0f, 10.0f);
            mCamera.LookAtLocal = new Vector3(0.0f, 1.5f, 0.0f);
            mCamera.TargetDirection = Vector3.Forward;
            mCamera.MaxRopeLengthSquared = mCamera.DesiredPositionLocal.LengthSquared();
            mCamera.MinRopeLengthSquared = mCamera.MaxRopeLengthSquared * 0.75f;

            mCamera.TrackTarget = true;

            mMoveForward  = new GamePadThumbStickInputAction(PlayerIndex.One, InputAction.ButtonAction.Down, InputAction.GamePadThumbStick.Left, InputAction.GamePadThumbStickAxis.Y, GamePadDeadZone.Circular, -0.2f, 0.2f);
            mMoveRight    = new GamePadThumbStickInputAction(PlayerIndex.One, InputAction.ButtonAction.Down, InputAction.GamePadThumbStick.Left, InputAction.GamePadThumbStickAxis.X, GamePadDeadZone.Circular, -0.2f, 0.2f);
            
            mLookForward  = new GamePadThumbStickInputAction(PlayerIndex.One, InputAction.ButtonAction.Down, InputAction.GamePadThumbStick.Right, InputAction.GamePadThumbStickAxis.Y, GamePadDeadZone.Circular, -0.2f, 0.2f);
            mLookRight    = new GamePadThumbStickInputAction(PlayerIndex.One, InputAction.ButtonAction.Down, InputAction.GamePadThumbStick.Right, InputAction.GamePadThumbStickAxis.X, GamePadDeadZone.Circular, -0.2f, 0.2f);
            
            mMoveForwardKey  = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Down, Keys.W);
            mMoveBackwardKey = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Down, Keys.S);
            mMoveRightKey    = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Down, Keys.D);
            mMoveLeftKey     = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Down, Keys.A);

            mLookForwardMouse  = new MouseMovementInputAction(PlayerIndex.One, InputAction.ButtonAction.Down, InputAction.MouseAxis.Y, 4.0f, 0f, 0f);
            mLookRightMouse    = new MouseMovementInputAction(PlayerIndex.One, InputAction.ButtonAction.Down, InputAction.MouseAxis.X, 4.0f, 0f, 0f);

            mPressPart1 = new GamePadButtonInputAction(PlayerIndex.One, InputAction.ButtonAction.Pressed, Buttons.X);
            mPressPart2 = new GamePadButtonInputAction(PlayerIndex.One, InputAction.ButtonAction.Pressed, Buttons.Y);
            mPressPart3 = new GamePadButtonInputAction(PlayerIndex.One, InputAction.ButtonAction.Pressed, Buttons.B);
            mReleasePart1 = new GamePadButtonInputAction(PlayerIndex.One, InputAction.ButtonAction.Released, Buttons.X);
            mReleasePart2 = new GamePadButtonInputAction(PlayerIndex.One, InputAction.ButtonAction.Released, Buttons.Y);
            mReleasePart3 = new GamePadButtonInputAction(PlayerIndex.One, InputAction.ButtonAction.Released, Buttons.B);
            mPressJump = new GamePadButtonInputAction(PlayerIndex.One, InputAction.ButtonAction.Pressed, Buttons.A);

            mUse = new List<InputAction>(NumParts);
            mUse.Add(mPressPart1);
            mUse.Add(mPressPart2);
            mUse.Add(mPressPart3);
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

            mFinishUse = new List<InputAction>(NumParts);
            mFinishUse.Add(mReleasePart1);
            mFinishUse.Add(mReleasePart2);
            mFinishUse.Add(mReleasePart3);
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

            mJumpKey = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Pressed, Keys.Space);

            mStealPressMouse = new MouseButtonInputAction(PlayerIndex.One, InputAction.ButtonAction.Pressed, InputAction.MouseButton.Left);
            mStealReleaseMouse = new MouseButtonInputAction(PlayerIndex.One, InputAction.ButtonAction.Released, InputAction.MouseButton.Left);

            mStealPress = new GamePadTriggerInputAction(PlayerIndex.One, InputAction.ButtonAction.Pressed, InputAction.GamePadTrigger.Right, GamePadDeadZone.Circular, 0.0f, 0.0f);
            mStealRelease = new GamePadTriggerInputAction(PlayerIndex.One, InputAction.ButtonAction.Released, InputAction.GamePadTrigger.Right, GamePadDeadZone.Circular, 0.0f, 0.0f);

            mPressDecBoneYaw = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Down, Keys.T);
            mPressIncBoneYaw = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Down, Keys.Y);
            mPressDecBonePitch = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Down, Keys.U);
            mPressIncBonePitch = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Down, Keys.I);
            mPressIncBoneRoll = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Down, Keys.O);
            mPressDecBoneRoll = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Down, Keys.P);
            mPressIncBoneIndex = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Pressed, Keys.OemPlus);
            mPressDecBoneIndex = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Pressed, Keys.OemMinus);

            mPressDecTransX = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Down, Keys.F);
            mPressIncTransX = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Down, Keys.G);
            mPressDecTransY = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Down, Keys.H);
            mPressIncTransY = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Down, Keys.J);
            mPressIncTransZ = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Down, Keys.K);
            mPressDecTransZ = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Down, Keys.L);

            mSaveBoneTransforms = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Pressed, Keys.OemQuestion);
        }

        ~PlayerController()
        {
            // TODO: make sure errything is destroyed
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

            mPressPart1.Destroy();
            mPressPart2.Destroy();
            mPressPart3.Destroy();
            mPressJump.Destroy();

            mStealPressMouse.Destroy();
            mStealReleaseMouse.Destroy();
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
            mCamera.TrackTarget = false;

            float elapsedTime = (float)gameTime.ElapsedGameTime.Milliseconds / 1000.0f;

            // Parse movement input.
            bool moveForwardActive = false;
            float moveForwardDegree = 0.0f;
            if (mMoveForward.Active)
            {
                if (mCreature is PlayerCreature)
                {
                    mCamera.TrackTarget = !(mCreature as PlayerCreature).Stealing;
                }
                else
                {
                    mCamera.TrackTarget = true;
                }
                rotationRate = MathHelper.Pi;

                moveForwardActive = true;
                moveForwardDegree = mMoveForward.Degree;
            }
            else if (mMoveForwardKey.Active || mMoveBackwardKey.Active)
            {
                mCamera.TrackTarget = false;
                rotationRate = MathHelper.PiOver2;

                moveForwardActive = true;
                moveForwardDegree = mMoveForwardKey.Degree - mMoveBackwardKey.Degree;
            }

            bool moveRightActive  = false;
            float moveRightDegree = 0.0f;
            if (mMoveRight.Active)
            {
                if (mCreature is PlayerCreature)
                {
                    mCamera.TrackTarget = !(mCreature as PlayerCreature).Stealing;
                }
                else
                {
                    mCamera.TrackTarget = true;
                }
                rotationRate = MathHelper.Pi;

                moveRightActive = true;
                moveRightDegree = mMoveRight.Degree;
            }
            else if (mMoveRightKey.Active || mMoveLeftKey.Active)
            {
                mCamera.TrackTarget = false;
                rotationRate = MathHelper.PiOver2;

                moveRightActive = true;
                moveRightDegree = mMoveRightKey.Degree - mMoveLeftKey.Degree;
            }

            // Moving player.
            Vector2 walkDirection = Vector2.Zero;
            Vector3 forward = mCamera.Forward;
            forward.Y = 0.0f;
            if (forward != Vector3.Zero)
            {
                forward.Normalize();
            }

            if (moveForwardActive || moveRightActive)
            {
                if (mCreature is PlayerCreature)
                {
                    (mCreature as PlayerCreature).Stance = Stance.Walking;
                }

                Vector3 right = mCamera.Right;

                walkDirection += moveForwardDegree * new Vector2(forward.X, forward.Z);
                walkDirection -= moveRightDegree * new Vector2(right.X, right.Z);
            }
            else if (mCreature is PlayerCreature)
            {
                (mCreature as PlayerCreature).Stance = Stance.Standing;
            }

            // Parse look input.
            bool lookForwardActive = false;
            float lookForwardDegree = 0.0f;
            if (mLookForward.Active)
            {
                mCamera.TrackTarget = !(mCreature as PlayerCreature).Stealing;
                rotationRate = MathHelper.Pi;

                lookForwardActive = true;
                lookForwardDegree = mLookForward.Degree;
            }
            else if (mLookForwardMouse.Active)
            {
                mCamera.TrackTarget = false;
                rotationRate = MathHelper.PiOver2;

                lookForwardActive = true;
                lookForwardDegree = -mLookForwardMouse.Degree;
            }

            bool lookRightActive = false;
            float lookRightDegree = 0.0f;
            if (mLookRight.Active)
            {
                mCamera.TrackTarget = !(mCreature as PlayerCreature).Stealing;
                rotationRate = MathHelper.Pi;

                lookRightActive = true;
                lookRightDegree = mLookRight.Degree;
            }
            else if (mLookRightMouse.Active)
            {
                mCamera.TrackTarget = false;
                rotationRate = MathHelper.PiOver2;

                lookRightActive = true;
                lookRightDegree = mLookRightMouse.Degree;
            }

            Vector2 cameraDirection = new Vector2(forward.X, forward.Z);
            Vector2 previousDirection = new Vector2(mCreature.Forward.X, mCreature.Forward.Z);

            // Rotating camera.
            if (lookForwardActive || lookRightActive)
            {
                mCamera.RotateAroundTarget(elapsedTime * rotationRate * lookRightDegree, elapsedTime * rotationRate * lookForwardDegree, 0.0f);
            }
            else if (mCamera.TrackTarget && (moveForwardActive || moveRightActive))
            {
                Vector3 walkDirection3D = new Vector3(walkDirection.X, 0.0f, walkDirection.Y);
                float theta = Vector3.Dot(walkDirection3D, mCamera.Forward);
                if (theta < 0.9f && theta > -0.9f)
                {
                    float direction = (Vector3.Cross(walkDirection3D, mCamera.Forward).Y < 0) ? -1.0f : 1.0f;
                    mCamera.RotateAroundTarget(elapsedTime * (1.0f - theta) * rotationRate * direction, 0.0f, 0.0f);
                }
            }

            if (mCreature.CharacterController.supportData.SupportObject == null && mCreature is PlayerCreature)
            {
                // In the air.
                (mCreature as PlayerCreature).Stance = Stance.Jumping;
            }

            if (!NoControl)
            {
                Vector2 direction2 = Vector2.Zero;
                if (mCreature is PlayerCreature && walkDirection == Vector2.Zero && !(mCreature as PlayerCreature).Stealing)
                {
                    direction2 = previousDirection;
                }
                else
                {
                    direction2 = cameraDirection;
                }

                mCreature.Move(walkDirection, (mCamera.TrackTarget) ? walkDirection : direction2);
            }
        }

        /// <summary>
        /// If player has requested action perform it.
        /// </summary>
        private void PerformActions()
        {
            PlayerCreature player = (mCreature as PlayerCreature);

            if (!NoControl && (mPressJump.Active || mJumpKey.Active))
            {
                mCreature.Jump();
            }

            if (mStealPressMouse.Active || mStealPress.Active)
            {
                if (player != null)
                {
                    player.ActivateStealPart();
                }
            }

            if (mStealReleaseMouse.Active || mStealRelease.Active)
            {
                if (player != null)
                {
                    player.DeactivateStealPart();
                }
            }

            for (int i = 0; i < NumParts; ++i)
            {
                int j = (i >= NumButtons) ? (i - NumButtons) : i;
                if (mUse[i].Active && !NoControl)
                {
                    if (player != null && player.FoundPart)
                    {
                        player.StealPart(j);
                    }
                    else
                    {
                        mCreature.UsePart(j, mCamera.Forward);
                    }
                }
                else if (mFinishUse[i].Active)
                {
                    mCreature.FinishUsePart(j, mCamera.Forward);
                }
            }

            //if (mSaveBoneTransforms.Active)
            //{
            //    //mCreature.WriteBoneTransforms();
            //    if (mCreature.PartAttachments[0] != null)
            //    {
            //        int count = 0;
            //        foreach (Part.SubPart subpart in mCreature.PartAttachments[0].Part.SubParts)
            //        {
            //            Console.WriteLine("Orientation for subPart: " + count++);
            //            Console.WriteLine("yaw: " + subpart.Yaw);
            //            Console.WriteLine("pitch: " + subpart.Pitch);
            //            Console.WriteLine("roll: " + subpart.Roll);
            //        }
            //    }
            //}

            //if (mPressDecTransX.Active)
            //{
            //    Matrix translation = Matrix.CreateTranslation(mCreature.BoneForward * MathHelper.Pi / 500.0f);
            //    mCreature.BoneRotations *= translation;
            //}
            //else if (mPressIncTransX.Active)
            //{
            //    Matrix translation = Matrix.CreateTranslation(mCreature.BoneForward * -MathHelper.Pi / 500.0f);
            //    mCreature.BoneRotations *= translation;
            //}

            //if (mPressIncBoneRoll.Active)
            //{
            //    //Matrix rotation = Matrix.CreateFromAxisAngle(mCreature.BoneForward, MathHelper.Pi / 500.0f);
            //    //mCreature.BoneRotations *= rotation;
            //    //mCreature.BoneRight = Vector3.Transform(mCreature.BoneRight, rotation);
            //    //mCreature.BoneUp = Vector3.Transform(mCreature.BoneUp, rotation);
            //    if (mCreature.PartAttachments[0] != null)
            //    {
            //        Part.SubPart subpart = mCreature.PartAttachments[0].Part.SubParts[mBoneIndex];
            //        subpart.Roll += MathHelper.Pi / 75.0f;
            //    }
            //}
            //else if (mPressDecBoneRoll.Active)
            //{
            //    //Matrix rotation = Matrix.CreateFromAxisAngle(mCreature.BoneForward, -MathHelper.Pi / 500.0f);
            //    //mCreature.BoneRotations *= rotation;
            //    //mCreature.BoneRight = Vector3.Transform(mCreature.BoneRight, rotation);
            //    //mCreature.BoneUp = Vector3.Transform(mCreature.BoneUp, rotation);
            //    if (mCreature.PartAttachments[0] != null)
            //    {
            //        Part.SubPart subpart = mCreature.PartAttachments[0].Part.SubParts[mBoneIndex];
            //        subpart.Roll -= MathHelper.Pi / 75.0f;
            //    }
            //}

            //if (mPressDecTransY.Active)
            //{
            //    Matrix translation = Matrix.CreateTranslation(mCreature.BoneUp * MathHelper.Pi / 500.0f);
            //    mCreature.BoneRotations *= translation;
            //}
            //else if (mPressIncTransY.Active)
            //{
            //    Matrix translation = Matrix.CreateTranslation(mCreature.BoneUp * -MathHelper.Pi / 500.0f);
            //    mCreature.BoneRotations *= translation;
            //}

            //if (mPressIncBoneYaw.Active)
            //{
            //    //Matrix rotation = Matrix.CreateFromAxisAngle(mCreature.BoneUp, MathHelper.Pi / 500.0f);
            //    //mCreature.BoneRotations *= rotation;
            //    //mCreature.BoneRight = Vector3.Transform(mCreature.BoneRight, rotation);
            //    //mCreature.BoneForward = Vector3.Transform(mCreature.BoneForward, rotation);
            //    if (mCreature.PartAttachments[0] != null)
            //    {
            //        Part.SubPart subpart = mCreature.PartAttachments[0].Part.SubParts[mBoneIndex];
            //        subpart.Yaw += MathHelper.Pi / 75.0f;
            //    }
            //}
            //else if (mPressDecBoneYaw.Active)
            //{
            //    //Matrix rotation = Matrix.CreateFromAxisAngle(mCreature.BoneUp, -MathHelper.Pi / 500.0f);
            //    //mCreature.BoneRotations *= rotation;
            //    //mCreature.BoneRight = Vector3.Transform(mCreature.BoneRight, rotation);
            //    //mCreature.BoneForward = Vector3.Transform(mCreature.BoneForward, rotation);
            //    if (mCreature.PartAttachments[0] != null)
            //    {
            //        Part.SubPart subpart = mCreature.PartAttachments[0].Part.SubParts[mBoneIndex];
            //        subpart.Yaw -= MathHelper.Pi / 75.0f;
            //    }
            //}

            //if (mPressDecTransZ.Active)
            //{
            //    Matrix translation = Matrix.CreateTranslation(mCreature.BoneRight * MathHelper.Pi / 500.0f);
            //    mCreature.BoneRotations *= translation;
            //}
            //else if (mPressIncTransZ.Active)
            //{
            //    Matrix translation = Matrix.CreateTranslation(mCreature.BoneRight * -MathHelper.Pi / 500.0f);
            //    mCreature.BoneRotations *= translation;
            //}

            //if (mPressIncBonePitch.Active)
            //{
            //    //Matrix rotation = Matrix.CreateFromAxisAngle(mCreature.BoneRight, MathHelper.Pi / 500.0f);
            //    //mCreature.BoneRotations *= rotation;
            //    //mCreature.BoneUp = Vector3.Transform(mCreature.BoneUp, rotation);
            //    //mCreature.BoneForward = Vector3.Transform(mCreature.BoneForward, rotation);
            //    if (mCreature.PartAttachments[0] != null)
            //    {
            //        Part.SubPart subpart = mCreature.PartAttachments[0].Part.SubParts[mBoneIndex];
            //        subpart.Pitch += MathHelper.Pi / 75.0f;
            //    }
            //}
            //else if (mPressDecBonePitch.Active)
            //{
            //    //Matrix rotation = Matrix.CreateFromAxisAngle(mCreature.BoneRight, -MathHelper.Pi / 500.0f);
            //    //mCreature.BoneRotations *= rotation;
            //    //mCreature.BoneUp = Vector3.Transform(mCreature.BoneUp, rotation);
            //    //mCreature.BoneForward = Vector3.Transform(mCreature.BoneForward, rotation);
            //    if (mCreature.PartAttachments[0] != null)
            //    {
            //        Part.SubPart subpart = mCreature.PartAttachments[0].Part.SubParts[mBoneIndex];
            //        subpart.Pitch -= MathHelper.Pi / 75.0f;
            //    }
            //}

            //if (mPressIncBoneIndex.Active)
            //{
            //    if (mCreature.PartAttachments[0] != null)
            //    {
            //        mBoneIndex++;
            //        if (mBoneIndex >= mCreature.PartAttachments[0].Part.SubParts.Length)
            //        {
            //            mBoneIndex = 0;
            //        }
            //    }
            //    //mCreature.BoneIndex++;
            //}
            //else if (mPressDecBoneIndex.Active)
            //{
            //    if (mCreature.PartAttachments[0] != null)
            //    {
            //        mBoneIndex--;
            //        if (mBoneIndex < 0)
            //        {
            //            mBoneIndex = mCreature.PartAttachments[0].Part.SubParts.Length - 1;
            //        }
            //    }
            //    //mCreature.BoneIndex--;
            //}
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
