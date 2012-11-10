using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameConstructLibrary
{
    public class PlayerController : Controller
    {
        private Camera mCamera;
        private Vector2 mMoveDirection;

        public override void Update(GameTime time, List<Creature> collidingCreatures)
        {
            mCreature.Forward = mCamera.Target;
            mCreature.Move(mMoveDirection);
        }

        public void Move(Vector2 direction)
        {
            mMoveDirection = direction;
        }

        public void MoveHoriz(float magnitude)
        {
            mMoveDirection.X = magnitude;
        }

        public void MoveVert(float magnitude)
        {
            mMoveDirection.Y = magnitude;
        }

        public void UsePart(int part, Vector3 direction)
        {
            mCreature.UsePart(part, direction);
        }
    }
}
