namespace WorldEditor.Dialogs
{
    partial class BrushSelectionForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BrushSelectionForm));
            this.BrushToolStrip = new System.Windows.Forms.ToolStrip();
            this.CircleBrushButton = new System.Windows.Forms.ToolStripButton();
            this.CircleFeatherBrushButton = new System.Windows.Forms.ToolStripButton();
            this.BlockBrushButton = new System.Windows.Forms.ToolStripButton();
            this.BlockFeatherBrushButton = new System.Windows.Forms.ToolStripButton();
            this.BrushLabel = new System.Windows.Forms.Label();
            this.shapeContainer1 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.lineShape1 = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.BrushToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // BrushToolStrip
            // 
            this.BrushToolStrip.AutoSize = false;
            this.BrushToolStrip.BackColor = System.Drawing.Color.Transparent;
            this.BrushToolStrip.ImageScalingSize = new System.Drawing.Size(40, 40);
            this.BrushToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CircleBrushButton,
            this.CircleFeatherBrushButton,
            this.BlockBrushButton,
            this.BlockFeatherBrushButton});
            this.BrushToolStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.BrushToolStrip.Location = new System.Drawing.Point(0, 0);
            this.BrushToolStrip.Name = "BrushToolStrip";
            this.BrushToolStrip.Size = new System.Drawing.Size(243, 47);
            this.BrushToolStrip.TabIndex = 0;
            this.BrushToolStrip.Text = "toolStrip1";
            // 
            // CircleBrushButton
            // 
            this.CircleBrushButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.CircleBrushButton.Image = ((System.Drawing.Image)(resources.GetObject("CircleBrushButton.Image")));
            this.CircleBrushButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.CircleBrushButton.Margin = new System.Windows.Forms.Padding(20, 1, 0, 2);
            this.CircleBrushButton.Name = "CircleBrushButton";
            this.CircleBrushButton.Size = new System.Drawing.Size(44, 44);
            this.CircleBrushButton.Text = "toolStripButton1";
            // 
            // CircleFeatherBrushButton
            // 
            this.CircleFeatherBrushButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.CircleFeatherBrushButton.Image = ((System.Drawing.Image)(resources.GetObject("CircleFeatherBrushButton.Image")));
            this.CircleFeatherBrushButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.CircleFeatherBrushButton.Margin = new System.Windows.Forms.Padding(10, 1, 0, 2);
            this.CircleFeatherBrushButton.Name = "CircleFeatherBrushButton";
            this.CircleFeatherBrushButton.Size = new System.Drawing.Size(44, 44);
            this.CircleFeatherBrushButton.Text = "toolStripButton2";
            // 
            // BlockBrushButton
            // 
            this.BlockBrushButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.BlockBrushButton.Image = ((System.Drawing.Image)(resources.GetObject("BlockBrushButton.Image")));
            this.BlockBrushButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BlockBrushButton.Margin = new System.Windows.Forms.Padding(10, 1, 0, 2);
            this.BlockBrushButton.Name = "BlockBrushButton";
            this.BlockBrushButton.Size = new System.Drawing.Size(44, 44);
            this.BlockBrushButton.Text = "toolStripButton3";
            // 
            // BlockFeatherBrushButton
            // 
            this.BlockFeatherBrushButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.BlockFeatherBrushButton.Image = ((System.Drawing.Image)(resources.GetObject("BlockFeatherBrushButton.Image")));
            this.BlockFeatherBrushButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BlockFeatherBrushButton.Margin = new System.Windows.Forms.Padding(10, 1, 0, 2);
            this.BlockFeatherBrushButton.Name = "BlockFeatherBrushButton";
            this.BlockFeatherBrushButton.Size = new System.Drawing.Size(44, 44);
            this.BlockFeatherBrushButton.Text = "toolStripButton4";
            // 
            // BrushLabel
            // 
            this.BrushLabel.AutoSize = true;
            this.BrushLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BrushLabel.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.BrushLabel.Location = new System.Drawing.Point(98, 53);
            this.BrushLabel.Name = "BrushLabel";
            this.BrushLabel.Size = new System.Drawing.Size(48, 15);
            this.BrushLabel.TabIndex = 1;
            this.BrushLabel.Text = "Brushes";
            // 
            // shapeContainer1
            // 
            this.shapeContainer1.Location = new System.Drawing.Point(0, 0);
            this.shapeContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer1.Name = "shapeContainer1";
            this.shapeContainer1.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.lineShape1});
            this.shapeContainer1.Size = new System.Drawing.Size(243, 92);
            this.shapeContainer1.TabIndex = 2;
            this.shapeContainer1.TabStop = false;
            // 
            // lineShape1
            // 
            this.lineShape1.BorderColor = System.Drawing.SystemColors.ControlDark;
            this.lineShape1.Name = "lineShape1";
            this.lineShape1.X1 = 15;
            this.lineShape1.X2 = 223;
            this.lineShape1.Y1 = 77;
            this.lineShape1.Y2 = 77;
            // 
            // BrushSelectionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.BrushLabel);
            this.Controls.Add(this.BrushToolStrip);
            this.Controls.Add(this.shapeContainer1);
            this.Name = "BrushSelectionForm";
            this.Size = new System.Drawing.Size(243, 92);
            this.BrushToolStrip.ResumeLayout(false);
            this.BrushToolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip BrushToolStrip;
        public System.Windows.Forms.ToolStripButton CircleBrushButton;
        public System.Windows.Forms.ToolStripButton CircleFeatherBrushButton;
        public System.Windows.Forms.ToolStripButton BlockBrushButton;
        public System.Windows.Forms.ToolStripButton BlockFeatherBrushButton;
        private System.Windows.Forms.Label BrushLabel;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer1;
        private Microsoft.VisualBasic.PowerPacks.LineShape lineShape1;
    }
}
