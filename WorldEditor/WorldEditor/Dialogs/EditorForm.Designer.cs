using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditorForm));
            this.ObjectList = new System.Windows.Forms.ListBox();
            this.MenuStrip = new System.Windows.Forms.MenuStrip();
            this.File = new System.Windows.Forms.ToolStripMenuItem();
            this.NewMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveAsMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.PlayMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.Edit = new System.Windows.Forms.ToolStripMenuItem();
            this.terrainModeButton = new System.Windows.Forms.PictureBox();
            this.paintModeButton = new System.Windows.Forms.PictureBox();
            this.objectModeButton = new System.Windows.Forms.PictureBox();
            this.background = new System.Windows.Forms.PictureBox();
            this.circleBrushButton = new System.Windows.Forms.PictureBox();
            this.circleFeatherBrushButton = new System.Windows.Forms.PictureBox();
            this.blockBrushButton = new System.Windows.Forms.PictureBox();
            this.blockFeatherBrushButton = new System.Windows.Forms.PictureBox();
            this.sizeUpDown = new System.Windows.Forms.NumericUpDown();
            this.strengthUpDown = new System.Windows.Forms.NumericUpDown();
            this.toolButton0 = new System.Windows.Forms.PictureBox();
            this.toolButton1 = new System.Windows.Forms.PictureBox();
            this.toolButton2 = new System.Windows.Forms.PictureBox();
            this.toolButton4 = new System.Windows.Forms.PictureBox();
            this.toolButton3 = new System.Windows.Forms.PictureBox();
            this.layerBackgroundButton = new System.Windows.Forms.PictureBox();
            this.layer1Button = new System.Windows.Forms.PictureBox();
            this.layer2Button = new System.Windows.Forms.PictureBox();
            this.layer4Button = new System.Windows.Forms.PictureBox();
            this.layer3Button = new System.Windows.Forms.PictureBox();
            this.MenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.terrainModeButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.paintModeButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.objectModeButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.background)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.circleBrushButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.circleFeatherBrushButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.blockBrushButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.blockFeatherBrushButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sizeUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.strengthUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.toolButton0)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.toolButton1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.toolButton2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.toolButton4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.toolButton3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layerBackgroundButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layer1Button)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layer2Button)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layer4Button)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layer3Button)).BeginInit();
            this.SuspendLayout();
            // 
            // ObjectList
            // 
            this.ObjectList.Enabled = false;
            this.ObjectList.FormattingEnabled = true;
            this.ObjectList.ItemHeight = 14;
            this.ObjectList.Location = new System.Drawing.Point(82, 175);
            this.ObjectList.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ObjectList.Name = "ObjectList";
            this.ObjectList.Size = new System.Drawing.Size(154, 312);
            this.ObjectList.TabIndex = 2;
            this.ObjectList.Visible = false;
            // 
            // MenuStrip
            // 
            this.MenuStrip.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.File,
            this.Edit});
            this.MenuStrip.Location = new System.Drawing.Point(0, 0);
            this.MenuStrip.Name = "MenuStrip";
            this.MenuStrip.Padding = new System.Windows.Forms.Padding(6, 3, 0, 3);
            this.MenuStrip.Size = new System.Drawing.Size(320, 28);
            this.MenuStrip.TabIndex = 4;
            this.MenuStrip.Text = "menuStrip1";
            // 
            // File
            // 
            this.File.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.NewMenu,
            this.OpenMenu,
            this.SaveMenu,
            this.SaveAsMenu,
            this.PlayMenu});
            this.File.Name = "File";
            this.File.Size = new System.Drawing.Size(46, 22);
            this.File.Text = "File";
            // 
            // NewMenu
            // 
            this.NewMenu.Name = "NewMenu";
            this.NewMenu.Size = new System.Drawing.Size(168, 22);
            this.NewMenu.Text = "New";
            // 
            // OpenMenu
            // 
            this.OpenMenu.Name = "OpenMenu";
            this.OpenMenu.Size = new System.Drawing.Size(168, 22);
            this.OpenMenu.Text = "Open";
            // 
            // SaveMenu
            // 
            this.SaveMenu.Name = "SaveMenu";
            this.SaveMenu.Size = new System.Drawing.Size(168, 22);
            this.SaveMenu.Text = "Save";
            // 
            // SaveAsMenu
            // 
            this.SaveAsMenu.Name = "SaveAsMenu";
            this.SaveAsMenu.Size = new System.Drawing.Size(168, 22);
            this.SaveAsMenu.Text = "Save As";
            // 
            // PlayMenu
            // 
            this.PlayMenu.Name = "PlayMenu";
            this.PlayMenu.Size = new System.Drawing.Size(168, 22);
            this.PlayMenu.Text = "Play In Game";
            // 
            // Edit
            // 
            this.Edit.Name = "Edit";
            this.Edit.Size = new System.Drawing.Size(48, 22);
            this.Edit.Text = "Edit";
            // 
            // terrainModeButton
            // 
            this.terrainModeButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("terrainModeButton.BackgroundImage")));
            this.terrainModeButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.terrainModeButton.Location = new System.Drawing.Point(15, 54);
            this.terrainModeButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.terrainModeButton.Name = "terrainModeButton";
            this.terrainModeButton.Size = new System.Drawing.Size(96, 103);
            this.terrainModeButton.TabIndex = 5;
            this.terrainModeButton.TabStop = false;
            this.terrainModeButton.Click += new System.EventHandler(this.terrainModeButton_Click);
            // 
            // paintModeButton
            // 
            this.paintModeButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("paintModeButton.BackgroundImage")));
            this.paintModeButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.paintModeButton.Location = new System.Drawing.Point(118, 54);
            this.paintModeButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.paintModeButton.Name = "paintModeButton";
            this.paintModeButton.Size = new System.Drawing.Size(96, 103);
            this.paintModeButton.TabIndex = 6;
            this.paintModeButton.TabStop = false;
            this.paintModeButton.Click += new System.EventHandler(this.paintModeButton_Click);
            // 
            // objectModeButton
            // 
            this.objectModeButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("objectModeButton.BackgroundImage")));
            this.objectModeButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.objectModeButton.Location = new System.Drawing.Point(221, 54);
            this.objectModeButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.objectModeButton.Name = "objectModeButton";
            this.objectModeButton.Size = new System.Drawing.Size(96, 103);
            this.objectModeButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.objectModeButton.TabIndex = 7;
            this.objectModeButton.TabStop = false;
            this.objectModeButton.Click += new System.EventHandler(this.objectModeButton_Click);
            // 
            // background
            // 
            this.background.BackColor = System.Drawing.Color.Transparent;
            this.background.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("background.BackgroundImage")));
            this.background.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.background.Location = new System.Drawing.Point(15, 175);
            this.background.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.background.Name = "background";
            this.background.Size = new System.Drawing.Size(302, 685);
            this.background.TabIndex = 8;
            this.background.TabStop = false;
            // 
            // circleBrushButton
            // 
            this.circleBrushButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(123)))), ((int)(((byte)(123)))));
            this.circleBrushButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("circleBrushButton.BackgroundImage")));
            this.circleBrushButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.circleBrushButton.Location = new System.Drawing.Point(25, 489);
            this.circleBrushButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.circleBrushButton.Name = "circleBrushButton";
            this.circleBrushButton.Size = new System.Drawing.Size(97, 104);
            this.circleBrushButton.TabIndex = 9;
            this.circleBrushButton.TabStop = false;
            this.circleBrushButton.Click += new System.EventHandler(this.circleBrushButton_Click);
            // 
            // circleFeatherBrushButton
            // 
            this.circleFeatherBrushButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(123)))), ((int)(((byte)(123)))));
            this.circleFeatherBrushButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("circleFeatherBrushButton.BackgroundImage")));
            this.circleFeatherBrushButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.circleFeatherBrushButton.Location = new System.Drawing.Point(117, 489);
            this.circleFeatherBrushButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.circleFeatherBrushButton.Name = "circleFeatherBrushButton";
            this.circleFeatherBrushButton.Size = new System.Drawing.Size(97, 104);
            this.circleFeatherBrushButton.TabIndex = 10;
            this.circleFeatherBrushButton.TabStop = false;
            this.circleFeatherBrushButton.Click += new System.EventHandler(this.circleFeatherBrushButton_Click);
            // 
            // blockBrushButton
            // 
            this.blockBrushButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(123)))), ((int)(((byte)(123)))));
            this.blockBrushButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("blockBrushButton.BackgroundImage")));
            this.blockBrushButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.blockBrushButton.Location = new System.Drawing.Point(210, 489);
            this.blockBrushButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.blockBrushButton.Name = "blockBrushButton";
            this.blockBrushButton.Size = new System.Drawing.Size(97, 104);
            this.blockBrushButton.TabIndex = 11;
            this.blockBrushButton.TabStop = false;
            this.blockBrushButton.Click += new System.EventHandler(this.blockBrushButton_Click);
            // 
            // blockFeatherBrushButton
            // 
            this.blockFeatherBrushButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(123)))), ((int)(((byte)(123)))));
            this.blockFeatherBrushButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("blockFeatherBrushButton.BackgroundImage")));
            this.blockFeatherBrushButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.blockFeatherBrushButton.Location = new System.Drawing.Point(25, 590);
            this.blockFeatherBrushButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.blockFeatherBrushButton.Name = "blockFeatherBrushButton";
            this.blockFeatherBrushButton.Size = new System.Drawing.Size(97, 104);
            this.blockFeatherBrushButton.TabIndex = 12;
            this.blockFeatherBrushButton.TabStop = false;
            this.blockFeatherBrushButton.Click += new System.EventHandler(this.blockFeatherBrushButton_Click);
            // 
            // sizeUpDown
            // 
            this.sizeUpDown.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(93)))), ((int)(((byte)(93)))), ((int)(((byte)(93)))));
            this.sizeUpDown.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.sizeUpDown.Font = new System.Drawing.Font("Arial", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sizeUpDown.ForeColor = System.Drawing.Color.White;
            this.sizeUpDown.Location = new System.Drawing.Point(35, 759);
            this.sizeUpDown.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.sizeUpDown.Name = "sizeUpDown";
            this.sizeUpDown.Size = new System.Drawing.Size(115, 48);
            this.sizeUpDown.TabIndex = 13;
            this.sizeUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.sizeUpDown.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // strengthUpDown
            // 
            this.strengthUpDown.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(93)))), ((int)(((byte)(93)))), ((int)(((byte)(93)))));
            this.strengthUpDown.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.strengthUpDown.Font = new System.Drawing.Font("Arial", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.strengthUpDown.ForeColor = System.Drawing.Color.White;
            this.strengthUpDown.Location = new System.Drawing.Point(184, 759);
            this.strengthUpDown.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.strengthUpDown.Name = "strengthUpDown";
            this.strengthUpDown.Size = new System.Drawing.Size(115, 48);
            this.strengthUpDown.TabIndex = 14;
            this.strengthUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.strengthUpDown.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // toolButton0
            // 
            this.toolButton0.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(123)))), ((int)(((byte)(123)))));
            this.toolButton0.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("toolButton0.BackgroundImage")));
            this.toolButton0.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.toolButton0.Location = new System.Drawing.Point(25, 184);
            this.toolButton0.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.toolButton0.Name = "toolButton0";
            this.toolButton0.Size = new System.Drawing.Size(97, 104);
            this.toolButton0.TabIndex = 15;
            this.toolButton0.TabStop = false;
            this.toolButton0.Click += new System.EventHandler(this.toolButton0_Click);
            // 
            // toolButton1
            // 
            this.toolButton1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(123)))), ((int)(((byte)(123)))));
            this.toolButton1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("toolButton1.BackgroundImage")));
            this.toolButton1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.toolButton1.Location = new System.Drawing.Point(117, 184);
            this.toolButton1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.toolButton1.Name = "toolButton1";
            this.toolButton1.Size = new System.Drawing.Size(97, 104);
            this.toolButton1.TabIndex = 16;
            this.toolButton1.TabStop = false;
            this.toolButton1.Click += new System.EventHandler(this.toolButton1_Click);
            // 
            // toolButton2
            // 
            this.toolButton2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(123)))), ((int)(((byte)(123)))));
            this.toolButton2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("toolButton2.BackgroundImage")));
            this.toolButton2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.toolButton2.Location = new System.Drawing.Point(210, 184);
            this.toolButton2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.toolButton2.Name = "toolButton2";
            this.toolButton2.Size = new System.Drawing.Size(97, 104);
            this.toolButton2.TabIndex = 17;
            this.toolButton2.TabStop = false;
            this.toolButton2.Click += new System.EventHandler(this.toolButton2_Click);
            // 
            // toolButton4
            // 
            this.toolButton4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(123)))), ((int)(((byte)(123)))));
            this.toolButton4.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("toolButton4.BackgroundImage")));
            this.toolButton4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.toolButton4.Location = new System.Drawing.Point(117, 282);
            this.toolButton4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.toolButton4.Name = "toolButton4";
            this.toolButton4.Size = new System.Drawing.Size(97, 104);
            this.toolButton4.TabIndex = 19;
            this.toolButton4.TabStop = false;
            this.toolButton4.Click += new System.EventHandler(this.toolButton4_Click);
            // 
            // toolButton3
            // 
            this.toolButton3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(123)))), ((int)(((byte)(123)))));
            this.toolButton3.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("toolButton3.BackgroundImage")));
            this.toolButton3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.toolButton3.Location = new System.Drawing.Point(25, 282);
            this.toolButton3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.toolButton3.Name = "toolButton3";
            this.toolButton3.Size = new System.Drawing.Size(97, 104);
            this.toolButton3.TabIndex = 18;
            this.toolButton3.TabStop = false;
            this.toolButton3.Click += new System.EventHandler(this.toolButton3_Click);
            // 
            // layerBackgroundButton
            // 
            this.layerBackgroundButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(123)))), ((int)(((byte)(123)))));
            this.layerBackgroundButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("layerBackgroundButton.BackgroundImage")));
            this.layerBackgroundButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.layerBackgroundButton.Location = new System.Drawing.Point(23, 338);
            this.layerBackgroundButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.layerBackgroundButton.Name = "layerBackgroundButton";
            this.layerBackgroundButton.Size = new System.Drawing.Size(70, 87);
            this.layerBackgroundButton.TabIndex = 20;
            this.layerBackgroundButton.TabStop = false;
            this.layerBackgroundButton.Visible = false;
            this.layerBackgroundButton.Click += new System.EventHandler(this.layerBackgroundButton_Click);
            // 
            // layer1Button
            // 
            this.layer1Button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(123)))), ((int)(((byte)(123)))));
            this.layer1Button.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("layer1Button.BackgroundImage")));
            this.layer1Button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.layer1Button.Location = new System.Drawing.Point(74, 338);
            this.layer1Button.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.layer1Button.Name = "layer1Button";
            this.layer1Button.Size = new System.Drawing.Size(70, 87);
            this.layer1Button.TabIndex = 21;
            this.layer1Button.TabStop = false;
            this.layer1Button.Visible = false;
            this.layer1Button.Click += new System.EventHandler(this.layer1Button_Click);
            // 
            // layer2Button
            // 
            this.layer2Button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(123)))), ((int)(((byte)(123)))));
            this.layer2Button.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("layer2Button.BackgroundImage")));
            this.layer2Button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.layer2Button.Location = new System.Drawing.Point(130, 338);
            this.layer2Button.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.layer2Button.Name = "layer2Button";
            this.layer2Button.Size = new System.Drawing.Size(70, 87);
            this.layer2Button.TabIndex = 22;
            this.layer2Button.TabStop = false;
            this.layer2Button.Visible = false;
            this.layer2Button.Click += new System.EventHandler(this.layer2Button_Click);
            // 
            // layer4Button
            // 
            this.layer4Button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(123)))), ((int)(((byte)(123)))));
            this.layer4Button.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("layer4Button.BackgroundImage")));
            this.layer4Button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.layer4Button.Location = new System.Drawing.Point(242, 338);
            this.layer4Button.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.layer4Button.Name = "layer4Button";
            this.layer4Button.Size = new System.Drawing.Size(70, 87);
            this.layer4Button.TabIndex = 24;
            this.layer4Button.TabStop = false;
            this.layer4Button.Visible = false;
            this.layer4Button.Click += new System.EventHandler(this.layer4Button_Click);
            // 
            // layer3Button
            // 
            this.layer3Button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(123)))), ((int)(((byte)(123)))));
            this.layer3Button.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("layer3Button.BackgroundImage")));
            this.layer3Button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.layer3Button.Location = new System.Drawing.Point(186, 338);
            this.layer3Button.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.layer3Button.Name = "layer3Button";
            this.layer3Button.Size = new System.Drawing.Size(70, 87);
            this.layer3Button.TabIndex = 23;
            this.layer3Button.TabStop = false;
            this.layer3Button.Visible = false;
            this.layer3Button.Click += new System.EventHandler(this.layer3Button_Click);
            // 
            // EditorForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(97)))), ((int)(((byte)(97)))), ((int)(((byte)(97)))));
            this.Controls.Add(this.ObjectList);
            this.Controls.Add(this.layer4Button);
            this.Controls.Add(this.layer3Button);
            this.Controls.Add(this.layer2Button);
            this.Controls.Add(this.layer1Button);
            this.Controls.Add(this.layerBackgroundButton);
            this.Controls.Add(this.toolButton4);
            this.Controls.Add(this.toolButton3);
            this.Controls.Add(this.toolButton2);
            this.Controls.Add(this.toolButton1);
            this.Controls.Add(this.toolButton0);
            this.Controls.Add(this.strengthUpDown);
            this.Controls.Add(this.sizeUpDown);
            this.Controls.Add(this.blockFeatherBrushButton);
            this.Controls.Add(this.blockBrushButton);
            this.Controls.Add(this.circleFeatherBrushButton);
            this.Controls.Add(this.circleBrushButton);
            this.Controls.Add(this.background);
            this.Controls.Add(this.objectModeButton);
            this.Controls.Add(this.paintModeButton);
            this.Controls.Add(this.terrainModeButton);
            this.Controls.Add(this.MenuStrip);
            this.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "EditorForm";
            this.MenuStrip.ResumeLayout(false);
            this.MenuStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.terrainModeButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.paintModeButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.objectModeButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.background)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.circleBrushButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.circleFeatherBrushButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.blockBrushButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.blockFeatherBrushButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sizeUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.strengthUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.toolButton0)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.toolButton1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.toolButton2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.toolButton4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.toolButton3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layerBackgroundButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layer1Button)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layer2Button)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layer4Button)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layer3Button)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip MenuStrip;
        private System.Windows.Forms.ToolStripMenuItem File;
        private System.Windows.Forms.ToolStripMenuItem SaveMenu;
        private System.Windows.Forms.ToolStripMenuItem OpenMenu;
        private System.Windows.Forms.ToolStripMenuItem Edit;
        public System.Windows.Forms.ListBox ObjectList;
        private PictureBox terrainModeButton;
        private PictureBox paintModeButton;
        private PictureBox objectModeButton;
        private PictureBox background;
        private PictureBox circleBrushButton;
        private PictureBox circleFeatherBrushButton;
        private PictureBox blockBrushButton;
        private PictureBox blockFeatherBrushButton;
        private NumericUpDown sizeUpDown;
        private NumericUpDown strengthUpDown;
        private PictureBox toolButton0;
        private PictureBox toolButton1;
        private PictureBox toolButton2;
        private PictureBox toolButton4;
        private PictureBox toolButton3;
        private PictureBox layerBackgroundButton;
        private PictureBox layer1Button;
        private PictureBox layer2Button;
        private PictureBox layer4Button;
        private PictureBox layer3Button;
        private ToolStripMenuItem NewMenu;
        private ToolStripMenuItem SaveAsMenu;
        private ToolStripMenuItem PlayMenu;

        public NumericUpDown SizeUpDown
        {
            get { return sizeUpDown; }
        }

        public NumericUpDown StrengthUpDown
        {
            get { return strengthUpDown; }
        }

        public PictureBox PaintModeButton
        {
            get { return paintModeButton; }
        }

        public PictureBox ObjectModeButton
        {
            get { return objectModeButton; }
        }

        public PictureBox HeightmapModeButton
        {
            get { return terrainModeButton; }
        }
    }
}