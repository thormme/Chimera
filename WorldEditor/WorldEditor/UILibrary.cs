﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace WorldEditor
{
    public static class UILibrary
    {
        public static Image HeightMapBackground = Image.FromFile(@"Artwork/heightmap_background.png");
        public static Image PaintingBackground = Image.FromFile(@"Artwork/painting_background.png");

        public static Image HeightmapModeButtonIcon = Image.FromFile(@"Artwork/heightmap_icon.png");
        public static Image HeightmapModeButtonSelectedIcon = Image.FromFile(@"Artwork/heightmap_icon_selected.png");

        public static Image PaintingModeButtonIcon = Image.FromFile(@"Artwork/painting_icon.png");
        public static Image PaintingModeButtonSelectedIcon = Image.FromFile(@"Artwork/painting_icon_selected.png");

        public static Image ObjectModeButtonIcon = Image.FromFile(@"Artwork/object_icon.png");
        public static Image ObjectModeButtonSelectedIcon = Image.FromFile(@"Artwork/object_icon_selected.png");

        public static Image CircleBrushIcon = Image.FromFile(@"Artwork/circle_brush_icon.png");
        public static Image CircleBrushSelectedIcon = Image.FromFile(@"Artwork/circle_brush_icon_selected.png");

        public static Image CircleFeatheredBrushIcon = Image.FromFile(@"Artwork/circle_brush_feathered_icon.png");
        public static Image CircleFeatheredBrushSelectedIcon = Image.FromFile(@"Artwork/circle_brush_feathered_icon_selected.png");

        public static Image BlockBrushIcon = Image.FromFile(@"Artwork/block_brush_icon.png");
        public static Image BlockBrushSelectedIcon = Image.FromFile(@"Artwork/block_brush_icon_selected.png");

        public static Image BlockFeatheredBrushIcon = Image.FromFile(@"Artwork/block_brush_feathered_icon.png");
        public static Image BlockFeatheredBrushSelectedIcon = Image.FromFile(@"Artwork/block_brush_feathered_icon_selected.png");

        public static Image RaiseTerrainIcon = Image.FromFile(@"Artwork/raise_terrain_icon.png");
        public static Image RaiseTerrainSelectedIcon = Image.FromFile(@"Artwork/raise_terrain_icon_selected.png");

        public static Image LowerTerrainIcon = Image.FromFile(@"Artwork/lower_terrain_icon.png");
        public static Image LowerTerrainSelectedIcon = Image.FromFile(@"Artwork/lower_terrain_icon_selected.png");

        public static Image SetTerrainIcon = Image.FromFile(@"Artwork/set_terrain_icon.png");
        public static Image SetTerrainSelectedIcon = Image.FromFile(@"Artwork/set_terrain_icon_selected.png");

        public static Image SmoothTerrainIcon = Image.FromFile(@"Artwork/smooth_terrain_icon.png");
        public static Image SmoothTerrainSelectedIcon = Image.FromFile(@"Artwork/smooth_terrain_icon_selected.png");

        public static Image FlattenTerrainIcon = Image.FromFile(@"Artwork/flatten_terrain_icon.png");
        public static Image FlattenTerrainSelectedIcon = Image.FromFile(@"Artwork/flatten_terrain_icon_selected.png");

        public static Image LayerBackgroundIcon = Image.FromFile(@"Artwork/layer_background_icon.png");
        public static Image LayerBackgroundSelectedIcon = Image.FromFile(@"Artwork/layer_background_icon_selected.png");

        public static Image Layer1Icon = Image.FromFile(@"Artwork/layer_1_icon.png");
        public static Image Layer1SelectedIcon = Image.FromFile(@"Artwork/layer_1_icon_selected.png");

        public static Image Layer2Icon = Image.FromFile(@"Artwork/layer_2_icon.png");
        public static Image Layer2SelectedIcon = Image.FromFile(@"Artwork/layer_2_icon_selected.png");

        public static Image Layer3Icon = Image.FromFile(@"Artwork/layer_3_icon.png");
        public static Image Layer3SelectedIcon = Image.FromFile(@"Artwork/layer_3_icon_selected.png");

        public static Image Layer4Icon = Image.FromFile(@"Artwork/layer_4_icon.png");
        public static Image Layer4SelectedIcon = Image.FromFile(@"Artwork/layer_4_icon_selected.png");
    }
}