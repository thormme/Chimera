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

        public enum EditorMode { HEIGHTMAP, PAINTING, OBJECTS };

        public enum Tools { RAISE, LOWER, SET, SMOOTH, FLATTEN, PAINT, ERASE, BLEND, SELECT, PLACE, ROTATE, SCALE, NONE };

        public enum Brushes { CIRCLE, CIRCLE_FEATHERED, BLOCK, BLOCK_FEATHERED, NONE };
        
        public enum Layers { BACKGROUND, LAYER1, LAYER2, LAYER3, LAYER4, NONE };

        #endregion

        #region State

        private EditorMode mMode = EditorMode.HEIGHTMAP;
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

        private Tools mTool = Tools.RAISE;
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

        public Brushes PaintingBrush = Brushes.CIRCLE;
        public Layers PaintingLayers = Layers.BACKGROUND;

        #endregion

        private readonly ReadOnlyCollection<ReadOnlyCollection<ToolStripButton>> mToolGroups;

        private readonly ReadOnlyCollection<ToolStripButton> mTerrainTools;
        private readonly ReadOnlyCollection<ToolStripButton> mTextureTools;
        private readonly ReadOnlyCollection<ToolStripButton> mObjectTools;

        public EditorForm()
        {
            InitializeComponent();

            InitializeButtonState();

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
                TranslateObjectButton,
                RotateObjectButton,
                ScaleObjectButton
            }.AsReadOnly();

            mToolGroups = new List<ReadOnlyCollection<ToolStripButton>>
            {
                mTerrainTools,
                mTextureTools,
                mObjectTools
            }.AsReadOnly();

            SetButtonImages();

            this.ToolStrip.Renderer = new HardEdgeToolStripRenderer();
            this.toolStrip1.Renderer = new HardEdgeToolStripRenderer();

            ObjectParametersForm.SizeChanged += delegate { ObjectParameterFormContainer.Height = ObjectParametersForm.Height; };
            ObjectParametersForm.VisibleChanged += UpdateParameterFormVisibility;
        }

        private void UpdateParameterFormVisibility(object sender, EventArgs e)
        {
            // Cannot simply set to parent visibility, if the parent is not visible, neither is the child.
            ObjectParameterFormContainer.Height = (sender as ObjectParametersForm).Visible ? (sender as ObjectParametersForm).Height : 0; 
        }

        private void InitializeButtonState()
        {
            this.raiseTerrainButton.Checked = true;

            this.raiseTerrainButton.Tag    = new Tuple<EditorMode, Tools>(EditorMode.HEIGHTMAP, Tools.RAISE);
            this.lowerTerrainButton.Tag    = new Tuple<EditorMode, Tools>(EditorMode.HEIGHTMAP, Tools.LOWER);
            this.setTerrainButton.Tag      = new Tuple<EditorMode, Tools>(EditorMode.HEIGHTMAP, Tools.SET);
            this.flattenTerrainButton.Tag  = new Tuple<EditorMode, Tools>(EditorMode.HEIGHTMAP, Tools.FLATTEN);
            this.smoothTerrainButton.Tag   = new Tuple<EditorMode, Tools>(EditorMode.HEIGHTMAP, Tools.SMOOTH);

            this.paintTextureButton.Tag    = new Tuple<EditorMode, Tools>(EditorMode.PAINTING, Tools.PAINT);
            this.eraseTextureButton.Tag    = new Tuple<EditorMode, Tools>(EditorMode.PAINTING, Tools.ERASE);
            this.smoothTextureButton.Tag   = new Tuple<EditorMode, Tools>(EditorMode.PAINTING, Tools.BLEND);

            this.SelectObjectButton.Tag    = new Tuple<EditorMode, Tools>(EditorMode.OBJECTS, Tools.SELECT);
            this.TranslateObjectButton.Tag = new Tuple<EditorMode, Tools>(EditorMode.OBJECTS, Tools.PLACE);
            this.RotateObjectButton.Tag    = new Tuple<EditorMode, Tools>(EditorMode.OBJECTS, Tools.ROTATE);
            this.ScaleObjectButton.Tag     = new Tuple<EditorMode, Tools>(EditorMode.OBJECTS, Tools.SCALE);
        }

        private void SetButtonImages()
        {
            this.raiseTerrainButton.Image   = UILibrary.RaiseTerrainIcon;
            this.lowerTerrainButton.Image   = UILibrary.LowerTerrainIcon;
            this.setTerrainButton.Image     = UILibrary.SetTerrainIcon;
            this.flattenTerrainButton.Image = UILibrary.FlattenTerrainIcon;
            this.smoothTerrainButton.Image  = UILibrary.SmoothTerrainIcon;

            this.paintTextureButton.Image  = UILibrary.PaintingModeButtonIcon;
            this.eraseTextureButton.Image  = UILibrary.LowerTerrainIcon;
            this.smoothTextureButton.Image = UILibrary.SmoothTerrainIcon;

            this.SelectObjectButton.Image    = UILibrary.ObjectModeButtonIcon;
            this.TranslateObjectButton.Image = UILibrary.ObjectModeButtonIcon;
            this.RotateObjectButton.Image    = UILibrary.ObjectModeButtonIcon;
            this.ScaleObjectButton.Image     = UILibrary.ObjectModeButtonIcon;

            this.BrushSelectionForm.CircleBrushButton.Image        = UILibrary.CircleBrushIcon;
            this.BrushSelectionForm.CircleFeatherBrushButton.Image = UILibrary.CircleFeatheredBrushIcon;
            this.BrushSelectionForm.BlockBrushButton.Image         = UILibrary.BlockBrushIcon;
            this.BrushSelectionForm.BlockFeatherBrushButton.Image  = UILibrary.BlockFeatheredBrushIcon;
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
    }

    public class HardEdgeToolStripRenderer : ToolStripProfessionalRenderer
    {
        public HardEdgeToolStripRenderer()
        {
            this.RoundedEdges = false;
        }
    }
}
