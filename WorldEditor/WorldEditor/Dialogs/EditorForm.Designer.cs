namespace WorldEditor.Dialogs
{
    partial class EditorForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditorForm));
            this.ToolStrip = new System.Windows.Forms.ToolStrip();
            this.raiseTerrainButton = new System.Windows.Forms.ToolStripButton();
            this.lowerTerrainButton = new System.Windows.Forms.ToolStripButton();
            this.setTerrainButton = new System.Windows.Forms.ToolStripButton();
            this.flattenTerrainButton = new System.Windows.Forms.ToolStripButton();
            this.smoothTerrainButton = new System.Windows.Forms.ToolStripButton();
            this.terrainSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.paintTextureButton = new System.Windows.Forms.ToolStripButton();
            this.eraseTextureButton = new System.Windows.Forms.ToolStripButton();
            this.smoothTextureButton = new System.Windows.Forms.ToolStripButton();
            this.textureSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.SelectObjectButton = new System.Windows.Forms.ToolStripButton();
            this.TranslateObjectButton = new System.Windows.Forms.ToolStripButton();
            this.RotateObjectButton = new System.Windows.Forms.ToolStripButton();
            this.ScaleObjectButton = new System.Windows.Forms.ToolStripButton();
            this.objectSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.MenuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.NewMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveAsMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.PlayMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.UndoMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.RedoMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.vIEWToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ViewSkyBoxMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.ViewWaterMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.ContextToolsPanel = new System.Windows.Forms.Panel();
            this.ObjectPlacementPanel = new WorldEditor.Dialogs.ObjectPlacementPanel();
            this.ObjectParametersForm = new WorldEditor.Dialogs.ObjectParametersForm();
            this.editorPanel = new WorldEditor.Dialogs.Editor();
            this.ToolStrip.SuspendLayout();
            this.MenuStrip.SuspendLayout();
            this.ContextToolsPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // ToolStrip
            // 
            this.ToolStrip.BackColor = System.Drawing.SystemColors.Menu;
            this.ToolStrip.ImageScalingSize = new System.Drawing.Size(40, 40);
            this.ToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.raiseTerrainButton,
            this.lowerTerrainButton,
            this.setTerrainButton,
            this.flattenTerrainButton,
            this.smoothTerrainButton,
            this.terrainSeparator,
            this.paintTextureButton,
            this.eraseTextureButton,
            this.smoothTextureButton,
            this.textureSeparator,
            this.SelectObjectButton,
            this.TranslateObjectButton,
            this.RotateObjectButton,
            this.ScaleObjectButton,
            this.objectSeparator});
            this.ToolStrip.Location = new System.Drawing.Point(0, 25);
            this.ToolStrip.Name = "ToolStrip";
            this.ToolStrip.Size = new System.Drawing.Size(926, 47);
            this.ToolStrip.TabIndex = 2;
            this.ToolStrip.Text = "ToolStrip";
            // 
            // raiseTerrainButton
            // 
            this.raiseTerrainButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.raiseTerrainButton.Image = ((System.Drawing.Image)(resources.GetObject("raiseTerrainButton.Image")));
            this.raiseTerrainButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.raiseTerrainButton.Name = "raiseTerrainButton";
            this.raiseTerrainButton.Size = new System.Drawing.Size(44, 44);
            this.raiseTerrainButton.Text = "Raise Terrain";
            this.raiseTerrainButton.Click += new System.EventHandler(this.ToolButton_Click);
            // 
            // lowerTerrainButton
            // 
            this.lowerTerrainButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.lowerTerrainButton.Image = ((System.Drawing.Image)(resources.GetObject("lowerTerrainButton.Image")));
            this.lowerTerrainButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.lowerTerrainButton.Name = "lowerTerrainButton";
            this.lowerTerrainButton.Size = new System.Drawing.Size(44, 44);
            this.lowerTerrainButton.Text = "Lower Terrain";
            this.lowerTerrainButton.Click += new System.EventHandler(this.ToolButton_Click);
            // 
            // setTerrainButton
            // 
            this.setTerrainButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.setTerrainButton.Image = ((System.Drawing.Image)(resources.GetObject("setTerrainButton.Image")));
            this.setTerrainButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.setTerrainButton.Name = "setTerrainButton";
            this.setTerrainButton.Size = new System.Drawing.Size(44, 44);
            this.setTerrainButton.Text = "Set Terrain";
            this.setTerrainButton.Click += new System.EventHandler(this.ToolButton_Click);
            // 
            // flattenTerrainButton
            // 
            this.flattenTerrainButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.flattenTerrainButton.Image = ((System.Drawing.Image)(resources.GetObject("flattenTerrainButton.Image")));
            this.flattenTerrainButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.flattenTerrainButton.Name = "flattenTerrainButton";
            this.flattenTerrainButton.Size = new System.Drawing.Size(44, 44);
            this.flattenTerrainButton.Text = "Flatten Terrain";
            this.flattenTerrainButton.Click += new System.EventHandler(this.ToolButton_Click);
            // 
            // smoothTerrainButton
            // 
            this.smoothTerrainButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.smoothTerrainButton.Image = ((System.Drawing.Image)(resources.GetObject("smoothTerrainButton.Image")));
            this.smoothTerrainButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.smoothTerrainButton.Name = "smoothTerrainButton";
            this.smoothTerrainButton.Size = new System.Drawing.Size(44, 44);
            this.smoothTerrainButton.Text = "Smooth Terrain";
            this.smoothTerrainButton.Click += new System.EventHandler(this.ToolButton_Click);
            // 
            // terrainSeparator
            // 
            this.terrainSeparator.Name = "terrainSeparator";
            this.terrainSeparator.Size = new System.Drawing.Size(6, 47);
            // 
            // paintTextureButton
            // 
            this.paintTextureButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.paintTextureButton.Image = ((System.Drawing.Image)(resources.GetObject("paintTextureButton.Image")));
            this.paintTextureButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.paintTextureButton.Name = "paintTextureButton";
            this.paintTextureButton.Size = new System.Drawing.Size(44, 44);
            this.paintTextureButton.Text = "Paint Terrain Texture";
            // 
            // eraseTextureButton
            // 
            this.eraseTextureButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.eraseTextureButton.Image = ((System.Drawing.Image)(resources.GetObject("eraseTextureButton.Image")));
            this.eraseTextureButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.eraseTextureButton.Name = "eraseTextureButton";
            this.eraseTextureButton.Size = new System.Drawing.Size(44, 44);
            this.eraseTextureButton.Text = "Erase Terrain Texture";
            this.eraseTextureButton.Click += new System.EventHandler(this.ToolButton_Click);
            // 
            // smoothTextureButton
            // 
            this.smoothTextureButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.smoothTextureButton.Image = ((System.Drawing.Image)(resources.GetObject("smoothTextureButton.Image")));
            this.smoothTextureButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.smoothTextureButton.Name = "smoothTextureButton";
            this.smoothTextureButton.Size = new System.Drawing.Size(44, 44);
            this.smoothTextureButton.Text = "Blend Terrain Texture";
            this.smoothTextureButton.Click += new System.EventHandler(this.ToolButton_Click);
            // 
            // textureSeparator
            // 
            this.textureSeparator.Name = "textureSeparator";
            this.textureSeparator.Size = new System.Drawing.Size(6, 47);
            // 
            // SelectObjectButton
            // 
            this.SelectObjectButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.SelectObjectButton.Image = ((System.Drawing.Image)(resources.GetObject("SelectObjectButton.Image")));
            this.SelectObjectButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SelectObjectButton.Name = "SelectObjectButton";
            this.SelectObjectButton.Size = new System.Drawing.Size(44, 44);
            this.SelectObjectButton.Text = "Select Objects";
            this.SelectObjectButton.Click += new System.EventHandler(this.ToolButton_Click);
            // 
            // TranslateObjectButton
            // 
            this.TranslateObjectButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.TranslateObjectButton.Image = ((System.Drawing.Image)(resources.GetObject("TranslateObjectButton.Image")));
            this.TranslateObjectButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TranslateObjectButton.Name = "TranslateObjectButton";
            this.TranslateObjectButton.Size = new System.Drawing.Size(44, 44);
            this.TranslateObjectButton.Text = "Translate Selected Object";
            this.TranslateObjectButton.Click += new System.EventHandler(this.ToolButton_Click);
            // 
            // RotateObjectButton
            // 
            this.RotateObjectButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.RotateObjectButton.Image = ((System.Drawing.Image)(resources.GetObject("RotateObjectButton.Image")));
            this.RotateObjectButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.RotateObjectButton.Name = "RotateObjectButton";
            this.RotateObjectButton.Size = new System.Drawing.Size(44, 44);
            this.RotateObjectButton.Text = "Rotate Selected Object";
            this.RotateObjectButton.Click += new System.EventHandler(this.ToolButton_Click);
            // 
            // ScaleObjectButton
            // 
            this.ScaleObjectButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ScaleObjectButton.Image = ((System.Drawing.Image)(resources.GetObject("ScaleObjectButton.Image")));
            this.ScaleObjectButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ScaleObjectButton.Name = "ScaleObjectButton";
            this.ScaleObjectButton.Size = new System.Drawing.Size(44, 44);
            this.ScaleObjectButton.Text = "Scale Selected Object";
            this.ScaleObjectButton.Click += new System.EventHandler(this.ToolButton_Click);
            // 
            // objectSeparator
            // 
            this.objectSeparator.Name = "objectSeparator";
            this.objectSeparator.Size = new System.Drawing.Size(6, 47);
            // 
            // MenuStrip
            // 
            this.MenuStrip.BackColor = System.Drawing.SystemColors.Menu;
            this.MenuStrip.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.vIEWToolStripMenuItem});
            this.MenuStrip.Location = new System.Drawing.Point(0, 0);
            this.MenuStrip.Name = "MenuStrip";
            this.MenuStrip.Size = new System.Drawing.Size(926, 25);
            this.MenuStrip.TabIndex = 3;
            this.MenuStrip.Text = "MenuStrip";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.NewMenu,
            this.OpenMenu,
            this.SaveMenu,
            this.SaveAsMenu,
            this.PlayMenu});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(42, 21);
            this.fileToolStripMenuItem.Text = "FILE";
            // 
            // NewMenu
            // 
            this.NewMenu.Name = "NewMenu";
            this.NewMenu.Size = new System.Drawing.Size(151, 22);
            this.NewMenu.Text = "New";
            // 
            // OpenMenu
            // 
            this.OpenMenu.Name = "OpenMenu";
            this.OpenMenu.Size = new System.Drawing.Size(151, 22);
            this.OpenMenu.Text = "Open";
            // 
            // SaveMenu
            // 
            this.SaveMenu.Name = "SaveMenu";
            this.SaveMenu.Size = new System.Drawing.Size(151, 22);
            this.SaveMenu.Text = "Save";
            // 
            // SaveAsMenu
            // 
            this.SaveAsMenu.Name = "SaveAsMenu";
            this.SaveAsMenu.Size = new System.Drawing.Size(151, 22);
            this.SaveAsMenu.Text = "Save As";
            // 
            // PlayMenu
            // 
            this.PlayMenu.Name = "PlayMenu";
            this.PlayMenu.Size = new System.Drawing.Size(151, 22);
            this.PlayMenu.Text = "Play In Game";
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.UndoMenu,
            this.RedoMenu});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(46, 21);
            this.editToolStripMenuItem.Text = "EDIT";
            // 
            // UndoMenu
            // 
            this.UndoMenu.Name = "UndoMenu";
            this.UndoMenu.Size = new System.Drawing.Size(108, 22);
            this.UndoMenu.Text = "Undo";
            // 
            // RedoMenu
            // 
            this.RedoMenu.Name = "RedoMenu";
            this.RedoMenu.Size = new System.Drawing.Size(108, 22);
            this.RedoMenu.Text = "Redo";
            // 
            // vIEWToolStripMenuItem
            // 
            this.vIEWToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ViewSkyBoxMenu,
            this.ViewWaterMenu});
            this.vIEWToolStripMenuItem.Name = "vIEWToolStripMenuItem";
            this.vIEWToolStripMenuItem.Size = new System.Drawing.Size(50, 21);
            this.vIEWToolStripMenuItem.Text = "VIEW";
            // 
            // ViewSkyBoxMenu
            // 
            this.ViewSkyBoxMenu.Checked = true;
            this.ViewSkyBoxMenu.CheckOnClick = true;
            this.ViewSkyBoxMenu.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ViewSkyBoxMenu.Name = "ViewSkyBoxMenu";
            this.ViewSkyBoxMenu.Size = new System.Drawing.Size(146, 22);
            this.ViewSkyBoxMenu.Text = "Sky Box";
            // 
            // ViewWaterMenu
            // 
            this.ViewWaterMenu.Checked = true;
            this.ViewWaterMenu.CheckOnClick = true;
            this.ViewWaterMenu.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ViewWaterMenu.Name = "ViewWaterMenu";
            this.ViewWaterMenu.Size = new System.Drawing.Size(146, 22);
            this.ViewWaterMenu.Text = "Water Plane";
            // 
            // ContextToolsPanel
            // 
            this.ContextToolsPanel.AutoScroll = true;
            this.ContextToolsPanel.Controls.Add(this.ObjectPlacementPanel);
            this.ContextToolsPanel.Controls.Add(this.ObjectParametersForm);
            this.ContextToolsPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.ContextToolsPanel.Location = new System.Drawing.Point(675, 72);
            this.ContextToolsPanel.Name = "ContextToolsPanel";
            this.ContextToolsPanel.Size = new System.Drawing.Size(251, 431);
            this.ContextToolsPanel.TabIndex = 4;
            // 
            // ObjectPlacementPanel
            // 
            this.ObjectPlacementPanel.Location = new System.Drawing.Point(3, 127);
            this.ObjectPlacementPanel.Name = "ObjectPlacementPanel";
            this.ObjectPlacementPanel.Size = new System.Drawing.Size(235, 379);
            this.ObjectPlacementPanel.TabIndex = 1;
            // 
            // ObjectParametersForm
            // 
            this.ObjectParametersForm.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ObjectParametersForm.Dock = System.Windows.Forms.DockStyle.Top;
            this.ObjectParametersForm.Location = new System.Drawing.Point(0, 0);
            this.ObjectParametersForm.MinimumSize = new System.Drawing.Size(260, 160);
            this.ObjectParametersForm.Name = "ObjectParametersForm";
            this.ObjectParametersForm.Size = new System.Drawing.Size(260, 160);
            this.ObjectParametersForm.TabIndex = 0;
            // 
            // editorPanel
            // 
            this.editorPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.editorPanel.Location = new System.Drawing.Point(2, 74);
            this.editorPanel.Name = "editorPanel";
            this.editorPanel.Size = new System.Drawing.Size(688, 428);
            this.editorPanel.TabIndex = 0;
            // 
            // EditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(926, 503);
            this.Controls.Add(this.ContextToolsPanel);
            this.Controls.Add(this.ToolStrip);
            this.Controls.Add(this.MenuStrip);
            this.Controls.Add(this.editorPanel);
            this.Name = "EditorForm";
            this.Text = "EditorForm";
            this.ToolStrip.ResumeLayout(false);
            this.ToolStrip.PerformLayout();
            this.MenuStrip.ResumeLayout(false);
            this.MenuStrip.PerformLayout();
            this.ContextToolsPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Editor editorPanel;
        private System.Windows.Forms.ToolStripButton raiseTerrainButton;
        private System.Windows.Forms.ToolStripButton lowerTerrainButton;
        private System.Windows.Forms.ToolStripButton setTerrainButton;
        private System.Windows.Forms.ToolStripButton flattenTerrainButton;
        private System.Windows.Forms.ToolStripButton smoothTerrainButton;
        private System.Windows.Forms.ToolStripSeparator terrainSeparator;
        private System.Windows.Forms.ToolStripButton paintTextureButton;
        private System.Windows.Forms.ToolStripButton eraseTextureButton;
        private System.Windows.Forms.ToolStripButton smoothTextureButton;
        private System.Windows.Forms.ToolStripSeparator textureSeparator;
        public System.Windows.Forms.ToolStripButton SelectObjectButton;
        public System.Windows.Forms.ToolStripButton TranslateObjectButton;
        public System.Windows.Forms.ToolStripButton RotateObjectButton;
        public System.Windows.Forms.ToolStripButton ScaleObjectButton;
        private System.Windows.Forms.ToolStripSeparator objectSeparator;
        public System.Windows.Forms.MenuStrip MenuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem NewMenu;
        public System.Windows.Forms.ToolStripMenuItem OpenMenu;
        public System.Windows.Forms.ToolStripMenuItem SaveMenu;
        public System.Windows.Forms.ToolStripMenuItem SaveAsMenu;
        public System.Windows.Forms.ToolStripMenuItem PlayMenu;
        public System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem UndoMenu;
        public System.Windows.Forms.ToolStripMenuItem RedoMenu;
        public System.Windows.Forms.ToolStripMenuItem vIEWToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem ViewSkyBoxMenu;
        public System.Windows.Forms.ToolStripMenuItem ViewWaterMenu;
        public System.Windows.Forms.Panel ContextToolsPanel;
        public ObjectParametersForm ObjectParametersForm;
        public System.Windows.Forms.ToolStrip ToolStrip;
        public global::WorldEditor.Dialogs.ObjectPlacementPanel ObjectPlacementPanel;
    }
}