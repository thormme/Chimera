using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WorldEditor.Dialogs
{
    public partial class EditorForm : Form
    {
        #region Events

        public delegate void ModeChangedHandler(object sender, EventArgs e);
        public event ModeChangedHandler ModeChanged;

        public delegate void ToolChangedHandler(object sender, EventArgs e);
        public event ModeChangedHandler ToolChanged;

        #endregion

        #region Enums

        public enum EditorMode { HEIGHTMAP, PAINTING, OBJECTS, BLOCKSELECTION, BLOCKCREATION };

        public enum Tools { RAISE, LOWER, SET, SMOOTH, FLATTEN, PAINT, ERASE, BLEND, SELECT, PLACE, BLOCKSELECT, BLOCKCREATE, NONE };

        public enum Brushes { CIRCLE, CIRCLE_FEATHERED, BLOCK, BLOCK_FEATHERED, NONE };

        public enum Layers { BACKGROUND, LAYER1, LAYER2, LAYER3, LAYER4, NONE };

        #endregion

        #region State

        private EditorMode mMode = EditorMode.OBJECTS;
        public EditorMode Mode
        {
            get
            {
                return mMode;
            }
            set
            {
                bool changed = mMode != value;
                mMode = value;
                if (changed && ModeChanged != null)
                {
                    ModeChanged(this, EventArgs.Empty);
                }
            }
        }

        private Tools mTool = Tools.SELECT;
        public Tools Tool
        {
            get
            {
                return mTool;
            }
            set
            {
                bool changed = mTool != value;
                mTool = value;
                if (changed && ToolChanged != null)
                {
                    ToolChanged(this, EventArgs.Empty);
                }
            }
        }
        public Brushes HeightMapBrush = Brushes.CIRCLE;
        public Brushes TextureBrush = Brushes.CIRCLE;

        public ObjectModificationGizmo.ModificationMode GizmoState = ObjectModificationGizmo.ModificationMode.TRANSLATE;

        public Layers PaintingLayer = Layers.BACKGROUND;

        public int BlockLayer = 0;

        #endregion

        private readonly ReadOnlyCollection<ReadOnlyCollection<ToolStripButton>> mToolGroups;

        private readonly ReadOnlyCollection<ToolStripButton> mTerrainTools;
        private readonly ReadOnlyCollection<ToolStripButton> mTextureTools;
        private readonly ReadOnlyCollection<ToolStripButton> mObjectTools;
        private readonly ReadOnlyCollection<ToolStripButton> mBlockTools;

        private readonly ReadOnlyCollection<ToolStripButton> mHeightMapBrushes;
        private readonly ReadOnlyCollection<ToolStripButton> mTextureBrushes;
        private readonly ReadOnlyCollection<ToolStripButton> mGizmoStates;

        private readonly ReadOnlyCollection<TextureLayerForm> mLayers;

        public EditorForm()
        {
            InitializeComponent();

            InitializeButtonState();

            this.HeightMapBrushPropertiesForm.CircleBrushButton.Click += HeightMapBrushButton_Click;
            this.HeightMapBrushPropertiesForm.CircleFeatherBrushButton.Click += HeightMapBrushButton_Click;
            this.HeightMapBrushPropertiesForm.BlockBrushButton.Click += HeightMapBrushButton_Click;
            this.HeightMapBrushPropertiesForm.BlockFeatherBrushButton.Click += HeightMapBrushButton_Click;

            this.TextureBrushPropertiesForm.CircleBrushButton.Click += TextureBrushButton_Click;
            this.TextureBrushPropertiesForm.CircleFeatherBrushButton.Click += TextureBrushButton_Click;
            this.TextureBrushPropertiesForm.BlockBrushButton.Click += TextureBrushButton_Click;
            this.TextureBrushPropertiesForm.BlockFeatherBrushButton.Click += TextureBrushButton_Click;

            this.TextureLayerForm.BackgroundLayer.InvisibleButton.Click += Layer_Click;
            this.TextureLayerForm.BackgroundLayer.LayerTexturePreview.Click += Layer_Click;
            this.TextureLayerForm.Layer1.InvisibleButton.Click += Layer_Click;
            this.TextureLayerForm.Layer1.LayerTexturePreview.Click += Layer_Click;
            this.TextureLayerForm.Layer2.InvisibleButton.Click += Layer_Click;
            this.TextureLayerForm.Layer2.LayerTexturePreview.Click += Layer_Click;
            this.TextureLayerForm.Layer3.InvisibleButton.Click += Layer_Click;
            this.TextureLayerForm.Layer3.LayerTexturePreview.Click += Layer_Click;
            this.TextureLayerForm.Layer4.InvisibleButton.Click += Layer_Click;
            this.TextureLayerForm.Layer4.LayerTexturePreview.Click += Layer_Click;

            this.TextureLayerForm.Layer1.LayerVisibilityButton.Click += Layer_Visibility_Click;
            this.TextureLayerForm.Layer2.LayerVisibilityButton.Click += Layer_Visibility_Click;
            this.TextureLayerForm.Layer3.LayerVisibilityButton.Click += Layer_Visibility_Click;
            this.TextureLayerForm.Layer4.LayerVisibilityButton.Click += Layer_Visibility_Click;

            mTerrainTools = new List<ToolStripButton>()
            {
                raiseTerrainButton,
                lowerTerrainButton,
                setTerrainButton,
                flattenTerrainButton,
                smoothTerrainButton
            }.AsReadOnly();

            mTextureTools = new List<ToolStripButton>()
            {
                paintTextureButton,
                eraseTextureButton,
                smoothTextureButton
            }.AsReadOnly();

            mObjectTools = new List<ToolStripButton>()
            {
                SelectObjectButton,
                CreateObjectButton
            }.AsReadOnly();

            mBlockTools = new List<ToolStripButton>()
            {
                SelectBlockButton,
                CreateBlockButton
            }.AsReadOnly();

            mToolGroups = new List<ReadOnlyCollection<ToolStripButton>>
            {
                mTerrainTools,
                mTextureTools,
                mObjectTools,
                mBlockTools
            }.AsReadOnly();

            mLayers = new List<TextureLayerForm>
            {
                TextureLayerForm.BackgroundLayer,
                TextureLayerForm.Layer1,
                TextureLayerForm.Layer2,
                TextureLayerForm.Layer3,
                TextureLayerForm.Layer4
            }.AsReadOnly();

            mHeightMapBrushes = new List<ToolStripButton>
            {
                HeightMapBrushPropertiesForm.CircleBrushButton,
                HeightMapBrushPropertiesForm.CircleFeatherBrushButton,
                HeightMapBrushPropertiesForm.BlockBrushButton,
                HeightMapBrushPropertiesForm.BlockFeatherBrushButton
            }.AsReadOnly();

            mTextureBrushes = new List<ToolStripButton>
            {
                TextureBrushPropertiesForm.CircleBrushButton,
                TextureBrushPropertiesForm.CircleFeatherBrushButton,
                TextureBrushPropertiesForm.BlockBrushButton,
                TextureBrushPropertiesForm.BlockFeatherBrushButton
            }.AsReadOnly();

            mGizmoStates = new List<ToolStripButton>
            {
                GizmoForm.TranslateButton,
                GizmoForm.RotateButton,
                GizmoForm.ScaleButton
            }.AsReadOnly();

            SetButtonImages();

            this.ToolStrip.Renderer = new HardEdgeToolStripRenderer();

            ObjectParametersForm.SizeChanged += delegate { ObjectParameterFormContainer.Height = ObjectParametersForm.Height; };
            ObjectParametersForm.VisibleChanged += UpdateParameterFormVisibility;

            BlockLayerSelectionForm.BlockLayerUpDown.ValueChanged += Block_Layer_Changed;
        }

        private void UpdateParameterFormVisibility(object sender, EventArgs e)
        {
            // Cannot simply set to parent visibility, if the parent is not visible, neither is the child.
            ObjectParameterFormContainer.Height = (sender as ObjectParametersForm).Visible ? (sender as ObjectParametersForm).Height : 0; 
        }

        private void InitializeButtonState()
        {
            this.SelectObjectButton.Checked = true;

            this.raiseTerrainButton.Tag = new Tuple<EditorMode, Tools>(EditorMode.HEIGHTMAP, Tools.RAISE);
            this.lowerTerrainButton.Tag = new Tuple<EditorMode, Tools>(EditorMode.HEIGHTMAP, Tools.LOWER);
            this.setTerrainButton.Tag = new Tuple<EditorMode, Tools>(EditorMode.HEIGHTMAP, Tools.SET);
            this.flattenTerrainButton.Tag = new Tuple<EditorMode, Tools>(EditorMode.HEIGHTMAP, Tools.FLATTEN);
            this.smoothTerrainButton.Tag = new Tuple<EditorMode, Tools>(EditorMode.HEIGHTMAP, Tools.SMOOTH);

            this.paintTextureButton.Tag = new Tuple<EditorMode, Tools>(EditorMode.PAINTING, Tools.PAINT);
            this.eraseTextureButton.Tag = new Tuple<EditorMode, Tools>(EditorMode.PAINTING, Tools.ERASE);
            this.smoothTextureButton.Tag = new Tuple<EditorMode, Tools>(EditorMode.PAINTING, Tools.BLEND);

            this.SelectObjectButton.Tag = new Tuple<EditorMode, Tools>(EditorMode.OBJECTS, Tools.SELECT);
            this.CreateObjectButton.Tag = new Tuple<EditorMode, Tools>(EditorMode.OBJECTS, Tools.PLACE);

            this.SelectBlockButton.Tag = new Tuple<EditorMode, Tools>(EditorMode.BLOCKSELECTION, Tools.BLOCKSELECT);
            this.CreateBlockButton.Tag = new Tuple<EditorMode, Tools>(EditorMode.BLOCKCREATION, Tools.BLOCKCREATE);

            this.TextureLayerForm.BackgroundLayer.Tag = new Layers?(Layers.BACKGROUND);
            this.TextureLayerForm.Layer1.Tag = new Layers?(Layers.LAYER1);
            this.TextureLayerForm.Layer2.Tag = new Layers?(Layers.LAYER2);
            this.TextureLayerForm.Layer3.Tag = new Layers?(Layers.LAYER3);
            this.TextureLayerForm.Layer4.Tag = new Layers?(Layers.LAYER4);

            this.HeightMapBrushPropertiesForm.CircleBrushButton.Tag = new Brushes?(Brushes.CIRCLE);
            this.HeightMapBrushPropertiesForm.CircleFeatherBrushButton.Tag = new Brushes?(Brushes.CIRCLE_FEATHERED);
            this.HeightMapBrushPropertiesForm.BlockBrushButton.Tag = new Brushes?(Brushes.BLOCK);
            this.HeightMapBrushPropertiesForm.BlockFeatherBrushButton.Tag = new Brushes?(Brushes.BLOCK_FEATHERED);

            this.TextureBrushPropertiesForm.CircleBrushButton.Tag = new Brushes?(Brushes.CIRCLE);
            this.TextureBrushPropertiesForm.CircleFeatherBrushButton.Tag = new Brushes?(Brushes.CIRCLE_FEATHERED);
            this.TextureBrushPropertiesForm.BlockBrushButton.Tag = new Brushes?(Brushes.BLOCK);
            this.TextureBrushPropertiesForm.BlockFeatherBrushButton.Tag = new Brushes?(Brushes.BLOCK_FEATHERED);

            this.GizmoForm.TranslateButton.Tag = new ObjectModificationGizmo.ModificationMode?(ObjectModificationGizmo.ModificationMode.TRANSLATE);
            this.GizmoForm.RotateButton.Tag = new ObjectModificationGizmo.ModificationMode?(ObjectModificationGizmo.ModificationMode.ROTATE);
            this.GizmoForm.ScaleButton.Tag = new ObjectModificationGizmo.ModificationMode?(ObjectModificationGizmo.ModificationMode.SCALE);
        }

        private void SetButtonImages()
        {
            this.raiseTerrainButton.Image   = UILibrary.RaiseTerrainIcon;
            this.lowerTerrainButton.Image   = UILibrary.LowerTerrainIcon;
            this.setTerrainButton.Image     = UILibrary.SetTerrainIcon;
            this.flattenTerrainButton.Image = UILibrary.FlattenTerrainIcon;
            this.smoothTerrainButton.Image  = UILibrary.SmoothTerrainIcon;

            this.paintTextureButton.Image  = UILibrary.PaintBrushIcon;
            this.eraseTextureButton.Image  = UILibrary.EraserIcon;
            this.smoothTextureButton.Image = UILibrary.SpongeIcon;

            this.SelectObjectButton.Image  = UILibrary.ObjectSelectionIcon;
            this.CreateObjectButton.Image  = UILibrary.NewObjectIcon;

            this.HeightMapBrushPropertiesForm.CircleBrushButton.Image        = UILibrary.CircleBrushIcon;
            this.HeightMapBrushPropertiesForm.CircleFeatherBrushButton.Image = UILibrary.CircleFeatheredBrushIcon;
            this.HeightMapBrushPropertiesForm.BlockBrushButton.Image         = UILibrary.BlockBrushIcon;
            this.HeightMapBrushPropertiesForm.BlockFeatherBrushButton.Image  = UILibrary.BlockFeatheredBrushIcon;

            this.TextureBrushPropertiesForm.CircleBrushButton.Image = UILibrary.CircleBrushIcon;
            this.TextureBrushPropertiesForm.CircleFeatherBrushButton.Image = UILibrary.CircleFeatheredBrushIcon;
            this.TextureBrushPropertiesForm.BlockBrushButton.Image = UILibrary.BlockBrushIcon;
            this.TextureBrushPropertiesForm.BlockFeatherBrushButton.Image = UILibrary.BlockFeatheredBrushIcon;

            this.TextureLayerForm.BackgroundLayer.LayerTexturePreview.Image = UILibrary.InvalidIcon;
            this.TextureLayerForm.Layer1.LayerTexturePreview.Image = UILibrary.InvalidIcon;
            this.TextureLayerForm.Layer2.LayerTexturePreview.Image = UILibrary.InvalidIcon;
            this.TextureLayerForm.Layer3.LayerTexturePreview.Image = UILibrary.InvalidIcon;
            this.TextureLayerForm.Layer4.LayerTexturePreview.Image = UILibrary.InvalidIcon;

            this.TextureLayerForm.BackgroundLayer.LayerVisibilityButton.BackgroundImage = UILibrary.VisibleLayerIcon;
            this.TextureLayerForm.BackgroundLayer.LayerVisibilityButton.Tag             = this.TextureLayerForm.BackgroundLayer.LayerVisibilityButton.BackgroundImage;
            
            this.TextureLayerForm.Layer1.LayerVisibilityButton.BackgroundImage = UILibrary.VisibleLayerIcon;
            this.TextureLayerForm.Layer1.LayerVisibilityButton.Tag             = this.TextureLayerForm.Layer1.LayerVisibilityButton.BackgroundImage;
            
            this.TextureLayerForm.Layer2.LayerVisibilityButton.BackgroundImage = UILibrary.VisibleLayerIcon;
            this.TextureLayerForm.Layer2.LayerVisibilityButton.Tag             = this.TextureLayerForm.Layer2.LayerVisibilityButton.BackgroundImage;
            
            this.TextureLayerForm.Layer3.LayerVisibilityButton.BackgroundImage = UILibrary.VisibleLayerIcon;
            this.TextureLayerForm.Layer3.LayerVisibilityButton.Tag             = this.TextureLayerForm.Layer3.LayerVisibilityButton.BackgroundImage;

            this.TextureLayerForm.Layer4.LayerVisibilityButton.BackgroundImage = UILibrary.VisibleLayerIcon;
            this.TextureLayerForm.Layer4.LayerVisibilityButton.Tag             = this.TextureLayerForm.Layer4.LayerVisibilityButton.BackgroundImage;

            this.TextureLayerForm.BackgroundLayer.MainPanel.BackColor = SystemColors.Highlight;
        }

        private void ToolButton_Click(object sender, EventArgs e)
        {
            Tuple<EditorMode, Tools> state = null;

            foreach (ReadOnlyCollection<ToolStripButton> group in mToolGroups)
            {
                foreach (ToolStripButton button in group)
                {
                    button.Checked = button == sender;
                    if (button.Checked == true)
                    {
                        state = button.Tag as Tuple<EditorMode, Tools>;
                    }
                }
            }

            this.Mode = state.Item1;
            this.Tool = state.Item2;
        }

        private void HeightMapBrushButton_Click(object sender, EventArgs e)
        {
            foreach (ToolStripButton button in mHeightMapBrushes)
            {
                button.Checked = button == sender;
                if (button.Checked == true)
                {
                    HeightMapBrush = (button.Tag as Brushes?).Value;
                }
            }
        }

        private void TextureBrushButton_Click(object sender, EventArgs e)
        {
            foreach (ToolStripButton button in mTextureBrushes)
            {
                button.Checked = button == sender;
                if (button.Checked == true)
                {
                    TextureBrush = (button.Tag as Brushes?).Value;
                }
            }
        }

        private void GizmoStateButton_Click(object sender, EventArgs e)
        {
            foreach (ToolStripButton button in mGizmoStates)
            {
                button.Checked = button == sender;
                if (button.Checked == true)
                {
                    GizmoState = (button.Tag as ObjectModificationGizmo.ModificationMode?).Value;
                }
            }
        }

        private void Layer_Click(object sender, EventArgs e)
        {
            foreach (TextureLayerForm layer in mLayers)
            {
                layer.MainPanel.BackColor = SystemColors.Window;
                if (layer.InvisibleButton == sender || layer.LayerTexturePreview == sender)
                {
                    this.PaintingLayer = (layer.Tag as Layers?).Value;
                    layer.MainPanel.BackColor = SystemColors.Highlight;
                }
            }
        }

        private void Layer_Visibility_Click(object sender, EventArgs e)
        {
            Button visibilityButton = sender as Button;
            visibilityButton.BackgroundImage = visibilityButton.BackgroundImage == null ? visibilityButton.BackgroundImage = visibilityButton.Tag as Image : null;
        }

        private void Block_Layer_Changed(object sender, EventArgs e)
        {
            NumericUpDown blockLayerControl = sender as NumericUpDown;
            this.BlockLayer = (int)blockLayerControl.Value;
        }
    }
    
    public class HardEdgeToolStripRenderer : ToolStripProfessionalRenderer
    {
        public HardEdgeToolStripRenderer()
        {
            this.RoundedEdges = false;
        }
    }
}
