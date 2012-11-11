using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameConstructLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using GraphicsLibrary;

namespace MapEditor
{
    class MapEntity
    {

        private const float speed = 100.0f;
        private const float sensitivity = 1.0f;
        
        private Camera mCamera;
        private Vector3 mPosition;
        private Vector2 mMovement;
        private Vector2 mOrientation;

        public MapEntity(Camera camera)
        {
            mCamera = camera;
            mPosition = new Vector3(0.0f, 0.0f, 0.0f);
            mMovement = new Vector2(0.0f, 1.0f);
            mOrientation = new Vector2(0.0f, 0.0f);
        }

        public void Update(GameTime gameTime)
        {
            Console.WriteLine(mPosition.X + "," + mPosition.Y + "," + mPosition.Z);
            mPosition += speed * mMovement.Y * mCamera.Target * gameTime.ElapsedGameTime.Milliseconds;
            mPosition += speed * mMovement.X * mCamera.Right * gameTime.ElapsedGameTime.Milliseconds;
            //mCamera.RotatePitch(sensitivity * mOrientation.Y * gameTime.ElapsedGameTime.Milliseconds);
            //mCamera.RotateYaw(sensitivity * mOrientation.X * gameTime.ElapsedGameTime.Milliseconds);
            mCamera.Position = mPosition;
        }

    }
}
