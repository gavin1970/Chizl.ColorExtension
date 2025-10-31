using System;
using System.Drawing;

namespace Chizl.ColorExtension
{
    /// <summary>
    /// Contrast Helper to help get level and get best color.
    /// </summary>
    public static class ColorContrastHelper
    {
        /// <summary>
        /// Returns the color (black or white) that has the highest contrast ratio with the given background color,
        /// along with the contrast ratio itself. You may optionally specify a minimum WCAG contrast ratio threshold.
        /// </summary>
        /// <param name="background">Background color</param>
        /// <param name="bestContrastColor">Returns black or white depending on best contrast</param>
        /// <param name="contrastRatio">Returns the actual contrast ratio</param>
        /// <param name="minThreshold">Optional minimum WCAG contrast ratio (default: 0)</param>
        /// <returns>True if contrast meets the minimum threshold, false otherwise</returns>
        public static bool GetContrast(Color background, out Color bestContrastColor, out double contrastRatio, double minThreshold = 0)
        {
            double bgLuminance = RelativeLuminance(background);

            // White and black luminance
            const double whiteLuminance = 1.0;
            const double blackLuminance = 0.0;

            double contrastWithWhite = ContrastRatio(bgLuminance, whiteLuminance);
            double contrastWithBlack = ContrastRatio(bgLuminance, blackLuminance);

            if (contrastWithWhite > contrastWithBlack)
            {
                bestContrastColor = Color.White;
                contrastRatio = contrastWithWhite;
            }
            else
            {
                bestContrastColor = Color.Black;
                contrastRatio = contrastWithBlack;
            }

            return contrastRatio >= minThreshold;
        }

        /// <summary>
        /// Returns the relative luminance of a color (0–1).
        /// </summary>
        /// <param name="c">Foreground color</param>
        /// <param name="background">
        /// (Optional) If foreground color has transparency, pass the solid background color to get the correct luminance<br/>
        /// Default: null
        /// </param>
        /// <returns></returns>
        public static double RelativeLuminance(Color c, Color? background = null)
        {
            if (c.A < 255)
            {
                // Blend against background (default white if none)
                var bg = background ?? Color.White;
                double alpha = c.A / 255.0;
                double r = (c.R * alpha) + (bg.R * (1 - alpha));
                double g = (c.G * alpha) + (bg.G * (1 - alpha));
                double b = (c.B * alpha) + (bg.B * (1 - alpha));
                c = Color.FromArgb(255, (int)r.ClampTo(0, 255), (int)g.ClampTo(0, 255), (int)b.ClampTo(0, 255));
            }

            double R = ChannelLuminance(c.R);
            double G = ChannelLuminance(c.G);
            double B = ChannelLuminance(c.B);
            return 0.2126 * R + 0.7152 * G + 0.0722 * B;
        }

        public static ContrastPassLevel GetContrastLevel(Color background, out Color bestContrastColor, out double contrastRatio)
        {
            GetContrast(background, out bestContrastColor, out contrastRatio);

            if (contrastRatio >= 7.0)
                return ContrastPassLevel.AAALevel;
            if (contrastRatio >= 4.5)
                return ContrastPassLevel.AALevel;
            if (contrastRatio >= 3.0)
                return ContrastPassLevel.LargeTextOnly;
            return ContrastPassLevel.None;
        }

        /// <summary>
        /// Returns the WCAG contrast ratio between two luminances.
        /// </summary>
        private static double ContrastRatio(double lum1, double lum2)
        {
            double L1 = Math.Max(lum1, lum2);
            double L2 = Math.Min(lum1, lum2);
            return (L1 + 0.05) / (L2 + 0.05);
        }

        /// <summary>
        /// Applies gamma correction to a single RGB channel (0–255).
        /// </summary>
        private static double ChannelLuminance(byte value)
        {
            double sRGB = value / 255.0;
            return sRGB <= 0.03928
                ? sRGB / 12.92
                : Math.Pow((sRGB + 0.055) / 1.055, 2.4);
        }
    }
}