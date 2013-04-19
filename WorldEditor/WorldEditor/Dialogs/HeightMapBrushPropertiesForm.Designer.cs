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
            this.BrushMagnitudeTrackBar.Location = new System.Drawing.Point(1, 42);
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
            this.BrushMagnitudeUpDown.Location = new System.Drawing.Point(165, 42);
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
            // HeightMapBrushPropertiesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.BrushMagnitudeUpDown);
            this.Controls.Add(this.BrushSizeUpDown);
            this.Controls.Add(this.BrushMagnitudeTrackBar);
            this.Controls.Add(this.BrushSizeTrackBar);
            this.Name = "HeightMapBrushPropertiesForm";
            this.Size = new System.Drawing.Size(234, 76);
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
    }
}
