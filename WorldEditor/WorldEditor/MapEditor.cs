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

    public class MapEditor
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
        public EditorForm EditorForm = null;

        public EditorForm EditorPane = new EditorForm();

        //Dialog for object parameters.
        public ObjectParametersForm ObjectParameterPane = null;

        public ObjectPlacementPanel ObjectPlacementPane = null;

        public TextureSelectionForm TextureSelectionPane = null;

        public BrushSelectionForm BrushSelectionPane = null;

        public HeightMapBrushPropertiesForm HeightMapBrushPropertiesPane = null;

        public TextureBrushPropertiesForm TextureBrushPropertiesPane = null;

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

        private Dictionary<string, EditorObjectDefinition> mObjectDefinitions = new Dictionary<string, EditorObjectDefinition>(); 

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

        private GameDeviceControl mGameControl = null;

        private double mTimeSinceUndo = 0.0;

        #endregion

        #region Public Interface

        public MapEditor(GraphicsDevice graphicsDevice, FPSCamera camera, ContentManager content, GameDeviceControl gameControl, EditorForm editorForm)
        {
            this.EditorForm = editorForm;
            this.BrushSelectionPane = editorForm.BrushSelectionForm;
            this.HeightMapBrushPropertiesPane = editorForm.HeightMapBrushPropertiesForm;
            this.ObjectParameterPane = editorForm.ObjectParametersForm;
            this.ObjectPlacementPane = editorForm.ObjectPlacementPanel;
            this.TextureBrushPropertiesPane = editorForm.TextureBrushPropertiesForm;
            this.TextureSelectionPane = editorForm.TextureSelectionForm;
            mGameControl = gameControl;
            mCamera = camera;
            mDummyWorld = new DummyWorld(mControls);
            mEntity = new Entity(graphicsDevice, mControls, mCamera);
            InitializePanes();

            mTextureTransformShader = content.Load<Effect>("shaders/TextureTransform");
        }

        public void Update(GameTime gameTime, bool gameWindowActive)
        {
            mIsActive = gameWindowActive;

            mControls.Update(gameTime, mGameControl.RectangleToScreen(mGameControl.ClientRectangle).Location);
            mDummyWorld.Update(gameTime, mCamera.Position);

            mPlaceable = false;

            if (mDummyWorld.Name != null)
            {
                mEntity.Update(gameTime);

                var pickingResult = mEntity.GetPickingLocation(mDummyWorld, null);

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
                switch (EditorForm.Mode)
                {
                    case EditorForm.EditorMode.HEIGHTMAP:
                        brush = EditorForm.HeightMapBrush == EditorForm.Brushes.BLOCK || EditorForm.HeightMapBrush == EditorForm.Brushes.BLOCK_FEATHERED ? 
                            TerrainRenderable.CursorShape.BLOCK : TerrainRenderable.CursorShape.CIRCLE;
                        break;
                    case EditorForm.EditorMode.PAINTING:
                        brush = EditorForm.PaintingBrush == EditorForm.Brushes.BLOCK || EditorForm.HeightMapBrush == EditorForm.Brushes.BLOCK_FEATHERED ? 
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

        private void InitializePanes()
        {
            EditorForm.NewMenu.Click    += NewHandler;
            EditorForm.SaveMenu.Click += SaveHandler;
            EditorForm.SaveAsMenu.Click += SaveAsHandler;
            EditorForm.OpenMenu.Click += OpenHandler;
            EditorForm.PlayMenu.Click += PlayHandler;
            EditorForm.ViewWaterMenu.Click += ViewWaterHandler;
            EditorForm.ViewSkyBoxMenu.Click += ViewSkyBoxHandler;
            EditorForm.ViewShadowsMenu.Click += ViewShadowsHandler;
            EditorForm.UndoMenu.Click += UndoHandler;
            EditorForm.RedoMenu.Click += RedoHandler;

            EditorForm.ModeChanged += UpdateModeContext;

            ObjectParameterPane.Show();
            ObjectParameterPane.Dock = DockStyle.Left;

            TextureSelectionPane.TextureList.SelectedIndexChanged += TextureHandler;
            TextureSelectionPane.UOffset.ValueChanged += TextureHandler;
            TextureSelectionPane.VOffset.ValueChanged += TextureHandler;
            TextureSelectionPane.UScale.ValueChanged += TextureHandler;
            TextureSelectionPane.VScale.ValueChanged += TextureHandler;

            EditorForm.ObjectPlacementPanel.ObjectTree.NodeMouseDoubleClick += CreateObjectButtonHandler;
            EditorForm.ObjectPlacementPanel.ObjectTree.AfterSelect += UpdatePreviewImage;

            HeightMapBrushPropertiesPane.BrushSizeTrackBar.ValueChanged += CursorResizeHandler;
            TextureBrushPropertiesPane.BrushSizeTrackBar.ValueChanged += CursorResizeHandler;

            TreeNode modelNode = new TreeNode("Models");
            TreeNode entityNode = new TreeNode("Entities");
            EditorForm.ObjectPlacementPanel.ObjectTree.Nodes.Add(modelNode);
            EditorForm.ObjectPlacementPanel.ObjectTree.Nodes.Add(entityNode);
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
                modelNode.Nodes.Add(new TreeNode(tempObject.Model));
            }

            FileInfo[] objects = (new DirectoryInfo(ContentPath + "/" + ObjectsPath + "/")).GetFiles();
            foreach (FileInfo file in objects)
            {
                try
                {
                    EditorObjectDefinition definition = new EditorObjectDefinition(file);

                    mObjectDefinitions.Add(definition.EditorType, definition);
                    mObjects.Add(definition.EditorType, definition.CreateDummyObject());
                    entityNode.Nodes.Add(definition.EditorType);
                }
                catch (SystemException)
                {
                    Console.WriteLine("Formatting error in object: " + file.Name + ".");
                }
            }

            foreach (var texture in AssetLibrary.TextureLibrary)
            {
                (TextureSelectionPane.TextureList as ListBox).Items.Add(texture.Key);
            }

            UpdateModeContext(EditorForm, null);
            CursorResizeHandler(HeightMapBrushPropertiesPane.BrushSizeTrackBar, null);
        }

        private void CloseHandler(object sender, EventArgs e)
        {
            this.Closed = true;
        }

        private void OpenTextureForm(object sender, EventArgs e)
        {
            if (!TextureSelectionPane.Visible)
            {
                TextureSelectionPane.Show();
            }
        }

        private void ViewWaterHandler(object sender, EventArgs e)
        {
            mDummyWorld.DrawWater = !mDummyWorld.DrawWater;
        }

        private void ViewSkyBoxHandler(object sender, EventArgs e)
        {
            mDummyWorld.DrawSkyBox = !mDummyWorld.DrawSkyBox;
        }

        private void ViewShadowsHandler(object sender, EventArgs e)
        {
            GraphicsManager.CastingShadows = !GraphicsManager.CastingShadows;
        }

        private void CloseTextureForm(object sender, EventArgs e)
        {
            TextureSelectionPane.Hide();
        }

        private void OpenObjectParameterForm(object sender, EventArgs e)
        {
            if (!ObjectParameterPane.Visible)
            {
                ObjectParameterPane.Show(/*(Form)Form.FromHandle(Window.Handle)*/);
            }
        }

        private void CloseObjectParameterForm(object sender, EventArgs e)
        {
            ObjectParameterPane.Hide();
        }

        private void OpenBrushSelectionForm(object sender, EventArgs e)
        {
            if (!BrushSelectionPane.Visible)
            {
                BrushSelectionPane.Show();
            }
        }

        private void CloseBrushSelectionForm(object sender, EventArgs e)
        {
            BrushSelectionPane.Hide();
        }

        private void OpenHeightMapBrushPropertiesPane(object sender, EventArgs e)
        {
            if (!HeightMapBrushPropertiesPane.Visible)
            {
                HeightMapBrushPropertiesPane.Show();
            }
        }

        private void CloseHeightMapBrushPropertiesPane(object sender, EventArgs e)
        {
            HeightMapBrushPropertiesPane.Hide();
        }

        private void OpenTextureBrushPropertiesPane(object sender, EventArgs e)
        {
            if (!TextureBrushPropertiesPane.Visible)
            {
                TextureBrushPropertiesPane.Show();
            }
        }

        private void CloseTextureBrushPropertiesPane(object sender, EventArgs e)
        {
            TextureBrushPropertiesPane.Hide();
        }

        private void OpenObjectCreationForm(object sender, EventArgs e)
        {
            if (!ObjectPlacementPane.Visible)
            {
                ObjectPlacementPane.Show();
            }
        }

        private void CloseObjectCreationForm(object sender, EventArgs e)
        {
            ObjectPlacementPane.Hide();
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

        private void SelectNewObjectHandler(object sender, EventArgs e)
        {
            ObjectParameterPane.Show(/*(Form)Form.FromHandle(Window.Handle)*/);
        }
                
        private void UpdatePreviewImage(object sender, EventArgs e)
        {
            TreeNode selectedObject = EditorForm.ObjectPlacementPanel.ObjectTree.SelectedNode;
            if (mObjects.ContainsKey(selectedObject.Text) && selectedObject.Nodes.Count == 0)
            {
                Texture2D previewImage = GraphicsManager.RenderPreviewImage(AssetLibrary.LookupModel(mObjects[selectedObject.Text].Model));
                MemoryStream ms = new MemoryStream();

                previewImage.SaveAsPng(ms, previewImage.Width, previewImage.Height);
                ms.Seek(0, SeekOrigin.Begin);

                System.Drawing.Image bmp = System.Drawing.Bitmap.FromStream(ms);

                ms.Close();
                ms = null;
                EditorForm.ObjectPlacementPanel.PreviewPictureBox.BackgroundImage = bmp;
                EditorForm.ObjectPlacementPanel.PreviewPictureBox.BackgroundImageLayout = ImageLayout.Stretch;
            }
        }

        private void CreateObjectButtonHandler(object sender, EventArgs e)
        {
            TreeNode selectedObject = EditorForm.ObjectPlacementPanel.ObjectTree.SelectedNode;
            if (selectedObject != null && mObjects.ContainsKey(selectedObject.Text) && selectedObject.Nodes.Count <= 0)
            {
                ObjectParameterPane.SelectedObjects.Clear();
                DummyObject dummy = new DummyObject(mObjects[selectedObject.Text]);
                ObjectParameterPane.SelectedObjects.Add(dummy);
                SetObjectPropertiesToForm(dummy);
                mDummyWorld.AddObject(dummy);
            }
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

            dummyObject.Height = (float)ObjectParameterPane.FloatingHeight.Value;

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
            mCursorObject.Scale = new Vector3((int)(sender as TrackBar).Value, 0, (int)(sender as TrackBar).Value);
        }

        private void UndoHandler(object sender, EventArgs e)
        {
            if (mTimeSinceUndo > UndoTimeLimit)
            {
                mTimeSinceUndo = 0.0;
                mDummyWorld.UndoHeightMap();
            }
        }

        private void RedoHandler(object sender, EventArgs e)
        {
            if (mTimeSinceUndo > UndoTimeLimit)
            {
                mTimeSinceUndo = 0.0;
                mDummyWorld.RedoHeightMap();
            }
        }

        private void UpdateModeContext(object sender, EventArgs e)
        {
            switch ((sender as EditorForm).Mode)
            {
                case Dialogs.EditorForm.EditorMode.OBJECTS:
                    CloseBrushSelectionForm(this, EventArgs.Empty);
                    CloseHeightMapBrushPropertiesPane(this, EventArgs.Empty);
                    CloseTextureBrushPropertiesPane(this, EventArgs.Empty);
                    OpenObjectCreationForm(this, EventArgs.Empty);
                    OpenObjectParameterForm(this, EventArgs.Empty);
                    CloseTextureForm(this, EventArgs.Empty);

                    if (ObjectPlacementPane.ObjectTree.SelectedNode == null)
                    {
                        ObjectPlacementPane.ObjectTree.SelectedNode = ObjectPlacementPane.ObjectTree.Nodes[0].Nodes[0];
                        ObjectPlacementPane.ObjectTree.CollapseAll();
                    }
                    break;
                case Dialogs.EditorForm.EditorMode.HEIGHTMAP:
                    OpenBrushSelectionForm(this, EventArgs.Empty);
                    OpenHeightMapBrushPropertiesPane(this, EventArgs.Empty);
                    CloseTextureBrushPropertiesPane(this, EventArgs.Empty);
                    CloseObjectCreationForm(this, EventArgs.Empty);
                    CloseObjectParameterForm(this, EventArgs.Empty);
                    CloseTextureForm(this, EventArgs.Empty);
                    break;
                case Dialogs.EditorForm.EditorMode.PAINTING:
                    OpenBrushSelectionForm(this, EventArgs.Empty);
                    CloseHeightMapBrushPropertiesPane(this, EventArgs.Empty);
                    OpenTextureBrushPropertiesPane(this, EventArgs.Empty);
                    CloseObjectCreationForm(this, EventArgs.Empty);
                    CloseObjectParameterForm(this, EventArgs.Empty);
                    OpenTextureForm(this, EventArgs.Empty);

                    if (TextureSelectionPane.TextureList.SelectedIndex < 0)
                    {
                        TextureSelectionPane.TextureList.SelectedIndex = 0;
                    }
                    break;
            }
        }

        private void UpdateToolContext(object sender, EventArgs e)
        {
            if ((sender as EditorForm).Tool == Dialogs.EditorForm.Tools.PLACE)
            {
                OpenObjectCreationForm(this, EventArgs.Empty);
            }
            else
            {
                CloseObjectCreationForm(this, EventArgs.Empty);
            }
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

            if (mControls.LeftReleased.Active && EditorForm.Mode == EditorForm.EditorMode.OBJECTS)
            {
                foreach (DummyObject oldObject in ObjectParameterPane.SelectedObjects)
                {
                    oldObject.IsHighlighted = false;
                }
                ObjectParameterPane.SelectedObjects.Clear();

                List<DummyObject> dummyObjects = Entity.GetObjectsInSelection(mDummyWorld);
                foreach (DummyObject newObject in dummyObjects)
                {
                    newObject.IsHighlighted = true;
                }

                ObjectParameterPane.SelectedObjects.AddRange(dummyObjects);
                    ObjectParameterPane.UpdateParameterFields();
            }
            else if (mControls.LeftHold.Active)
            {
                switch (EditorForm.Mode)
                {
                    case EditorForm.EditorMode.HEIGHTMAP:
                    {
                        float size = HeightMapBrushPropertiesPane.BrushSizeTrackBar.Value;
                        float strength = HeightMapBrushPropertiesPane.BrushMagnitudeTrackBar.Value * 10.0f;
                        mDummyWorld.ModifyHeightMap(mCursorObject.Position, size, strength, EditorForm.HeightMapBrush, EditorForm.Tool);
                        break;
                    }
                    case EditorForm.EditorMode.PAINTING:
                    {
                        TextureSelectionForm textureForm = TextureSelectionPane as TextureSelectionForm;
                        if ((textureForm.TextureList as ListBox).SelectedItem != null)
                        {
                            float alpha = /*EditorPane.Strength*/50 / 100.0f; // TODO: Fixit
                            GameConstructLibrary.TerrainTexture.TextureLayer layer = (GameConstructLibrary.TerrainTexture.TextureLayer)(/*EditorPane.PaintingLayers*/GameConstructLibrary.TerrainTexture.TextureLayer.BACKGROUND);
                            string textureName = (textureForm.TextureList as ListBox).SelectedItem.ToString();

                            float uOffset = (float)textureForm.UOffset.Value, vOffset = (float)textureForm.VOffset.Value;
                            float uScale = (float)textureForm.UScale.Value, vScale = (float)textureForm.VScale.Value;

                            mDummyWorld.ModifyTextureMap(
                                mCursorObject.Position,
                                textureName,
                                new Vector2(uOffset, vOffset),
                                new Vector2(uScale, vScale),
                                /*EditorPane.Size*/1, alpha,// TODO Fixit
                                (EditorForm.Brushes)/*EditorPane.PaintingBrush*/EditorForm.Brushes.CIRCLE,
                                EditorForm.Tool,
                                layer);
                        }
                        break;
                    }
                }
            }
        }

        #endregion
    }
}