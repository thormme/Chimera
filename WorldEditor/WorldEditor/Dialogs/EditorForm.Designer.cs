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
            this.IntensityLabel = new System.Windows.Forms.Label();
            this.RadiusLabel = new System.Windows.Forms.Label();
            this.SmoothBox = new System.Windows.Forms.CheckBox();
            this.FlattenBox = new System.Windows.Forms.CheckBox();
            this.InvertBox = new System.Windows.Forms.CheckBox();
            this.FeatherBox = new System.Windows.Forms.CheckBox();
            this.SetBox = new System.Windows.Forms.CheckBox();
            this.IntensityField = new System.Windows.Forms.TextBox();
            this.RadiusField = new System.Windows.Forms.TextBox();
            this.Textures = new System.Windows.Forms.TabPage();
            this.EditTabs.SuspendLayout();
            this.Objects.SuspendLayout();
            this.Heights.SuspendLayout();
            this.SuspendLayout();
            // 
            // EditTabs
            // 
            this.EditTabs.CausesValidation = false;
            this.EditTabs.Controls.Add(this.Objects);
            this.EditTabs.Controls.Add(this.Heights);
            this.EditTabs.Controls.Add(this.Textures);
            this.EditTabs.Location = new System.Drawing.Point(12, 12);
            this.EditTabs.Name = "EditTabs";
            this.EditTabs.SelectedIndex = 0;
            this.EditTabs.Size = new System.Drawing.Size(198, 303);
            this.EditTabs.TabIndex = 3;
            // 
            // Objects
            // 
            this.Objects.Controls.Add(this.ObjectList);
            this.Objects.Location = new System.Drawing.Point(4, 22);
            this.Objects.Name = "Objects";
            this.Objects.Padding = new System.Windows.Forms.Padding(3);
            this.Objects.Size = new System.Drawing.Size(190, 277);
            this.Objects.TabIndex = 3;
            this.Objects.Text = "Objects";
            this.Objects.UseVisualStyleBackColor = true;
            // 
            // ObjectList
            // 
            this.ObjectList.FormattingEnabled = true;
            this.ObjectList.Location = new System.Drawing.Point(16, 21);
            this.ObjectList.Name = "ObjectList";
            this.ObjectList.Size = new System.Drawing.Size(154, 238);
            this.ObjectList.TabIndex = 0;
            // 
            // Heights
            // 
            this.Heights.Controls.Add(this.IntensityLabel);
            this.Heights.Controls.Add(this.RadiusLabel);
            this.Heights.Controls.Add(this.SmoothBox);
            this.Heights.Controls.Add(this.FlattenBox);
            this.Heights.Controls.Add(this.InvertBox);
            this.Heights.Controls.Add(this.FeatherBox);
            this.Heights.Controls.Add(this.SetBox);
            this.Heights.Controls.Add(this.IntensityField);
            this.Heights.Controls.Add(this.RadiusField);
            this.Heights.Location = new System.Drawing.Point(4, 22);
            this.Heights.Name = "Heights";
            this.Heights.Padding = new System.Windows.Forms.Padding(3);
            this.Heights.Size = new System.Drawing.Size(190, 277);
            this.Heights.TabIndex = 1;
            this.Heights.Text = "Heights";
            this.Heights.UseVisualStyleBackColor = true;
            // 
            // IntensityLabel
            // 
            this.IntensityLabel.AutoSize = true;
            this.IntensityLabel.Location = new System.Drawing.Point(8, 44);
            this.IntensityLabel.Name = "IntensityLabel";
            this.IntensityLabel.Size = new System.Drawing.Size(49, 13);
            this.IntensityLabel.TabIndex = 12;
            this.IntensityLabel.Text = "Intensity:";
            this.IntensityLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // RadiusLabel
            // 
            this.RadiusLabel.AutoSize = true;
            this.RadiusLabel.Location = new System.Drawing.Point(14, 18);
            this.RadiusLabel.Name = "RadiusLabel";
            this.RadiusLabel.Size = new System.Drawing.Size(43, 13);
            this.RadiusLabel.TabIndex = 11;
            this.RadiusLabel.Text = "Radius:";
            // 
            // SmoothBox
            // 
            this.SmoothBox.AutoSize = true;
            this.SmoothBox.Location = new System.Drawing.Point(73, 120);
            this.SmoothBox.Name = "SmoothBox";
            this.SmoothBox.Size = new System.Drawing.Size(62, 17);
            this.SmoothBox.TabIndex = 10;
            this.SmoothBox.Text = "Smooth";
            this.SmoothBox.UseVisualStyleBackColor = true;
            // 
            // FlattenBox
            // 
            this.FlattenBox.AutoSize = true;
            this.FlattenBox.Location = new System.Drawing.Point(73, 97);
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
            this.FeatherBox.Location = new System.Drawing.Point(73, 74);
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
            // IntensityField
            // 
            this.IntensityField.Location = new System.Drawing.Point(57, 41);
            this.IntensityField.Name = "IntensityField";
            this.IntensityField.Size = new System.Drawing.Size(60, 20);
            this.IntensityField.TabIndex = 3;
            this.IntensityField.Text = "10";
            // 
            // RadiusField
            // 
            this.RadiusField.Location = new System.Drawing.Point(57, 15);
            this.RadiusField.Name = "RadiusField";
            this.RadiusField.Size = new System.Drawing.Size(60, 20);
            this.RadiusField.TabIndex = 0;
            this.RadiusField.Text = "10";
            // 
            // Textures
            // 
            this.Textures.Location = new System.Drawing.Point(4, 22);
            this.Textures.Name = "Textures";
            this.Textures.Padding = new System.Windows.Forms.Padding(3);
            this.Textures.Size = new System.Drawing.Size(190, 277);
            this.Textures.TabIndex = 2;
            this.Textures.Text = "Textures";
            this.Textures.UseVisualStyleBackColor = true;
            // 
            // EditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(222, 332);
            this.Controls.Add(this.EditTabs);
            this.Name = "EditorForm";
            this.Text = "Editor Menu";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.EditorMenu_Load);
            this.EditTabs.ResumeLayout(false);
            this.Objects.ResumeLayout(false);
            this.Heights.ResumeLayout(false);
            this.Heights.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl EditTabs;
        private System.Windows.Forms.TabPage Heights;
        private System.Windows.Forms.TabPage Textures;
        private System.Windows.Forms.TabPage Objects;
        private System.Windows.Forms.TextBox RadiusField;
        private System.Windows.Forms.TextBox IntensityField;
        private System.Windows.Forms.CheckBox SmoothBox;
        private System.Windows.Forms.CheckBox FlattenBox;
        private System.Windows.Forms.CheckBox InvertBox;
        private System.Windows.Forms.CheckBox FeatherBox;
        private System.Windows.Forms.CheckBox SetBox;
        private System.Windows.Forms.Label IntensityLabel;
        private System.Windows.Forms.Label RadiusLabel;
        private System.Windows.Forms.ListBox ObjectList;
    }
}