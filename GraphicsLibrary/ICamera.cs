using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GraphicsLibrary
{
    public interface ICamera
    {
        /// <summary>
        /// Position of camera in world space.
        /// </summary>
        Vector3 GetPosition();

        /// <summary>
        /// Direction camera is facing in world space.
        /// </summary>
        Vector3 GetForward();

        /// <summary>
        /// Transformation of vector in to camera space.
        /// </summary>
        Matrix GetViewTransform();

        /// <summary>
        /// Orthographic projection in to view frustum.
        /// </summary>
        Matrix GetProjectionTransform();

        /// <summary>
        /// Distance of far plane from camera.
        /// </summary>
        float GetFarPlaneDistance();

        /// <summary>
        /// Distance of the near plance from camera.
        /// </summary>
        float GetNearPlaneDistance();

        /// <summary>
        /// Sets distance of far plane from camera.
        /// </summary>
        void SetFarPlaneDistance(float value);
    }
}
