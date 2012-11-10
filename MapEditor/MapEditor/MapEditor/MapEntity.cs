using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameConstructLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MapEditor
{
    class MapEntity
    {

        private const float speed = 1.0f;
        private const float sensitivity = 1.0f;
        
        private Camera mCamera;
        private Vector2 mMovement;
        private Vector2 mOrientation;

        public MapEntity(Camera camera)
        {
            mCamera = camera;
            mMovement = new Vector2(0.0f, -1.0f);
            mOrientation = new Vector2(0.0f, 0.0f);
        }

        public void Update(GameTime gameTime)
        {
            mCamera.MoveForward(speed * mMovement.Y * gameTime.ElapsedGameTime.Milliseconds);
            mCamera.MoveRight(speed * mMovement.X * gameTime.ElapsedGameTime.Milliseconds);
            Console.WriteLine(speed * mMovement.Y * gameTime.ElapsedGameTime.Milliseconds);
            Console.WriteLine(mCamera.Position.X + ", " + mCamera.Position.Y + ", " + mCamera.Position.Z);
            //mCamera.RotatePitch(sensitivity * mOrientation.Y * gameTime.ElapsedGameTime.Milliseconds);
            //mCamera.RotateYaw(sensitivity * mOrientation.X * gameTime.ElapsedGameTime.Milliseconds);
        }

    }
}
