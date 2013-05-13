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

        //Dialog for object parameters.
        public ObjectParametersForm ObjectParameterPane = null;

        public ObjectPlacementPanel ObjectPlacementPane = null;

        public TextureSelectionForm TextureSelectionPane = null;

        public BlockLayerSelectionForm BlockLayerSelectionForm = null;

        public HeightMapBrushPropertiesForm HeightMapBrushPropertiesPane = null;

        public TextureBrushPropertiesForm TextureBrushPropertiesPane = null;

        public GizmoForm GizmoForm = null;

        public TextureLayerContainerForm TextureLayerPane = null;

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

        private GraphicsDevice mGraphics = null;

        private Dictionary<string, EditorObjectDefinition> mObjectDefinitions = new Dictionary<string, EditorObjectDefinition>(); 

        //Stores all placeable objects.
        private Dictionary<string, DummyObject> mObjects = new Dictionary<string, DummyObject>();

        //Stores all useable textures.
        private Dictionary<string, Texture2D> mTextures = new Dictionary<string, Texture2D>();
        
        //Object that will be drawn at the cursor in object tab.
        private DummyObject mCursorObject = new DummyObject();

        //Determines whether or not you are able to place an object/modify heights or textures.
        private bool mPlaceable;
        
        //Whether the object transformation gizmo is shown.
        private bool IsGizmoVisible = false;

        //Handles input.
        private Controls mControls = new Controls();

        //Stores the objects placed in the world and the height map.
        private DummyWorld mDummyWorld = null;

        private ObjectModificationGizmo mGizmo = null;

        private GameDeviceControl mGameControl = null;

        private EditorForm.Layers mLastLayer = EditorForm.Layers.BACKGROUND;
        private string mLastTexture = "default_terrain_detail";

        #endregion

        #region Public Interface

        public MapEditor(GraphicsDevice graphicsDevice, FPSCamera camera, ContentManager content, GameDeviceControl gameControl, EditorForm editorForm)
        {
            this.EditorForm = editorForm;
            this.HeightMapBrushPropertiesPane = editorForm.HeightMapBrushPropertiesForm;
            this.ObjectParameterPane = editorForm.ObjectParametersForm;
            this.ObjectPlacementPane = editorForm.ObjectPlacementPanel;
            this.TextureBrushPropertiesPane = editorForm.TextureBrushPropertiesForm;
            this.TextureLayerPane = editorForm.TextureLayerForm;
            this.TextureSelectionPane = editorForm.TextureSelectionForm;
            this.BlockLayerSelectionForm = editorForm.BlockLayerSelectionForm;
            this.GizmoForm = editorForm.GizmoForm;
            mGraphics = graphicsDevice;
            mGameControl = gameControl;
            mCamera = camera;
            mDummyWorld = new DummyWorld(mControls);
            mEntity = new Entity(graphicsDevice, mControls, mCamera);
            mGizmo = new ObjectModificationGizmo(mControls, mCamera);
            InitializePanes();

            mTextureTransformShader = content.Load<Effect>("shaders/TextureTransform");

            NewHandler(this, EventArgs.Empty);
        }

        public void Update(GameTime gameTime, bool gameWindowActive)
        {
            mIsActive = gameWindowActive;

            mControls.Update(gameTime, mGameControl.RectangleToScreen(mGameControl.ClientRectangle));
            mDummyWorld.Update(gameTime, mCamera.Position, EditorForm.BlockLayer);

            mPlaceable = false;

            if (mDummyWorld.Name != null)
            {
                mEntity.Update(gameTime);

                var pickingResult = mEntity.GetPickingLocation(mDummyWorld, null);

                mPlaceable = pickingResult != null && mControls.MouseInViewport;

                if (mPlaceable && mCursorObject != null)
                {
                    mCursorObject.Position = pickingResult.Item1.Location;
                }

            }

            PerformActions(gameTime);

            mGizmo.Update(ObjectParameterPane.SelectedObjects, mGraphics.Viewport);
        }

        public void Draw()
        {
            HeightMapRenderable.CursorShape brush = HeightMapRenderable.CursorShape.NONE;

            if (mPlaceable)
            {
                switch (EditorForm.Mode)
                {
                    case EditorForm.EditorMode.HEIGHTMAP:
                        brush = EditorForm.HeightMapBrush == EditorForm.Brushes.BLOCK || EditorForm.HeightMapBrush == EditorForm.Brushes.BLOCK_FEATHERED ?
                            HeightMapRenderable.CursorShape.BLOCK : HeightMapRenderable.CursorShape.CIRCLE;
                        break;
                    case EditorForm.EditorMode.PAINTING:
                        brush = EditorForm.TextureBrush == EditorForm.Brushes.BLOCK || EditorForm.TextureBrush == EditorForm.Brushes.BLOCK_FEATHERED ?
                            HeightMapRenderable.CursorShape.BLOCK : HeightMapRenderable.CursorShape.CIRCLE;
                        break;
                }

                mDummyWorld.CursorPosition = mCursorObject.Position;
                mDummyWorld.CursorInnerRadius = mCursorObject.Scale.X;
                mDummyWorld.CursorOuterRadius = 9.0f / 8.0f * mCursorObject.Scale.X;
                mDummyWorld.DrawCursor = brush;
            }

            Vector4 layerMask = new Vector4(
                EditorForm.TextureLayerForm.Layer1.LayerVisibilityButton.BackgroundImage == null ? 0.0f : 1.0f,
                EditorForm.TextureLayerForm.Layer2.LayerVisibilityButton.BackgroundImage == null ? 0.0f : 1.0f,
                EditorForm.TextureLayerForm.Layer3.LayerVisibilityButton.BackgroundImage == null ? 0.0f : 1.0f,
                EditorForm.TextureLayerForm.Layer4.LayerVisibilityButton.BackgroundImage == null ? 0.0f : 1.0f);
            mDummyWorld.TerrainLayerMask = layerMask;

            if (IsGizmoVisible)
            {
                mGizmo.Draw();
            }

            foreach (DummyObject selectedDummy in ObjectParameterPane.SelectedObjects)
            {
                selectedDummy.IsHighlighted = true;
            }

            mDummyWorld.Draw(mCamera.Position);
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
            EditorForm.ViewWireframeMenu.Click += ViewWireframeHandler;
            EditorForm.UndoMenu.Click += UndoHandler;
            EditorForm.RedoMenu.Click += RedoHandler;

            EditorForm.ModeChanged += UpdateModeContext;
            EditorForm.ToolChanged += UpdateToolContext;

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
            foreach (var model in AssetLibrary.InanimateModelLibrary)
            {
                try
                {
                    EditorObjectDefinition definition = null;
                    if (model.Key != "playerBean")
                    {
                        definition = new EditorObjectDefinition(model.Key, "Chimera.Prop, Chimera", model.Key, new string[0]);
                    }
                    else
                    {
                        definition = new EditorObjectDefinition(model.Key, Utils.PlayerTypeName, model.Key, new string[0]);
                    }

                    mObjectDefinitions.Add(definition.EditorType, definition);
                    mObjects.Add(definition.EditorType, definition.CreateDummyObject());
                    modelNode.Nodes.Add(definition.EditorType);
                }
                catch (SystemException)
                {
                    Console.WriteLine("Couldn't make object from model: " + model.Key + ".");
                }
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
            UpdateToolContext(EditorForm, null);
            CursorResizeHandler(HeightMapBrushPropertiesPane.BrushSizeTrackBar, null);
            UpdateLayerPaneImages(Dialogs.EditorForm.Layers.BACKGROUND, "default_terrain_detail");
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

        private void ViewWireframeHandler(object sender, EventArgs e)
        {
            GraphicsManager.WireframeRendering = GraphicsManager.WireframeRendering == GraphicsManager.Wireframe.Both ? GraphicsManager.Wireframe.Solid : GraphicsManager.Wireframe.Both;
        }

        private void CloseTextureForm(object sender, EventArgs e)
        {
            TextureSelectionPane.Hide();
        }

        private void OpenObjectParameterForm(object sender, EventArgs e)
        {
            if (!ObjectParameterPane.Visible)
            {
                ObjectParameterPane.Show();
            }
        }

        private void CloseObjectParameterForm(object sender, EventArgs e)
        {
            ObjectParameterPane.Hide();
        }

        private void OpenBlockLayerSelectionForm(object sender, EventArgs e)
        {
            if (!BlockLayerSelectionForm.Visible)
            {
                BlockLayerSelectionForm.Show();
            }
        }

        private void CloseBlockLayerSelectionForm(object sender, EventArgs e)
        {
            BlockLayerSelectionForm.Hide();
        }

        private void OpenGizmoForm(object sender, EventArgs e)
        {
            if (!GizmoForm.Visible)
            {
                GizmoForm.Show();
            }
        }

        private void CloseGizmoForm(object sender, EventArgs e)
        {
            GizmoForm.Hide();
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

        private void OpenTextureLayerPane(object sender, EventArgs e)
        {
            if (!TextureLayerPane.Visible)
            {
                TextureLayerPane.Show();
            }
        }

        private void CloseTextureLayerPane(object sender, EventArgs e)
        {
            TextureLayerPane.Hide();
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
            DummyObject player = new DummyObject(mObjects["playerBean"]);
            SetObjectPropertiesToForm(player);
            player.Scale = new Vector3(5.0f);
            player.Position = new Vector3(Level.BLOCK_SIZE / 2.0f, 0.0f, Level.BLOCK_SIZE / 2.0f);

            mDummyWorld.AddObject(player);
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
            else
            {
                SaveHandler(sender, e);
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
                Texture2D previewImage = GraphicsManager.RenderPreviewImage(AssetLibrary.LookupInanimateModel(mObjects[selectedObject.Text].Model));
                MemoryStream ms = new MemoryStream();

                previewImage.SaveAsPng(ms, previewImage.Width, previewImage.Height);
                ms.Seek(0, SeekOrigin.Begin);

                System.Drawing.Image bmp = System.Drawing.Bitmap.FromStream(ms);

                ms.Close();
                ms = null;
                EditorForm.ObjectPlacementPanel.PreviewPictureBox.Image = bmp;
            }
        }

        private void CreateObjectButtonHandler(object sender, EventArgs e)
        {
            CreateNewObjectFromParameters();
        }

        private DummyObject CreateNewObjectFromParameters()
        {
            TreeNode selectedObject = EditorForm.ObjectPlacementPanel.ObjectTree.SelectedNode;
            if (selectedObject != null && mObjects.ContainsKey(selectedObject.Text) && selectedObject.Nodes.Count <= 0)
            {
                ObjectParameterPane.SelectedObjects.Clear();
                DummyObject dummy = new DummyObject(mObjects[selectedObject.Text]);
                ObjectParameterPane.SelectedObjects.Add(dummy);
                SetObjectPropertiesToForm(dummy);
                mDummyWorld.AddObject(dummy);
                return dummy;
            }
            return null;
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
            dummyObject.Rotation = Quaternion.CreateFromYawPitchRoll(Yaw, Pitch, Roll);

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
            Texture2D texture = AssetLibrary.LookupTexture(TextureSelectionPane.TextureList.SelectedItem.ToString());
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
            mDummyWorld.UndoHeightMap();
        }

        private void RedoHandler(object sender, EventArgs e)
        {
            mDummyWorld.RedoHeightMap();
        }

        private void UpdateModeContext(object sender, EventArgs e)
        {
            switch ((sender as EditorForm).Mode)
            {
                case Dialogs.EditorForm.EditorMode.OBJECTS:
                    OpenGizmoForm(this, EventArgs.Empty);
                    CloseBlockLayerSelectionForm(this, EventArgs.Empty);
                    CloseHeightMapBrushPropertiesPane(this, EventArgs.Empty);
                    CloseTextureBrushPropertiesPane(this, EventArgs.Empty);
                    OpenObjectCreationForm(this, EventArgs.Empty);
                    OpenObjectParameterForm(this, EventArgs.Empty);
                    CloseTextureForm(this, EventArgs.Empty);
                    CloseTextureLayerPane(this, EventArgs.Empty);

                    if (ObjectPlacementPane.ObjectTree.SelectedNode == null)
                    {
                        ObjectPlacementPane.ObjectTree.SelectedNode = ObjectPlacementPane.ObjectTree.Nodes[0].Nodes[0];
                        ObjectPlacementPane.ObjectTree.CollapseAll();
                    }
                    break;
                case Dialogs.EditorForm.EditorMode.BLOCKCREATION:
                    CloseGizmoForm(this, EventArgs.Empty);
                    OpenBlockLayerSelectionForm(this, EventArgs.Empty);
                    CloseHeightMapBrushPropertiesPane(this, EventArgs.Empty);
                    CloseTextureBrushPropertiesPane(this, EventArgs.Empty);
                    CloseObjectCreationForm(this, EventArgs.Empty);
                    CloseObjectParameterForm(this, EventArgs.Empty);
                    CloseTextureForm(this, EventArgs.Empty);
                    CloseTextureLayerPane(this, EventArgs.Empty);
                    break;
                case Dialogs.EditorForm.EditorMode.BLOCKSELECTION:
                    CloseGizmoForm(this, EventArgs.Empty);
                    OpenBlockLayerSelectionForm(this, EventArgs.Empty);
                    CloseHeightMapBrushPropertiesPane(this, EventArgs.Empty);
                    CloseTextureBrushPropertiesPane(this, EventArgs.Empty);
                    CloseObjectCreationForm(this, EventArgs.Empty);
                    CloseObjectParameterForm(this, EventArgs.Empty);
                    CloseTextureForm(this, EventArgs.Empty);
                    CloseTextureLayerPane(this, EventArgs.Empty);
                    break;
                case Dialogs.EditorForm.EditorMode.HEIGHTMAP:
                    CloseGizmoForm(this, EventArgs.Empty);
                    CloseBlockLayerSelectionForm(this, EventArgs.Empty);
                    OpenHeightMapBrushPropertiesPane(this, EventArgs.Empty);
                    CloseTextureBrushPropertiesPane(this, EventArgs.Empty);
                    CloseObjectCreationForm(this, EventArgs.Empty);
                    CloseObjectParameterForm(this, EventArgs.Empty);
                    CloseTextureForm(this, EventArgs.Empty);
                    CloseTextureLayerPane(this, EventArgs.Empty);
                    break;
                case Dialogs.EditorForm.EditorMode.PAINTING:
                    CloseGizmoForm(this, EventArgs.Empty);
                    CloseBlockLayerSelectionForm(this, EventArgs.Empty);
                    CloseHeightMapBrushPropertiesPane(this, EventArgs.Empty);
                    OpenTextureBrushPropertiesPane(this, EventArgs.Empty);
                    CloseObjectCreationForm(this, EventArgs.Empty);
                    CloseObjectParameterForm(this, EventArgs.Empty);
                    OpenTextureForm(this, EventArgs.Empty);
                    OpenTextureLayerPane(this, EventArgs.Empty);

                    if (TextureSelectionPane.TextureList.SelectedIndex < 0)
                    {
                        TextureSelectionPane.TextureList.SelectedIndex = 0;
                    }
                    break;
            }
        }

        private void UpdateToolContext(object sender, EventArgs e)
        {
            if ((sender as EditorForm).Tool == Dialogs.EditorForm.Tools.SELECT)
            {
                ShowObjectGizmo(this, EventArgs.Empty);
                OpenGizmoForm(this, EventArgs.Empty);
            }
            else
            {
                HideObjectGizmo(this, EventArgs.Empty);
                CloseGizmoForm(this, EventArgs.Empty);
            }
            if ((sender as EditorForm).Tool == Dialogs.EditorForm.Tools.PLACE)
            {
                OpenObjectCreationForm(this, EventArgs.Empty);
            }
            else
            {
                CloseObjectCreationForm(this, EventArgs.Empty);
            }
        }

        private void ShowObjectGizmo(MapEditor mapEditor, EventArgs eventArgs)
        {
            IsGizmoVisible = true;
        }

        private void HideObjectGizmo(MapEditor mapEditor, EventArgs eventArgs)
        {
            IsGizmoVisible = false;
            ObjectParameterPane.SelectedObjects.Clear();
            ObjectParameterPane.UpdateParameterFields();
        }

        #endregion

        #region Update Helpers

        private void PerformActions(GameTime gameTime)
        {
            mGizmo.Mode = EditorForm.GizmoState;

            if (!mIsActive)
            {
                return;
            }

            if (mControls.Play.Active)
            {
                PlayHandler(this, EventArgs.Empty);
            }

            if (mControls.Control.Active)
            {
                if (mControls.Undo.Active)
                {
                    mDummyWorld.UndoHeightMap();
                }
                else if (mControls.Redo.Active)
                {
                    mDummyWorld.RedoHeightMap();
                }
            }

            if (mControls.LeftReleased.Active && mControls.MouseInViewport)
            {
                switch (EditorForm.Mode)
                {
                    case EditorForm.EditorMode.OBJECTS:
                    {
                        if (!mGizmo.IsDragging)
                        {
                            if (!mControls.Control.Active)
                            {
                                ObjectParameterPane.SelectedObjects.Clear();
                            }
                            List<DummyObject> dummyObjects = Entity.GetObjectsInSelection(mDummyWorld);
                            ObjectParameterPane.SelectedObjects.AddRange(dummyObjects);
                        }
                        ObjectParameterPane.UpdateParameterFields();
                        break;
                    }
                    case Dialogs.EditorForm.EditorMode.BLOCKCREATION:
                    {
                        Ray ray = Utils.CreateWorldRayFromScreenPoint(
                            new Vector2(mControls.MouseState.X, mControls.MouseState.Y),
                            mGraphics.Viewport,
                            mCamera.Position,
                            mCamera.ViewTransform,
                            mCamera.ProjectionTransform);

                        Vector3 coordinate = Utils.ProjectVectorOntoPlane(ray, new Vector3(0, EditorForm.BlockLayer * Level.BLOCK_SIZE, 0), Vector3.Up);
                        mDummyWorld.AddBlock(new Vector3((float)Math.Floor(coordinate.X / Level.BLOCK_SIZE), (float)Math.Floor(coordinate.Y / Level.BLOCK_SIZE), (float)Math.Floor(coordinate.Z / Level.BLOCK_SIZE)));
                        break;
                    }
                    case Dialogs.EditorForm.EditorMode.BLOCKSELECTION:
                    {
                        Ray ray = Utils.CreateWorldRayFromScreenPoint(
                            new Vector2(mControls.MouseState.X, mControls.MouseState.Y),
                            mGraphics.Viewport,
                            mCamera.Position,
                            mCamera.ViewTransform,
                            mCamera.ProjectionTransform);

                        Vector3 coordinate = Utils.ProjectVectorOntoPlane(ray, new Vector3(0, EditorForm.BlockLayer * Level.BLOCK_SIZE, 0), Vector3.Up);
                        coordinate = new Vector3((float)Math.Floor(coordinate.X / Level.BLOCK_SIZE), (float)Math.Floor(coordinate.Y / Level.BLOCK_SIZE), (float)Math.Floor(coordinate.Z / Level.BLOCK_SIZE));
                        if (!mDummyWorld.ContainsBlock(coordinate) ||!mControls.Control.Active)
                        {
                            mDummyWorld.ClearSelectedBlocks();
                        }

                        mDummyWorld.SelectBlock(coordinate);
                        break;
                    }
                }
            }
            else if (mControls.LeftHold.Active)
            {
                switch (EditorForm.Mode)
                {
                    case Dialogs.EditorForm.EditorMode.OBJECTS:
                    {
                        if (!mGizmo.IsDragging && mControls.MouseInViewport)
                        {
                            Entity.HighlightObjectsInSelection();
                        }
                        break;
                    }
                    case EditorForm.EditorMode.HEIGHTMAP:
                    {
                        if (!mPlaceable)
                        {
                            return;
                        }

                        float size = HeightMapBrushPropertiesPane.BrushSizeTrackBar.Value;
                        float strength = HeightMapBrushPropertiesPane.BrushMagnitudeTrackBar.Value / 5.0f;
                        mDummyWorld.ModifyHeightMap(mCursorObject.Position, size, strength, EditorForm.HeightMapBrush, EditorForm.Tool);
                        break;
                    }
                    case EditorForm.EditorMode.PAINTING:
                    {
                        if (!mPlaceable)
                        {
                            return;
                        }

                        bool layerHidden = false;
                        switch (EditorForm.PaintingLayer)
                        {
                            case Dialogs.EditorForm.Layers.LAYER1:
                                layerHidden = TextureLayerPane.Layer1.LayerVisibilityButton.BackgroundImage == null;
                                break;
                            case Dialogs.EditorForm.Layers.LAYER2:
                                layerHidden = TextureLayerPane.Layer2.LayerVisibilityButton.BackgroundImage == null;
                                break;
                            case Dialogs.EditorForm.Layers.LAYER3:
                                layerHidden = TextureLayerPane.Layer3.LayerVisibilityButton.BackgroundImage == null;
                                break;
                            case Dialogs.EditorForm.Layers.LAYER4:
                                layerHidden = TextureLayerPane.Layer4.LayerVisibilityButton.BackgroundImage == null;
                                break;
                        }

                        TextureSelectionForm textureForm = TextureSelectionPane as TextureSelectionForm;
                        if (!layerHidden && (textureForm.TextureList as ListBox).SelectedItem != null)
                        {
                            float size = TextureBrushPropertiesPane.BrushSizeTrackBar.Value;
                            float alpha = TextureBrushPropertiesPane.BrushMagnitudeTrackBar.Value / 100.0f;
                            HeightMapMesh.TextureLayer layer = (HeightMapMesh.TextureLayer)(EditorForm.PaintingLayer);
                            string textureName = EditorForm.Tool == Dialogs.EditorForm.Tools.PAINT ? (textureForm.TextureList as ListBox).SelectedItem.ToString() : null;

                            float uOffset = (float)textureForm.UOffset.Value, vOffset = (float)textureForm.VOffset.Value;
                            float uScale = (float)textureForm.UScale.Value, vScale = (float)textureForm.VScale.Value;

                            mDummyWorld.ModifyTextureMap(mCursorObject.Position, textureName, new Vector2(uOffset, vOffset), new Vector2(uScale, vScale), size, alpha, (EditorForm.Brushes)EditorForm.TextureBrush, EditorForm.Tool, layer);

                            if (EditorForm.Tool == EditorForm.Tools.PAINT && (mLastTexture != textureName || mLastLayer != EditorForm.PaintingLayer))
                            {
                                mLastTexture = textureName;
                                mLastLayer = EditorForm.PaintingLayer;
                                UpdateLayerPaneImages(mLastLayer, mLastTexture);
                            }
                        }
                        break;
                    }
                }
            }
            if (mControls.LeftPressed.Active)
            {
                switch (EditorForm.Tool)
                {
                    case Dialogs.EditorForm.Tools.PLACE:
                        if (mPlaceable)
                        {
                            DummyObject dummyObject = CreateNewObjectFromParameters();
                            if (dummyObject != null)
                            {
                                dummyObject.Position = mCursorObject.Position;
                            }
                        }
                        break;
                }
            }
            if (mControls.Delete.Active)
            {
                switch (EditorForm.Mode)
                {
                    case Dialogs.EditorForm.EditorMode.OBJECTS:
                    {
                        foreach (DummyObject dummyObject in ObjectParameterPane.SelectedObjects)
                        {
                            mDummyWorld.RemoveObject(dummyObject);
                        }
                        ObjectParameterPane.SelectedObjects.Clear();
                        break;
                    }
                    case Dialogs.EditorForm.EditorMode.BLOCKSELECTION:
                    {
                        mDummyWorld.RemoveSelectedBlocks();
                        break;
                    }
                }
            }
        }

        private void UpdateLayerPaneImages(EditorForm.Layers layer, string textureName)
        {
            MemoryStream ms = new MemoryStream();
            Texture2D texture = AssetLibrary.LookupTexture(textureName);

            texture.SaveAsPng(ms, 30, 30);

            ms.Seek(0, SeekOrigin.Begin);

            System.Drawing.Image bmp = System.Drawing.Bitmap.FromStream(ms);

            ms.Close();
            ms = null;

            switch (layer)
            {
                case Dialogs.EditorForm.Layers.BACKGROUND:
                    TextureLayerPane.BackgroundLayer.LayerTexturePreview.Image = bmp;
                    break;
                case Dialogs.EditorForm.Layers.LAYER1:
                    TextureLayerPane.Layer1.LayerTexturePreview.Image = bmp;
                    break;
                case Dialogs.EditorForm.Layers.LAYER2:
                    TextureLayerPane.Layer2.LayerTexturePreview.Image = bmp;
                    break;
                case Dialogs.EditorForm.Layers.LAYER3:
                    TextureLayerPane.Layer3.LayerTexturePreview.Image = bmp;
                    break;
                case Dialogs.EditorForm.Layers.LAYER4:
                    TextureLayerPane.Layer4.LayerTexturePreview.Image = bmp;
                    break;
            }
        }

        #endregion
    }
}
