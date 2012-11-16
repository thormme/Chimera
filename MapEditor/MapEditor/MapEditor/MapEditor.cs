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

        private MapEditorDialog mMapEditorDialog;
        public MapEditorDialog MapEditorDialog { get { return mMapEditorDialog; } set { mMapEditorDialog = value; } }

        private DummyMap mDummyMap;
        public DummyMap DummyMap { get { return mDummyMap; } set { mDummyMap = value; } }

        private MapEntity mMapEntity;
        public MapEntity MapEntity { get { return mMapEntity; } set { mMapEntity = value; } }

        private KeyInputAction mTab;

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

            mTab = new KeyInputAction(0, InputAction.ButtonAction.Pressed, Keys.Tab);
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
            
            mMapEntity.Update(gameTime);
            mDummyMap.Update();
            
        }

        public void Render()
        {

            mDummyMap.Render();
            mMapEntity.Render();

        }

    }
}
