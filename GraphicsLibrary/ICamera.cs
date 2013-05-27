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
        Matrix View
        {
            get;
        }

        /// <summary>
        /// Orthographic projection in to view frustum.
        /// </summary>
        Matrix Projection
        {
            get;
        }

        /// <summary>
        /// Distance of far plane from camera.
        /// </summary>
        float FarPlaneDistance
        {
            get;
        }

        /// <summary>
        /// Distance of the near plance from camera.
        /// </summary>
        float NearPlaneDistance
        {
            get;
        }
    }
}
