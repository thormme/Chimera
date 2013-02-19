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
            this.EditTabs = new System.Windows.Forms.TabControl();
            this.Objects = new System.Windows.Forms.TabPage();
            this.ObjectList = new System.Windows.Forms.ListBox();
            this.Heights = new System.Windows.Forms.TabPage();
            this.HeightIntensityField = new System.Windows.Forms.NumericUpDown();
            this.HeightRadiusField = new System.Windows.Forms.NumericUpDown();
            this.HeightIntensityLabel = new System.Windows.Forms.Label();
            this.HeightRadiusLabel = new System.Windows.Forms.Label();
            this.SmoothBox = new System.Windows.Forms.CheckBox();
            this.FlattenBox = new System.Windows.Forms.CheckBox();
            this.InvertBox = new System.Windows.Forms.CheckBox();
            this.FeatherBox = new System.Windows.Forms.CheckBox();
            this.SetBox = new System.Windows.Forms.CheckBox();
            this.Textures = new System.Windows.Forms.TabPage();
            this.TextureAlphaField = new System.Windows.Forms.NumericUpDown();
            this.TextureRadiusField = new System.Windows.Forms.NumericUpDown();
            this.TextureAlphaLabel = new System.Windows.Forms.Label();
            this.TextureRadiusLabel = new System.Windows.Forms.Label();
            this.TextureList = new System.Windows.Forms.ListBox();
            this.Picture = new System.Windows.Forms.PictureBox();
            this.MenuStrip = new System.Windows.Forms.MenuStrip();
            this.File = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.Edit = new System.Windows.Forms.ToolStripMenuItem();
            this.EditTabs.SuspendLayout();
            this.Objects.SuspendLayout();
            this.Heights.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.HeightIntensityField)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.HeightRadiusField)).BeginInit();
            this.Textures.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TextureAlphaField)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TextureRadiusField)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Picture)).BeginInit();
            this.MenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // EditTabs
            // 
            this.EditTabs.CausesValidation = false;
            this.EditTabs.Controls.Add(this.Objects);
            this.EditTabs.Controls.Add(this.Heights);
            this.EditTabs.Controls.Add(this.Textures);
            this.EditTabs.Location = new System.Drawing.Point(12, 38);
            this.EditTabs.Name = "EditTabs";
            this.EditTabs.SelectedIndex = 0;
            this.EditTabs.Size = new System.Drawing.Size(198, 357);
            this.EditTabs.TabIndex = 3;
            // 
            // Objects
            // 
            this.Objects.Controls.Add(this.ObjectList);
            this.Objects.Location = new System.Drawing.Point(4, 22);
            this.Objects.Name = "Objects";
            this.Objects.Padding = new System.Windows.Forms.Padding(3);
            this.Objects.Size = new System.Drawing.Size(190, 331);
            this.Objects.TabIndex = 3;
            this.Objects.Text = "Objects";
            this.Objects.UseVisualStyleBackColor = true;
            // 
            // ObjectList
            // 
            this.ObjectList.FormattingEnabled = true;
            this.ObjectList.Location = new System.Drawing.Point(16, 21);
            this.ObjectList.Name = "ObjectList";
            this.ObjectList.Size = new System.Drawing.Size(154, 290);
            this.ObjectList.TabIndex = 0;
            // 
            // Heights
            // 
            this.Heights.Controls.Add(this.HeightIntensityField);
            this.Heights.Controls.Add(this.HeightRadiusField);
            this.Heights.Controls.Add(this.HeightIntensityLabel);
            this.Heights.Controls.Add(this.HeightRadiusLabel);
            this.Heights.Controls.Add(this.SmoothBox);
            this.Heights.Controls.Add(this.FlattenBox);
            this.Heights.Controls.Add(this.InvertBox);
            this.Heights.Controls.Add(this.FeatherBox);
            this.Heights.Controls.Add(this.SetBox);
            this.Heights.Location = new System.Drawing.Point(4, 22);
            this.Heights.Name = "Heights";
            this.Heights.Padding = new System.Windows.Forms.Padding(3);
            this.Heights.Size = new System.Drawing.Size(190, 331);
            this.Heights.TabIndex = 1;
            this.Heights.Text = "Heights";
            this.Heights.UseVisualStyleBackColor = true;
            // 
            // HeightIntensityField
            // 
            this.HeightIntensityField.Location = new System.Drawing.Point(57, 41);
            this.HeightIntensityField.Name = "HeightIntensityField";
            this.HeightIntensityField.Size = new System.Drawing.Size(60, 20);
            this.HeightIntensityField.TabIndex = 14;
            this.HeightIntensityField.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // HeightRadiusField
            // 
            this.HeightRadiusField.Location = new System.Drawing.Point(57, 15);
            this.HeightRadiusField.Name = "HeightRadiusField";
            this.HeightRadiusField.Size = new System.Drawing.Size(60, 20);
            this.HeightRadiusField.TabIndex = 13;
            this.HeightRadiusField.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // HeightIntensityLabel
            // 
            this.HeightIntensityLabel.AutoSize = true;
            this.HeightIntensityLabel.Location = new System.Drawing.Point(8, 44);
            this.HeightIntensityLabel.Name = "HeightIntensityLabel";
            this.HeightIntensityLabel.Size = new System.Drawing.Size(49, 13);
            this.HeightIntensityLabel.TabIndex = 12;
            this.HeightIntensityLabel.Text = "Intensity:";
            this.HeightIntensityLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // HeightRadiusLabel
            // 
            this.HeightRadiusLabel.AutoSize = true;
            this.HeightRadiusLabel.Location = new System.Drawing.Point(14, 18);
            this.HeightRadiusLabel.Name = "HeightRadiusLabel";
            this.HeightRadiusLabel.Size = new System.Drawing.Size(43, 13);
            this.HeightRadiusLabel.TabIndex = 11;
            this.HeightRadiusLabel.Text = "Radius:";
            // 
            // SmoothBox
            // 
            this.SmoothBox.AutoSize = true;
            this.SmoothBox.Location = new System.Drawing.Point(10, 166);
            this.SmoothBox.Name = "SmoothBox";
            this.SmoothBox.Size = new System.Drawing.Size(62, 17);
            this.SmoothBox.TabIndex = 10;
            this.SmoothBox.Text = "Smooth";
            this.SmoothBox.UseVisualStyleBackColor = true;
            // 
            // FlattenBox
            // 
            this.FlattenBox.AutoSize = true;
            this.FlattenBox.Location = new System.Drawing.Point(10, 143);
            this.FlattenBox.Name = "FlattenBox";
            this.FlattenBox.Size = new System.Drawing.Size(58, 17);
            this.FlattenBox.TabIndex = 9;
            this.FlattenBox.Text = "Flatten";
            this.FlattenBox.UseVisualStyleBackColor = true;
            // 
            // InvertBox
            // 
            this.InvertBox.AutoSize = true;
            this.InvertBox.Location = new System.Drawing.Point(10, 97);
            this.InvertBox.Name = "InvertBox";
            this.InvertBox.Size = new System.Drawing.Size(53, 17);
            this.InvertBox.TabIndex = 8;
            this.InvertBox.Text = "Invert";
            this.InvertBox.UseVisualStyleBackColor = true;
            // 
            // FeatherBox
            // 
            this.FeatherBox.AutoSize = true;
            this.FeatherBox.Location = new System.Drawing.Point(10, 120);
            this.FeatherBox.Name = "FeatherBox";
            this.FeatherBox.Size = new System.Drawing.Size(62, 17);
            this.FeatherBox.TabIndex = 7;
            this.FeatherBox.Text = "Feather";
            this.FeatherBox.UseVisualStyleBackColor = true;
            // 
            // SetBox
            // 
            this.SetBox.AutoSize = true;
            this.SetBox.Location = new System.Drawing.Point(10, 74);
            this.SetBox.Name = "SetBox";
            this.SetBox.Size = new System.Drawing.Size(42, 17);
            this.SetBox.TabIndex = 6;
            this.SetBox.Text = "Set";
            this.SetBox.UseVisualStyleBackColor = true;
            // 
            // Textures
            // 
            this.Textures.Controls.Add(this.TextureAlphaField);
            this.Textures.Controls.Add(this.TextureRadiusField);
            this.Textures.Controls.Add(this.TextureAlphaLabel);
            this.Textures.Controls.Add(this.TextureRadiusLabel);
            this.Textures.Controls.Add(this.TextureList);
            this.Textures.Controls.Add(this.Picture);
            this.Textures.Location = new System.Drawing.Point(4, 22);
            this.Textures.Name = "Textures";
            this.Textures.Padding = new System.Windows.Forms.Padding(3);
            this.Textures.Size = new System.Drawing.Size(190, 331);
            this.Textures.TabIndex = 2;
            this.Textures.Text = "Textures";
            this.Textures.UseVisualStyleBackColor = true;
            // 
            // TextureAlphaField
            // 
            this.TextureAlphaField.Location = new System.Drawing.Point(57, 41);
            this.TextureAlphaField.Name = "TextureAlphaField";
            this.TextureAlphaField.Size = new System.Drawing.Size(60, 20);
            this.TextureAlphaField.TabIndex = 18;
            this.TextureAlphaField.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // TextureRadiusField
            // 
            this.TextureRadiusField.Location = new System.Drawing.Point(57, 15);
            this.TextureRadiusField.Name = "TextureRadiusField";
            this.TextureRadiusField.Size = new System.Drawing.Size(60, 20);
            this.TextureRadiusField.TabIndex = 17;
            this.TextureRadiusField.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // TextureAlphaLabel
            // 
            this.TextureAlphaLabel.AutoSize = true;
            this.TextureAlphaLabel.Location = new System.Drawing.Point(8, 44);
            this.TextureAlphaLabel.Name = "TextureAlphaLabel";
            this.TextureAlphaLabel.Size = new System.Drawing.Size(48, 13);
            this.TextureAlphaLabel.TabIndex = 16;
            this.TextureAlphaLabel.Text = "Alpha %:";
            this.TextureAlphaLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // TextureRadiusLabel
            // 
            this.TextureRadiusLabel.AutoSize = true;
            this.TextureRadiusLabel.Location = new System.Drawing.Point(14, 18);
            this.TextureRadiusLabel.Name = "TextureRadiusLabel";
            this.TextureRadiusLabel.Size = new System.Drawing.Size(43, 13);
            this.TextureRadiusLabel.TabIndex = 15;
            this.TextureRadiusLabel.Text = "Radius:";
            // 
            // TextureList
            // 
            this.TextureList.FormattingEnabled = true;
            this.TextureList.Location = new System.Drawing.Point(15, 80);
            this.TextureList.Name = "TextureList";
            this.TextureList.Size = new System.Drawing.Size(160, 56);
            this.TextureList.TabIndex = 1;
            // 
            // Picture
            // 
            this.Picture.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.Picture.Location = new System.Drawing.Point(15, 154);
            this.Picture.Name = "Picture";
            this.Picture.Size = new System.Drawing.Size(160, 160);
            this.Picture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Picture.TabIndex = 0;
            this.Picture.TabStop = false;
            // 
            // MenuStrip
            // 
            this.MenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.File,
            this.Edit});
            this.MenuStrip.Location = new System.Drawing.Point(0, 0);
            this.MenuStrip.Name = "MenuStrip";
            this.MenuStrip.Size = new System.Drawing.Size(222, 24);
            this.MenuStrip.TabIndex = 4;
            this.MenuStrip.Text = "menuStrip1";
            // 
            // File
            // 
            this.File.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SaveMenu,
            this.OpenMenu});
            this.File.Name = "File";
            this.File.Size = new System.Drawing.Size(37, 20);
            this.File.Text = "File";
            // 
            // SaveMenu
            // 
            this.SaveMenu.Name = "SaveMenu";
            this.SaveMenu.Size = new System.Drawing.Size(103, 22);
            this.SaveMenu.Text = "Save";
            // 
            // OpenMenu
            // 
            this.OpenMenu.Name = "OpenMenu";
            this.OpenMenu.Size = new System.Drawing.Size(103, 22);
            this.OpenMenu.Text = "Open";
            // 
            // Edit
            // 
            this.Edit.Name = "Edit";
            this.Edit.Size = new System.Drawing.Size(39, 20);
            this.Edit.Text = "Edit";
            // 
            // EditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(222, 407);
            this.ControlBox = false;
            this.Controls.Add(this.EditTabs);
            this.Controls.Add(this.MenuStrip);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EditorForm";
            this.Text = "Editor Menu";
            this.Load += new System.EventHandler(this.EditorMenu_Load);
            this.EditTabs.ResumeLayout(false);
            this.Objects.ResumeLayout(false);
            this.Heights.ResumeLayout(false);
            this.Heights.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.HeightIntensityField)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.HeightRadiusField)).EndInit();
            this.Textures.ResumeLayout(false);
            this.Textures.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TextureAlphaField)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TextureRadiusField)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Picture)).EndInit();
            this.MenuStrip.ResumeLayout(false);
            this.MenuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label HeightIntensityLabel;
        private System.Windows.Forms.Label HeightRadiusLabel;
        private System.Windows.Forms.Label TextureAlphaLabel;
        private System.Windows.Forms.Label TextureRadiusLabel;
        private System.Windows.Forms.MenuStrip MenuStrip;
        private System.Windows.Forms.ToolStripMenuItem File;
        private System.Windows.Forms.ToolStripMenuItem SaveMenu;
        private System.Windows.Forms.ToolStripMenuItem OpenMenu;
        private System.Windows.Forms.ToolStripMenuItem Edit;
        public System.Windows.Forms.TabControl EditTabs;
        public System.Windows.Forms.NumericUpDown HeightRadiusField;
        public System.Windows.Forms.NumericUpDown HeightIntensityField;
        public System.Windows.Forms.CheckBox SmoothBox;
        public System.Windows.Forms.CheckBox FlattenBox;
        public System.Windows.Forms.CheckBox InvertBox;
        public System.Windows.Forms.CheckBox FeatherBox;
        public System.Windows.Forms.CheckBox SetBox;
        public System.Windows.Forms.ListBox ObjectList;
        public System.Windows.Forms.NumericUpDown TextureAlphaField;
        public System.Windows.Forms.NumericUpDown TextureRadiusField;
        public System.Windows.Forms.ListBox TextureList;
        public System.Windows.Forms.PictureBox Picture;
        public System.Windows.Forms.TabPage Heights;
        public System.Windows.Forms.TabPage Textures;
        public System.Windows.Forms.TabPage Objects;
    }
}