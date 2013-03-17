using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Utility;

namespace WorldEditor.Dialogs
{
    public partial class EditorForm : Form
    {
        #region Enums

        public enum EditorMode { HEIGHTMAP, PAINTING, OBJECTS };

        public enum HeightMapTools { RAISE, LOWER, SET, SMOOTH, FLATTEN, NONE };

        public enum Brushes { CIRCLE, CIRCLE_FEATHERED, BLOCK, BLOCK_FEATHERED, NONE };

        public enum PaintingTools { BRUSH, ERASER, NONE };

        public enum Layers { BACKGROUND, LAYER1, LAYER2, LAYER3, LAYER4, NONE };

        #endregion

        #region State

        public EditorMode Mode = EditorMode.HEIGHTMAP;

        public HeightMapTools HeightMapTool = HeightMapTools.RAISE;
        public Brushes HeightMapBrush = Brushes.CIRCLE;

        public PaintingTools PaintingTool = PaintingTools.BRUSH;
        public Brushes PaintingBrush = Brushes.CIRCLE;
        public Layers PaintingLayers = Layers.BACKGROUND;

        #endregion

        #region Public Properties

        public float Size
        {
            get { return (float)sizeUpDown.Value; }
        }

        public float Strength
        {
            get { return (float)strengthUpDown.Value; }
        }

        #endregion

        #region Images

        Image heightMapBackground = Image.FromFile(@"D:\Users\Josh\My Documents\GitHub\eecs494FinalProject\WorldEditor\WorldEditor\Artwork\heightmap_background.png");
        Image paintingBackground = Image.FromFile(@"D:\Users\Josh\My Documents\GitHub\eecs494FinalProject\WorldEditor\WorldEditor\Artwork\painting_background.png");

        Image heightmapModeButtonIcon = Image.FromFile(@"D:\Users\Josh\My Documents\GitHub\eecs494FinalProject\WorldEditor\WorldEditor\Artwork\heightmap_icon.png");
        Image heightmapModeButtonSelectedIcon = Image.FromFile(@"D:\Users\Josh\My Documents\GitHub\eecs494FinalProject\WorldEditor\WorldEditor\Artwork\heightmap_icon_selected.png");

        Image paintingModeButtonIcon = Image.FromFile(@"D:\Users\Josh\My Documents\GitHub\eecs494FinalProject\WorldEditor\WorldEditor\Artwork\painting_icon.png");
        Image paintingModeButtonSelectedIcon = Image.FromFile(@"D:\Users\Josh\My Documents\GitHub\eecs494FinalProject\WorldEditor\WorldEditor\Artwork\painting_icon_selected.png");

        Image objectModeButtonIcon = Image.FromFile(@"D:\Users\Josh\My Documents\GitHub\eecs494FinalProject\WorldEditor\WorldEditor\Artwork\object_icon.png");
        Image objectModeButtonSelectedIcon = Image.FromFile(@"D:\Users\Josh\My Documents\GitHub\eecs494FinalProject\WorldEditor\WorldEditor\Artwork\object_icon_selected.png");

        Image circleBrushIcon = Image.FromFile(@"D:\Users\Josh\My Documents\GitHub\eecs494FinalProject\WorldEditor\WorldEditor\Artwork\circle_brush_icon.png");
        Image circleBrushSelectedIcon = Image.FromFile(@"D:\Users\Josh\My Documents\GitHub\eecs494FinalProject\WorldEditor\WorldEditor\Artwork\circle_brush_icon_selected.png");

        Image circleFeatheredBrushIcon = Image.FromFile(@"D:\Users\Josh\My Documents\GitHub\eecs494FinalProject\WorldEditor\WorldEditor\Artwork\circle_brush_feathered_icon.png");
        Image circleFeatheredBrushSelectedIcon = Image.FromFile(@"D:\Users\Josh\My Documents\GitHub\eecs494FinalProject\WorldEditor\WorldEditor\Artwork\circle_brush_feathered_icon_selected.png");

        Image blockBrushIcon = Image.FromFile(@"D:\Users\Josh\My Documents\GitHub\eecs494FinalProject\WorldEditor\WorldEditor\Artwork\block_brush_icon.png");
        Image blockBrushSelectedIcon = Image.FromFile(@"D:\Users\Josh\My Documents\GitHub\eecs494FinalProject\WorldEditor\WorldEditor\Artwork\block_brush_icon_selected.png");

        Image blockFeatheredBrushIcon = Image.FromFile(@"D:\Users\Josh\My Documents\GitHub\eecs494FinalProject\WorldEditor\WorldEditor\Artwork\block_brush_feathered_icon.png");
        Image blockFeatheredBrushSelectedIcon = Image.FromFile(@"D:\Users\Josh\My Documents\GitHub\eecs494FinalProject\WorldEditor\WorldEditor\Artwork\block_brush_feathered_icon_selected.png");

        Image raiseTerrainIcon = Image.FromFile(@"D:\Users\Josh\My Documents\GitHub\eecs494FinalProject\WorldEditor\WorldEditor\Artwork\raise_terrain_icon.png");
        Image raiseTerrainSelectedIcon = Image.FromFile(@"D:\Users\Josh\My Documents\GitHub\eecs494FinalProject\WorldEditor\WorldEditor\Artwork\raise_terrain_icon_selected.png");

        Image lowerTerrainIcon = Image.FromFile(@"D:\Users\Josh\My Documents\GitHub\eecs494FinalProject\WorldEditor\WorldEditor\Artwork\lower_terrain_icon.png");
        Image lowerTerrainSelectedIcon = Image.FromFile(@"D:\Users\Josh\My Documents\GitHub\eecs494FinalProject\WorldEditor\WorldEditor\Artwork\lower_terrain_icon_selected.png");

        Image setTerrainIcon = Image.FromFile(@"D:\Users\Josh\My Documents\GitHub\eecs494FinalProject\WorldEditor\WorldEditor\Artwork\set_terrain_icon.png");
        Image setTerrainSelectedIcon = Image.FromFile(@"D:\Users\Josh\My Documents\GitHub\eecs494FinalProject\WorldEditor\WorldEditor\Artwork\set_terrain_icon_selected.png");

        Image smoothTerrainIcon = Image.FromFile(@"D:\Users\Josh\My Documents\GitHub\eecs494FinalProject\WorldEditor\WorldEditor\Artwork\smooth_terrain_icon.png");
        Image smoothTerrainSelectedIcon = Image.FromFile(@"D:\Users\Josh\My Documents\GitHub\eecs494FinalProject\WorldEditor\WorldEditor\Artwork\smooth_terrain_icon_selected.png");

        Image flattenTerrainIcon = Image.FromFile(@"D:\Users\Josh\My Documents\GitHub\eecs494FinalProject\WorldEditor\WorldEditor\Artwork\flatten_terrain_icon.png");
        Image flattenTerrainSelectedIcon = Image.FromFile(@"D:\Users\Josh\My Documents\GitHub\eecs494FinalProject\WorldEditor\WorldEditor\Artwork\flatten_terrain_icon_selected.png");

        Image layerBackgroundIcon = Image.FromFile(@"D:\Users\Josh\My Documents\GitHub\eecs494FinalProject\WorldEditor\WorldEditor\Artwork\layer_background_icon.png");
        Image layerBackgroundSelectedIcon = Image.FromFile(@"D:\Users\Josh\My Documents\GitHub\eecs494FinalProject\WorldEditor\WorldEditor\Artwork\layer_background_icon_selected.png");

        Image layer1Icon = Image.FromFile(@"D:\Users\Josh\My Documents\GitHub\eecs494FinalProject\WorldEditor\WorldEditor\Artwork\layer_1_icon.png");
        Image layer1SelectedIcon = Image.FromFile(@"D:\Users\Josh\My Documents\GitHub\eecs494FinalProject\WorldEditor\WorldEditor\Artwork\layer_1_icon_selected.png");

        Image layer2Icon = Image.FromFile(@"D:\Users\Josh\My Documents\GitHub\eecs494FinalProject\WorldEditor\WorldEditor\Artwork\layer_2_icon.png");
        Image layer2SelectedIcon = Image.FromFile(@"D:\Users\Josh\My Documents\GitHub\eecs494FinalProject\WorldEditor\WorldEditor\Artwork\layer_2_icon_selected.png");

        Image layer3Icon = Image.FromFile(@"D:\Users\Josh\My Documents\GitHub\eecs494FinalProject\WorldEditor\WorldEditor\Artwork\layer_3_icon.png");
        Image layer3SelectedIcon = Image.FromFile(@"D:\Users\Josh\My Documents\GitHub\eecs494FinalProject\WorldEditor\WorldEditor\Artwork\layer_3_icon_selected.png");

        Image layer4Icon = Image.FromFile(@"D:\Users\Josh\My Documents\GitHub\eecs494FinalProject\WorldEditor\WorldEditor\Artwork\layer_4_icon.png");
        Image layer4SelectedIcon = Image.FromFile(@"D:\Users\Josh\My Documents\GitHub\eecs494FinalProject\WorldEditor\WorldEditor\Artwork\layer_4_icon_selected.png");

        #endregion

        public EditorForm()
        {
            InitializeComponent();
            SelectHeightMapEditorMode();

            UpdateBrushIcons();
            UpdateLayerIcons();
            UpdateToolIcons();
        }

        private void SelectHeightMapEditorMode()
        {
            Mode = EditorMode.HEIGHTMAP;

            this.terrainModeButton.BackgroundImage = heightmapModeButtonSelectedIcon;
            this.paintModeButton.BackgroundImage   = paintingModeButtonIcon;
            this.objectModeButton.BackgroundImage  = objectModeButtonIcon;

            this.background.Visible = true;
            this.background.BackgroundImage = heightMapBackground;

            this.ObjectList.Visible = false;
            this.ObjectList.Enabled = false;

            this.SizeUpDown.Visible = true;
            this.SizeUpDown.Enabled = true;

            this.StrengthUpDown.Visible = true;
            this.StrengthUpDown.Enabled = true;
        }

        private void SelectPaintingMode()
        {
            Mode = EditorMode.PAINTING;

            this.terrainModeButton.BackgroundImage = heightmapModeButtonIcon;
            this.paintModeButton.BackgroundImage   = paintingModeButtonSelectedIcon;
            this.objectModeButton.BackgroundImage  = objectModeButtonIcon;

            this.background.Visible = true;
            this.background.BackgroundImage = paintingBackground;

            this.ObjectList.Visible = false;
            this.ObjectList.Enabled = false;

            this.SizeUpDown.Visible = true;
            this.SizeUpDown.Enabled = true;

            this.StrengthUpDown.Visible = true;
            this.StrengthUpDown.Enabled = true;
        }

        private void SelectObjectPlacementMode()
        {
            Mode = EditorMode.OBJECTS;

            this.terrainModeButton.BackgroundImage = heightmapModeButtonIcon;
            this.paintModeButton.BackgroundImage   = paintingModeButtonIcon;
            this.objectModeButton.BackgroundImage  = objectModeButtonSelectedIcon;

            this.background.Visible = false;

            this.ObjectList.Visible = true;
            this.ObjectList.Enabled = true;

            this.SizeUpDown.Visible = false;
            this.SizeUpDown.Enabled = false;

            this.StrengthUpDown.Visible = false;
            this.StrengthUpDown.Enabled = false;
        }

        private void UpdateBrushIcons()
        {
            if (Mode == EditorMode.OBJECTS)
            {
                circleBrushButton.Enabled = false;
                circleBrushButton.Visible = false;

                circleFeatherBrushButton.Enabled = false;
                circleFeatherBrushButton.Visible = false;

                blockBrushButton.Enabled = false;
                blockBrushButton.Visible = false;

                blockFeatherBrushButton.Enabled = false;
                blockFeatherBrushButton.Visible = false;

                return;
            }
            else
            {
                circleBrushButton.BackgroundImage = circleBrushIcon;
                circleBrushButton.Enabled = true;
                circleBrushButton.Visible = true;

                circleFeatherBrushButton.BackgroundImage = circleFeatheredBrushIcon;
                circleFeatherBrushButton.Enabled = true;
                circleFeatherBrushButton.Visible = true;

                blockBrushButton.BackgroundImage = blockBrushIcon;
                blockBrushButton.Enabled = true;
                blockBrushButton.Visible = true;

                blockFeatherBrushButton.BackgroundImage = blockFeatheredBrushIcon;
                blockFeatherBrushButton.Enabled = true;
                blockFeatherBrushButton.Visible = true;
            }

            Brushes selectedBrush = Brushes.NONE;
            switch (Mode)
            {
                case EditorMode.HEIGHTMAP:
                    selectedBrush = HeightMapBrush;
                    break;
                case EditorMode.PAINTING:
                    selectedBrush = PaintingBrush;
                    break;
            }

            switch (selectedBrush)
            {
                case Brushes.BLOCK:
                    blockBrushButton.BackgroundImage = blockBrushSelectedIcon;
                    break;
                case Brushes.BLOCK_FEATHERED:
                    blockFeatherBrushButton.BackgroundImage = blockFeatheredBrushSelectedIcon;
                    break;
                case Brushes.CIRCLE:
                    circleBrushButton.BackgroundImage = circleBrushSelectedIcon;
                    break;
                case Brushes.CIRCLE_FEATHERED:
                    circleFeatherBrushButton.BackgroundImage = circleFeatheredBrushSelectedIcon;
                    break;
            }
        }

        private void UpdateHeightMapToolIcons()
        {
            toolButton0.BackgroundImage = raiseTerrainIcon;
            toolButton0.Enabled = true;
            toolButton0.Visible = true;

            toolButton1.BackgroundImage = lowerTerrainIcon;
            toolButton1.Enabled = true;
            toolButton1.Visible = true;

            toolButton2.BackgroundImage = setTerrainIcon;
            toolButton2.Enabled = true;
            toolButton2.Visible = true;
            
            toolButton3.BackgroundImage = smoothTerrainIcon;
            toolButton3.Enabled = true;
            toolButton3.Visible = true;

            toolButton4.BackgroundImage = flattenTerrainIcon;
            toolButton4.Enabled = true;
            toolButton4.Visible = true;

            switch (HeightMapTool)
            {
                case HeightMapTools.RAISE:
                    toolButton0.BackgroundImage = raiseTerrainSelectedIcon;
                    break;
                case HeightMapTools.LOWER:
                    toolButton1.BackgroundImage = lowerTerrainSelectedIcon;
                    break;
                case HeightMapTools.SET:
                    toolButton2.BackgroundImage = setTerrainSelectedIcon;
                    break;
                case HeightMapTools.SMOOTH:
                    toolButton3.BackgroundImage = smoothTerrainSelectedIcon;
                    break;
                case HeightMapTools.FLATTEN:
                    toolButton4.BackgroundImage = flattenTerrainSelectedIcon;
                    break;
            }
        }

        private void UpdatePaintingToolIcons()
        {
            toolButton0.BackgroundImage = raiseTerrainIcon;
            toolButton0.Enabled = true;
            toolButton0.Visible = true;

            toolButton1.BackgroundImage = lowerTerrainIcon;
            toolButton1.Enabled = true;
            toolButton1.Visible = true;

            switch (PaintingTool)
            {
                case PaintingTools.BRUSH:
                    toolButton0.BackgroundImage = raiseTerrainSelectedIcon;
                    break;
                case PaintingTools.ERASER:
                    toolButton1.BackgroundImage = lowerTerrainSelectedIcon;
                    break;
            }
        }

        private void UpdateToolIcons()
        {
            toolButton0.Enabled = false;
            toolButton0.Visible = false;

            toolButton1.Enabled = false;
            toolButton1.Visible = false;

            toolButton2.Enabled = false;
            toolButton2.Visible = false;

            toolButton3.Enabled = false;
            toolButton3.Visible = false;

            toolButton4.Enabled = false;
            toolButton4.Visible = false;

            switch (Mode)
            {
                case EditorMode.HEIGHTMAP:
                    UpdateHeightMapToolIcons();
                    break;
                case EditorMode.PAINTING:
                    UpdatePaintingToolIcons();
                    break;
            }
        }

        private void UpdateLayerIcons()
        {
            if (Mode == EditorMode.PAINTING)
            {
                layerBackgroundButton.BackgroundImage = layerBackgroundIcon;
                layerBackgroundButton.Enabled = true;
                layerBackgroundButton.Visible = true;

                layer1Button.BackgroundImage = layer1Icon;
                layer1Button.Enabled = true;
                layer1Button.Visible = true;

                layer2Button.BackgroundImage = layer2Icon;
                layer2Button.Enabled = true;
                layer2Button.Visible = true;

                layer3Button.BackgroundImage = layer3Icon;
                layer3Button.Enabled = true;
                layer3Button.Visible = true;

                layer4Button.BackgroundImage = layer4Icon;
                layer4Button.Enabled = true;
                layer4Button.Visible = true;

                switch (PaintingLayers)
                {
                    case Layers.BACKGROUND:
                        layerBackgroundButton.BackgroundImage = layerBackgroundSelectedIcon;
                        break;
                    case Layers.LAYER1:
                        layer1Button.BackgroundImage = layer1SelectedIcon;
                        break;
                    case Layers.LAYER2:
                        layer2Button.BackgroundImage = layer2SelectedIcon;
                        break;
                    case Layers.LAYER3:
                        layer3Button.BackgroundImage = layer3SelectedIcon;
                        break;
                    case Layers.LAYER4:
                        layer4Button.BackgroundImage = layer4SelectedIcon;
                        break;
                }
            }
            else
            {
                layerBackgroundButton.Enabled = false;
                layerBackgroundButton.Visible = false;

                layer1Button.Enabled = false;
                layer1Button.Visible = false;

                layer2Button.Enabled = false;
                layer2Button.Visible = false;

                layer3Button.Enabled = false;
                layer3Button.Visible = false;

                layer4Button.Enabled = false;
                layer4Button.Visible = false;
            }
        }

        private void raiseButton_CheckedChanged(object sender, EventArgs e)
        {
            TabControl editModes = (Controls["EditTabs"] as TabControl);
            NumericUpDown intensity = editModes.SelectedTab.Controls["HeightIntensityField"] as NumericUpDown;

            intensity.Enabled = true;

            intensity.Increment = (decimal)(5.0f / Utils.WorldScale.Y);
            intensity.Minimum = (decimal)0.0f;
            intensity.Maximum = (decimal)(100.0f / Utils.WorldScale.Y);
            intensity.Value = (decimal)(20.0f / Utils.WorldScale.Y);
        }

        private void lowerButton_CheckedChanged(object sender, EventArgs e)
        {
            TabControl editModes = (Controls["EditTabs"] as TabControl);
            NumericUpDown intensity = editModes.SelectedTab.Controls["HeightIntensityField"] as NumericUpDown;

            intensity.Enabled = true;

            intensity.Increment = (decimal)(5.0f / Utils.WorldScale.Y);
            intensity.Minimum = (decimal)(-100.0f / Utils.WorldScale.Y);
            intensity.Maximum = (decimal)0.0f;
            intensity.Value = (decimal)(-20.0f / Utils.WorldScale.Y);
        }

        private void setButton_CheckedChanged(object sender, EventArgs e)
        {
            TabControl editModes = (Controls["EditTabs"] as TabControl);
            NumericUpDown intensity = editModes.SelectedTab.Controls["HeightIntensityField"] as NumericUpDown;

            intensity.Enabled = true;

            intensity.Increment = (decimal)(10.0f / Utils.WorldScale.Y);
            intensity.Minimum = (decimal)0.0f;
            intensity.Maximum = (decimal)(1000.0f / Utils.WorldScale.Y);
            intensity.Value = (decimal)(20.0f / Utils.WorldScale.Y);
        }

        private void flattenButton_CheckedChanged(object sender, EventArgs e)
        {
            TabControl editModes = (Controls["EditTabs"] as TabControl);
            NumericUpDown intensity = editModes.SelectedTab.Controls["HeightIntensityField"] as NumericUpDown;

            intensity.Enabled = false;
        }

        private void smoothButton_CheckedChanged(object sender, EventArgs e)
        {
            TabControl editModes = (Controls["EditTabs"] as TabControl);
            NumericUpDown intensity = editModes.SelectedTab.Controls["HeightIntensityField"] as NumericUpDown;

            intensity.Enabled = false;
        }

        private void terrainModeButton_Click(object sender, EventArgs e)
        {
            SelectHeightMapEditorMode();

            UpdateBrushIcons();
            UpdateToolIcons();
            UpdateLayerIcons();
        }

        private void paintModeButton_Click(object sender, EventArgs e)
        {
            SelectPaintingMode();

            UpdateBrushIcons();
            UpdateToolIcons();
            UpdateLayerIcons();
        }

        private void objectModeButton_Click(object sender, EventArgs e)
        {
            SelectObjectPlacementMode();

            UpdateBrushIcons();
            UpdateToolIcons();
            UpdateLayerIcons();
        }

        private void circleBrushButton_Click(object sender, EventArgs e)
        {
            switch (Mode)
            {
                case EditorMode.HEIGHTMAP:
                    HeightMapBrush = Brushes.CIRCLE;
                    break;
                case EditorMode.PAINTING:
                    PaintingBrush = Brushes.CIRCLE;
                    break;
            }

            UpdateBrushIcons();
        }

        private void circleFeatherBrushButton_Click(object sender, EventArgs e)
        {
            switch (Mode)
            {
                case EditorMode.HEIGHTMAP:
                    HeightMapBrush = Brushes.CIRCLE_FEATHERED;
                    break;
                case EditorMode.PAINTING:
                    PaintingBrush = Brushes.CIRCLE_FEATHERED;
                    break;
            }

            UpdateBrushIcons();
        }

        private void blockBrushButton_Click(object sender, EventArgs e)
        {
            switch (Mode)
            {
                case EditorMode.HEIGHTMAP:
                    HeightMapBrush = Brushes.BLOCK;
                    break;
                case EditorMode.PAINTING:
                    PaintingBrush = Brushes.BLOCK;
                    break;
            }

            UpdateBrushIcons();
        }

        private void blockFeatherBrushButton_Click(object sender, EventArgs e)
        {
            switch (Mode)
            {
                case EditorMode.HEIGHTMAP:
                    HeightMapBrush = Brushes.BLOCK_FEATHERED;
                    break;
                case EditorMode.PAINTING:
                    PaintingBrush = Brushes.BLOCK_FEATHERED;
                    break;
            }

            UpdateBrushIcons();
        }

        private void toolButton0_Click(object sender, EventArgs e)
        {
            switch (Mode)
            {
                case EditorMode.HEIGHTMAP:
                    HeightMapTool = HeightMapTools.RAISE;
                    break;
                case EditorMode.PAINTING:
                    PaintingTool = PaintingTools.BRUSH;
                    break;
            }

            UpdateToolIcons();
        }

        private void toolButton1_Click(object sender, EventArgs e)
        {
            switch (Mode)
            {
                case EditorMode.HEIGHTMAP:
                    HeightMapTool = HeightMapTools.LOWER;
                    break;
                case EditorMode.PAINTING:
                    PaintingTool = PaintingTools.ERASER;
                    break;
            }

            UpdateToolIcons();
        }

        private void toolButton2_Click(object sender, EventArgs e)
        {
            switch (Mode)
            {
                case EditorMode.HEIGHTMAP:
                    HeightMapTool = HeightMapTools.SET;
                    break;
                case EditorMode.PAINTING:
                    break;
            }

            UpdateToolIcons();
        }

        private void toolButton3_Click(object sender, EventArgs e)
        {
            switch (Mode)
            {
                case EditorMode.HEIGHTMAP:
                    HeightMapTool = HeightMapTools.SMOOTH;
                    break;
                case EditorMode.PAINTING:
                    break;
            }

            UpdateToolIcons();
        }

        private void toolButton4_Click(object sender, EventArgs e)
        {
            switch (Mode)
            {
                case EditorMode.HEIGHTMAP:
                    HeightMapTool = HeightMapTools.FLATTEN;
                    break;
                case EditorMode.PAINTING:
                    break;
            }

            UpdateToolIcons();
        }

        private void layerBackgroundButton_Click(object sender, EventArgs e)
        {
            PaintingLayers = Layers.BACKGROUND;
            UpdateLayerIcons();
        }

        private void layer1Button_Click(object sender, EventArgs e)
        {
            PaintingLayers = Layers.LAYER1;
            UpdateLayerIcons();
        }

        private void layer2Button_Click(object sender, EventArgs e)
        {
            PaintingLayers = Layers.LAYER2;
            UpdateLayerIcons();
        }

        private void layer3Button_Click(object sender, EventArgs e)
        {
            PaintingLayers = Layers.LAYER3;
            UpdateLayerIcons();
        }

        private void layer4Button_Click(object sender, EventArgs e)
        {
            PaintingLayers = Layers.LAYER4;
            UpdateLayerIcons();
        }
    }
}
