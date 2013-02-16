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
        public Form mEditorForm = new EditorForm();

        //Dialog for object parameters.
        public Form mObjectParametersForm = new ObjectParametersForm();

        private FPSCamera mCamera = null;

        //Stores all placeable objects.
        private Dictionary<string, DummyObject> mObjects = new Dictionary<string, DummyObject>();

        //Stores all useable textures.
        private Dictionary<string, Texture2D> mTextures = new Dictionary<string, Texture2D>();
        
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
                    TransparentModel tempModel = new TransparentModel(mDummyObject.Model);
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

            MenuStrip editMenu = (mEditorForm.Controls["MenuStrip"] as MenuStrip);
            (editMenu.Items["File"] as ToolStripMenuItem).DropDownItems["NewMenu"].Click += NewHandler;
            (editMenu.Items["File"] as ToolStripMenuItem).DropDownItems["SaveMenu"].Click += SaveHandler;
            (editMenu.Items["File"] as ToolStripMenuItem).DropDownItems["LoadMenu"].Click += LoadHandler;

            TabControl editModes = (mEditorForm.Controls["EditTabs"] as TabControl);

            editModes.SelectedIndexChanged += EditHandler;
            (editModes.Controls["Objects"].Controls["ObjectList"] as ListBox).SelectedIndexChanged += ObjectHandler;
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

        private void NewHandler(object sender, EventArgs e)
        {
            
        }

        private void SaveHandler(object sender, EventArgs e)
        {
            SaveForm save = new SaveForm();
            save.Show();
            save.Location = new System.Drawing.Point(80, 80);
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
            mDummyObject.Model = "editor";
            mDummyObject.Position = Vector3.Zero;
            mDummyObject.Orientation = Vector3.Up;
            mDummyObject.Scale = Vector3.One;
        }

        private void ObjectHandler(object sender, EventArgs e)
        {
            mDummyObject = mObjects[(sender as ListBox).SelectedItem.ToString()];
            mObjectParametersForm.Show();
        }

        private void TextureHandler(object sender, EventArgs e)
        {
            TabControl editModes = (mEditorForm.Controls["EditTabs"] as TabControl);

            Texture2D texture = GraphicsManager.LookupSprite((editModes.Controls["Textures"].Controls["TextureList"] as ListBox).SelectedItem.ToString());

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

            (editModes.Controls["Textures"].Controls["Picture"] as PictureBox).Image = bmp;

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

                        try
                        {

                            int radius = Convert.ToInt32((editModes.SelectedTab.Controls["HeightRadiusField"] as TextBox).Text);
                            int intensity = Convert.ToInt32((editModes.SelectedTab.Controls["HeightIntensityField"] as TextBox).Text);

                            mDummyObject.Scale = new Vector3((Utils.WorldScale.X + Utils.WorldScale.Z) / 2.0f * radius);

                            mDummyWorld.ModifyHeightMap(
                                mDummyObject.Position, 
                                radius,
                                intensity,
                                (editModes.SelectedTab.Controls["SetBox"] as CheckBox).Checked,
                                (editModes.SelectedTab.Controls["InvertBox"] as CheckBox).Checked,
                                (editModes.SelectedTab.Controls["FeatherBox"] as CheckBox).Checked,
                                (editModes.SelectedTab.Controls["FlattenBox"] as CheckBox).Checked,
                                (editModes.SelectedTab.Controls["SmoothBox"] as CheckBox).Checked);

                        }
                        catch (SystemException)
                        {
                            Console.WriteLine("Invalid input.");
                        }

                    }
                    else if (editModes.SelectedTab == editModes.Controls["Textures"])
                    {
                        try
                        {
                            
                            int radius = Convert.ToInt32((editModes.SelectedTab.Controls["TextureRadiusField"] as TextBox).Text);
                            float alpha = Convert.ToSingle((editModes.SelectedTab.Controls["TextureAlphaField"] as TextBox).Text);

                            mDummyObject.Scale = new Vector3((Utils.WorldScale.X + Utils.WorldScale.Z) / 2.0f * radius);

                            mDummyWorld.ModifyTextureMap(
                                mDummyObject.Position,
                                (editModes.SelectedTab.Controls["TextureList"] as ListBox).SelectedItem.ToString(),
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
