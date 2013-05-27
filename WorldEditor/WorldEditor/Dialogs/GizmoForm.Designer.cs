﻿namespace WorldEditor.Dialogs
{
    partial class GizmoForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GizmoForm));
            this.GizmoButtonStrip = new System.Windows.Forms.ToolStrip();
            this.TranslateButton = new System.Windows.Forms.ToolStripButton();
            this.RotateButton = new System.Windows.Forms.ToolStripButton();
            this.ScaleButton = new System.Windows.Forms.ToolStripButton();
            this.GizmoButtonStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // GizmoButtonStrip
            // 
            this.GizmoButtonStrip.AutoSize = false;
            this.GizmoButtonStrip.BackColor = System.Drawing.Color.Transparent;
            this.GizmoButtonStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.GizmoButtonStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.GizmoButtonStrip.ImageScalingSize = new System.Drawing.Size(40, 40);
            this.GizmoButtonStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TranslateButton,
            this.RotateButton,
            this.ScaleButton});
            this.GizmoButtonStrip.Location = new System.Drawing.Point(31, 4);
            this.GizmoButtonStrip.Name = "GizmoButtonStrip";
            this.GizmoButtonStrip.Size = new System.Drawing.Size(202, 52);
            this.GizmoButtonStrip.TabIndex = 0;
            this.GizmoButtonStrip.Text = "toolStrip1";
            // 
            // TranslateButton
            // 
            this.TranslateButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.TranslateButton.Image = ((System.Drawing.Image)(resources.GetObject("TranslateButton.Image")));
            this.TranslateButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TranslateButton.Name = "TranslateButton";
            this.TranslateButton.Size = new System.Drawing.Size(44, 49);
            this.TranslateButton.Text = "toolStripButton1";
            // 
            // RotateButton
            // 
            this.RotateButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.RotateButton.Image = ((System.Drawing.Image)(resources.GetObject("RotateButton.Image")));
            this.RotateButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.RotateButton.Margin = new System.Windows.Forms.Padding(25, 1, 0, 2);
            this.RotateButton.Name = "RotateButton";
            this.RotateButton.Size = new System.Drawing.Size(44, 49);
            this.RotateButton.Text = "toolStripButton2";
            // 
            // ScaleButton
            // 
            this.ScaleButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ScaleButton.Image = ((System.Drawing.Image)(resources.GetObject("ScaleButton.Image")));
            this.ScaleButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ScaleButton.Margin = new System.Windows.Forms.Padding(25, 1, 0, 2);
            this.ScaleButton.Name = "ScaleButton";
            this.ScaleButton.Size = new System.Drawing.Size(44, 49);
            this.ScaleButton.Text = "toolStripButton3";
            // 
            // GizmoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.GizmoButtonStrip);
            this.Name = "GizmoForm";
            this.Size = new System.Drawing.Size(245, 56);
            this.GizmoButtonStrip.ResumeLayout(false);
            this.GizmoButtonStrip.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStrip GizmoButtonStrip;
        public System.Windows.Forms.ToolStripButton TranslateButton;
        public System.Windows.Forms.ToolStripButton RotateButton;
        public System.Windows.Forms.ToolStripButton ScaleButton;
    }
}