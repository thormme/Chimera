﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GameConstructLibrary;
using Microsoft.Xna.Framework.Graphics;
using WorldEditor.Dialogs;
using System.Windows.Forms;
using System.IO;
using GraphicsLibrary;
using Utility;

namespace WorldEditor
{

    public enum EditMode { Object, Height, Texture }

    public class WorldEditor
    {

        #region Constants
        private const string ContentPath = "Content";
        private const string ModelsPath = "models";
        private const string ObjectsPath = "objects";

        private const int DefaultWidth = 100;
        private const int DefaultHeight = 100;
        #endregion

        #region UndoRedo
        public int NumUndoRedoStates = 10;
        private int mUndoRedoCurrentState = 0;
        private int mUndoLimit = 0;
        private int mRedoLimit = 0;
        private DummyWorld[] mUndoRedoStates = new DummyWorld[10];
        #endregion

        #region Selection Box
        private Vector2 mSelectAreaPressed = Vector2.Zero;
        private Vector2 mSelectAreaReleased = Vector2.Zero;
        private Vector2 mSelectTopLeft = Vector2.Zero;
        private Vector2 mSelectBottomRight = Vector2.Zero;
        private Rectangle mSelectRectangle = new Rectangle();
        private List<DummyObject> mSelectedObjects = new List<DummyObject>();
        #endregion

        //Dialog for the world editor.
        public Form mEditorForm = new EditorForm();

        //Dialog for object parameters.
        public Form mObjectParametersForm = new ObjectParametersForm();

        private FPSCamera mCamera = null;

        //Stores all placeable objects.
        private Dictionary<string, DummyObject> mObjects = new Dictionary<string, DummyObject>();
        
        //Object that will be drawn at the cursor in object tab.
        private DummyObject mDummyObject = new DummyObject();

        //Determines whether or not you are able to place an object/modify heights or textures.
        private bool mPlaceable;

        //Handles input.
        private Controls mControls = new Controls();

        //Stores the objects placed in the world and the height map.
        private DummyWorld mDummyWorld = null;

        //Handles camera movement and picking.
        private Entity mEntity = null;

        public WorldEditor(Viewport viewport, FPSCamera camera)
        {
            mCamera = camera;
            mDummyWorld = new DummyWorld(mControls, DefaultWidth, DefaultHeight);
            mEntity = new Entity(viewport, mControls, mCamera);
            CreateEditorForm();
        }

        public void Update(GameTime gameTime)
        {
            mControls.Update(gameTime);
            mDummyWorld.Update(gameTime);
            mPlaceable = mEntity.Update(gameTime, mDummyWorld, mDummyObject);
            PerformActions();
        }

        public void Draw()
        {
            mDummyWorld.Draw();
            if (mPlaceable)
            {
                try
                {
                    InanimateModel tempModel = new InanimateModel(mDummyObject.Model);
                    tempModel.Render(
                        new Vector3(mDummyObject.Position.X, mDummyObject.Position.Y + mDummyObject.Height * Utils.WorldScale.Y, mDummyObject.Position.Z),
                        Matrix.CreateFromYawPitchRoll(mDummyObject.Orientation.X, mDummyObject.Orientation.Y, mDummyObject.Orientation.Z),
                        mDummyObject.Scale);
                }
                catch (SystemException)
                {
                    Console.WriteLine("Not a valid model file.");
                }
            }
        }

        #region Editor Form Creation and Handling

        private void CreateEditorForm()
        {
            mEditorForm.Show();
            mEditorForm.Location = new System.Drawing.Point(80, 80);

            TabControl editModes = (mEditorForm.Controls["EditTabs"] as TabControl);

            editModes.SelectedIndexChanged += EditHandler;
            (editModes.Controls["Objects"].Controls["ObjectList"] as ListBox).SelectedIndexChanged += ObjectHandler;

            DirectoryInfo baseDirectory = new DirectoryInfo(ContentPath + "/" + ModelsPath + "/");
            DirectoryInfo[] subDirectories = baseDirectory.GetDirectories();
            foreach (DirectoryInfo subDirectory in subDirectories)
            {
                FileInfo[] models = subDirectory.GetFiles("*.xnb");
                foreach (FileInfo model in models)
                {
                    DummyObject tempObject = new DummyObject();
                    tempObject.Type = "Chimera.Prop";
                    tempObject.Model = Path.GetFileNameWithoutExtension(model.Name);
                    tempObject.Parameters = null;
                    tempObject.Position = Vector3.Zero;
                    tempObject.Orientation = Vector3.Up;
                    tempObject.Scale = Vector3.One;
                    tempObject.Height = 0.0f;
                    mObjects.Add(tempObject.Model, tempObject);
                    (editModes.Controls["Objects"].Controls["ObjectList"] as ListBox).Items.Add(tempObject.Model);
                }
            }

            FileInfo[] objects = (new DirectoryInfo(ContentPath + "/" + ObjectsPath + "/")).GetFiles();
            foreach (FileInfo file in objects)
            {
                DummyObject tempObject = new DummyObject();
                try
                {
                    string[] data = System.IO.File.ReadAllLines(file.FullName);

                    tempObject.Type = data[0];
                    tempObject.Model = data[1];

                    List<string> parameters = new List<string>();
                    if (data.Length - 2 > 0)
                    {
                        for (int count = 0; count < data.Length - 2; count++)
                        {
                            parameters.Add(data[count + 2]);
                        }
                    }
                    tempObject.Parameters = parameters.ToArray();

                    tempObject.Position = Vector3.Zero;
                    tempObject.Orientation = Vector3.Up;
                    tempObject.Scale = Vector3.One;
                    tempObject.Height = 0.0f;

                    mObjects.Add(tempObject.Model, tempObject);
                    (editModes.Controls["Objects"].Controls["ObjectList"] as ListBox).Items.Add(tempObject.Model);
                }
                catch (SystemException)
                {
                    Console.WriteLine("Formatting error in object: " + file.Name + ".");
                }
            }
        }

        private void EditHandler(object sender, EventArgs e)
        {
            TabControl editModes = (sender as TabControl);
            if (editModes.SelectedTab == editModes.Controls["Objects"])
            {

            }
            else if (editModes.SelectedTab == editModes.Controls["Heights"])
            {
                mObjectParametersForm.Hide();
            }
            else if (editModes.SelectedTab == editModes.Controls["Textures"])
            {
                mObjectParametersForm.Hide();
            }
        }

        private void ObjectHandler(object sender, EventArgs e)
        {
            mDummyObject = mObjects[(sender as ListBox).SelectedItem.ToString()];
            mObjectParametersForm.Show();
        }

        #endregion

        private void PerformActions()
        {
            TabControl editModes = (mEditorForm.Controls["EditTabs"] as TabControl);
            if (mControls.LeftPressed.Active)
            {
                if (mPlaceable)
                {
                    AddState(mDummyWorld);
                    if (editModes.SelectedTab == editModes.Controls["Objects"])
                    {
                        Console.WriteLine(mDummyObject.Position);
                        mDummyWorld.AddObject(new DummyObject(mDummyObject));
                    }
                }
            }
            else if (mControls.LeftHold.Active)
            {
                if (mPlaceable)
                {
                    if (editModes.SelectedTab == editModes.Controls["Heights"])
                    {
                        mDummyWorld.ModifyHeightMap();
                    }
                    else if (editModes.SelectedTab == editModes.Controls["Textures"])
                    {
                        mDummyWorld.ModifyHeightMap();
                    }
                }
            }
        }

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
