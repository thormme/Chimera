using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameConstructLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nuclex.UserInterface;
using GraphicsLibrary;
using Microsoft.Xna.Framework.Graphics;

namespace MapEditor
{

    public class MapEditor
    {

        private const int NumStates = 10;
        private int mCurrentState;
        private int mUndoLimit;
        private int mRedoLimit;
        private DummyMap[] mUndoStates;

        private KeyInputAction mToggleEditMode;
        private KeyInputAction mUndoRedo;
        private KeyInputAction mDelete;
        private KeyInputAction mIncrease;
        private KeyInputAction mDecrease;
        private KeyInputAction mAlt;
        private KeyInputAction mCtrl;
        private KeyInputAction mShift;
        private KeyInputAction mToggleReminder;

        private KeyInputAction mLeft;
        private KeyInputAction mRight;
        private KeyInputAction mUp;
        private KeyInputAction mDown;

        private MouseButtonInputAction mLeftPressed;
        private MouseButtonInputAction mLeftHold;
        private MouseButtonInputAction mLeftReleased;

        public MapEditor()
        {

            mCurrentState = 0;
            mUndoLimit = 0;
            mUndoStates = new DummyMap[10];

            mToggleEditMode = new KeyInputAction(0, InputAction.ButtonAction.Pressed, Keys.Tab);
            mUndoRedo = new KeyInputAction(0, InputAction.ButtonAction.Pressed, Keys.Z);
            mDelete = new KeyInputAction(0, InputAction.ButtonAction.Pressed, Keys.Delete);
            mIncrease = new KeyInputAction(0, InputAction.ButtonAction.Down, Keys.OemPlus);
            mDecrease = new KeyInputAction(0, InputAction.ButtonAction.Down, Keys.OemMinus);
            mAlt = new KeyInputAction(0, InputAction.ButtonAction.Down, Keys.LeftAlt);
            mCtrl = new KeyInputAction(0, InputAction.ButtonAction.Down, Keys.LeftControl);
            mShift = new KeyInputAction(0, InputAction.ButtonAction.Down, Keys.LeftShift);
            mToggleReminder = new KeyInputAction(0, InputAction.ButtonAction.Pressed, Keys.OemTilde);

            mLeft = new KeyInputAction(0, InputAction.ButtonAction.Down, Keys.Left);
            mRight = new KeyInputAction(0, InputAction.ButtonAction.Down, Keys.Right);
            mUp = new KeyInputAction(0, InputAction.ButtonAction.Down, Keys.Up);
            mDown = new KeyInputAction(0, InputAction.ButtonAction.Down, Keys.Down);

            mLeftPressed = new MouseButtonInputAction(0, InputAction.ButtonAction.Pressed, MouseButtonInputAction.MouseButton.Left);
            mLeftHold = new MouseButtonInputAction(0, InputAction.ButtonAction.Down, MouseButtonInputAction.MouseButton.Left);
            mLeftReleased = new MouseButtonInputAction(0, InputAction.ButtonAction.Released, MouseButtonInputAction.MouseButton.Left);

        }

        public void Update(GameTime gameTime)
        {

            //if (mToggleEditMode.Active) GameMapEditor.ToggleEditMode();

            if (mDelete.Active) GameMapEditor.Delete();

            if (!mCtrl.Active && !mShift.Active)
            {

                Vector3 movement = new Vector3(0.0f, 0.0f, 0.0f);

                if (mIncrease.Active) movement.Y = 1.0f;
                else if (mDecrease.Active) movement.Y = -1.0f;

                if (mRight.Active) movement.X = -1.0f;
                else if (mLeft.Active) movement.X = 1.0f;

                if (mUp.Active) movement.Z = 1.0f;
                else if (mDown.Active) movement.Z = -1.0f;

                GameMapEditor.Move(movement);

            }

            if (mCtrl.Active)
            {
                if (mIncrease.Active) GameMapEditor.Scale(true);
                else if (mDecrease.Active) GameMapEditor.Scale(false);
            }
            
            if (mShift.Active)
            {
                if (mIncrease.Active) GameMapEditor.Rotate(true);
                else if (mDecrease.Active) GameMapEditor.Rotate(false);
            }

            if (mUndoRedo.Active && mCtrl.Active && mShift.Active) Redo();
            else if (mUndoRedo.Active && mCtrl.Active) Undo();

            if (mToggleReminder.Active) GameMapEditor.ToggleReminder();

            if (mLeftPressed.Active) GameMapEditor.Pressed();
            else if (mLeftHold.Active) GameMapEditor.Hold();
            else if (mLeftReleased.Active) GameMapEditor.Released();

        }

        public void AddState(DummyMap state)
        {

            if (mCurrentState == mUndoLimit) mUndoLimit = mCurrentState + 1;
            if (mUndoLimit >= NumStates) mUndoLimit = 0;

            mUndoStates[mCurrentState] = new DummyMap(state);
            mCurrentState++;
            mRedoLimit = mCurrentState;

            if (mCurrentState >= NumStates) mCurrentState = 0;

        }

        public void Undo()
        {

            int originalState = mCurrentState;
            mCurrentState--;
            if (mCurrentState < 0) mCurrentState = NumStates - 1;
            if (mUndoStates[mCurrentState] == null || mCurrentState == mUndoLimit)
            {
                mCurrentState = originalState;
                return;
            }

            mUndoStates[originalState] = new DummyMap(GameMapEditor.Map);

            GameMapEditor.Map = mUndoStates[mCurrentState];
            GameMapEditor.Map.LinkHeightMap();

        }

        public void Redo()
        {

            int originalState = mCurrentState;
            mCurrentState++;
            if (mCurrentState >= NumStates) mCurrentState = 0;
            if (mUndoStates[mCurrentState] == null || mCurrentState == mRedoLimit + 1)
            {
                mCurrentState = originalState;
                return;
            }

            mUndoStates[originalState] = new DummyMap(GameMapEditor.Map);

            GameMapEditor.Map = mUndoStates[mCurrentState];
            GameMapEditor.Map.LinkHeightMap();

        }

    }
}
