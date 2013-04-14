namespace WorldEditor.Dialogs
{
    partial class EditorForm
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
            this.editorPanel = new Editor();
            this.ContextToolsPanel = new System.Windows.Forms.Panel();
            this.ObjectParametersForm = new ObjectParametersForm();
            this.ContextToolsPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // editorPanel
            // 
            this.editorPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.editorPanel.Location = new System.Drawing.Point(2, 74);
            this.editorPanel.Name = "editorPanel";
            this.editorPanel.Size = new System.Drawing.Size(688, 428);
            this.editorPanel.TabIndex = 0;
            // 
            // ContextToolsPanel
            // 
            this.ContextToolsPanel.AutoScroll = true;
            this.ContextToolsPanel.Controls.Add(this.ObjectParametersForm);
            this.ContextToolsPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.ContextToolsPanel.Location = new System.Drawing.Point(675, 0);
            this.ContextToolsPanel.Name = "ContextToolsPanel";
            this.ContextToolsPanel.Size = new System.Drawing.Size(251, 503);
            this.ContextToolsPanel.TabIndex = 1;
            // 
            // ObjectParametersForm
            // 
            this.ObjectParametersForm.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ObjectParametersForm.Dock = System.Windows.Forms.DockStyle.Top;
            this.ObjectParametersForm.Location = new System.Drawing.Point(0, 0);
            this.ObjectParametersForm.MinimumSize = new System.Drawing.Size(260, 160);
            this.ObjectParametersForm.Name = "ObjectParametersForm";
            this.ObjectParametersForm.Size = new System.Drawing.Size(260, 160);
            this.ObjectParametersForm.TabIndex = 0;
            // 
            // EditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(926, 503);
            this.Controls.Add(this.ContextToolsPanel);
            this.Controls.Add(this.editorPanel);
            this.Name = "EditorForm";
            this.Text = "EditorForm";
            this.ContextToolsPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Editor editorPanel;
        public System.Windows.Forms.Panel ContextToolsPanel;
        public ObjectParametersForm ObjectParametersForm;
    }
}