using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace WorldEditor
{
    public static class UILibrary
    {
        public static Image HeightmapModeButtonIcon = Image.FromFile(@"Artwork/heightmap_icon.png");
        public static Image PaintingModeButtonIcon = Image.FromFile(@"Artwork/painting_icon.png");
        public static Image ObjectModeButtonIcon = Image.FromFile(@"Artwork/object_icon.png");

        public static Image CircleBrushIcon = Image.FromFile(@"Artwork/circle_brush_icon.png");
        public static Image CircleFeatheredBrushIcon = Image.FromFile(@"Artwork/circle_brush_feathered_icon.png");
        public static Image BlockBrushIcon = Image.FromFile(@"Artwork/block_brush_icon.png");
        public static Image BlockFeatheredBrushIcon = Image.FromFile(@"Artwork/block_brush_feathered_icon.png");

        public static Image RaiseTerrainIcon = Image.FromFile(@"Artwork/raise_terrain_icon.png");
        public static Image LowerTerrainIcon = Image.FromFile(@"Artwork/lower_terrain_icon.png");
        public static Image SetTerrainIcon = Image.FromFile(@"Artwork/set_terrain_icon.png");
        public static Image SmoothTerrainIcon = Image.FromFile(@"Artwork/smooth_terrain_icon.png");
        public static Image FlattenTerrainIcon = Image.FromFile(@"Artwork/flatten_terrain_icon.png");

        public static Image LayerBackgroundIcon = Image.FromFile(@"Artwork/layer_background_icon.png");
        public static Image Layer1Icon = Image.FromFile(@"Artwork/layer_1_icon.png");
        public static Image Layer2Icon = Image.FromFile(@"Artwork/layer_2_icon.png");
        public static Image Layer3Icon = Image.FromFile(@"Artwork/layer_3_icon.png");
        public static Image Layer4Icon = Image.FromFile(@"Artwork/layer_4_icon.png");
    }
}
