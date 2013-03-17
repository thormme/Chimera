using System.Windows.Forms;
using System.Drawing;
namespace WorldEditor.Dialogs
{
    partial class TextureSelectionForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TextureSelectionForm));
            this.textureList = new System.Windows.Forms.ListBox();
            this.texturePreview = new System.Windows.Forms.PictureBox();
            this.textureBackground = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.texturePreview)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textureBackground)).BeginInit();
            this.SuspendLayout();
            // 
            // textureList
            // 
            this.textureList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(93)))), ((int)(((byte)(93)))), ((int)(((byte)(93)))));
            this.textureList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textureList.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textureList.ForeColor = System.Drawing.Color.White;
            this.textureList.FormattingEnabled = true;
            this.textureList.HorizontalScrollbar = true;
            this.textureList.ItemHeight = 18;
            this.textureList.Location = new System.Drawing.Point(38, 35);
            this.textureList.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.textureList.Name = "textureList";
            this.textureList.Size = new System.Drawing.Size(168, 218);
            this.textureList.TabIndex = 3;
            // 
            // texturePreview
            // 
            this.texturePreview.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.texturePreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.texturePreview.Location = new System.Drawing.Point(255, 36);
            this.texturePreview.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.texturePreview.Name = "texturePreview";
            this.texturePreview.Size = new System.Drawing.Size(223, 218);
            this.texturePreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.texturePreview.TabIndex = 2;
            this.texturePreview.TabStop = false;
            // 
            // textureBackground
            // 
            this.textureBackground.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("textureBackground.BackgroundImage")));
            this.textureBackground.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.textureBackground.Location = new System.Drawing.Point(-2, -1);
            this.textureBackground.Name = "textureBackground";
            this.textureBackground.Size = new System.Drawing.Size(522, 327);
            this.textureBackground.TabIndex = 4;
            this.textureBackground.TabStop = false;
            // 
            // TextureSelectionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(97)))), ((int)(((byte)(97)))), ((int)(((byte)(97)))));
            this.ClientSize = new System.Drawing.Size(521, 331);
            this.ControlBox = false;
            this.Controls.Add(this.textureList);
            this.Controls.Add(this.texturePreview);
            this.Controls.Add(this.textureBackground);
            this.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "TextureSelectionForm";
            this.Text = "Texture Selection";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.texturePreview)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textureBackground)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ListBox textureList;
        private PictureBox texturePreview;
        private PictureBox textureBackground;

        public ListBox TextureList
        {
            get { return textureList; }
        }

        public PictureBox TexturePreview
        {
            get { return texturePreview; }
        }
    }
}