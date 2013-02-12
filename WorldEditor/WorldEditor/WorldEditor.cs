using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GameConstructLibrary;
using Microsoft.Xna.Framework.Graphics;
using Nuclex.Input;
using Nuclex.UserInterface;
using WorldEditor.Dialogs;

namespace WorldEditor
{

    public enum EditMode { None, Height, Object, Texture }
    public enum EditState { None, Height, Object, Parameters, New, Save, Load };

    public class WorldEditor
    {

        private const int DefaultWidth = 100;
        private const int DefaultHeight = 100;

        public EditMode mEditMode = EditMode.None;
        public EditState mEditState = EditState.None;
        public Dialog mDialog = new HeightDialog();

        private FPSCamera mCamera = null;

        private Controls mControls = new Controls();
        private DummyWorld mDummyWorld = null;
        private Entity mEntity = null;

        private Screen mScreen = null;
        private InputManager mInputManager = null;
        private GuiManager mGUIManager = null;

        public int NumUndoRedoStates = 10;
        private int mUndoRedoCurrentState = 0;
        private int mUndoLimit = 0;
        private int mRedoLimit = 0;
        private DummyWorld[] mUndoRedoStates = new DummyWorld[10];

        private Vector2 mSelectAreaPressed = Vector2.Zero;
        private Vector2 mSelectAreaReleased = Vector2.Zero;
        private Vector2 mSelectTopLeft = Vector2.Zero;
        private Vector2 mSelectBottomRight = Vector2.Zero;
        private Rectangle mSelectRectangle = new Rectangle();
        private List<DummyObject> mSelectedObjects = new List<DummyObject>();

        public WorldEditor(Screen screen, InputManager inputManager, GuiManager GUIManager, FPSCamera camera)
        {
            mScreen = screen;
            mInputManager = inputManager;
            mGUIManager = GUIManager;

            mCamera = camera;

            mDummyWorld = new DummyWorld(mControls, DefaultWidth, DefaultHeight);
            mEntity = new Entity(mControls, mCamera);

            mScreen.Desktop.Children.Add(mDialog);
        }

        public void Update(GameTime gameTime)
        {
            mDummyWorld.Update(gameTime);
            mEntity.Update(gameTime);
        }

        public void Draw()
        {
            //mDummyWorld.Draw();
            //mEntity.Draw();
        }

        #region ClickEvents



        #endregion

        #region SelectionBox



        #endregion

        #region UndoRedo

        public void AddState(DummyWorld state)
        {
            
            if (mUndoRedoCurrentState == mUndoLimit)
            {
                mUndoLimit = mUndoRedoCurrentState + 1;
            }

            if (mUndoLimit >= NumUndoRedoStates)
            {
                mUndoLimit = 0;
            }

            mUndoRedoStates[mUndoRedoCurrentState] = new DummyWorld(state);
            mUndoRedoCurrentState++;
            mRedoLimit = mUndoRedoCurrentState;

            if (mUndoRedoCurrentState >= NumUndoRedoStates)
            {
                mUndoRedoCurrentState = 0;
            }

        }

        public void Undo()
        {

            int originalState = mUndoRedoCurrentState;
            mUndoRedoCurrentState--;

            if (mUndoRedoCurrentState < 0)
            {
                mUndoRedoCurrentState = NumUndoRedoStates - 1;
            }

            if (mUndoRedoStates[mUndoRedoCurrentState] == null || mUndoRedoCurrentState == mUndoLimit)
            {
                mUndoRedoCurrentState = originalState;
                return;
            }

            SwapWorld(originalState);

        }

        public void Redo()
        {

            int originalState = mUndoRedoCurrentState;
            mUndoRedoCurrentState++;

            if (mUndoRedoCurrentState >= NumUndoRedoStates)
            {
                mUndoRedoCurrentState = 0;
            }

            if (mUndoRedoStates[mUndoRedoCurrentState] == null || mUndoRedoCurrentState == mRedoLimit + 1)
            {
                mUndoRedoCurrentState = originalState;
                return;
            }

            SwapWorld(originalState);

        }

        private void SwapWorld(int originalState)
        {
            mUndoRedoStates[originalState] = new DummyWorld(mDummyWorld);
            mDummyWorld = mUndoRedoStates[mUndoRedoCurrentState];
            mDummyWorld.LinkHeightMap();
        }

        #endregion

    }
}
