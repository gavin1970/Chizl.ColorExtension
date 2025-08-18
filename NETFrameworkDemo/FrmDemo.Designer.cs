namespace NETFrameworkDemo
{
    partial class FrmDemo
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
            this.FGColorPanel = new System.Windows.Forms.GroupBox();
            this.FGLabel = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.BtnBGColor = new System.Windows.Forms.Button();
            this.BtnFGColor = new System.Windows.Forms.Button();
            this.paintColorPanel = new System.Windows.Forms.GroupBox();
            this.ColorOpaqueHexLabel = new System.Windows.Forms.Label();
            this.ShouldBeSameLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.BGColorPanel = new System.Windows.Forms.GroupBox();
            this.BGLabel = new System.Windows.Forms.Label();
            this.objectColorPanel = new System.Windows.Forms.GroupBox();
            this.OverlayHexLabel = new System.Windows.Forms.Label();
            this.OverlayLabel = new System.Windows.Forms.Label();
            this.FGColorPanel.SuspendLayout();
            this.panel1.SuspendLayout();
            this.paintColorPanel.SuspendLayout();
            this.BGColorPanel.SuspendLayout();
            this.objectColorPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // FGColorPanel
            // 
            this.FGColorPanel.BackColor = System.Drawing.Color.Red;
            this.FGColorPanel.Controls.Add(this.FGLabel);
            this.FGColorPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.FGColorPanel.Location = new System.Drawing.Point(0, 25);
            this.FGColorPanel.Name = "FGColorPanel";
            this.FGColorPanel.Size = new System.Drawing.Size(200, 739);
            this.FGColorPanel.TabIndex = 0;
            this.FGColorPanel.TabStop = false;
            // 
            // FGLabel
            // 
            this.FGLabel.BackColor = System.Drawing.Color.White;
            this.FGLabel.Location = new System.Drawing.Point(0, 0);
            this.FGLabel.Name = "FGLabel";
            this.FGLabel.Size = new System.Drawing.Size(200, 23);
            this.FGLabel.TabIndex = 2;
            this.FGLabel.Text = "Foreground Color";
            this.FGLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.panel1.Controls.Add(this.BtnBGColor);
            this.panel1.Controls.Add(this.BtnFGColor);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1264, 25);
            this.panel1.TabIndex = 1;
            // 
            // BtnBGColor
            // 
            this.BtnBGColor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.BtnBGColor.Dock = System.Windows.Forms.DockStyle.Right;
            this.BtnBGColor.Location = new System.Drawing.Point(1064, 0);
            this.BtnBGColor.Name = "BtnBGColor";
            this.BtnBGColor.Size = new System.Drawing.Size(200, 25);
            this.BtnBGColor.TabIndex = 1;
            this.BtnBGColor.Text = "Select &Background Color";
            this.BtnBGColor.UseVisualStyleBackColor = false;
            this.BtnBGColor.Click += new System.EventHandler(this.BtnBGColor_Click);
            // 
            // BtnFGColor
            // 
            this.BtnFGColor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.BtnFGColor.Dock = System.Windows.Forms.DockStyle.Left;
            this.BtnFGColor.Location = new System.Drawing.Point(0, 0);
            this.BtnFGColor.Name = "BtnFGColor";
            this.BtnFGColor.Size = new System.Drawing.Size(200, 25);
            this.BtnFGColor.TabIndex = 0;
            this.BtnFGColor.Text = "Select &Foreground Color";
            this.BtnFGColor.UseVisualStyleBackColor = false;
            this.BtnFGColor.Click += new System.EventHandler(this.BtnFGColor_Click);
            // 
            // paintColorPanel
            // 
            this.paintColorPanel.BackColor = System.Drawing.Color.Blue;
            this.paintColorPanel.Controls.Add(this.ColorOpaqueHexLabel);
            this.paintColorPanel.Controls.Add(this.ShouldBeSameLabel);
            this.paintColorPanel.Controls.Add(this.label1);
            this.paintColorPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.paintColorPanel.Location = new System.Drawing.Point(200, 279);
            this.paintColorPanel.Name = "paintColorPanel";
            this.paintColorPanel.Size = new System.Drawing.Size(864, 485);
            this.paintColorPanel.TabIndex = 2;
            this.paintColorPanel.TabStop = false;
            this.paintColorPanel.Text = "[ Overlayed Colors ]";
            this.paintColorPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.OverlayColorsPanel_Paint);
            // 
            // ColorOpaqueHexLabel
            // 
            this.ColorOpaqueHexLabel.AutoSize = true;
            this.ColorOpaqueHexLabel.BackColor = System.Drawing.Color.White;
            this.ColorOpaqueHexLabel.Location = new System.Drawing.Point(271, 25);
            this.ColorOpaqueHexLabel.Name = "ColorOpaqueHexLabel";
            this.ColorOpaqueHexLabel.Padding = new System.Windows.Forms.Padding(2);
            this.ColorOpaqueHexLabel.Size = new System.Drawing.Size(126, 17);
            this.ColorOpaqueHexLabel.TabIndex = 2;
            this.ColorOpaqueHexLabel.Text = "Should Match Top Color";
            this.ColorOpaqueHexLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ShouldBeSameLabel
            // 
            this.ShouldBeSameLabel.AutoSize = true;
            this.ShouldBeSameLabel.BackColor = System.Drawing.Color.White;
            this.ShouldBeSameLabel.Location = new System.Drawing.Point(3, 25);
            this.ShouldBeSameLabel.Name = "ShouldBeSameLabel";
            this.ShouldBeSameLabel.Padding = new System.Windows.Forms.Padding(2);
            this.ShouldBeSameLabel.Size = new System.Drawing.Size(126, 17);
            this.ShouldBeSameLabel.TabIndex = 1;
            this.ShouldBeSameLabel.Text = "Should Match Top Color";
            this.ShouldBeSameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(3);
            this.label1.Size = new System.Drawing.Size(338, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "The Overlay: top stays white,  bottom is what you set as background.";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BGColorPanel
            // 
            this.BGColorPanel.BackColor = System.Drawing.Color.Blue;
            this.BGColorPanel.Controls.Add(this.BGLabel);
            this.BGColorPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.BGColorPanel.Location = new System.Drawing.Point(1064, 25);
            this.BGColorPanel.Name = "BGColorPanel";
            this.BGColorPanel.Size = new System.Drawing.Size(200, 739);
            this.BGColorPanel.TabIndex = 3;
            this.BGColorPanel.TabStop = false;
            // 
            // BGLabel
            // 
            this.BGLabel.BackColor = System.Drawing.Color.White;
            this.BGLabel.Location = new System.Drawing.Point(0, 0);
            this.BGLabel.Name = "BGLabel";
            this.BGLabel.Size = new System.Drawing.Size(200, 23);
            this.BGLabel.TabIndex = 3;
            this.BGLabel.Text = "Background Color";
            this.BGLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // objectColorPanel
            // 
            this.objectColorPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(0)))), ((int)(((byte)(128)))));
            this.objectColorPanel.Controls.Add(this.OverlayHexLabel);
            this.objectColorPanel.Controls.Add(this.OverlayLabel);
            this.objectColorPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.objectColorPanel.Location = new System.Drawing.Point(200, 25);
            this.objectColorPanel.Name = "objectColorPanel";
            this.objectColorPanel.Padding = new System.Windows.Forms.Padding(0, 0, 0, 125);
            this.objectColorPanel.Size = new System.Drawing.Size(864, 254);
            this.objectColorPanel.TabIndex = 4;
            this.objectColorPanel.TabStop = false;
            // 
            // OverlayHexLabel
            // 
            this.OverlayHexLabel.BackColor = System.Drawing.Color.White;
            this.OverlayHexLabel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.OverlayHexLabel.Location = new System.Drawing.Point(0, 106);
            this.OverlayHexLabel.Name = "OverlayHexLabel";
            this.OverlayHexLabel.Size = new System.Drawing.Size(864, 23);
            this.OverlayHexLabel.TabIndex = 3;
            this.OverlayHexLabel.Text = "New Color from Chizl.ColorExtensions";
            this.OverlayHexLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // OverlayLabel
            // 
            this.OverlayLabel.BackColor = System.Drawing.Color.White;
            this.OverlayLabel.Location = new System.Drawing.Point(0, 0);
            this.OverlayLabel.Name = "OverlayLabel";
            this.OverlayLabel.Size = new System.Drawing.Size(400, 23);
            this.OverlayLabel.TabIndex = 1;
            this.OverlayLabel.Text = "This is the color created by Chizl.ColorExtensions for 50% opaque foreground";
            this.OverlayLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FrmDemo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 764);
            this.Controls.Add(this.paintColorPanel);
            this.Controls.Add(this.objectColorPanel);
            this.Controls.Add(this.BGColorPanel);
            this.Controls.Add(this.FGColorPanel);
            this.Controls.Add(this.panel1);
            this.Name = "FrmDemo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FrmDemo";
            this.Load += new System.EventHandler(this.FrmDemo_Load);
            this.FGColorPanel.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.paintColorPanel.ResumeLayout(false);
            this.paintColorPanel.PerformLayout();
            this.BGColorPanel.ResumeLayout(false);
            this.objectColorPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox FGColorPanel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox paintColorPanel;
        private System.Windows.Forms.GroupBox BGColorPanel;
        private System.Windows.Forms.Button BtnBGColor;
        private System.Windows.Forms.Button BtnFGColor;
        private System.Windows.Forms.GroupBox objectColorPanel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label OverlayLabel;
        private System.Windows.Forms.Label FGLabel;
        private System.Windows.Forms.Label BGLabel;
        private System.Windows.Forms.Label OverlayHexLabel;
        private System.Windows.Forms.Label ShouldBeSameLabel;
        private System.Windows.Forms.Label ColorOpaqueHexLabel;
    }
}

