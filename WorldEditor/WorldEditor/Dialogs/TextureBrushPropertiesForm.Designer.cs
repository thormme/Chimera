namespace WorldEditor.Dialogs
{
    partial class TextureBrushPropertiesForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TextureBrushPropertiesForm));
            this.BrushSizeTrackBar = new System.Windows.Forms.TrackBar();
            this.BrushMagnitudeTrackBar = new System.Windows.Forms.TrackBar();
            this.BrushSizeUpDown = new System.Windows.Forms.NumericUpDown();
            this.BrushMagnitudeUpDown = new System.Windows.Forms.NumericUpDown();
            this.StrengthLabel = new System.Windows.Forms.Label();
            this.SizeLabel = new System.Windows.Forms.Label();
            this.BrushToolStrip = new System.Windows.Forms.ToolStrip();
            this.CircleBrushButton = new System.Windows.Forms.ToolStripButton();
            this.CircleFeatherBrushButton = new System.Windows.Forms.ToolStripButton();
            this.BlockBrushButton = new System.Windows.Forms.ToolStripButton();
            this.BlockFeatherBrushButton = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.BrushSizeTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BrushMagnitudeTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BrushSizeUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BrushMagnitudeUpDown)).BeginInit();
            this.BrushToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // BrushSizeTrackBar
            // 
            this.BrushSizeTrackBar.Location = new System.Drawing.Point(0, 73);
            this.BrushSizeTrackBar.Maximum = 100;
            this.BrushSizeTrackBar.Minimum = 1;
            this.BrushSizeTrackBar.Name = "BrushSizeTrackBar";
            this.BrushSizeTrackBar.Size = new System.Drawing.Size(125, 45);
            this.BrushSizeTrackBar.TabIndex = 0;
            this.BrushSizeTrackBar.Value = 10;
            // 
            // BrushMagnitudeTrackBar
            // 
            this.BrushMagnitudeTrackBar.Location = new System.Drawing.Point(121, 73);
            this.BrushMagnitudeTrackBar.Maximum = 100;
            this.BrushMagnitudeTrackBar.Name = "BrushMagnitudeTrackBar";
            this.BrushMagnitudeTrackBar.Size = new System.Drawing.Size(125, 45);
            this.BrushMagnitudeTrackBar.TabIndex = 1;
            this.BrushMagnitudeTrackBar.Value = 100;
            // 
            // BrushSizeUpDown
            // 
            this.BrushSizeUpDown.Location = new System.Drawing.Point(73, 51);
            this.BrushSizeUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.BrushSizeUpDown.Name = "BrushSizeUpDown";
            this.BrushSizeUpDown.Size = new System.Drawing.Size(44, 20);
            this.BrushSizeUpDown.TabIndex = 2;
            this.BrushSizeUpDown.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // BrushMagnitudeUpDown
            // 
            this.BrushMagnitudeUpDown.Location = new System.Drawing.Point(193, 51);
            this.BrushMagnitudeUpDown.Name = "BrushMagnitudeUpDown";
            this.BrushMagnitudeUpDown.Size = new System.Drawing.Size(44, 20);
            this.BrushMagnitudeUpDown.TabIndex = 3;
            this.BrushMagnitudeUpDown.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // StrengthLabel
            // 
            this.StrengthLabel.AutoSize = true;
            this.StrengthLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StrengthLabel.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.StrengthLabel.Location = new System.Drawing.Point(127, 53);
            this.StrengthLabel.Name = "StrengthLabel";
            this.StrengthLabel.Size = new System.Drawing.Size(48, 15);
            this.StrengthLabel.TabIndex = 7;
            this.StrengthLabel.Text = "Opacity";
            // 
            // SizeLabel
            // 
            this.SizeLabel.AutoSize = true;
            this.SizeLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SizeLabel.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.SizeLabel.Location = new System.Drawing.Point(8, 53);
            this.SizeLabel.Name = "SizeLabel";
            this.SizeLabel.Size = new System.Drawing.Size(60, 15);
            this.SizeLabel.TabIndex = 6;
            this.SizeLabel.Text = "Brush Size";
            // 
            // BrushToolStrip
            // 
            this.BrushToolStrip.AutoSize = false;
            this.BrushToolStrip.BackColor = System.Drawing.Color.Transparent;
            this.BrushToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.BrushToolStrip.ImageScalingSize = new System.Drawing.Size(40, 40);
            this.BrushToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CircleBrushButton,
            this.CircleFeatherBrushButton,
            this.BlockBrushButton,
            this.BlockFeatherBrushButton});
            this.BrushToolStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.BrushToolStrip.Location = new System.Drawing.Point(10, 2);
            this.BrushToolStrip.Name = "BrushToolStrip";
            this.BrushToolStrip.Size = new System.Drawing.Size(224, 52);
            this.BrushToolStrip.TabIndex = 8;
            this.BrushToolStrip.Text = "toolStrip1";
            // 
            // CircleBrushButton
            // 
            this.CircleBrushButton.Checked = true;
            this.CircleBrushButton.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CircleBrushButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.CircleBrushButton.Image = ((System.Drawing.Image)(resources.GetObject("CircleBrushButton.Image")));
            this.CircleBrushButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.CircleBrushButton.Margin = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.CircleBrushButton.Name = "CircleBrushButton";
            this.CircleBrushButton.Size = new System.Drawing.Size(44, 44);
            this.CircleBrushButton.Text = "toolStripButton1";
            // 
            // CircleFeatherBrushButton
            // 
            this.CircleFeatherBrushButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.CircleFeatherBrushButton.Image = ((System.Drawing.Image)(resources.GetObject("CircleFeatherBrushButton.Image")));
            this.CircleFeatherBrushButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.CircleFeatherBrushButton.Margin = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.CircleFeatherBrushButton.Name = "CircleFeatherBrushButton";
            this.CircleFeatherBrushButton.Size = new System.Drawing.Size(44, 44);
            this.CircleFeatherBrushButton.Text = "toolStripButton2";
            // 
            // BlockBrushButton
            // 
            this.BlockBrushButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.BlockBrushButton.Image = ((System.Drawing.Image)(resources.GetObject("BlockBrushButton.Image")));
            this.BlockBrushButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BlockBrushButton.Margin = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.BlockBrushButton.Name = "BlockBrushButton";
            this.BlockBrushButton.Size = new System.Drawing.Size(44, 44);
            this.BlockBrushButton.Text = "toolStripButton3";
            // 
            // BlockFeatherBrushButton
            // 
            this.BlockFeatherBrushButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.BlockFeatherBrushButton.Image = ((System.Drawing.Image)(resources.GetObject("BlockFeatherBrushButton.Image")));
            this.BlockFeatherBrushButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BlockFeatherBrushButton.Margin = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.BlockFeatherBrushButton.Name = "BlockFeatherBrushButton";
            this.BlockFeatherBrushButton.Size = new System.Drawing.Size(44, 44);
            this.BlockFeatherBrushButton.Text = "toolStripButton4";
            // 
            // TextureBrushPropertiesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.StrengthLabel);
            this.Controls.Add(this.SizeLabel);
            this.Controls.Add(this.BrushMagnitudeUpDown);
            this.Controls.Add(this.BrushSizeUpDown);
            this.Controls.Add(this.BrushMagnitudeTrackBar);
            this.Controls.Add(this.BrushSizeTrackBar);
            this.Controls.Add(this.BrushToolStrip);
            this.Name = "TextureBrushPropertiesForm";
            this.Size = new System.Drawing.Size(243, 109);
            ((System.ComponentModel.ISupportInitialize)(this.BrushSizeTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BrushMagnitudeTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BrushSizeUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BrushMagnitudeUpDown)).EndInit();
            this.BrushToolStrip.ResumeLayout(false);
            this.BrushToolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TrackBar BrushSizeTrackBar;
        public System.Windows.Forms.TrackBar BrushMagnitudeTrackBar;
        private System.Windows.Forms.NumericUpDown BrushSizeUpDown;
        private System.Windows.Forms.NumericUpDown BrushMagnitudeUpDown;
        private System.Windows.Forms.Label StrengthLabel;
        private System.Windows.Forms.Label SizeLabel;
        public System.Windows.Forms.ToolStrip BrushToolStrip;
        public System.Windows.Forms.ToolStripButton CircleBrushButton;
        public System.Windows.Forms.ToolStripButton CircleFeatherBrushButton;
        public System.Windows.Forms.ToolStripButton BlockBrushButton;
        public System.Windows.Forms.ToolStripButton BlockFeatherBrushButton;
    }
}
