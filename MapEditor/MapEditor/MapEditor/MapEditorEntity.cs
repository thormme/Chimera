using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameConstructLibrary;
using GraphicsLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using BEPUphysics;
using BEPUphysics.CollisionShapes;
using BEPUphysics.Collidables;
using BEPUphysics.CollisionShapes.ConvexShapes;

namespace MapEditor
{
    public class MapEditorEntity
    {

        private const float speed = 0.5f;
        private const float sensitivity = 0.1f;
        private const float length = 2000.0f;

        private KeyInputAction mForward;
        private KeyInputAction mBackward;
        private KeyInputAction mLeft;
        private KeyInputAction mRight;
        private KeyInputAction mDown;
        private KeyInputAction mUp;

        private MouseState mMouse;
        private Vector2 mMouseClick;
        private MouseButtonInputAction mRightPressed;
        private MouseButtonInputAction mRightHold;
        private MouseButtonInputAction mRightReleased;

        private Vector3 mMovement;
        private Vector3 mDirection;
        
        private DummyObject mDummy;

        public MapEditorEntity()
        {

            mForward = new KeyInputAction(0, InputAction.ButtonAction.Down, Keys.W);
            mBackward = new KeyInputAction(0, InputAction.ButtonAction.Down, Keys.S);
            mLeft = new KeyInputAction(0, InputAction.ButtonAction.Down, Keys.A);
            mRight = new KeyInputAction(0, InputAction.ButtonAction.Down, Keys.D);
            mDown = new KeyInputAction(0, InputAction.ButtonAction.Down, Keys.Q);
            mUp = new KeyInputAction(0, InputAction.ButtonAction.Down, Keys.E);

            mMouseClick = new Vector2(0.0f, 0.0f);

            mRightPressed = new MouseButtonInputAction(0, InputAction.ButtonAction.Pressed, MouseButtonInputAction.MouseButton.Right);
            mRightHold = new MouseButtonInputAction(0, InputAction.ButtonAction.Down, MouseButtonInputAction.MouseButton.Right);
            mRightReleased = new MouseButtonInputAction(0, InputAction.ButtonAction.Released, MouseButtonInputAction.MouseButton.Right);

            mMovement = new Vector3(0.0f, 0.0f, 0.0f);
            mDirection = new Vector3(0.0f, 0.0f, 0.0f);

            mDummy = new DummyObject();
            mDummy.Model = "editor";
            mDummy.Position = new Vector3(0.0f, 0.0f, 0.0f);
            mDummy.Scale = new Vector3(1.0f, 1.0f, 1.0f);
            mDummy.YawPitchRoll = new Vector3(0.0f, 0.0f, 0.0f);

        }

        public void Update(GameTime gameTime)
        {
            InputAction.Update();
            mMouse = Mouse.GetState();

            UpdatePosition();
            UpdateOrientation();
            UpdatePicking();

            GameMapEditor.Camera.Move(speed * mMovement.Y * gameTime.ElapsedGameTime.Milliseconds, speed * mMovement.X * gameTime.ElapsedGameTime.Milliseconds, 0.0f);
            GameMapEditor.Camera.RotateAroundSelf(sensitivity * mDirection.X * gameTime.ElapsedGameTime.Milliseconds, sensitivity * mDirection.Y * gameTime.ElapsedGameTime.Milliseconds, 0.0f);

        }

        private void UpdatePosition()
        {
            mMovement.Z = (float)(mUp.Degree - mDown.Degree);
            mMovement.Y = (float)(mForward.Degree - mBackward.Degree);
            mMovement.X = -(float)(mRight.Degree - mLeft.Degree);
        }

        private void UpdateOrientation()
        {
            if (mRightPressed.Active == true)
            {
                mMouseClick.Y = mMouse.Y;
                mMouseClick.X = mMouse.X;
            }

            if (mRightHold.Active == true)
            {
                mDirection.Y = MathHelper.ToRadians((mMouse.Y - mMouseClick.Y) * sensitivity); // pitch
                mDirection.X = -MathHelper.ToRadians((mMouse.X - mMouseClick.X) * sensitivity); // yaw
                mMouseClick.Y = mMouse.Y;
                mMouseClick.X = mMouse.X;
            }

            if (mRightReleased.Active == true)
            {
                mDirection.Y = 0;
                mDirection.X = 0;
            }
        }

        private void UpdatePicking()
        {

            Vector3 nearScreen = new Vector3(mMouse.X, mMouse.Y, 0.0f);
            Vector3 farScreen = new Vector3(mMouse.X, mMouse.Y, 1.0f);
            Vector3 nearWorld = GameMapEditor.Viewport.Unproject(nearScreen, GameMapEditor.Camera.ProjectionTransform, GameMapEditor.Camera.ViewTransform, Matrix.Identity);
            Vector3 farWorld = GameMapEditor.Viewport.Unproject(farScreen, GameMapEditor.Camera.ProjectionTransform, GameMapEditor.Camera.ViewTransform, Matrix.Identity);
            Vector3 projectionDirection = (farWorld - nearWorld);
            projectionDirection.Normalize();
            Ray ray = new Ray(GameMapEditor.Camera.Position, projectionDirection);
            RayHit result;
            
            if (GameMapEditor.Map.TerrainPhysics.StaticCollidable.RayCast(ray, length, out result))
            {
                GameMapEditor.Placeable = true;
                GameMapEditor.Position = result.Location;

                mDummy.Position = new Vector3(result.Location.X, result.Location.Y, result.Location.Z);

            }
            else
            {
                GameMapEditor.Placeable = false;
                GameMapEditor.Position = new Vector3(0.0f, 10000.0f, 0.0f);
            }

            if (GameMapEditor.EditMode == Edit.None) mDummy.Position = new Vector3(0.0f, 10000.0f, 0.0f);

        }

        public void SetModel(DummyObject obj)
        {
            mDummy = obj;
        }

        public void Render()
        {
            InanimateModel temp = new InanimateModel(mDummy.Model);
            temp.Render(new Vector3(mDummy.Position.X, mDummy.Position.Y + mDummy.Height * GameMapEditor.MapScale.Y, mDummy.Position.Z), Matrix.CreateFromYawPitchRoll(mDummy.YawPitchRoll.X, mDummy.YawPitchRoll.Y, mDummy.YawPitchRoll.Z), mDummy.Scale);
        }

    }
}
