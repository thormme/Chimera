namespace WorldEditor.Dialogs
{
    partial class TextureLayerForm
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
            this.LayerTexturePreview = new System.Windows.Forms.PictureBox();
            this.LayerVisibilityButton = new System.Windows.Forms.Button();
            this.MainPanel = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.InvisibleButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.LayerTexturePreview)).BeginInit();
            this.MainPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // LayerTexturePreview
            // 
            this.LayerTexturePreview.BackColor = System.Drawing.Color.Black;
            this.LayerTexturePreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LayerTexturePreview.Location = new System.Drawing.Point(4, 4);
            this.LayerTexturePreview.Name = "LayerTexturePreview";
            this.LayerTexturePreview.Size = new System.Drawing.Size(30, 30);
            this.LayerTexturePreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.LayerTexturePreview.TabIndex = 0;
            this.LayerTexturePreview.TabStop = false;
            // 
            // LayerVisibilityButton
            // 
            this.LayerVisibilityButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.LayerVisibilityButton.Location = new System.Drawing.Point(192, 8);
            this.LayerVisibilityButton.Name = "LayerVisibilityButton";
            this.LayerVisibilityButton.Size = new System.Drawing.Size(22, 22);
            this.LayerVisibilityButton.TabIndex = 2;
            this.LayerVisibilityButton.UseVisualStyleBackColor = true;
            // 
            // MainPanel
            // 
            this.MainPanel.BackColor = System.Drawing.SystemColors.Window;
            this.MainPanel.Controls.Add(this.InvisibleButton);
            this.MainPanel.Location = new System.Drawing.Point(1, 1);
            this.MainPanel.Name = "MainPanel";
            this.MainPanel.Size = new System.Drawing.Size(183, 36);
            this.MainPanel.TabIndex = 3;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.Window;
            this.panel2.Location = new System.Drawing.Point(185, 1);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(36, 36);
            this.panel2.TabIndex = 4;
            // 
            // InvisibleButton
            // 
            this.InvisibleButton.BackColor = System.Drawing.Color.Transparent;
            this.InvisibleButton.FlatAppearance.BorderSize = 0;
            this.InvisibleButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.InvisibleButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.InvisibleButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.InvisibleButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InvisibleButton.ForeColor = System.Drawing.Color.Black;
            this.InvisibleButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.InvisibleButton.Location = new System.Drawing.Point(1, 1);
            this.InvisibleButton.Name = "InvisibleButton";
            this.InvisibleButton.Size = new System.Drawing.Size(183, 34);
            this.InvisibleButton.TabIndex = 0;
            this.InvisibleButton.Text = "           ";
            this.InvisibleButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.InvisibleButton.UseVisualStyleBackColor = false;
            // 
            // TextureLayerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.Controls.Add(this.LayerVisibilityButton);
            this.Controls.Add(this.LayerTexturePreview);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.MainPanel);
            this.Name = "TextureLayerForm";
            this.Size = new System.Drawing.Size(222, 38);
            ((System.ComponentModel.ISupportInitialize)(this.LayerTexturePreview)).EndInit();
            this.MainPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.PictureBox LayerTexturePreview;
        public System.Windows.Forms.Button LayerVisibilityButton;
        public System.Windows.Forms.Panel MainPanel;
        public System.Windows.Forms.Panel panel2;
        public System.Windows.Forms.Button InvisibleButton;
    }
}
