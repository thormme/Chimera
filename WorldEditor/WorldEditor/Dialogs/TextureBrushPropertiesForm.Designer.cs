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
            this.BrushSizeTrackBar = new System.Windows.Forms.TrackBar();
            this.BrushMagnitudeTrackBar = new System.Windows.Forms.TrackBar();
            this.BrushSizeTextBox = new System.Windows.Forms.TextBox();
            this.BrushMagnitudeTextBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.BrushSizeTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BrushMagnitudeTrackBar)).BeginInit();
            this.SuspendLayout();
            // 
            // BrushSizeTrackBar
            // 
            this.BrushSizeTrackBar.Location = new System.Drawing.Point(1, 10);
            this.BrushSizeTrackBar.Name = "BrushSizeTrackBar";
            this.BrushSizeTrackBar.Size = new System.Drawing.Size(159, 45);
            this.BrushSizeTrackBar.TabIndex = 0;
            // 
            // BrushMagnitudeTrackBar
            // 
            this.BrushMagnitudeTrackBar.Location = new System.Drawing.Point(1, 42);
            this.BrushMagnitudeTrackBar.Name = "BrushMagnitudeTrackBar";
            this.BrushMagnitudeTrackBar.Size = new System.Drawing.Size(159, 45);
            this.BrushMagnitudeTrackBar.TabIndex = 1;
            // 
            // BrushSizeTextBox
            // 
            this.BrushSizeTextBox.Location = new System.Drawing.Point(165, 12);
            this.BrushSizeTextBox.Name = "BrushSizeTextBox";
            this.BrushSizeTextBox.Size = new System.Drawing.Size(60, 20);
            this.BrushSizeTextBox.TabIndex = 2;
            // 
            // BrushMagnitudeTextBox
            // 
            this.BrushMagnitudeTextBox.Location = new System.Drawing.Point(165, 44);
            this.BrushMagnitudeTextBox.Name = "BrushMagnitudeTextBox";
            this.BrushMagnitudeTextBox.Size = new System.Drawing.Size(60, 20);
            this.BrushMagnitudeTextBox.TabIndex = 3;
            // 
            // BrushPropertiesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.BrushMagnitudeTextBox);
            this.Controls.Add(this.BrushSizeTextBox);
            this.Controls.Add(this.BrushMagnitudeTrackBar);
            this.Controls.Add(this.BrushSizeTrackBar);
            this.Name = "BrushPropertiesForm";
            this.Size = new System.Drawing.Size(234, 76);
            ((System.ComponentModel.ISupportInitialize)(this.BrushSizeTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BrushMagnitudeTrackBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TrackBar BrushSizeTrackBar;
        private System.Windows.Forms.TrackBar BrushMagnitudeTrackBar;
        private System.Windows.Forms.TextBox BrushSizeTextBox;
        private System.Windows.Forms.TextBox BrushMagnitudeTextBox;
    }
}
