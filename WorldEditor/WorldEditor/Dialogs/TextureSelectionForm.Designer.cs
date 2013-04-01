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
            this.textureList = new System.Windows.Forms.ListBox();
            this.texturePreview = new System.Windows.Forms.PictureBox();
            this.VOffset = new System.Windows.Forms.NumericUpDown();
            this.UOffset = new System.Windows.Forms.NumericUpDown();
            this.VScale = new System.Windows.Forms.NumericUpDown();
            this.UScale = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.texturePreview)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VOffset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UOffset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VScale)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UScale)).BeginInit();
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
            this.textureList.Location = new System.Drawing.Point(66, 34);
            this.textureList.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.textureList.Name = "textureList";
            this.textureList.Size = new System.Drawing.Size(168, 218);
            this.textureList.TabIndex = 3;
            // 
            // texturePreview
            // 
            this.texturePreview.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.texturePreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.texturePreview.Location = new System.Drawing.Point(38, 291);
            this.texturePreview.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.texturePreview.Name = "texturePreview";
            this.texturePreview.Size = new System.Drawing.Size(223, 218);
            this.texturePreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.texturePreview.TabIndex = 2;
            this.texturePreview.TabStop = false;
            // 
            // VOffset
            // 
            this.VOffset.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(93)))), ((int)(((byte)(93)))), ((int)(((byte)(93)))));
            this.VOffset.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.VOffset.DecimalPlaces = 1;
            this.VOffset.Font = new System.Drawing.Font("Arial", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.VOffset.ForeColor = System.Drawing.Color.White;
            this.VOffset.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.VOffset.Location = new System.Drawing.Point(159, 548);
            this.VOffset.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.VOffset.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.VOffset.Name = "VOffset";
            this.VOffset.Size = new System.Drawing.Size(115, 48);
            this.VOffset.TabIndex = 16;
            this.VOffset.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // UOffset
            // 
            this.UOffset.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(93)))), ((int)(((byte)(93)))), ((int)(((byte)(93)))));
            this.UOffset.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.UOffset.DecimalPlaces = 1;
            this.UOffset.Font = new System.Drawing.Font("Arial", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UOffset.ForeColor = System.Drawing.Color.White;
            this.UOffset.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.UOffset.Location = new System.Drawing.Point(38, 548);
            this.UOffset.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.UOffset.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.UOffset.Name = "UOffset";
            this.UOffset.Size = new System.Drawing.Size(115, 48);
            this.UOffset.TabIndex = 15;
            this.UOffset.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // VScale
            // 
            this.VScale.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(93)))), ((int)(((byte)(93)))), ((int)(((byte)(93)))));
            this.VScale.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.VScale.DecimalPlaces = 1;
            this.VScale.Font = new System.Drawing.Font("Arial", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.VScale.ForeColor = System.Drawing.Color.White;
            this.VScale.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.VScale.Location = new System.Drawing.Point(159, 684);
            this.VScale.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.VScale.Name = "VScale";
            this.VScale.Size = new System.Drawing.Size(115, 48);
            this.VScale.TabIndex = 18;
            this.VScale.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.VScale.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // UScale
            // 
            this.UScale.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(93)))), ((int)(((byte)(93)))), ((int)(((byte)(93)))));
            this.UScale.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.UScale.DecimalPlaces = 1;
            this.UScale.Font = new System.Drawing.Font("Arial", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UScale.ForeColor = System.Drawing.Color.White;
            this.UScale.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.UScale.Location = new System.Drawing.Point(38, 684);
            this.UScale.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.UScale.Name = "UScale";
            this.UScale.Size = new System.Drawing.Size(115, 48);
            this.UScale.TabIndex = 17;
            this.UScale.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.UScale.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // TextureSelectionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(97)))), ((int)(((byte)(97)))), ((int)(((byte)(97)))));
            this.Controls.Add(this.VScale);
            this.Controls.Add(this.UScale);
            this.Controls.Add(this.VOffset);
            this.Controls.Add(this.UOffset);
            this.Controls.Add(this.textureList);
            this.Controls.Add(this.texturePreview);
            this.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "TextureSelectionForm";
            this.Size = new System.Drawing.Size(310, 772);
            ((System.ComponentModel.ISupportInitialize)(this.texturePreview)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.VOffset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UOffset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.VScale)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UScale)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ListBox textureList;
        private PictureBox texturePreview;
        public NumericUpDown VOffset;
        public NumericUpDown UOffset;
        public NumericUpDown VScale;
        public NumericUpDown UScale;

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