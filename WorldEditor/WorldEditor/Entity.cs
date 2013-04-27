using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GameConstructLibrary;
using BEPUphysics;
using Microsoft.Xna.Framework.Graphics;
using GraphicsLibrary;
using System.Windows.Forms;
using Utility;

namespace WorldEditor
{
    public class Entity
    {
        private const float SpeedMinimum = 0.0025f;
        private const float SpeedAcceleration = 0.05f;
        private const float Sensitivity = 0.175f;

        GraphicsDevice mGraphicsDevice = null;
        private FPSCamera mCamera = null;
        private Controls mControls = null;
        private Vector2 mDragPoint = Vector2.Zero;

        private Vector3 mMovement = Vector3.Zero;
        private Vector3 mDirection = Vector3.Zero;
        private float mSpeed = 0.05f;

        private Rectangle SelectionRectangle
        {
            get
            {
                return new Rectangle(
                        (int)Math.Min(mControls.MouseState.X, mDragPoint.X) - mGraphicsDevice.Viewport.X,
                        (int)Math.Min(mControls.MouseState.Y, mDragPoint.Y) - mGraphicsDevice.Viewport.Y,
                        (int)Math.Max(Math.Abs(mDragPoint.X - mControls.MouseState.X), 1),
                        (int)Math.Max(Math.Abs(mDragPoint.Y - mControls.MouseState.Y), 1));
            }
        }

        public Entity(GraphicsDevice graphicsDevice, Controls controls, FPSCamera camera)
        {
            mGraphicsDevice = graphicsDevice;
            mControls = controls;
            mCamera = camera;
        }

        public void Update(GameTime gameTime)
        {
            UpdateMovement(gameTime);
            UpdateDirection();
            UpdateCamera(gameTime);
        }

        private void UpdateMovement(GameTime gameTime)
        {
            mSpeed += SpeedAcceleration * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (!(mControls.Right.Active || mControls.Left.Active || mControls.Forward.Active || mControls.Backward.Active))
            {
                mSpeed = SpeedMinimum;
            }
            mMovement.X = -(float)(mControls.Right.Degree - mControls.Left.Degree);
            mMovement.Y = (float)(mControls.Forward.Degree - mControls.Backward.Degree);
        }

        private void UpdateDirection()
        {
            if (mControls.LeftPressed.Active)
            {
                UpdateDragPoint();
            }
            if (mControls.RightPressed.Active)
            {
                UpdateDragPoint();
            }
            else if (mControls.RightHold.Active)
            {
                mDirection.X = -MathHelper.ToRadians((mControls.MouseState.X - mDragPoint.X) * Sensitivity);
                mDirection.Y = MathHelper.ToRadians((mControls.MouseState.Y - mDragPoint.Y) * Sensitivity);
                UpdateDragPoint();
            }
            else if (mControls.RightReleased.Active)
            {
                mDirection = Vector3.Zero;
            }
        }

        private void UpdateCamera(GameTime gameTime)
        {
            mCamera.Move(mSpeed * mMovement.Y * gameTime.ElapsedGameTime.Milliseconds, mSpeed * mMovement.X * gameTime.ElapsedGameTime.Milliseconds, 0.0f);
            
            // Rotate horizontally
            mCamera.RotateAroundSelf(mDirection.X, 0.0f, 0.0f);

            // Get the normalized vector along the x/z plane
            Vector3 startForward = mCamera.Forward;
            startForward.Y = 0;
            startForward.Normalize();

            // Rotate vertically
            mCamera.RotateAroundSelf(0.0f, mDirection.Y, 0.0f);

            // Get the normalized vector along the x/z plane again
            Vector3 rotForward = mCamera.Forward;
            rotForward.Y = 0;
            rotForward.Normalize();

            // Check if they are different, if so you've passed
            // the top or bottom of the rotation range.
            if ((rotForward - startForward).Length() > .5)
            {
                mCamera.RotateAroundSelf(0.0f, -mDirection.Y, 0.0f);
            }
        }

        private void UpdateDragPoint()
        {
            mDragPoint.X = mControls.MouseState.X;
            mDragPoint.Y = mControls.MouseState.Y;
        }

        public List<DummyObject> GetObjectsInSelection(DummyWorld dummyWorld)
        {
            return dummyWorld.GetDummyObjectFromID(
                GraphicsManager.GetPickingScreenObjects(SelectionRectangle).ToArray());
        }

        public void HighlightObjectsInSelection()
        {
            GraphicsManager.HighlightSelection(SelectionRectangle);
        }

        public Tuple<RayHit, DummyObject> GetPickingLocation(DummyWorld dummyWorld, Form gameForm)
        {
            Ray ray = Utils.CreateWorldRayFromScreenPoint(
                new Vector2(mControls.MouseState.X, mControls.MouseState.Y),
                mGraphicsDevice.Viewport,
                mCamera.Position,
                mCamera.ViewTransform,
                mCamera.ProjectionTransform);
            FPSCamera cam =  new FPSCamera(new Viewport(0, 0, 1, 1));
            cam.Position = mCamera.Position;

            Tuple<RayHit, DummyObject> castResult;
            if (dummyWorld.RayCast(ray, 2000.0f, out castResult))
            {
                return castResult;
            }
            return null;
        }
    }
}
