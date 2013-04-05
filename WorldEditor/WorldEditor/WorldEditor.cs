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
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;

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

        private const double UndoTimeLimit = 0.10;

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
        #endregion

        #region Public Properties

        //Dialog for the world editor.
        public EditorForm EditorPane = new EditorForm();

        //Dialog for object parameters.
        public ObjectParametersForm ObjectParameterPane = new ObjectParametersForm();

        public TextureSelectionForm TextureSelectionPane = new TextureSelectionForm();

        public bool Closed = false;

        //Handles camera movement and picking.
        public Entity Entity
        {
            get { return mEntity; }
            set { mEntity = value; }
        }
        private Entity mEntity = null;

        #endregion

        #region Private Variables

        private string mName = null;
        private string mFilePath = null;

        private bool mIsActive = false;

        private Effect mTextureTransformShader = null;

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

        private double mTimeSinceUndo = 0.0;

        #endregion

        #region Public Interface

        public WorldEditor(GraphicsDevice graphicsDevice, FPSCamera camera, ContentManager content)
        {
            mCamera = camera;
            mDummyWorld = new DummyWorld(mControls);
            mEntity = new Entity(graphicsDevice, mControls, mCamera);
            CreateEditorForm();

            mTextureTransformShader = content.Load<Effect>("shaders/TextureTransform");
        }

        public void Update(GameTime gameTime, bool gameWindowActive)
        {
            mIsActive = gameWindowActive;

            mControls.Update(gameTime);
            mDummyWorld.Update(gameTime, mCamera.Position);

            mPlaceable = false;

            if (mDummyWorld.Name != null)
            {
                mEntity.Update(gameTime);

                var pickingResult = mEntity.GetPickingLocation(mDummyWorld);

                mPlaceable = pickingResult != null;

                if (mPlaceable && mCursorObject != null)
                {
                    mCursorObject.Position = pickingResult.Item1.Location;
                }

            }

            PerformActions(gameTime);
        }

        public void Draw()
        {
            TerrainRenderable.CursorShape brush = TerrainRenderable.CursorShape.NONE;

            if (mPlaceable)
            {
                switch ((EditorPane as EditorForm).Mode)
                {
                    case EditorForm.EditorMode.HEIGHTMAP:
                        brush = (EditorPane as EditorForm).HeightMapBrush == EditorForm.Brushes.BLOCK || (EditorPane as EditorForm).HeightMapBrush == EditorForm.Brushes.BLOCK_FEATHERED ? 
                            TerrainRenderable.CursorShape.BLOCK : TerrainRenderable.CursorShape.CIRCLE;
                        break;
                    case EditorForm.EditorMode.PAINTING:
                        brush = (EditorPane as EditorForm).PaintingBrush == EditorForm.Brushes.BLOCK || (EditorPane as EditorForm).HeightMapBrush == EditorForm.Brushes.BLOCK_FEATHERED ? 
                            TerrainRenderable.CursorShape.BLOCK : TerrainRenderable.CursorShape.CIRCLE;
                        break;
                }

                mDummyWorld.Terrain.TerrainRenderable.CursorPosition = mCursorObject.Position - mCursorObject.Scale;
                mDummyWorld.Terrain.TerrainRenderable.CursorInnerRadius = 8.0f * mCursorObject.Scale.X;
                mDummyWorld.Terrain.TerrainRenderable.CursorOuterRadius = 9.0f * mCursorObject.Scale.X;
                mDummyWorld.Terrain.TerrainRenderable.DrawCursor = brush;
            }

            mDummyWorld.Draw();
        }

        #endregion

        #region Editor Form Creation and Handling

        private void CreateEditorForm()
        {
            EditorPane.Show();

            TextureSelectionPane.Hide();

            MenuStrip editMenu = (EditorPane.Controls["MenuStrip"] as MenuStrip);
            (editMenu.Items["File"] as ToolStripMenuItem).DropDownItems["NewMenu"].Click    += NewHandler;
            (editMenu.Items["File"] as ToolStripMenuItem).DropDownItems["SaveMenu"].Click   += SaveHandler;
            (editMenu.Items["File"] as ToolStripMenuItem).DropDownItems["SaveAsMenu"].Click += SaveAsHandler;
            (editMenu.Items["File"] as ToolStripMenuItem).DropDownItems["OpenMenu"].Click   += OpenHandler;
            (editMenu.Items["File"] as ToolStripMenuItem).DropDownItems["PlayMenu"].Click   += PlayHandler;

            TabControl editModes = (EditorPane.Controls["EditTabs"] as TabControl);

            ((TextureSelectionPane as TextureSelectionForm).TextureList as ListBox).SelectedIndexChanged += TextureHandler;

            (EditorPane as EditorForm).HeightmapModeButton.Click += new System.EventHandler(this.CloseTextureForm);
            (EditorPane as EditorForm).HeightmapModeButton.Click += new System.EventHandler(this.CloseObjectParameterForm);

            (EditorPane as EditorForm).PaintModeButton.Click += new System.EventHandler(this.OpenTextureForm);
            (EditorPane as EditorForm).PaintModeButton.Click += new System.EventHandler(this.CloseObjectParameterForm);

            (EditorPane as EditorForm).ObjectModeButton.Click += new System.EventHandler(this.CloseTextureForm);
            (EditorPane as EditorForm).ObjectModeButton.Click += new System.EventHandler(this.OpenObjectParameterForm);

            (EditorPane as EditorForm).SizeUpDown.ValueChanged += CursorResizeHandler;

            TextureSelectionPane.UOffset.ValueChanged += TextureHandler;
            TextureSelectionPane.VOffset.ValueChanged += TextureHandler;
            TextureSelectionPane.UScale.ValueChanged += TextureHandler;
            TextureSelectionPane.VScale.ValueChanged += TextureHandler;

            ObjectParameterPane.Create.Click += CreateObjectButtonHandler;

            foreach (var model in AssetLibrary.ModelLibrary)
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
                ((EditorPane as EditorForm).ObjectList as ListBox).Items.Add(tempObject.Model);
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

            foreach (var texture in AssetLibrary.TextureLibrary)
            {
                ((TextureSelectionPane as TextureSelectionForm).TextureList as ListBox).Items.Add(texture.Key);
            }

            ScaleCursor();
        }

        private void CloseHandler(object sender, EventArgs e)
        {
            this.Closed = true;
        }

        private void OpenTextureForm(object sender, EventArgs e)
        {
            TextureSelectionPane.Show();
        }

        private void CloseTextureForm(object sender, EventArgs e)
        {
            TextureSelectionPane.Hide();
        }

        private void OpenObjectParameterForm(object sender, EventArgs e)
        {
            ObjectParameterPane.Show();
        }

        private void CloseObjectParameterForm(object sender, EventArgs e)
        {
            ObjectParameterPane.Hide();
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
                mFilePath = fileInfo.FullName;
            }

        }

        private void PlayHandler(object sender, EventArgs e)
        {
            if (mFilePath == null)
            {
                SaveAsHandler(sender, e);
            }

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = false;
            startInfo.UseShellExecute = false;
            startInfo.FileName = "Chimera.exe";
            startInfo.WindowStyle = ProcessWindowStyle.Normal;
            startInfo.Arguments = "\"" + mFilePath + "\"";

            using (Process gameProcess = Process.Start(startInfo))
            {
            }
        }

        private void EditHandler(object sender, EventArgs e)
        {
            TabControl editModes = (sender as TabControl);
            if (editModes.SelectedTab == editModes.Controls["Heights"])
            {
                ObjectParameterPane.Hide();
                ScaleCursor();
            }
            else if (editModes.SelectedTab == editModes.Controls["Textures"])
            {
                ObjectParameterPane.Hide();
                ScaleCursor();
            }
            else if (editModes.SelectedTab == editModes.Controls["Objects"])
            {

            }
        }

        private void ScaleCursor()
        {
            mCursorObject.Scale = new Vector3((int)(EditorPane as EditorForm).Size, 0, (int)(EditorPane as EditorForm).Size);
        }

        private void SelectNewObjectHandler(object sender, EventArgs e)
        {
            ObjectParameterPane.Show();
        }

        private void CreateObjectButtonHandler(object sender, EventArgs e)
        {
            ObjectParameterPane.SelectedObjects.Clear();
            DummyObject dummy = new DummyObject(mObjects[EditorPane.ObjectList.SelectedItem.ToString()]);
            ObjectParameterPane.SelectedObjects.Add(dummy);
            SetObjectPropertiesToForm(dummy);
            mDummyWorld.AddObject(dummy);
        }

        private void SetObjectPropertiesToForm(DummyObject dummyObject)
        {
            float X = (float)ObjectParameterPane.PositionX.Value;
            float Y = (float)ObjectParameterPane.PositionY.Value;
            float Z = (float)ObjectParameterPane.PositionZ.Value;
            dummyObject.Position = new Vector3(X, Y, Z);

            float Roll = (float)ObjectParameterPane.Roll.Value * (float)Math.PI / 180.0f;
            float Pitch = (float)ObjectParameterPane.Pitch.Value * (float)Math.PI / 180.0f;
            float Yaw = (float)ObjectParameterPane.Yaw.Value * (float)Math.PI / 180.0f;
            dummyObject.YawPitchRoll = new Vector3(Roll, Pitch, Yaw);

            float ScaleX = (float)ObjectParameterPane.ScaleX.Value;
            float ScaleY = (float)ObjectParameterPane.ScaleY.Value;
            float ScaleZ = (float)ObjectParameterPane.ScaleZ.Value;
            dummyObject.Scale = new Vector3(ScaleX, ScaleY, ScaleZ);

            dummyObject.Height = (float)ObjectParameterPane.Height.Value;

            dummyObject.Floating = ObjectParameterPane.Floating.Checked;
        }

        private void TextureHandler(object sender, EventArgs e)
        {
            PictureBox pictureBox = (TextureSelectionPane as TextureSelectionForm).TexturePreview as PictureBox;
            Texture2D texture = AssetLibrary.LookupSprite(TextureSelectionPane.TextureList.SelectedItem.ToString());
            RenderTarget2D transformedTexture = new RenderTarget2D(GraphicsManager.Device, pictureBox.Width, pictureBox.Height);

            GraphicsManager.Device.SetRenderTarget(transformedTexture);
            GraphicsManager.Device.Clear(Color.CornflowerBlue);

            float widthReducedScale = (float)texture.Width / (float)pictureBox.Width;
            float uScale = widthReducedScale * (float)TextureSelectionPane.UScale.Value;

            float heightReducedScale = (float)texture.Height / (float)pictureBox.Height;
            float vScale = heightReducedScale * (float)TextureSelectionPane.VScale.Value;

            float uOffset = (float)TextureSelectionPane.UOffset.Value;
            float vOffset = (float)TextureSelectionPane.VOffset.Value;

            mTextureTransformShader.Parameters["Texture"].SetValue(texture);
            mTextureTransformShader.Parameters["UVScale"].SetValue(new Vector2(uScale, vScale));
            mTextureTransformShader.Parameters["UVOffset"].SetValue(new Vector2(uOffset, vOffset));

            GraphicsManager.SpriteBatch.Begin(SpriteSortMode.FrontToBack, null, SamplerState.LinearWrap, null, null, mTextureTransformShader);
            GraphicsManager.SpriteBatch.Draw(texture, new Rectangle(0, 0, texture.Width, texture.Height), Color.White);
            GraphicsManager.SpriteBatch.End();

            GraphicsManager.Device.SetRenderTarget(null);

            //Store data from texture in to image handle.
            MemoryStream ms = new MemoryStream();

            transformedTexture.SaveAsPng(ms, transformedTexture.Width, transformedTexture.Height);

            ms.Seek(0, SeekOrigin.Begin);

            System.Drawing.Image bmp = System.Drawing.Bitmap.FromStream(ms);

            ms.Close();
            ms = null;
            pictureBox.Image = bmp;
        }

        private void CursorResizeHandler(object sender, EventArgs e)
        {
            mCursorObject.Scale = new Vector3((int)(sender as NumericUpDown).Value, 0, (int)(sender as NumericUpDown).Value);
        }

        #endregion

        #region Update Helpers

        private void PerformActions(GameTime gameTime)
        {
            mTimeSinceUndo += gameTime.ElapsedGameTime.TotalSeconds;

            if (!mIsActive)
            {
                return;
            }

            EditorForm form = EditorPane as EditorForm;

            if (mControls.Control.Active && mTimeSinceUndo > UndoTimeLimit)
            {
                if (mControls.Undo.Active)
                {
                    mTimeSinceUndo = 0.0;
                    mDummyWorld.UndoHeightMap();
                }
                else if (mControls.Redo.Active)
                {
                    mTimeSinceUndo = 0.0;
                    mDummyWorld.RedoHeightMap();
                }
            }

            if (!mPlaceable)
            {
                return;
            }

            mDummyWorld.NewHeightMapAction = mDummyWorld.NewHeightMapAction || !mControls.LeftHold.Active;

            TabControl editModes = (EditorPane.Controls["EditTabs"] as TabControl);

            if (mControls.LeftPressed.Active && form.Mode == EditorForm.EditorMode.OBJECTS)
            {
                ObjectParameterPane.SelectedObjects.Clear();
                DummyObject dummy = Entity.GetPickingLocation(mDummyWorld).Item2;
                if (dummy != null)
                {
                    ObjectParameterPane.SelectedObjects.Add(dummy);
                    ObjectParameterPane.UpdateParameterFields();
                }
            }
            else if (mControls.LeftHold.Active)
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
                        TextureSelectionForm textureForm = TextureSelectionPane as TextureSelectionForm;
                        float alpha = form.Strength / 100.0f;
                        GameConstructLibrary.TerrainTexture.TextureLayer layer = (GameConstructLibrary.TerrainTexture.TextureLayer)(form.PaintingLayers);
                        string textureName = (textureForm.TextureList as ListBox).SelectedItem.ToString();

                        float uOffset = (float)textureForm.UOffset.Value, vOffset = (float)textureForm.VOffset.Value;
                        float uScale = (float)textureForm.UScale.Value, vScale = (float)textureForm.VScale.Value;

                        mDummyWorld.ModifyTextureMap(
                            mCursorObject.Position, 
                            textureName, 
                            new Vector2(uOffset, vOffset), 
                            new Vector2(uScale, vScale), 
                            form.Size, alpha,
                            form.PaintingBrush, 
                            form.PaintingTool, 
                            layer);

                        break;
                    }
                }
            }
        }

        #endregion
    }
}
