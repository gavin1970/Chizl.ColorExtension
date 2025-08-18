using System;
using System.Drawing;
using System.Windows.Forms;
using Chizl.ColorExtension;
using NETFrameworkDemo.utils;

namespace NETFrameworkDemo
{
    public partial class FrmDemo : Form
    {
        private static Color opaqueFGColor = Color.Empty;
        public FrmDemo()
        {
            InitializeComponent();
        }

        #region Event Methods
        private void FrmDemo_Load(object sender, EventArgs e) => UpdateColors();
        private void OverlayColorsPanel_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;

            // 50% opaque of the color selected on the left, "FG Color".
            //var opaqueFGColor = Color.FromArgb(127, FGColorPanel.BackColor);
            // For Ellipse with Alpha "127".
            var ellipseBrush = new SolidBrush(opaqueFGColor);
            // For white top half.
            var whiteBGBrush = new SolidBrush(Color.White);
            // Get Height
            var hieght = paintColorPanel.Height;
            // Get Width
            var width = paintColorPanel.Width;
            // Get vertical center
            var topHalf = hieght / 2;

            // Paint top half, white
            g.FillRectangle(whiteBGBrush, 0, 0, width, topHalf);
            // Paint ellipse on top with opaque FG Color.
            g.FillEllipse(ellipseBrush, 0, 0, width, hieght);
            // Update labels and sizing if needed.
            UpdateComponents();
        }
        private void BtnFGColor_Click(object sender, EventArgs e)
        {
            FGColorPanel.BackColor = Common.GetColor(this, FGColorPanel.BackColor);
            UpdateColors();
        }
        private void BtnBGColor_Click(object sender, EventArgs e)
        {
            BGColorPanel.BackColor = Common.GetColor(this, BGColorPanel.BackColor);
            UpdateColors();
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Update color info then initiate paint event.
        /// </summary>
        private void UpdateColors()
        {
            var fgColor = FGColorPanel.BackColor;
            var bgColor = BGColorPanel.BackColor;

            //create opaque color from selected color.
            opaqueFGColor = Color.FromArgb(127, fgColor.R, fgColor.G, fgColor.B);
            //set panels
            objectColorPanel.BackColor = opaqueFGColor.ApplyBgColor(bgColor);
            paintColorPanel.BackColor = bgColor;

            //kick off paint event for the two panels
            objectColorPanel.Invalidate();
            paintColorPanel.Invalidate();
        }
        /// <summary>
        /// Update sizing and color info text.
        /// </summary>
        private void UpdateComponents()
        {
            OverlayHexLabel.Text = $"New Color from Chizl.ColorExtensions: {objectColorPanel.BackColor.ToHexArgb()}";
            ShouldBeSameLabel.Text = $"Opaque: {opaqueFGColor.ToHexArgb()} on top of {paintColorPanel.BackColor.ToHexRgb()}";
            ColorOpaqueHexLabel.Text = $"Opaque: {opaqueFGColor.ToHexArgb()}, on top of White";
            FGLabel.Text = $"Foreground Color: {FGColorPanel.BackColor.ToHexRgb()}";
            BGLabel.Text = $"Background Color: {paintColorPanel.BackColor.ToHexRgb()}";

            OverlayLabel.Width = objectColorPanel.Width;
            objectColorPanel.Height = (this.Height / 2) - 40;
            objectColorPanel.Padding = new Padding(0, 0, 0, (objectColorPanel.Height / 2) - OverlayHexLabel.Height);
            ShouldBeSameLabel.Left = (paintColorPanel.Width / 2) - (ShouldBeSameLabel.Width / 2);
            ShouldBeSameLabel.Top = (paintColorPanel.Height - (paintColorPanel.Height / 4)) - (ShouldBeSameLabel.Height / 2);

            ColorOpaqueHexLabel.Left = (paintColorPanel.Width / 2) - (ColorOpaqueHexLabel.Width / 2);
            ColorOpaqueHexLabel.Top = (paintColorPanel.Height / 4) - (ColorOpaqueHexLabel.Height / 2);
        }
        #endregion
    }
}
