using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GraphicsLibrary
{
    public interface ICamera
    {
        Vector3 Position
        {
            get;
        }

        Vector3 Forward
        {
            get;
        }

        Vector3 Right
        {
            get;
        }

        float AspectRatio
        {
            get;
        }

        float FieldOfView
        {
            get;
        }

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
