using System.Windows.Forms;
namespace WorldEditor.Dialogs
{
    partial class ToolMenu
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ToolMenu));
            this.toolStrip = new System.Windows.Forms.ToolStrip();
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
            this.selectObjectButton = new System.Windows.Forms.ToolStripButton();
            this.translateObjectButton = new System.Windows.Forms.ToolStripButton();
            this.rotateObjectButton = new System.Windows.Forms.ToolStripButton();
            this.scaleObjectButton = new System.Windows.Forms.ToolStripButton();
            this.objectSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
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
            this.toolStrip.SuspendLayout();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip
            // 
            this.toolStrip.BackColor = System.Drawing.SystemColors.Menu;
            this.toolStrip.ImageScalingSize = new System.Drawing.Size(40, 40);
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
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
            this.selectObjectButton,
            this.translateObjectButton,
            this.rotateObjectButton,
            this.scaleObjectButton,
            this.objectSeparator});
            this.toolStrip.Location = new System.Drawing.Point(0, 25);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(980, 47);
            this.toolStrip.TabIndex = 0;
            this.toolStrip.Text = "toolStrip";
            // 
            // raiseTerrainButton
            // 
            this.raiseTerrainButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.raiseTerrainButton.Image = ((System.Drawing.Image)(resources.GetObject("raiseTerrainButton.Image")));
            this.raiseTerrainButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.raiseTerrainButton.Name = "raiseTerrainButton";
            this.raiseTerrainButton.Size = new System.Drawing.Size(44, 44);
            this.raiseTerrainButton.Text = "Raise Terrain";
            this.raiseTerrainButton.Click += new System.EventHandler(this.toolButton_Click);
            // 
            // lowerTerrainButton
            // 
            this.lowerTerrainButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.lowerTerrainButton.Image = ((System.Drawing.Image)(resources.GetObject("lowerTerrainButton.Image")));
            this.lowerTerrainButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.lowerTerrainButton.Name = "lowerTerrainButton";
            this.lowerTerrainButton.Size = new System.Drawing.Size(44, 44);
            this.lowerTerrainButton.Text = "Lower Terrain";
            this.lowerTerrainButton.Click += new System.EventHandler(this.toolButton_Click);
            // 
            // setTerrainButton
            // 
            this.setTerrainButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.setTerrainButton.Image = ((System.Drawing.Image)(resources.GetObject("setTerrainButton.Image")));
            this.setTerrainButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.setTerrainButton.Name = "setTerrainButton";
            this.setTerrainButton.Size = new System.Drawing.Size(44, 44);
            this.setTerrainButton.Text = "Set Terrain";
            this.setTerrainButton.Click += new System.EventHandler(this.toolButton_Click);
            // 
            // flattenTerrainButton
            // 
            this.flattenTerrainButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.flattenTerrainButton.Image = ((System.Drawing.Image)(resources.GetObject("flattenTerrainButton.Image")));
            this.flattenTerrainButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.flattenTerrainButton.Name = "flattenTerrainButton";
            this.flattenTerrainButton.Size = new System.Drawing.Size(44, 44);
            this.flattenTerrainButton.Text = "Flatten Terrain";
            this.flattenTerrainButton.Click += new System.EventHandler(this.toolButton_Click);
            // 
            // smoothTerrainButton
            // 
            this.smoothTerrainButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.smoothTerrainButton.Image = ((System.Drawing.Image)(resources.GetObject("smoothTerrainButton.Image")));
            this.smoothTerrainButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.smoothTerrainButton.Name = "smoothTerrainButton";
            this.smoothTerrainButton.Size = new System.Drawing.Size(44, 44);
            this.smoothTerrainButton.Text = "Smooth Terrain";
            this.smoothTerrainButton.Click += new System.EventHandler(this.toolButton_Click);
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
            this.paintTextureButton.Click += new System.EventHandler(this.toolButton_Click);
            // 
            // eraseTextureButton
            // 
            this.eraseTextureButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.eraseTextureButton.Image = ((System.Drawing.Image)(resources.GetObject("eraseTextureButton.Image")));
            this.eraseTextureButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.eraseTextureButton.Name = "eraseTextureButton";
            this.eraseTextureButton.Size = new System.Drawing.Size(44, 44);
            this.eraseTextureButton.Text = "Erase Terrain Texture";
            this.eraseTextureButton.Click += new System.EventHandler(this.toolButton_Click);
            // 
            // smoothTextureButton
            // 
            this.smoothTextureButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.smoothTextureButton.Image = ((System.Drawing.Image)(resources.GetObject("smoothTextureButton.Image")));
            this.smoothTextureButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.smoothTextureButton.Name = "smoothTextureButton";
            this.smoothTextureButton.Size = new System.Drawing.Size(44, 44);
            this.smoothTextureButton.Text = "Blend Terrain Texture";
            this.smoothTextureButton.Click += new System.EventHandler(this.toolButton_Click);
            // 
            // textureSeparator
            // 
            this.textureSeparator.Name = "textureSeparator";
            this.textureSeparator.Size = new System.Drawing.Size(6, 47);
            // 
            // selectObjectButton
            // 
            this.selectObjectButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.selectObjectButton.Image = ((System.Drawing.Image)(resources.GetObject("selectObjectButton.Image")));
            this.selectObjectButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.selectObjectButton.Name = "selectObjectButton";
            this.selectObjectButton.Size = new System.Drawing.Size(44, 44);
            this.selectObjectButton.Text = "Select Objects";
            this.selectObjectButton.Click += new System.EventHandler(this.toolButton_Click);
            // 
            // translateObjectButton
            // 
            this.translateObjectButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.translateObjectButton.Image = ((System.Drawing.Image)(resources.GetObject("translateObjectButton.Image")));
            this.translateObjectButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.translateObjectButton.Name = "translateObjectButton";
            this.translateObjectButton.Size = new System.Drawing.Size(44, 44);
            this.translateObjectButton.Text = "Translate Selected Object";
            this.translateObjectButton.Click += new System.EventHandler(this.toolButton_Click);
            // 
            // rotateObjectButton
            // 
            this.rotateObjectButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.rotateObjectButton.Image = ((System.Drawing.Image)(resources.GetObject("rotateObjectButton.Image")));
            this.rotateObjectButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.rotateObjectButton.Name = "rotateObjectButton";
            this.rotateObjectButton.Size = new System.Drawing.Size(44, 44);
            this.rotateObjectButton.Text = "Rotate Selected Object";
            this.rotateObjectButton.Click += new System.EventHandler(this.toolButton_Click);
            // 
            // scaleObjectButton
            // 
            this.scaleObjectButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.scaleObjectButton.Image = ((System.Drawing.Image)(resources.GetObject("scaleObjectButton.Image")));
            this.scaleObjectButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.scaleObjectButton.Name = "scaleObjectButton";
            this.scaleObjectButton.Size = new System.Drawing.Size(44, 44);
            this.scaleObjectButton.Text = "Scale Selected Object";
            this.scaleObjectButton.Click += new System.EventHandler(this.toolButton_Click);
            // 
            // objectSeparator
            // 
            this.objectSeparator.Name = "objectSeparator";
            this.objectSeparator.Size = new System.Drawing.Size(6, 47);
            // 
            // menuStrip
            // 
            this.menuStrip.BackColor = System.Drawing.SystemColors.Menu;
            this.menuStrip.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.vIEWToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(980, 25);
            this.menuStrip.TabIndex = 1;
            this.menuStrip.Text = "menuStrip";
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
            // ToolMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.menuStrip);
            this.Name = "ToolMenu";
            this.Size = new System.Drawing.Size(980, 255);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip;
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
        private System.Windows.Forms.ToolStripButton selectObjectButton;
        private System.Windows.Forms.ToolStripButton translateObjectButton;
        private System.Windows.Forms.ToolStripButton rotateObjectButton;
        private System.Windows.Forms.ToolStripButton scaleObjectButton;
        private System.Windows.Forms.ToolStripSeparator objectSeparator;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        public ToolStripMenuItem NewMenu;
        public ToolStripMenuItem OpenMenu;
        public ToolStripMenuItem SaveMenu;
        public ToolStripMenuItem SaveAsMenu;
        public ToolStripMenuItem editToolStripMenuItem;
        public ToolStripMenuItem UndoMenu;
        public ToolStripMenuItem RedoMenu;
        public ToolStripMenuItem PlayMenu;
        public ToolStripMenuItem vIEWToolStripMenuItem;
        public ToolStripMenuItem ViewSkyBoxMenu;
        public ToolStripMenuItem ViewWaterMenu;

        public ToolStrip ToolStrip
        {
            get { return toolStrip; }
        }

        public MenuStrip MenuStrip
        {
            get { return menuStrip; }
        }
    }
}
