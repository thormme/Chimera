﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace WorldEditor
{
    public static class UILibrary
    {
        public static Image HeightmapModeButtonIcon = Image.FromFile(@"Artwork/heightmap_icon.png");

        public static Image PaintBrushIcon = Image.FromFile(@"Artwork/painting_icon.png");
        public static Image EraserIcon = Image.FromFile(@"Artwork/eraser_icon.png");
        public static Image SpongeIcon = Image.FromFile(@"Artwork/sponge_icon.png");
        
        public static Image NewObjectIcon = Image.FromFile(@"Artwork/new_object_icon.png");
        public static Image ObjectSelectionIcon = Image.FromFile(@"Artwork/object_selection_icon.png");

        public static Image CircleBrushIcon = Image.FromFile(@"Artwork/circle_brush_icon.png");
        public static Image CircleFeatheredBrushIcon = Image.FromFile(@"Artwork/circle_brush_feathered_icon.png");
        public static Image BlockBrushIcon = Image.FromFile(@"Artwork/block_brush_icon.png");
        public static Image BlockFeatheredBrushIcon = Image.FromFile(@"Artwork/block_brush_feathered_icon.png");

        public static Image RaiseTerrainIcon = Image.FromFile(@"Artwork/raise_terrain_icon.png");
        public static Image LowerTerrainIcon = Image.FromFile(@"Artwork/lower_terrain_icon.png");
        public static Image SetTerrainIcon = Image.FromFile(@"Artwork/set_terrain_icon.png");
        public static Image SmoothTerrainIcon = Image.FromFile(@"Artwork/smooth_terrain_icon.png");
        public static Image FlattenTerrainIcon = Image.FromFile(@"Artwork/flatten_terrain_icon.png");

        public static Image BlockSelectionIcon = Image.FromFile(@"Artwork/block_selection_icon.png");
        public static Image BlockCreationIcon = Image.FromFile(@"Artwork/block_creation_icon.png");

        public static Image GizmoScaleIcon = Image.FromFile(@"Artwork/gizmo_scale_icon.png");
        public static Image GizmoRotateIcon = Image.FromFile(@"Artwork/gizmo_rotate_icon.png");
        public static Image GizmoTranslateIcon = Image.FromFile(@"Artwork/gizmo_translate_icon.png");

        public static Image VisibleLayerIcon = Image.FromFile(@"Artwork/visible_layer_icon.png");

        public static Image InvalidIcon = Image.FromFile(@"Artwork/invalidTexture.png");
    }
}
