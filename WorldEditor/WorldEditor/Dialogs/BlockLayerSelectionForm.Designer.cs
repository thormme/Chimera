namespace WorldEditor.Dialogs
{
    partial class BlockLayerSelectionForm
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
            this.BlockLayerUpDown = new System.Windows.Forms.NumericUpDown();
            this.SizeLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.BlockLayerUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // BlockLayerUpDown
            // 
            this.BlockLayerUpDown.Location = new System.Drawing.Point(117, 4);
            this.BlockLayerUpDown.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.BlockLayerUpDown.Minimum = new decimal(new int[] {
            999999999,
            0,
            0,
            -2147483648});
            this.BlockLayerUpDown.Name = "BlockLayerUpDown";
            this.BlockLayerUpDown.Size = new System.Drawing.Size(68, 20);
            this.BlockLayerUpDown.TabIndex = 0;
            // 
            // SizeLabel
            // 
            this.SizeLabel.AutoSize = true;
            this.SizeLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SizeLabel.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.SizeLabel.Location = new System.Drawing.Point(39, 6);
            this.SizeLabel.Name = "SizeLabel";
            this.SizeLabel.Size = new System.Drawing.Size(67, 15);
            this.SizeLabel.TabIndex = 7;
            this.SizeLabel.Text = "Block Layer";
            // 
            // BlockLayerSelectionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.SizeLabel);
            this.Controls.Add(this.BlockLayerUpDown);
            this.Name = "BlockLayerSelectionForm";
            this.Size = new System.Drawing.Size(243, 30);
            ((System.ComponentModel.ISupportInitialize)(this.BlockLayerUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label SizeLabel;
        public System.Windows.Forms.NumericUpDown BlockLayerUpDown;
    }
}
