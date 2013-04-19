namespace WorldEditor.Dialogs
{
    partial class ObjectParametersForm
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
            this.Physical = new System.Windows.Forms.CheckBox();
            this.PositionLabel = new System.Windows.Forms.Label();
            this.OrientationLabel = new System.Windows.Forms.Label();
            this.ScaleLabel = new System.Windows.Forms.Label();
            this.HeightLabel = new System.Windows.Forms.Label();
            this.PositionX = new System.Windows.Forms.NumericUpDown();
            this.PositionZ = new System.Windows.Forms.NumericUpDown();
            this.Yaw = new System.Windows.Forms.NumericUpDown();
            this.Pitch = new System.Windows.Forms.NumericUpDown();
            this.Roll = new System.Windows.Forms.NumericUpDown();
            this.ScaleX = new System.Windows.Forms.NumericUpDown();
            this.ScaleY = new System.Windows.Forms.NumericUpDown();
            this.ScaleZ = new System.Windows.Forms.NumericUpDown();
            this.FloatingHeight = new System.Windows.Forms.NumericUpDown();
            this.PositionY = new System.Windows.Forms.NumericUpDown();
            this.Floating = new System.Windows.Forms.CheckBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.AdditionalParametersPanel = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.PositionX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PositionZ)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Yaw)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Pitch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Roll)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScaleX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScaleY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScaleZ)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FloatingHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PositionY)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // Physical
            // 
            this.Physical.AutoSize = true;
            this.Physical.Location = new System.Drawing.Point(179, 88);
            this.Physical.Name = "Physical";
            this.Physical.Size = new System.Drawing.Size(65, 17);
            this.Physical.TabIndex = 0;
            this.Physical.Text = "Physical";
            this.Physical.UseVisualStyleBackColor = true;
            this.Physical.CheckedChanged += new System.EventHandler(this.Physical_CheckedChanged);
            // 
            // PositionLabel
            // 
            this.PositionLabel.AutoSize = true;
            this.PositionLabel.Location = new System.Drawing.Point(4, 8);
            this.PositionLabel.Name = "PositionLabel";
            this.PositionLabel.Size = new System.Drawing.Size(47, 13);
            this.PositionLabel.TabIndex = 1;
            this.PositionLabel.Text = "Position:";
            this.PositionLabel.Click += new System.EventHandler(this.PositionLabel_Click);
            // 
            // OrientationLabel
            // 
            this.OrientationLabel.AutoSize = true;
            this.OrientationLabel.Location = new System.Drawing.Point(3, 34);
            this.OrientationLabel.Name = "OrientationLabel";
            this.OrientationLabel.Size = new System.Drawing.Size(50, 13);
            this.OrientationLabel.TabIndex = 2;
            this.OrientationLabel.Text = "Rotation:";
            this.OrientationLabel.Click += new System.EventHandler(this.OrientationLabel_Click);
            // 
            // ScaleLabel
            // 
            this.ScaleLabel.AutoSize = true;
            this.ScaleLabel.Location = new System.Drawing.Point(3, 60);
            this.ScaleLabel.Name = "ScaleLabel";
            this.ScaleLabel.Size = new System.Drawing.Size(37, 13);
            this.ScaleLabel.TabIndex = 3;
            this.ScaleLabel.Text = "Scale:";
            this.ScaleLabel.Click += new System.EventHandler(this.ScaleLabel_Click);
            // 
            // HeightLabel
            // 
            this.HeightLabel.AutoSize = true;
            this.HeightLabel.Location = new System.Drawing.Point(3, 87);
            this.HeightLabel.Name = "HeightLabel";
            this.HeightLabel.Size = new System.Drawing.Size(41, 13);
            this.HeightLabel.TabIndex = 4;
            this.HeightLabel.Text = "Height:";
            this.HeightLabel.Click += new System.EventHandler(this.HeightLabel_Click);
            // 
            // PositionX
            // 
            this.PositionX.DecimalPlaces = 2;
            this.PositionX.Location = new System.Drawing.Point(53, 6);
            this.PositionX.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.PositionX.Minimum = new decimal(new int[] {
            10000,
            0,
            0,
            -2147483648});
            this.PositionX.Name = "PositionX";
            this.PositionX.Size = new System.Drawing.Size(60, 20);
            this.PositionX.TabIndex = 5;
            this.PositionX.ValueChanged += new System.EventHandler(this.PositionX_ValueChanged);
            // 
            // PositionZ
            // 
            this.PositionZ.DecimalPlaces = 2;
            this.PositionZ.Location = new System.Drawing.Point(178, 6);
            this.PositionZ.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.PositionZ.Minimum = new decimal(new int[] {
            10000,
            0,
            0,
            -2147483648});
            this.PositionZ.Name = "PositionZ";
            this.PositionZ.Size = new System.Drawing.Size(60, 20);
            this.PositionZ.TabIndex = 7;
            this.PositionZ.ValueChanged += new System.EventHandler(this.PositionZ_ValueChanged);
            // 
            // Yaw
            // 
            this.Yaw.DecimalPlaces = 2;
            this.Yaw.Location = new System.Drawing.Point(53, 32);
            this.Yaw.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.Yaw.Minimum = new decimal(new int[] {
            10000,
            0,
            0,
            -2147483648});
            this.Yaw.Name = "Yaw";
            this.Yaw.Size = new System.Drawing.Size(60, 20);
            this.Yaw.TabIndex = 8;
            this.Yaw.ValueChanged += new System.EventHandler(this.Yaw_ValueChanged);
            // 
            // Pitch
            // 
            this.Pitch.DecimalPlaces = 2;
            this.Pitch.Location = new System.Drawing.Point(116, 32);
            this.Pitch.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.Pitch.Minimum = new decimal(new int[] {
            10000,
            0,
            0,
            -2147483648});
            this.Pitch.Name = "Pitch";
            this.Pitch.Size = new System.Drawing.Size(60, 20);
            this.Pitch.TabIndex = 9;
            this.Pitch.ValueChanged += new System.EventHandler(this.Pitch_ValueChanged);
            // 
            // Roll
            // 
            this.Roll.DecimalPlaces = 2;
            this.Roll.Location = new System.Drawing.Point(178, 32);
            this.Roll.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.Roll.Minimum = new decimal(new int[] {
            10000,
            0,
            0,
            -2147483648});
            this.Roll.Name = "Roll";
            this.Roll.Size = new System.Drawing.Size(60, 20);
            this.Roll.TabIndex = 10;
            this.Roll.ValueChanged += new System.EventHandler(this.Roll_ValueChanged);
            // 
            // ScaleX
            // 
            this.ScaleX.DecimalPlaces = 2;
            this.ScaleX.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.ScaleX.Location = new System.Drawing.Point(53, 58);
            this.ScaleX.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.ScaleX.Minimum = new decimal(new int[] {
            10000,
            0,
            0,
            -2147483648});
            this.ScaleX.Name = "ScaleX";
            this.ScaleX.Size = new System.Drawing.Size(60, 20);
            this.ScaleX.TabIndex = 11;
            this.ScaleX.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ScaleX.ValueChanged += new System.EventHandler(this.ScaleX_ValueChanged);
            // 
            // ScaleY
            // 
            this.ScaleY.DecimalPlaces = 2;
            this.ScaleY.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.ScaleY.Location = new System.Drawing.Point(116, 58);
            this.ScaleY.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.ScaleY.Minimum = new decimal(new int[] {
            10000,
            0,
            0,
            -2147483648});
            this.ScaleY.Name = "ScaleY";
            this.ScaleY.Size = new System.Drawing.Size(60, 20);
            this.ScaleY.TabIndex = 12;
            this.ScaleY.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ScaleY.ValueChanged += new System.EventHandler(this.ScaleY_ValueChanged);
            // 
            // ScaleZ
            // 
            this.ScaleZ.DecimalPlaces = 2;
            this.ScaleZ.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.ScaleZ.Location = new System.Drawing.Point(178, 58);
            this.ScaleZ.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.ScaleZ.Minimum = new decimal(new int[] {
            10000,
            0,
            0,
            -2147483648});
            this.ScaleZ.Name = "ScaleZ";
            this.ScaleZ.Size = new System.Drawing.Size(60, 20);
            this.ScaleZ.TabIndex = 13;
            this.ScaleZ.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ScaleZ.ValueChanged += new System.EventHandler(this.ScaleZ_ValueChanged);
            // 
            // FloatingHeight
            // 
            this.FloatingHeight.DecimalPlaces = 2;
            this.FloatingHeight.Location = new System.Drawing.Point(53, 85);
            this.FloatingHeight.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.FloatingHeight.Minimum = new decimal(new int[] {
            10000,
            0,
            0,
            -2147483648});
            this.FloatingHeight.Name = "FloatingHeight";
            this.FloatingHeight.Size = new System.Drawing.Size(60, 20);
            this.FloatingHeight.TabIndex = 14;
            this.FloatingHeight.ValueChanged += new System.EventHandler(this.Height_ValueChanged);
            // 
            // PositionY
            // 
            this.PositionY.DecimalPlaces = 2;
            this.PositionY.Enabled = false;
            this.PositionY.Location = new System.Drawing.Point(116, 6);
            this.PositionY.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.PositionY.Minimum = new decimal(new int[] {
            10000,
            0,
            0,
            -2147483648});
            this.PositionY.Name = "PositionY";
            this.PositionY.Size = new System.Drawing.Size(60, 20);
            this.PositionY.TabIndex = 6;
            this.PositionY.ValueChanged += new System.EventHandler(this.PositionY_ValueChanged);
            // 
            // Floating
            // 
            this.Floating.AutoSize = true;
            this.Floating.Checked = true;
            this.Floating.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Floating.Location = new System.Drawing.Point(116, 88);
            this.Floating.Name = "Floating";
            this.Floating.Size = new System.Drawing.Size(63, 17);
            this.Floating.TabIndex = 15;
            this.Floating.Text = "Floating";
            this.Floating.UseVisualStyleBackColor = true;
            this.Floating.CheckedChanged += new System.EventHandler(this.Floating_CheckedChanged);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.PositionX);
            this.panel2.Controls.Add(this.Physical);
            this.panel2.Controls.Add(this.Floating);
            this.panel2.Controls.Add(this.PositionLabel);
            this.panel2.Controls.Add(this.FloatingHeight);
            this.panel2.Controls.Add(this.OrientationLabel);
            this.panel2.Controls.Add(this.ScaleZ);
            this.panel2.Controls.Add(this.ScaleLabel);
            this.panel2.Controls.Add(this.ScaleY);
            this.panel2.Controls.Add(this.HeightLabel);
            this.panel2.Controls.Add(this.ScaleX);
            this.panel2.Controls.Add(this.PositionY);
            this.panel2.Controls.Add(this.Roll);
            this.panel2.Controls.Add(this.PositionZ);
            this.panel2.Controls.Add(this.Pitch);
            this.panel2.Controls.Add(this.Yaw);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(243, 112);
            this.panel2.TabIndex = 20;
            // 
            // AdditionalParametersPanel
            // 
            this.AdditionalParametersPanel.AutoSize = true;
            this.AdditionalParametersPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.AdditionalParametersPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.AdditionalParametersPanel.Location = new System.Drawing.Point(0, 112);
            this.AdditionalParametersPanel.Name = "AdditionalParametersPanel";
            this.AdditionalParametersPanel.Size = new System.Drawing.Size(243, 0);
            this.AdditionalParametersPanel.TabIndex = 21;
            // 
            // ObjectParametersForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.AdditionalParametersPanel);
            this.Controls.Add(this.panel2);
            this.Name = "ObjectParametersForm";
            this.Size = new System.Drawing.Size(243, 111);
            ((System.ComponentModel.ISupportInitialize)(this.PositionX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PositionZ)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Yaw)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Pitch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Roll)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScaleX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScaleY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScaleZ)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FloatingHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PositionY)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label PositionLabel;
        private System.Windows.Forms.Label OrientationLabel;
        private System.Windows.Forms.Label ScaleLabel;
        private System.Windows.Forms.Label HeightLabel;
        public System.Windows.Forms.CheckBox Physical;
        public System.Windows.Forms.NumericUpDown PositionX;
        public System.Windows.Forms.NumericUpDown PositionZ;
        public System.Windows.Forms.NumericUpDown Yaw;
        public System.Windows.Forms.NumericUpDown Pitch;
        public System.Windows.Forms.NumericUpDown Roll;
        public System.Windows.Forms.NumericUpDown ScaleX;
        public System.Windows.Forms.NumericUpDown ScaleY;
        public System.Windows.Forms.NumericUpDown ScaleZ;
        public System.Windows.Forms.NumericUpDown FloatingHeight;
        public System.Windows.Forms.NumericUpDown PositionY;
        public System.Windows.Forms.CheckBox Floating;
        private System.Windows.Forms.Panel panel2;
        public System.Windows.Forms.Panel AdditionalParametersPanel;
    }
}