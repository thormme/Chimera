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

namespace WorldEditor
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

        private UIModel mXPositionArm = new UIModel("translateGizmo");
        private UIModel mYPositionArm = new UIModel("translateGizmo");
        private UIModel mZPositionArm = new UIModel("translateGizmo");
        private UIModel mXScaleArm = new UIModel("scaleGizmo");
        private UIModel mYScaleArm = new UIModel("scaleGizmo");
        private UIModel mZScaleArm = new UIModel("scaleGizmo");
        private UIModel mYawArm = new UIModel("rotateGizmo");
        private UIModel mPitchArm = new UIModel("rotateGizmo");
        private UIModel mRollArm = new UIModel("rotateGizmo");

        private FPSCamera mCamera = null;
        private Viewport mViewport;
        private Vector3 Position = Vector3.Zero;
        private bool mDrawable = false;
        private Controls mControls = null;
        private Vector3 mStartDragPoint = Vector3.Zero;
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
                List<uint> objectIDs = GraphicsManager.GetPickingScreenObjects(mousePixelRectangle);
                uint mousePixel = objectIDs.Count > 0 ? objectIDs[0] : 0;
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
                mStartDragPoint = GetMouseWorldPosition();
            }

            if (mIsDragging)
            { 
                

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
            if (mDrawable)
            {
                Vector3 scale = Vector3.One * 100;

                if (Mode == ModificationMode.Position)
                {
                    mXPositionArm.Render(Position + Vector3.UnitX * 20, Matrix.CreateRotationZ(-MathHelper.PiOver2), scale, Color.Red,   1.0f, false);
                    mYPositionArm.Render(Position + Vector3.UnitY * 20, Matrix.Identity,                             scale, Color.Green, 1.0f, false);
                    mZPositionArm.Render(Position + Vector3.UnitZ * 20, Matrix.CreateRotationX(MathHelper.PiOver2),  scale, Color.Blue,  1.0f, false);
                }
                else if (Mode == ModificationMode.Scale)
                {
                    mXScaleArm.Render(Position + Vector3.UnitX * 20, Matrix.CreateRotationZ(-MathHelper.PiOver2), scale, Color.Red,   1.0f, false);
                    mYScaleArm.Render(Position + Vector3.UnitY * 20, Matrix.Identity,                             scale, Color.Green, 1.0f, false);
                    mZScaleArm.Render(Position + Vector3.UnitZ * 20, Matrix.CreateRotationX(MathHelper.PiOver2),  scale, Color.Blue,  1.0f, false);
                }

                mPitchArm.Render(Position, Matrix.CreateRotationZ(MathHelper.PiOver2), scale, Color.Red, 1.0f, false);
                mYawArm.Render(Position,   Matrix.Identity,                            scale, Color.Green, 1.0f, false);
                mRollArm.Render(Position,  Matrix.CreateRotationX(MathHelper.PiOver2), scale, Color.Blue, 1.0f, false);
            }
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

        private Vector3 GetRotationPlaneNormal()
        {
            switch (mDragDirection)
            {
                case ModificationDirection.Yaw:
                    return Vector3.UnitY;
                case ModificationDirection.Pitch:
                    return Vector3.UnitX;
                case ModificationDirection.Roll:
                    return Vector3.UnitZ;
                default:
                    return Vector3.Zero;
            }
        }

        private Vector3 GetMouseWorldPosition()
        {
            if (mDragDirection == ModificationDirection.X || mDragDirection == ModificationDirection.Y || mDragDirection == ModificationDirection.Z)
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
                Vector3 worldPosition = Utils.ProjectVectorOntoPlane(
                    Utils.CreateWorldRayFromScreenPoint(
                        new Vector2(mControls.MouseState.X, mControls.MouseState.Y),
                        mViewport,
                        mCamera.Position,
                        mCamera.ViewTransform,
                        mCamera.ProjectionTransform),
                    Position,
                    pickingPlane.Normal);
                return worldPosition;
            }
            else
            {
                Vector3 worldPosition = Utils.ProjectVectorOntoPlane(
                    Utils.CreateWorldRayFromScreenPoint(
                        new Vector2(mControls.MouseState.X, mControls.MouseState.Y),
                        mViewport,
                        mCamera.Position,
                        mCamera.ViewTransform,
                        mCamera.ProjectionTransform),
                    Position,
                    GetRotationPlaneNormal());
                return worldPosition;
            }
        }
    }
}
