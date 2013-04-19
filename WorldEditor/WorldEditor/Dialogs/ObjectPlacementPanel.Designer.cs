namespace WorldEditor.Dialogs
{
    partial class ObjectPlacementPanel
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
            this.ObjectTree = new System.Windows.Forms.TreeView();
            this.PreviewPictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.PreviewPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // ObjectTree
            // 
            this.ObjectTree.Location = new System.Drawing.Point(3, 3);
            this.ObjectTree.Name = "ObjectTree";
            this.ObjectTree.Size = new System.Drawing.Size(229, 214);
            this.ObjectTree.TabIndex = 0;
            // 
            // PreviewPictureBox
            // 
            this.PreviewPictureBox.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.PreviewPictureBox.Location = new System.Drawing.Point(19, 235);
            this.PreviewPictureBox.Name = "PreviewPictureBox";
            this.PreviewPictureBox.Size = new System.Drawing.Size(200, 200);
            this.PreviewPictureBox.TabIndex = 1;
            this.PreviewPictureBox.TabStop = false;
            // 
            // ObjectPlacementPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.PreviewPictureBox);
            this.Controls.Add(this.ObjectTree);
            this.Name = "ObjectPlacementPanel";
            this.Size = new System.Drawing.Size(235, 455);
            ((System.ComponentModel.ISupportInitialize)(this.PreviewPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.TreeView ObjectTree;
        public System.Windows.Forms.PictureBox PreviewPictureBox;

    }
}
