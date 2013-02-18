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
            this.Height = new System.Windows.Forms.NumericUpDown();
            this.PositionY = new System.Windows.Forms.NumericUpDown();
            this.Floating = new System.Windows.Forms.CheckBox();
            this.Create = new System.Windows.Forms.Button();
            this.Place = new System.Windows.Forms.Button();
            this.CreatePlace = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.PositionX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PositionZ)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Yaw)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Pitch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Roll)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScaleX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScaleY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScaleZ)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Height)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PositionY)).BeginInit();
            this.SuspendLayout();
            // 
            // Physical
            // 
            this.Physical.AutoSize = true;
            this.Physical.Location = new System.Drawing.Point(16, 21);
            this.Physical.Name = "Physical";
            this.Physical.Size = new System.Drawing.Size(65, 17);
            this.Physical.TabIndex = 0;
            this.Physical.Text = "Physical";
            this.Physical.UseVisualStyleBackColor = true;
            // 
            // PositionLabel
            // 
            this.PositionLabel.AutoSize = true;
            this.PositionLabel.Location = new System.Drawing.Point(13, 53);
            this.PositionLabel.Name = "PositionLabel";
            this.PositionLabel.Size = new System.Drawing.Size(47, 13);
            this.PositionLabel.TabIndex = 1;
            this.PositionLabel.Text = "Position:";
            // 
            // OrientationLabel
            // 
            this.OrientationLabel.AutoSize = true;
            this.OrientationLabel.Location = new System.Drawing.Point(13, 79);
            this.OrientationLabel.Name = "OrientationLabel";
            this.OrientationLabel.Size = new System.Drawing.Size(61, 13);
            this.OrientationLabel.TabIndex = 2;
            this.OrientationLabel.Text = "Orientation:";
            // 
            // ScaleLabel
            // 
            this.ScaleLabel.AutoSize = true;
            this.ScaleLabel.Location = new System.Drawing.Point(12, 105);
            this.ScaleLabel.Name = "ScaleLabel";
            this.ScaleLabel.Size = new System.Drawing.Size(37, 13);
            this.ScaleLabel.TabIndex = 3;
            this.ScaleLabel.Text = "Scale:";
            // 
            // HeightLabel
            // 
            this.HeightLabel.AutoSize = true;
            this.HeightLabel.Location = new System.Drawing.Point(12, 132);
            this.HeightLabel.Name = "HeightLabel";
            this.HeightLabel.Size = new System.Drawing.Size(41, 13);
            this.HeightLabel.TabIndex = 4;
            this.HeightLabel.Text = "Height:";
            // 
            // PositionX
            // 
            this.PositionX.DecimalPlaces = 2;
            this.PositionX.Location = new System.Drawing.Point(105, 50);
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
            // 
            // PositionZ
            // 
            this.PositionZ.DecimalPlaces = 2;
            this.PositionZ.Location = new System.Drawing.Point(237, 50);
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
            // 
            // Yaw
            // 
            this.Yaw.DecimalPlaces = 2;
            this.Yaw.Location = new System.Drawing.Point(105, 76);
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
            // 
            // Pitch
            // 
            this.Pitch.DecimalPlaces = 2;
            this.Pitch.Location = new System.Drawing.Point(171, 76);
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
            this.Pitch.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // Roll
            // 
            this.Roll.DecimalPlaces = 2;
            this.Roll.Location = new System.Drawing.Point(237, 76);
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
            // 
            // ScaleX
            // 
            this.ScaleX.DecimalPlaces = 2;
            this.ScaleX.Location = new System.Drawing.Point(105, 102);
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
            // 
            // ScaleY
            // 
            this.ScaleY.DecimalPlaces = 2;
            this.ScaleY.Location = new System.Drawing.Point(171, 103);
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
            // 
            // ScaleZ
            // 
            this.ScaleZ.DecimalPlaces = 2;
            this.ScaleZ.Location = new System.Drawing.Point(237, 102);
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
            // 
            // Height
            // 
            this.Height.DecimalPlaces = 2;
            this.Height.Location = new System.Drawing.Point(105, 129);
            this.Height.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.Height.Minimum = new decimal(new int[] {
            10000,
            0,
            0,
            -2147483648});
            this.Height.Name = "Height";
            this.Height.Size = new System.Drawing.Size(60, 20);
            this.Height.TabIndex = 14;
            // 
            // PositionY
            // 
            this.PositionY.DecimalPlaces = 2;
            this.PositionY.Location = new System.Drawing.Point(171, 50);
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
            // 
            // Floating
            // 
            this.Floating.AutoSize = true;
            this.Floating.Location = new System.Drawing.Point(171, 132);
            this.Floating.Name = "Floating";
            this.Floating.Size = new System.Drawing.Size(63, 17);
            this.Floating.TabIndex = 15;
            this.Floating.Text = "Floating";
            this.Floating.UseVisualStyleBackColor = true;
            // 
            // Create
            // 
            this.Create.Location = new System.Drawing.Point(16, 171);
            this.Create.Name = "Create";
            this.Create.Size = new System.Drawing.Size(75, 23);
            this.Create.TabIndex = 16;
            this.Create.Text = "Create";
            this.Create.UseVisualStyleBackColor = true;
            // 
            // Place
            // 
            this.Place.Location = new System.Drawing.Point(105, 170);
            this.Place.Name = "Place";
            this.Place.Size = new System.Drawing.Size(75, 23);
            this.Place.TabIndex = 17;
            this.Place.Text = "Place";
            this.Place.UseVisualStyleBackColor = true;
            // 
            // CreatePlace
            // 
            this.CreatePlace.Location = new System.Drawing.Point(197, 170);
            this.CreatePlace.Name = "CreatePlace";
            this.CreatePlace.Size = new System.Drawing.Size(111, 23);
            this.CreatePlace.TabIndex = 18;
            this.CreatePlace.Text = "Create and Place";
            this.CreatePlace.UseVisualStyleBackColor = true;
            // 
            // ObjectParametersForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(320, 206);
            this.Controls.Add(this.CreatePlace);
            this.Controls.Add(this.Place);
            this.Controls.Add(this.Create);
            this.Controls.Add(this.Floating);
            this.Controls.Add(this.Height);
            this.Controls.Add(this.ScaleZ);
            this.Controls.Add(this.ScaleY);
            this.Controls.Add(this.ScaleX);
            this.Controls.Add(this.Roll);
            this.Controls.Add(this.Pitch);
            this.Controls.Add(this.Yaw);
            this.Controls.Add(this.PositionZ);
            this.Controls.Add(this.PositionY);
            this.Controls.Add(this.PositionX);
            this.Controls.Add(this.HeightLabel);
            this.Controls.Add(this.ScaleLabel);
            this.Controls.Add(this.OrientationLabel);
            this.Controls.Add(this.PositionLabel);
            this.Controls.Add(this.Physical);
            this.Name = "ObjectParametersForm";
            this.Text = "Object Parameters Menu";
            ((System.ComponentModel.ISupportInitialize)(this.PositionX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PositionZ)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Yaw)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Pitch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Roll)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScaleX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScaleY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScaleZ)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Height)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PositionY)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox Physical;
        private System.Windows.Forms.Label PositionLabel;
        private System.Windows.Forms.Label OrientationLabel;
        private System.Windows.Forms.Label ScaleLabel;
        private System.Windows.Forms.Label HeightLabel;
        private System.Windows.Forms.NumericUpDown PositionX;
        private System.Windows.Forms.NumericUpDown PositionZ;
        private System.Windows.Forms.NumericUpDown Yaw;
        private System.Windows.Forms.NumericUpDown Pitch;
        private System.Windows.Forms.NumericUpDown Roll;
        private System.Windows.Forms.NumericUpDown ScaleX;
        private System.Windows.Forms.NumericUpDown ScaleY;
        private System.Windows.Forms.NumericUpDown ScaleZ;
        private System.Windows.Forms.NumericUpDown Height;
        private System.Windows.Forms.NumericUpDown PositionY;
        private System.Windows.Forms.CheckBox Floating;
        private System.Windows.Forms.Button Create;
        private System.Windows.Forms.Button Place;
        private System.Windows.Forms.Button CreatePlace;
    }
}