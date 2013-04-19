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
            this.texturePreview = new System.Windows.Forms.PictureBox();
            this.textureList = new System.Windows.Forms.ListBox();
            this.UScale = new System.Windows.Forms.NumericUpDown();
            this.VScale = new System.Windows.Forms.NumericUpDown();
            this.VOffset = new System.Windows.Forms.NumericUpDown();
            this.UOffset = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.texturePreview)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UScale)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VScale)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VOffset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UOffset)).BeginInit();
            this.SuspendLayout();
            // 
            // texturePreview
            // 
            this.texturePreview.BackColor = System.Drawing.Color.Black;
            this.texturePreview.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.texturePreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.texturePreview.Location = new System.Drawing.Point(19, 235);
            this.texturePreview.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.texturePreview.Name = "texturePreview";
            this.texturePreview.Size = new System.Drawing.Size(200, 200);
            this.texturePreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.texturePreview.TabIndex = 2;
            this.texturePreview.TabStop = false;
            // 
            // textureList
            // 
            this.textureList.BackColor = System.Drawing.SystemColors.Window;
            this.textureList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textureList.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textureList.ForeColor = System.Drawing.Color.Black;
            this.textureList.FormattingEnabled = true;
            this.textureList.HorizontalScrollbar = true;
            this.textureList.Location = new System.Drawing.Point(3, 3);
            this.textureList.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.textureList.Name = "textureList";
            this.textureList.Size = new System.Drawing.Size(229, 197);
            this.textureList.TabIndex = 20;
            // 
            // UScale
            // 
            this.UScale.DecimalPlaces = 2;
            this.UScale.Location = new System.Drawing.Point(19, 457);
            this.UScale.Name = "UScale";
            this.UScale.Size = new System.Drawing.Size(93, 26);
            this.UScale.TabIndex = 21;
            this.UScale.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // VScale
            // 
            this.VScale.DecimalPlaces = 2;
            this.VScale.Location = new System.Drawing.Point(126, 457);
            this.VScale.Name = "VScale";
            this.VScale.Size = new System.Drawing.Size(93, 26);
            this.VScale.TabIndex = 22;
            this.VScale.Value = new decimal(new int[] {
            10,
            0,
            0,
            65536});
            // 
            // VOffset
            // 
            this.VOffset.DecimalPlaces = 2;
            this.VOffset.Location = new System.Drawing.Point(126, 506);
            this.VOffset.Name = "VOffset";
            this.VOffset.Size = new System.Drawing.Size(93, 26);
            this.VOffset.TabIndex = 24;
            // 
            // UOffset
            // 
            this.UOffset.DecimalPlaces = 2;
            this.UOffset.Location = new System.Drawing.Point(19, 506);
            this.UOffset.Name = "UOffset";
            this.UOffset.Size = new System.Drawing.Size(93, 26);
            this.UOffset.TabIndex = 23;
            // 
            // TextureSelectionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.VOffset);
            this.Controls.Add(this.UOffset);
            this.Controls.Add(this.VScale);
            this.Controls.Add(this.UScale);
            this.Controls.Add(this.textureList);
            this.Controls.Add(this.texturePreview);
            this.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "TextureSelectionForm";
            this.Size = new System.Drawing.Size(235, 555);
            ((System.ComponentModel.ISupportInitialize)(this.texturePreview)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UScale)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.VScale)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.VOffset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UOffset)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private PictureBox texturePreview;
        private ListBox textureList;
        public NumericUpDown UScale;
        public NumericUpDown VScale;
        public NumericUpDown VOffset;
        public NumericUpDown UOffset;

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