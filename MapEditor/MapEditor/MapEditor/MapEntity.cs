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

        private const float speed = 0.5f;
        private const float sensitivity = 0.5f;
        
        private Camera mCamera;
        private Vector3 mPosition;
        private Vector2 mMovement;
        private Vector2 mOrientation;

        public MapEntity(Camera camera)
        {
            mCamera = camera;
            mPosition = new Vector3(0.0f, 0.0f, 0.0f);
            mMovement = new Vector2(0.0f, -1.0f);
            mOrientation = new Vector2(1.0f, 0.0f);
        }

        public void Update(GameTime gameTime)
        {

            //mCamera.MoveForward(speed * mMovement.Y * gameTime.ElapsedGameTime.Milliseconds);
            //mCamera.MoveRight(speed * mMovement.X * gameTime.ElapsedGameTime.Milliseconds);
            mCamera.RotatePitch(sensitivity * mOrientation.Y * gameTime.ElapsedGameTime.Milliseconds);
            mCamera.RotateYaw(sensitivity * mOrientation.X * gameTime.ElapsedGameTime.Milliseconds);
            
        }

    }
}
