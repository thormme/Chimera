namespace WorldEditor.Dialogs
{
    partial class HeightMapBrushPropertiesForm
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
            this.BrushSizeTrackBar = new System.Windows.Forms.TrackBar();
            this.BrushMagnitudeTrackBar = new System.Windows.Forms.TrackBar();
            this.BrushSizeUpDown = new System.Windows.Forms.NumericUpDown();
            this.BrushMagnitudeUpDown = new System.Windows.Forms.NumericUpDown();
            this.SizeLabel = new System.Windows.Forms.Label();
            this.StrengthLabel = new System.Windows.Forms.Label();
            this.lineShape1 = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.shapeContainer1 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.lineShape2 = new Microsoft.VisualBasic.PowerPacks.LineShape();
            ((System.ComponentModel.ISupportInitialize)(this.BrushSizeTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BrushMagnitudeTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BrushSizeUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BrushMagnitudeUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // BrushSizeTrackBar
            // 
            this.BrushSizeTrackBar.Location = new System.Drawing.Point(1, 10);
            this.BrushSizeTrackBar.Maximum = 100;
            this.BrushSizeTrackBar.Minimum = 1;
            this.BrushSizeTrackBar.Name = "BrushSizeTrackBar";
            this.BrushSizeTrackBar.Size = new System.Drawing.Size(159, 45);
            this.BrushSizeTrackBar.TabIndex = 0;
            this.BrushSizeTrackBar.Value = 10;
            // 
            // BrushMagnitudeTrackBar
            // 
            this.BrushMagnitudeTrackBar.Location = new System.Drawing.Point(1, 93);
            this.BrushMagnitudeTrackBar.Maximum = 100;
            this.BrushMagnitudeTrackBar.Minimum = 1;
            this.BrushMagnitudeTrackBar.Name = "BrushMagnitudeTrackBar";
            this.BrushMagnitudeTrackBar.Size = new System.Drawing.Size(159, 45);
            this.BrushMagnitudeTrackBar.TabIndex = 1;
            this.BrushMagnitudeTrackBar.Value = 50;
            // 
            // BrushSizeUpDown
            // 
            this.BrushSizeUpDown.Location = new System.Drawing.Point(165, 10);
            this.BrushSizeUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.BrushSizeUpDown.Name = "BrushSizeUpDown";
            this.BrushSizeUpDown.Size = new System.Drawing.Size(60, 20);
            this.BrushSizeUpDown.TabIndex = 2;
            this.BrushSizeUpDown.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // BrushMagnitudeUpDown
            // 
            this.BrushMagnitudeUpDown.Location = new System.Drawing.Point(165, 93);
            this.BrushMagnitudeUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.BrushMagnitudeUpDown.Name = "BrushMagnitudeUpDown";
            this.BrushMagnitudeUpDown.Size = new System.Drawing.Size(60, 20);
            this.BrushMagnitudeUpDown.TabIndex = 3;
            this.BrushMagnitudeUpDown.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // SizeLabel
            // 
            this.SizeLabel.AutoSize = true;
            this.SizeLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SizeLabel.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.SizeLabel.Location = new System.Drawing.Point(82, 47);
            this.SizeLabel.Name = "SizeLabel";
            this.SizeLabel.Size = new System.Drawing.Size(65, 15);
            this.SizeLabel.TabIndex = 4;
            this.SizeLabel.Text = "Cursor Size";
            // 
            // StrengthLabel
            // 
            this.StrengthLabel.AutoSize = true;
            this.StrengthLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StrengthLabel.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.StrengthLabel.Location = new System.Drawing.Point(84, 131);
            this.StrengthLabel.Name = "StrengthLabel";
            this.StrengthLabel.Size = new System.Drawing.Size(79, 15);
            this.StrengthLabel.TabIndex = 5;
            this.StrengthLabel.Text = "Tool Strength";
            // 
            // lineShape1
            // 
            this.lineShape1.BorderColor = System.Drawing.SystemColors.ControlDark;
            this.lineShape1.Name = "lineShape1";
            this.lineShape1.X1 = 14;
            this.lineShape1.X2 = 217;
            this.lineShape1.Y1 = 72;
            this.lineShape1.Y2 = 72;
            // 
            // shapeContainer1
            // 
            this.shapeContainer1.Location = new System.Drawing.Point(0, 0);
            this.shapeContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer1.Name = "shapeContainer1";
            this.shapeContainer1.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.lineShape2,
            this.lineShape1});
            this.shapeContainer1.Size = new System.Drawing.Size(234, 171);
            this.shapeContainer1.TabIndex = 6;
            this.shapeContainer1.TabStop = false;
            // 
            // lineShape2
            // 
            this.lineShape2.BorderColor = System.Drawing.SystemColors.ControlDark;
            this.lineShape2.Name = "lineShape2";
            this.lineShape2.X1 = 14;
            this.lineShape2.X2 = 217;
            this.lineShape2.Y1 = 155;
            this.lineShape2.Y2 = 155;
            // 
            // HeightMapBrushPropertiesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.StrengthLabel);
            this.Controls.Add(this.SizeLabel);
            this.Controls.Add(this.BrushMagnitudeUpDown);
            this.Controls.Add(this.BrushSizeUpDown);
            this.Controls.Add(this.BrushMagnitudeTrackBar);
            this.Controls.Add(this.BrushSizeTrackBar);
            this.Controls.Add(this.shapeContainer1);
            this.Name = "HeightMapBrushPropertiesForm";
            this.Size = new System.Drawing.Size(234, 171);
            ((System.ComponentModel.ISupportInitialize)(this.BrushSizeTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BrushMagnitudeTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BrushSizeUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BrushMagnitudeUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TrackBar BrushSizeTrackBar;
        public System.Windows.Forms.TrackBar BrushMagnitudeTrackBar;
        public System.Windows.Forms.NumericUpDown BrushSizeUpDown;
        public System.Windows.Forms.NumericUpDown BrushMagnitudeUpDown;
        private System.Windows.Forms.Label SizeLabel;
        private System.Windows.Forms.Label StrengthLabel;
        private Microsoft.VisualBasic.PowerPacks.LineShape lineShape1;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer1;
        private Microsoft.VisualBasic.PowerPacks.LineShape lineShape2;
    }
}
