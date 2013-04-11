using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections.ObjectModel;

namespace WorldEditor.Dialogs
{
    public partial class ToolMenu : UserControl
    {
        #region Enums

        public enum EditorMode { HEIGHTMAP, PAINTING, OBJECTS };

        public enum Tools { RAISE, LOWER, SET, SMOOTH, FLATTEN, PAINT, ERASE, BLEND, SELECT, TRANSLATE, ROTATE, SCALE, NONE };

        public enum Brushes { CIRCLE, CIRCLE_FEATHERED, BLOCK, BLOCK_FEATHERED, NONE };
        
        public enum Layers { BACKGROUND, LAYER1, LAYER2, LAYER3, LAYER4, NONE };

        #endregion

        #region State

        public EditorMode Mode = EditorMode.HEIGHTMAP;

        public Tools Tool = Tools.RAISE;
        public Brushes HeightMapBrush = Brushes.CIRCLE;

        public Brushes PaintingBrush = Brushes.CIRCLE;
        public Layers PaintingLayers = Layers.BACKGROUND;

        #endregion

        private readonly ReadOnlyCollection<ReadOnlyCollection<ToolStripButton>> mToolGroups;

        private readonly ReadOnlyCollection<ToolStripButton> mTerrainTools;
        private readonly ReadOnlyCollection<ToolStripButton> mTextureTools;
        private readonly ReadOnlyCollection<ToolStripButton> mObjectTools;

        public ToolMenu()
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
                selectObjectButton,
                translateObjectButton,
                rotateObjectButton,
                scaleObjectButton
            }.AsReadOnly();

            mToolGroups = new List<ReadOnlyCollection<ToolStripButton>>
            {
                mTerrainTools,
                mTextureTools,
                mObjectTools
            }.AsReadOnly();

            SetButtonImages();
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

            this.selectObjectButton.Tag    = new Tuple<EditorMode, Tools>(EditorMode.OBJECTS, Tools.SELECT);
            this.translateObjectButton.Tag = new Tuple<EditorMode, Tools>(EditorMode.OBJECTS, Tools.TRANSLATE);
            this.rotateObjectButton.Tag    = new Tuple<EditorMode, Tools>(EditorMode.OBJECTS, Tools.ROTATE);
            this.scaleObjectButton.Tag     = new Tuple<EditorMode, Tools>(EditorMode.OBJECTS, Tools.SCALE);
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

            this.selectObjectButton.Image    = UILibrary.ObjectModeButtonIcon;
            this.translateObjectButton.Image = UILibrary.ObjectModeButtonIcon;
            this.rotateObjectButton.Image    = UILibrary.ObjectModeButtonIcon;
            this.scaleObjectButton.Image     = UILibrary.ObjectModeButtonIcon;
        }

        private void toolButton_Click(object sender, EventArgs e)
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
}
