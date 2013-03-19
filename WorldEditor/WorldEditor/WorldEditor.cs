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

        private string mName = null;
        private string mFilePath = null;

        private bool mIsActive = false;

        //Dialog for the world editor.
        public EditorForm mEditorForm = new EditorForm();

        //Dialog for object parameters.
        public ObjectParametersForm mObjectParametersForm = new ObjectParametersForm();

        public TextureSelectionForm mTextureSelectionForm = new TextureSelectionForm();

        public bool Closed = false;

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

        public void Update(GameTime gameTime, bool gameWindowActive)
        {
            mIsActive = gameWindowActive;

            mControls.Update(gameTime);
            mDummyWorld.Update(gameTime);

            mPlaceable = false;

            if (mDummyWorld.Name != null)
            {
                mEntity.Update(gameTime);

                Tuple<Vector3, Vector3> pickingPosition = mEntity.GetPickingLocation(mDummyWorld);

                mPlaceable = pickingPosition != null;

                if (mPlaceable && mCursorObject != null)
                {
                    mCursorObject.Position = pickingPosition.Item1;
                }

            }

            PerformActions();
        }

        public void Draw()
        {
            GraphicsManager.CursorShape brush = GraphicsManager.CursorShape.NONE;

            if (mPlaceable)
            {
                switch ((mEditorForm as EditorForm).Mode)
                {
                    case EditorForm.EditorMode.HEIGHTMAP:
                        brush = (mEditorForm as EditorForm).HeightMapBrush == EditorForm.Brushes.BLOCK ? GraphicsManager.CursorShape.BLOCK : GraphicsManager.CursorShape.CIRCLE;
                        break;
                    case EditorForm.EditorMode.PAINTING:
                        brush = (mEditorForm as EditorForm).PaintingBrush == EditorForm.Brushes.BLOCK ? GraphicsManager.CursorShape.BLOCK : GraphicsManager.CursorShape.CIRCLE;
                        break;
                }

                GraphicsManager.CursorPosition = mCursorObject.Position - mCursorObject.Scale;
                GraphicsManager.CursorInnerRadius = 8.0f * mCursorObject.Scale.X;
                GraphicsManager.CursorOuterRadius = 9.0f * mCursorObject.Scale.X;
            }

            GraphicsManager.DrawCursor = brush;

            mDummyWorld.Draw();
        }

        #region Editor Form Creation and Handling

        private void CreateEditorForm()
        {
            mEditorForm.Show();
            mEditorForm.Location = new System.Drawing.Point(80, 80);

            (mEditorForm as EditorForm).FormClosing += CloseHandler;

            mTextureSelectionForm.Hide();
            mTextureSelectionForm.Location = new System.Drawing.Point(80, 80);

            MenuStrip editMenu = (mEditorForm.Controls["MenuStrip"] as MenuStrip);
            (editMenu.Items["File"] as ToolStripMenuItem).DropDownItems["NewMenu"].Click    += NewHandler;
            (editMenu.Items["File"] as ToolStripMenuItem).DropDownItems["SaveMenu"].Click   += SaveHandler;
            (editMenu.Items["File"] as ToolStripMenuItem).DropDownItems["SaveAsMenu"].Click += SaveAsHandler;
            (editMenu.Items["File"] as ToolStripMenuItem).DropDownItems["OpenMenu"].Click   += OpenHandler;

            TabControl editModes = (mEditorForm.Controls["EditTabs"] as TabControl);

            ((mTextureSelectionForm as TextureSelectionForm).TextureList as ListBox).SelectedIndexChanged += TextureHandler;

            (mEditorForm as EditorForm).HeightmapModeButton.Click += new System.EventHandler(this.CloseTextureForm);
            (mEditorForm as EditorForm).HeightmapModeButton.Click += new System.EventHandler(this.CloseObjectParameterForm);

            (mEditorForm as EditorForm).PaintModeButton.Click += new System.EventHandler(this.OpenTextureForm);
            (mEditorForm as EditorForm).PaintModeButton.Click += new System.EventHandler(this.CloseObjectParameterForm);

            (mEditorForm as EditorForm).ObjectModeButton.Click += new System.EventHandler(this.CloseTextureForm);
            (mEditorForm as EditorForm).ObjectModeButton.Click += new System.EventHandler(this.OpenObjectParameterForm);

            (mEditorForm as EditorForm).SizeUpDown.ValueChanged += SelectionHandler;
            (mEditorForm as EditorForm).StrengthUpDown.ValueChanged += SelectionHandler;

            foreach (var model in GraphicsManager.ModelLibrary)
            {
                DummyObject tempObject = new DummyObject();
                tempObject.Type = "Chimera.Prop";
                tempObject.Model = model.Key;
                tempObject.Parameters = null;
                tempObject.Position = Vector3.Zero;
                tempObject.YawPitchRoll = Vector3.Up;
                tempObject.Scale = Vector3.One;
                tempObject.Height = 0.0f;
                mObjects.Add(tempObject.Model, tempObject);
                ((mEditorForm as EditorForm).ObjectList as ListBox).Items.Add(tempObject.Model);
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
                    tempObject.YawPitchRoll = Vector3.Up;
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
                ((mTextureSelectionForm as TextureSelectionForm).TextureList as ListBox).Items.Add(texture.Key);
            }

            SwitchToEdit();
        }

        private void CloseHandler(object sender, EventArgs e)
        {
            this.Closed = true;
        }

        private void OpenTextureForm(object sender, EventArgs e)
        {
            mTextureSelectionForm.Show();
        }

        private void CloseTextureForm(object sender, EventArgs e)
        {
            mTextureSelectionForm.Hide();
        }

        private void OpenObjectParameterForm(object sender, EventArgs e)
        {
            mObjectParametersForm.Show();
        }

        private void CloseObjectParameterForm(object sender, EventArgs e)
        {
            mObjectParametersForm.Hide();
        }

        private void NewHandler(object sender, EventArgs e)
        {
            mDummyWorld.New();
        }

        private void SaveHandler(object sender, EventArgs e)
        {
            if (mFilePath == null)
            {
                SaveAsHandler(sender, e);
                return;
            }

            mDummyWorld.Save(mFilePath);
        }

        private void SaveAsHandler(object sender, EventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.AddExtension = true;
            saveDialog.Filter = "Level Files | *.lvl";
            saveDialog.DefaultExt = ".lvl";

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                mDummyWorld.Save(saveDialog.FileName);
                FileInfo fileInfo = new FileInfo(saveDialog.FileName);
                mName = fileInfo.Name;
                mFilePath = fileInfo.FullName;
            }

        }

        private void OpenHandler(object sender, EventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.AddExtension = true;
            openDialog.Filter = "Level Files | *.lvl";
            openDialog.DefaultExt = ".lvl";

            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                FileInfo fileInfo = new FileInfo(openDialog.FileName);
                mDummyWorld.Open(fileInfo);
                mName = fileInfo.Name;
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
            mCursorObject.Model = "circleCursor";
            mCursorObject.Position = Vector3.Zero;
            mCursorObject.YawPitchRoll = Vector3.Zero;
            mCursorObject.Scale = new Vector3((int)(mEditorForm as EditorForm).Size, 0, (int)(mEditorForm as EditorForm).Size);
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
            dummyObject.YawPitchRoll = new Vector3(Roll, Pitch, Yaw);

            float ScaleX = (float)mObjectParametersForm.ScaleX.Value;
            float ScaleY = (float)mObjectParametersForm.ScaleY.Value;
            float ScaleZ = (float)mObjectParametersForm.ScaleZ.Value;
            dummyObject.YawPitchRoll = new Vector3(ScaleX, ScaleY, ScaleZ);

            dummyObject.Height = (float)mObjectParametersForm.Height.Value;
        }

        private void TextureHandler(object sender, EventArgs e)
        {
            Texture2D texture = GraphicsManager.LookupSprite(mTextureSelectionForm.TextureList.SelectedItem.ToString());

            MemoryStream ms = new MemoryStream();

            texture.SaveAsPng(ms, texture.Width, texture.Height);

            ms.Seek(0, SeekOrigin.Begin);

            System.Drawing.Image bmp = System.Drawing.Bitmap.FromStream(ms);

            ms.Close();
            ms = null;
            ((mTextureSelectionForm as TextureSelectionForm).TexturePreview as PictureBox).Image = bmp;
        }

        private void SelectionHandler(object sender, EventArgs e)
        {
            mCursorObject.Scale = new Vector3((int)(sender as NumericUpDown).Value, 0, (int)(sender as NumericUpDown).Value);
        }

        #endregion

        private void PerformActions()
        {
            if (mIsActive)
            {
                EditorForm form = mEditorForm as EditorForm;
                TabControl editModes = (mEditorForm.Controls["EditTabs"] as TabControl);
                if (mControls.LeftPressed.Active)
                {
                    if (mPlaceable && form.Mode == EditorForm.EditorMode.OBJECTS)
                    {
                        AddState(mDummyWorld);
                        mDummyWorld.AddObject(new DummyObject(mCursorObject));
                    }
                }
                else if (mControls.LeftHold.Active)
                {
                    if (mPlaceable)
                    {
                        switch (form.Mode)
                        {
                            case EditorForm.EditorMode.HEIGHTMAP:
                                {
                                    float strength = form.Strength * 10.0f;
                                    mDummyWorld.ModifyHeightMap(mCursorObject.Position, form.Size, strength, form.HeightMapBrush, form.HeightMapTool);
                                    break;
                                }
                            case EditorForm.EditorMode.PAINTING:
                                {
                                    float alpha = form.Strength / 100.0f;
                                    GameConstructLibrary.TerrainTexture.TextureLayer layer = (GameConstructLibrary.TerrainTexture.TextureLayer)(form.PaintingLayers);
                                    string textureName = ((mTextureSelectionForm as TextureSelectionForm).TextureList as ListBox).SelectedItem.ToString();
                                    mDummyWorld.ModifyTextureMap(mCursorObject.Position, textureName, form.Size, alpha, form.PaintingBrush, form.PaintingTool, layer);
                                    break;
                                }
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
            
            //if (mUndoRedoCurrentState == mUndoLimit)
            //{
            //    mUndoLimit = mUndoRedoCurrentState + 1;
            //}

            //if (mUndoLimit >= NumUndoRedoStates)
            //{
            //    mUndoLimit = 0;
            //}

            //mUndoRedoStates[mUndoRedoCurrentState] = new DummyWorld(state);
            //mUndoRedoCurrentState++;
            //mRedoLimit = mUndoRedoCurrentState;

            //if (mUndoRedoCurrentState >= NumUndoRedoStates)
            //{
            //    mUndoRedoCurrentState = 0;
            //}

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
