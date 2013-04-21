using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameConstructLibrary;
using GraphicsLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Utility;
using WorldEditor;

namespace MapEditor
{
    public class ObjectModificationGizmo
    {
        const uint mXPickingID = 16777100;
        const uint mYPickingID = 16777101;
        const uint mZPickingID = 16777102;
        const uint mYawPickingID   = 16777110;
        const uint mPitchPickingID = 16777111;
        const uint mRollPickingID  = 16777112;

        public enum ModificationMode { Position, Scale, Rotate };

        private enum ModificationDirection { X, Y, Z, Yaw, Pitch, Roll };

        private InanimateModel mXPositionArm = new InanimateModel("box");
        private InanimateModel mYPositionArm = new InanimateModel("box");
        private InanimateModel mZPositionArm = new InanimateModel("box");
        private InanimateModel mXScaleArm = new InanimateModel("box");
        private InanimateModel mYScaleArm = new InanimateModel("box");
        private InanimateModel mZScaleArm = new InanimateModel("box");
        private InanimateModel mYawArm = new InanimateModel("box");
        private InanimateModel mPitchArm = new InanimateModel("box");
        private InanimateModel mRollArm = new InanimateModel("box");

        private FPSCamera mCamera = null;
        private Viewport mViewport;
        private Vector3 Position = Vector3.Zero;
        private bool mDrawable = false;
        private Controls mControls = null;
        private Point mStartDragPoint = Point.Zero;
        private bool mIsDragging = false;
        private ModificationMode mDragMode = ModificationMode.Position;
        private ModificationDirection mDragDirection = ModificationDirection.X;

        public ModificationMode Mode = ModificationMode.Position;

        public ObjectModificationGizmo(Controls controls, FPSCamera camera, Viewport viewport)
        {
            mControls = controls;
            mCamera = camera;
            mViewport = viewport;

            mXPositionArm.ObjectID = mXPickingID;
            mYPositionArm.ObjectID = mYPickingID;
            mZPositionArm.ObjectID = mZPickingID;

            mXScaleArm.ObjectID = mXPickingID;
            mYScaleArm.ObjectID = mYPickingID;
            mZScaleArm.ObjectID = mZPickingID;

            mYawArm.ObjectID = mYawPickingID;
            mPitchArm.ObjectID = mPitchPickingID;
            mRollArm.ObjectID = mRollPickingID;
        }

        public void Update(List<DummyObject> selectedObjects)
        {
            if (mControls.LeftPressed.Active)
            {
                Rectangle mousePixelRectangle = new Rectangle(mControls.MouseState.X, mControls.MouseState.Y, 1, 1);
                uint mousePixel = GraphicsManager.GetPickingScreenObjects(mousePixelRectangle)[0];
                if (mousePixel == mXPickingID || mousePixel == mYPickingID || mousePixel == mZPickingID)
                {
                    mDragMode = Mode;
                    mIsDragging = true;
                }
                else if (mousePixel == mYawPickingID || mousePixel == mPitchPickingID || mousePixel == mRollPickingID)
                {
                    mDragMode = ModificationMode.Rotate;
                    mIsDragging = true;
                }
                else
                {
                    mIsDragging = false;
                }
                switch (mousePixel)
                {
                    case mXPickingID:
                        mDragDirection = ModificationDirection.X;
                        break;
                    case mYPickingID:
                        mDragDirection = ModificationDirection.Y;
                        break;
                    case mZPickingID:
                        mDragDirection = ModificationDirection.Z;
                        break;
                    case mYawPickingID:
                        mDragDirection = ModificationDirection.Yaw;
                        break;
                    case mPitchPickingID:
                        mDragDirection = ModificationDirection.Pitch;
                        break;
                    case mRollPickingID:
                        mDragDirection = ModificationDirection.Roll;
                        break;
                }
                mStartDragPoint = mousePixelRectangle.Location;
            }

            if (mIsDragging)
            { 
                Vector3 dragDirection = GetDragDirection();
                Plane perpendicularPlane = new Plane(mCamera.Position, Position + dragDirection, Position);
                Plane pickingPlane = new Plane(Position, Position + dragDirection, Position + perpendicularPlane.Normal);
                Vector2 rayPosition;
                Vector2 rayDirection;
                Utils.ProjectRayToScreenSpace(new Ray(Position, dragDirection),
                    mViewport, mCamera.ViewTransform,
                    mCamera.ProjectionTransform,
                    out rayPosition,
                    out rayDirection);
                Vector2 screenPoint = Utils.NearestPointOnLine(
                    rayPosition,
                    rayDirection,
                    new Vector2(mControls.MouseState.X, mControls.MouseState.Y));
                Utils.ProjectVectorOntoPlane(
                    Utils.CreateWorldRayFromScreenPoint(
                        new Vector2(mControls.MouseState.X, mControls.MouseState.Y),
                        mViewport,
                        mCamera.Position,
                        mCamera.ViewTransform,
                        mCamera.ProjectionTransform),
                    Position,
                    pickingPlane.Normal);

                switch (mDragMode)
                {
                    case ModificationMode.Position:

                        break;
                    case ModificationMode.Scale:

                        break;
                    case ModificationMode.Rotate:

                        break;
                }
            }

            Position = Vector3.Zero;
            foreach (DummyObject dummyObject in selectedObjects)
            {
                Position += dummyObject.Position;
            }
            if (selectedObjects.Count > 0)
            {
                mDrawable = true;
                Position /= selectedObjects.Count;
            }
            else
            {
                mDrawable = false;
            }
        }

        public void Draw()
        {
            Vector3 scale = Vector3.One;

            if (Mode == ModificationMode.Position)
            {
                mXPositionArm.Render(Position, Vector3.UnitX, scale, Color.Red, 1.0f);
                mYPositionArm.Render(Position, Vector3.UnitY, scale, Color.Blue, 1.0f);
                mZPositionArm.Render(Position, Vector3.UnitZ, scale, Color.Green, 1.0f);
            }
            else if (Mode == ModificationMode.Scale)
            {
                mXScaleArm.Render(Position, Vector3.UnitX, scale, Color.Red, 1.0f);
                mYScaleArm.Render(Position, Vector3.UnitY, scale, Color.Blue, 1.0f);
                mZScaleArm.Render(Position, Vector3.UnitZ, scale, Color.Green, 1.0f);
            }

            mYawArm.Render(Position, Vector3.UnitX, scale, Color.Red, 1.0f);
            mPitchArm.Render(Position, Vector3.UnitY, scale, Color.Blue, 1.0f);
            mRollArm.Render(Position, Vector3.UnitZ, scale, Color.Green, 1.0f);
        }

        private Vector3 GetDragDirection()
        {
            switch (mDragDirection)
            {
                case ModificationDirection.X:
                    return Vector3.UnitX;
                case ModificationDirection.Y:
                    return Vector3.UnitY;
                case ModificationDirection.Z:
                    return Vector3.UnitZ;
                default:
                    return Vector3.Zero;
            }
        }
    }
}
