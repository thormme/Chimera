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

        private Matrix mYawArmBaseRotation   = Matrix.CreateRotationY(MathHelper.Pi);
        private Matrix mPitchArmBaseRotation = Matrix.CreateRotationY(MathHelper.PiOver2) * Matrix.CreateRotationZ(-MathHelper.PiOver2);
        private Matrix mRollArmBaseRotation = Matrix.CreateRotationY(-MathHelper.PiOver2) * Matrix.CreateRotationX(MathHelper.PiOver2);

        public enum ModificationMode { TRANSLATE, SCALE, ROTATE };

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
        private Vector3 Position = Vector3.Zero;
        private bool mDrawable = false;
        private Controls mControls = null;
        private Vector3 mStartDragPoint = Vector3.Zero;
        private Vector3 mPreviousDragPoint = Vector3.Zero;
        private ModificationMode mDragMode = ModificationMode.TRANSLATE;
        private ModificationDirection mDragDirection = ModificationDirection.X;

        public ModificationMode Mode = ModificationMode.TRANSLATE;
        public bool IsDragging
        {
            get;
            protected set;
        }

        public ObjectModificationGizmo(Controls controls, FPSCamera camera)
        {
            mControls = controls;
            mCamera = camera;

            mXPositionArm.ObjectID = mXPickingID;
            mYPositionArm.ObjectID = mYPickingID;
            mZPositionArm.ObjectID = mZPickingID;

            mXScaleArm.ObjectID = mXPickingID;
            mYScaleArm.ObjectID = mYPickingID;
            mZScaleArm.ObjectID = mZPickingID;

            mYawArm.ObjectID = mYawPickingID;
            mPitchArm.ObjectID = mPitchPickingID;
            mRollArm.ObjectID = mRollPickingID;

            IsDragging = false;
        }

        public void Update(List<DummyObject> selectedObjects, Viewport viewport)
        {
            if (mControls.LeftPressed.Active)
            {
                Rectangle mousePixelRectangle = new Rectangle(mControls.MouseState.X, mControls.MouseState.Y, 1, 1);
                List<uint> objectIDs = GraphicsManager.GetPickingScreenObjects(mousePixelRectangle);
                uint mousePixel = objectIDs.Count > 0 ? objectIDs[0] : 0;
                if (mousePixel == mXPickingID || mousePixel == mYPickingID || mousePixel == mZPickingID)
                {
                    mDragMode = Mode;
                    IsDragging = true;
                }
                else if (mousePixel == mYawPickingID || mousePixel == mPitchPickingID || mousePixel == mRollPickingID)
                {
                    mDragMode = ModificationMode.ROTATE;
                    IsDragging = true;
                }
                else
                {
                    IsDragging = false;
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
                mStartDragPoint = GetMouseWorldPosition(viewport);
                mPreviousDragPoint = mStartDragPoint;
            }
            else if (mControls.LeftReleased.Active)
            {
                IsDragging = false;
            }

            if (IsDragging)
            {
                Vector3 newDragPoint = GetMouseWorldPosition(viewport);
                switch (mDragMode)
                {
                    case ModificationMode.TRANSLATE:
                        foreach (DummyObject dummyObject in selectedObjects)
                        {
                            dummyObject.Position += (newDragPoint - mPreviousDragPoint) * GetDragDirection();
                        }
                        break;
                    case ModificationMode.SCALE:
                        float scaleMultiplier = (newDragPoint - Position).Length() / (mPreviousDragPoint - Position).Length();
                        foreach (DummyObject dummyObject in selectedObjects)
                        {
                            dummyObject.Scale = dummyObject.Scale * scaleMultiplier * GetDragDirection() + (dummyObject.Scale * (Vector3.One - GetDragDirection()));
                        }
                        break;
                    case ModificationMode.ROTATE:
                        Vector3 axis = GetRotationPlaneNormal();
                        Vector3 newDirection = Vector3.Normalize(newDragPoint - Position);
                        Vector3 previousDirection = Vector3.Normalize(mPreviousDragPoint - Position);

                        float dot = Math.Min(1.0f, Math.Max(-1.0f, Vector3.Dot(newDirection, previousDirection)));
                        float angle = (float)Math.Acos(dot);
                        angle *= Vector3.Dot(Vector3.Cross(newDirection, previousDirection), axis) < 0.0f ? 1.0f : -1.0f;
                        
                        foreach (DummyObject dummyObject in selectedObjects)
                        {
                            Vector3 dummyOffset = dummyObject.Position - Position;
                            if (dummyOffset != Vector3.Zero)
                            {
                                Matrix rotation = Matrix.CreateFromAxisAngle(axis, angle);
                                Vector3 rotatedOffset = Vector3.Transform(dummyOffset, rotation);
                                dummyObject.Position = Position + Vector3.Normalize(rotatedOffset) * dummyOffset.Length();
                            }

                            Vector3 deltaYawPitchRoll = axis * angle;
                            dummyObject.YawPitchRoll += deltaYawPitchRoll;
                            dummyObject.YawPitchRoll = new Vector3(dummyObject.YawPitchRoll.X % MathHelper.TwoPi, dummyObject.YawPitchRoll.Y % MathHelper.TwoPi, dummyObject.YawPitchRoll.Z % MathHelper.TwoPi);
                        }
                        break;
                }
                mPreviousDragPoint = newDragPoint;
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
                float scaleMagnitude = (mCamera.Position - Position).Length() / 2.0f;
                Vector3 scale = Vector3.One * scaleMagnitude;

                switch (Mode)
                {
                    case ModificationMode.TRANSLATE:
                    {
                        mXPositionArm.Render(Position + Vector3.UnitX * scaleMagnitude / 5, Matrix.CreateRotationZ(-MathHelper.PiOver2), scale, Color.Red, 1.0f, false);
                        mYPositionArm.Render(Position + Vector3.UnitY * scaleMagnitude / 5, Matrix.Identity, scale, Color.Green, 1.0f, false);
                        mZPositionArm.Render(Position + Vector3.UnitZ * scaleMagnitude / 5, Matrix.CreateRotationX(MathHelper.PiOver2), scale, Color.Blue, 1.0f, false);
                        break;
                    }
                    case ModificationMode.SCALE:
                    {
                        mXScaleArm.Render(Position + Vector3.UnitX * scaleMagnitude / 5, Matrix.CreateRotationZ(-MathHelper.PiOver2), scale, Color.Red, 1.0f, false);
                        mYScaleArm.Render(Position + Vector3.UnitY * scaleMagnitude / 5, Matrix.Identity, scale, Color.Green, 1.0f, false);
                        mZScaleArm.Render(Position + Vector3.UnitZ * scaleMagnitude / 5, Matrix.CreateRotationX(MathHelper.PiOver2), scale, Color.Blue, 1.0f, false);
                        break;
                    }
                    case ModificationMode.ROTATE:
                    {
                        Vector3 gizmoToCamera = mCamera.Position - Position;
                        gizmoToCamera = Vector3.Normalize(new Vector3(
                            gizmoToCamera.X /= Math.Abs(gizmoToCamera.X),
                            gizmoToCamera.Y /= Math.Abs(gizmoToCamera.Y),
                            gizmoToCamera.Z /= Math.Abs(gizmoToCamera.Z)));

                        Vector3 pitchGizmoToCamera = Vector3.Normalize(new Vector3(0.0f, gizmoToCamera.Y, gizmoToCamera.Z));
                        Vector3 pitchDefault = Vector3.Normalize(new Vector3(0.0f, 1.0f, 1.0f));

                        Vector3 yawGizmoToCamera = Vector3.Normalize(new Vector3(gizmoToCamera.X, 0.0f, gizmoToCamera.Z));
                        Vector3 yawDefault = Vector3.Normalize(new Vector3(1.0f, 0.0f, 1.0f));

                        Vector3 rollGizmoToCamera = Vector3.Normalize(new Vector3(gizmoToCamera.X, gizmoToCamera.Y, 0.0f));
                        Vector3 rollDefault = Vector3.Normalize(new Vector3(1.0f, 1.0f, 0.0f));

                        float pitchAngle = (float)Math.Acos(Vector3.Dot(pitchGizmoToCamera, pitchDefault));
                        if (Vector3.Cross(pitchGizmoToCamera, pitchDefault).X >= 0.0f)
                        {
                            pitchAngle *= -1.0f;
                        }

                        float yawAngle = (float)Math.Acos(Vector3.Dot(yawGizmoToCamera, yawDefault));
                        if (Vector3.Cross(yawGizmoToCamera, yawDefault).Y >= 0.0f)
                        {
                            yawAngle *= -1.0f;
                        }

                        float rollAngle = (float)Math.Acos(Vector3.Dot(rollGizmoToCamera, rollDefault));
                        if (Vector3.Cross(rollGizmoToCamera, rollDefault).Z >= 0.0f)
                        {
                            rollAngle *= -1.0f;
                        }

                        mPitchArm.Render(Position, mPitchArmBaseRotation * Matrix.CreateRotationX(pitchAngle), scale, Color.Red, 1.0f, false);
                        mYawArm.Render(Position, mYawArmBaseRotation * Matrix.CreateRotationY(yawAngle), scale, Color.Green, 1.0f, false);
                        mRollArm.Render(Position, mRollArmBaseRotation * Matrix.CreateRotationZ(rollAngle), scale, Color.Blue, 1.0f, false);
                        break;
                    }
                }
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

        private Vector3 GetMouseWorldPosition(Viewport viewport)
        {
            if (mDragDirection == ModificationDirection.X || mDragDirection == ModificationDirection.Y || mDragDirection == ModificationDirection.Z)
            {
                Vector3 dragDirection = GetDragDirection();
                Plane perpendicularPlane = new Plane(mCamera.Position, Position + dragDirection, Position);
                Plane pickingPlane = new Plane(Position, Position + dragDirection, Position + perpendicularPlane.Normal);
                Vector2 rayPosition;
                Vector2 rayDirection;
                Utils.ProjectRayToScreenSpace(new Ray(Position, dragDirection),
                    viewport, mCamera.ViewTransform,
                    mCamera.ProjectionTransform,
                    out rayPosition,
                    out rayDirection);
                Vector2 screenPoint = Utils.NearestPointOnLine(
                    rayPosition,
                    rayDirection,
                    new Vector2(mControls.MouseState.X, mControls.MouseState.Y));
                Vector3 worldPosition = Utils.ProjectVectorOntoPlane(
                    Utils.CreateWorldRayFromScreenPoint(
                        screenPoint,
                        viewport,
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
                        viewport,
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
