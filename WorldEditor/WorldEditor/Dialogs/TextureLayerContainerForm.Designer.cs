namespace WorldEditor.Dialogs
{
    partial class TextureLayerContainerForm
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
            this.LayerPanel = new System.Windows.Forms.Panel();
            this.Layer4 = new WorldEditor.Dialogs.TextureLayerForm();
            this.Layer3 = new WorldEditor.Dialogs.TextureLayerForm();
            this.Layer2 = new WorldEditor.Dialogs.TextureLayerForm();
            this.Layer1 = new WorldEditor.Dialogs.TextureLayerForm();
            this.BackgroundLayer = new WorldEditor.Dialogs.TextureLayerForm();
            this.LayerPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // LayerPanel
            // 
            this.LayerPanel.BackColor = System.Drawing.SystemColors.ControlDark;
            this.LayerPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LayerPanel.Controls.Add(this.Layer4);
            this.LayerPanel.Controls.Add(this.Layer3);
            this.LayerPanel.Controls.Add(this.Layer2);
            this.LayerPanel.Controls.Add(this.Layer1);
            this.LayerPanel.Controls.Add(this.BackgroundLayer);
            this.LayerPanel.Location = new System.Drawing.Point(10, 10);
            this.LayerPanel.Name = "LayerPanel";
            this.LayerPanel.Size = new System.Drawing.Size(223, 191);
            this.LayerPanel.TabIndex = 0;
            // 
            // Layer4
            // 
            this.Layer4.BackColor = System.Drawing.SystemColors.ControlDark;
            this.Layer4.Dock = System.Windows.Forms.DockStyle.Top;
            this.Layer4.Location = new System.Drawing.Point(0, 152);
            this.Layer4.Name = "Layer4";
            this.Layer4.Size = new System.Drawing.Size(221, 38);
            this.Layer4.TabIndex = 5;
            // 
            // Layer3
            // 
            this.Layer3.BackColor = System.Drawing.SystemColors.ControlDark;
            this.Layer3.Dock = System.Windows.Forms.DockStyle.Top;
            this.Layer3.Location = new System.Drawing.Point(0, 114);
            this.Layer3.Name = "Layer3";
            this.Layer3.Size = new System.Drawing.Size(221, 38);
            this.Layer3.TabIndex = 4;
            // 
            // Layer2
            // 
            this.Layer2.BackColor = System.Drawing.SystemColors.ControlDark;
            this.Layer2.Dock = System.Windows.Forms.DockStyle.Top;
            this.Layer2.Location = new System.Drawing.Point(0, 76);
            this.Layer2.Name = "Layer2";
            this.Layer2.Size = new System.Drawing.Size(221, 38);
            this.Layer2.TabIndex = 3;
            // 
            // Layer1
            // 
            this.Layer1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.Layer1.Dock = System.Windows.Forms.DockStyle.Top;
            this.Layer1.Location = new System.Drawing.Point(0, 38);
            this.Layer1.Name = "Layer1";
            this.Layer1.Size = new System.Drawing.Size(221, 38);
            this.Layer1.TabIndex = 2;
            // 
            // BackgroundLayer
            // 
            this.BackgroundLayer.BackColor = System.Drawing.SystemColors.ControlDark;
            this.BackgroundLayer.Dock = System.Windows.Forms.DockStyle.Top;
            this.BackgroundLayer.Location = new System.Drawing.Point(0, 0);
            this.BackgroundLayer.Name = "BackgroundLayer";
            this.BackgroundLayer.Size = new System.Drawing.Size(221, 38);
            this.BackgroundLayer.TabIndex = 1;
            // 
            // TextureLayerContainerForm
            // 
            this.Controls.Add(this.LayerPanel);
            this.Name = "TextureLayerContainerForm";
            this.Size = new System.Drawing.Size(243, 211);
            this.LayerPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel LayerPanel;
        public TextureLayerForm Layer4;
        public TextureLayerForm Layer3;
        public TextureLayerForm Layer2;
        public TextureLayerForm Layer1;
        public TextureLayerForm BackgroundLayer;

    }
}
