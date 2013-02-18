using System;
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
        private const string MapTexturesPath = "textures/maps";

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
        public EditorForm mEditorForm = new EditorForm();

        //Dialog for object parameters.
        public ObjectParametersForm mObjectParametersForm = new ObjectParametersForm();

        private FPSCamera mCamera = null;

        //Stores all placeable objects.
        private Dictionary<string, DummyObject> mObjects = new Dictionary<string, DummyObject>();

        //Stores all useable textures.
        private Dictionary<string, Texture2D> mTextures = new Dictionary<string, Texture2D>();
        
        //Object that will be drawn at the cursor in object tab.
        private DummyObject mCursorObject = new DummyObject();

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
            mDummyWorld = new DummyWorld(mControls);
            mEntity = new Entity(viewport, mControls, mCamera);
            CreateEditorForm();
        }

        public void Update(GameTime gameTime)
        {
            mControls.Update(gameTime);
            mDummyWorld.Update(gameTime);
            mEntity.Update(gameTime);

            Vector3? pickingPosition = mEntity.GetPickingLocation(mDummyWorld);
            if (pickingPosition != null && mCursorObject != null)
            {
                mCursorObject.Position = pickingPosition.Value;
            }
            mPlaceable = pickingPosition.HasValue;

            PerformActions();
        }

        public void Draw()
        {
            mDummyWorld.Draw();
            if (mPlaceable)
            {
                try
                {
                    if (mCursorObject != null)
                    {
                        mCursorObject.Draw();
                    }
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

            MenuStrip editMenu = (mEditorForm.Controls["MenuStrip"] as MenuStrip);
            (editMenu.Items["File"] as ToolStripMenuItem).DropDownItems["SaveMenu"].Click += SaveHandler;
            (editMenu.Items["File"] as ToolStripMenuItem).DropDownItems["LoadMenu"].Click += LoadHandler;

            TabControl editModes = (mEditorForm.Controls["EditTabs"] as TabControl);

            editModes.SelectedIndexChanged += EditHandler;

            (editModes.Controls["Objects"].Controls["ObjectList"] as ListBox).SelectedIndexChanged += SelectNewObjectHandler;
            (mObjectParametersForm.Controls["Create"] as Button).Click += CreateObjectButtonHandler;
            (editModes.Controls["Textures"].Controls["TextureList"] as ListBox).SelectedIndexChanged += TextureHandler;

            foreach (var model in GraphicsManager.ModelLibrary)
            {
                DummyObject tempObject = new DummyObject();
                tempObject.Type = "Chimera.Prop";
                tempObject.Model = model.Key;
                tempObject.Parameters = null;
                tempObject.Position = Vector3.Zero;
                tempObject.Orientation = Vector3.Up;
                tempObject.Scale = Vector3.One;
                tempObject.Height = 0.0f;
                mObjects.Add(tempObject.Model, tempObject);
                (editModes.Controls["Objects"].Controls["ObjectList"] as ListBox).Items.Add(tempObject.Model);
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

            foreach (var texture in GraphicsManager.TextureLibrary)
            {
                (editModes.Controls["Textures"].Controls["TextureList"] as ListBox).Items.Add(texture.Key);
            }

        }

        private void SaveHandler(object sender, EventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                mDummyWorld.Save(saveDialog.FileName);
            }

        }

        private void LoadHandler(object sender, EventArgs e)
        {

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
                SwitchToEdit();
            }
            else if (editModes.SelectedTab == editModes.Controls["Textures"])
            {
                mObjectParametersForm.Hide();
                SwitchToEdit();
            }
        }

        private void SwitchToEdit()
        {
            // TODO: possibly create new object here.
            mCursorObject.Model = "editor";
            mCursorObject.Position = Vector3.Zero;
            mCursorObject.Orientation = Vector3.Up;
            mCursorObject.Scale = Vector3.One;
        }

        private void SelectNewObjectHandler(object sender, EventArgs e)
        {
            mObjectParametersForm.Show();
        }

        private void CreateObjectButtonHandler(object sender, EventArgs e)
        {
            AddState(mDummyWorld);
            mSelectedObjects.Clear();
            mSelectedObjects.Add(new DummyObject(mObjects[(mEditorForm.Controls["EditTabs"].Controls["Objects"].Controls["ObjectList"] as ListBox).SelectedItem.ToString()]));
            SetObjectPropertiesToForm(mSelectedObjects[0]);
            mDummyWorld.AddObject(mSelectedObjects[0]);
        }

        private void SetObjectPropertiesToForm(DummyObject dummyObject)
        {
            float X = (float)mObjectParametersForm.PositionX.Value;
            float Y = (float)mObjectParametersForm.PositionY.Value;
            float Z = (float)mObjectParametersForm.PositionZ.Value;
            dummyObject.Position = new Vector3(X, Y, Z);

            float Roll = (float)mObjectParametersForm.Roll.Value;
            float Pitch = (float)mObjectParametersForm.Pitch.Value;
            float Yaw = (float)mObjectParametersForm.Yaw.Value;
            dummyObject.Orientation = new Vector3(Roll, Pitch, Yaw);

            float ScaleX = (float)mObjectParametersForm.ScaleX.Value;
            float ScaleY = (float)mObjectParametersForm.ScaleY.Value;
            float ScaleZ = (float)mObjectParametersForm.ScaleZ.Value;
            dummyObject.Orientation = new Vector3(ScaleX, ScaleY, ScaleZ);

            dummyObject.Height = (float)mObjectParametersForm.Height.Value;
        }

        private void TextureHandler(object sender, EventArgs e)
        {
            Texture2D texture = GraphicsManager.LookupSprite(mEditorForm.TextureList.SelectedItem.ToString());

            byte[] textureData = new byte[4 * texture.Width * texture.Height];
            texture.GetData<byte>(textureData);

            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(
                texture.Width, 
                texture.Height, 
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            System.Drawing.Imaging.BitmapData bmpData = bmp.LockBits(
               new System.Drawing.Rectangle(0, 0, texture.Width, texture.Height), 
               System.Drawing.Imaging.ImageLockMode.WriteOnly,
               System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            IntPtr safePtr = bmpData.Scan0;
            System.Runtime.InteropServices.Marshal.Copy(textureData, 0, safePtr, textureData.Length);
            bmp.UnlockBits(bmpData);

            mEditorForm.Picture.Image = bmp;

        }

        #endregion

        private void PerformActions()
        {
            if (mControls.LeftPressed.Active)
            {
                if (mPlaceable)
                {
                    if (mEditorForm.EditTabs.SelectedTab == mEditorForm.Objects)
                    {
                        //AddState(mDummyWorld);
                        mCursorObject = null;
                    }
                }
            }
            else if (mControls.LeftHold.Active)
            {
                if (mPlaceable)
                {
                    if (mEditorForm.EditTabs.SelectedTab == mEditorForm.Heights)
                    {

                        try
                        {

                            int radius = Convert.ToInt32(mEditorForm.HeightRadiusField.Text);
                            int intensity = Convert.ToInt32(mEditorForm.HeightIntensityField.Text);

                            mCursorObject.Scale = new Vector3((Utils.WorldScale.X + Utils.WorldScale.Z) / 2.0f * radius);

                            mDummyWorld.ModifyHeightMap(
                                mCursorObject.Position, 
                                radius,
                                intensity,
                                mEditorForm.SetBox.Checked,
                                mEditorForm.InvertBox.Checked,
                                mEditorForm.FeatherBox.Checked,
                                mEditorForm.FlattenBox.Checked,
                                mEditorForm.SmoothBox.Checked);

                        }
                        catch (SystemException)
                        {
                            Console.WriteLine("Invalid input.");
                        }

                    }
                    else if (mEditorForm.EditTabs.SelectedTab == mEditorForm.Textures)
                    {
                        try
                        {
                            
                            int radius = Convert.ToInt32(mEditorForm.TextureRadiusField.Text);
                            float alpha = Convert.ToSingle(mEditorForm.TextureAlphaField.Text);

                            mCursorObject.Scale = new Vector3((Utils.WorldScale.X + Utils.WorldScale.Z) / 2.0f * radius);

                            mDummyWorld.ModifyTextureMap(
                                mCursorObject.Position,
                                mEditorForm.TextureList.SelectedItem.ToString(),
                                radius,
                                alpha);
                            
                        }
                        catch (SystemException)
                        {
                            Console.WriteLine("Invalid input.");
                        }
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
