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

    public enum States { None, Height, Object };

    public class MapEditor
    {

        private const int numUndos = 10;

        private MapEditorDialog mMapEditorDialog;
        public MapEditorDialog MapEditorDialog { get { return mMapEditorDialog; } set { mMapEditorDialog = value; } }

        private DummyMap mDummyMap;
        public DummyMap DummyMap { get { return mDummyMap; } set { mDummyMap = value; } }

        private int mCurrentState;
        private int mUndoLimit;
        private int mRedoLimit;
        private DummyMap[] mUndoStates;

        private MapEntity mMapEntity;
        public MapEntity MapEntity { get { return mMapEntity; } set { mMapEntity = value; } }

        private KeyInputAction mTab;
        private KeyInputAction mZ;
        private KeyInputAction mX;

        private bool mEditMode;
        public bool EditMode { get { return mEditMode; } set { mEditMode = value; } }

        public States State { get { return mState; } set { mState = value; } }
        private States mState;

        public MapEditor(Screen mainScreen, Camera camera, Viewport viewport)
        {

            // Create map editor dialog and add to GUIs
            mMapEditorDialog = new MapEditorDialog(this, mainScreen);
            mainScreen.Desktop.Children.Add(mMapEditorDialog);

            // Create a new level based around the default map
            mDummyMap = new DummyMap(this, 100, 100);

            // Create a new entity to navigate and modify the map
            mMapEntity = new MapEntity(this, camera, viewport);

            Initialize();

        }

        private void Initialize()
        {

            mCurrentState = 0;
            mUndoLimit = 0;
            mUndoStates = new DummyMap[10];

            mTab = new KeyInputAction(0, InputAction.ButtonAction.Pressed, Keys.Tab);
            mZ = new KeyInputAction(0, InputAction.ButtonAction.Pressed, Keys.Z);
            mX = new KeyInputAction(0, InputAction.ButtonAction.Pressed, Keys.X);
            mEditMode = false;
        }

        public void Update(GameTime gameTime)
        {
            if (mTab.Active)
            {
                mEditMode = !mEditMode;
                if (mEditMode)
                {
                    MapEditorDialog.Disable();
                }
                else
                {
                    MapEditorDialog.Enable();
                }
            }

            if (mZ.Active) Undo();
            else if (mX.Active) Redo();
            
            mMapEntity.Update(gameTime);
            mDummyMap.Update();
        }

        public void Render()
        {
            mDummyMap.Render();
            mMapEntity.Render();
        }

        public void AddState(DummyMap state)
        {

            if (mCurrentState == mUndoLimit) mUndoLimit = mCurrentState + 1;
            if (mUndoLimit >= numUndos) mUndoLimit = 0;
            
            mUndoStates[mCurrentState] = new DummyMap(state);
            mCurrentState++;
            mRedoLimit = mCurrentState;
            
            if (mCurrentState >= numUndos) mCurrentState = 0;
            
        }

        public void Undo()
        {
            
            int originalState = mCurrentState;
            mCurrentState--;
            if (mCurrentState < 0) mCurrentState = numUndos - 1;
            if (mUndoStates[mCurrentState] == null || mCurrentState == mUndoLimit)
            {
                mCurrentState = originalState;
                return;
            }

            mUndoStates[originalState] = new DummyMap(mDummyMap);

            mDummyMap = mUndoStates[mCurrentState];
            mDummyMap.LinkHeightMap();
        }

        public void Redo()
        {
            int originalState = mCurrentState;
            mCurrentState++;
            if (mCurrentState >= numUndos) mCurrentState = 0;
            if (mUndoStates[mCurrentState] == null || mCurrentState == mRedoLimit + 1)
            {
                mCurrentState = originalState;
                return;
            }

            mUndoStates[originalState] = new DummyMap(mDummyMap);

            mDummyMap = mUndoStates[mCurrentState];
            mDummyMap.LinkHeightMap();
        }
    }
}
