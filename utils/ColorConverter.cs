using System;
using System.Drawing;

namespace Chizl.ColorExtension
{
    public static class ColorConverterEx
    {
        #region CMYK Conversions
        /// <summary>
        /// Converts CMYK to RGB decimal value. (w/o alpha)<br/>
        /// NOTE: Converting is not always a direct correlation.<br/>
        /// CONVERSIONS: There is not always a direct 1 to 1 correlation.<br/>
        /// - When converting doubles to create RGB byte/int values means there is rounding.<br/>
        ///   This provides you the correct values, but the conversion back to doubles might not be what you passed in.
        /// </summary>
        /// <param name="c">Cyan component (0.0 - 1.0)</param>
        /// <param name="m">Magenta component (0.0 - 1.0)</param>
        /// <param name="y">Yellow component (0.0 - 1.0)</param>
        /// <param name="k">Black (Key) component (0.0 - 1.0)</param>
        /// <returns>(int) RGB "Color" decimal value with alpha.</returns>
        public static int CmykToArgb(double c, double m, double y, double k) => CmykToColor(c, m, y, k).ToArgb();
        /// <summary>
        /// Converts CMYK to RGB decimal value. (w/o alpha)<br/>
        /// NOTE: Converting is not always a direct correlation.<br/>
        /// CONVERSIONS: There is not always a direct 1 to 1 correlation.<br/>
        /// - When converting doubles to create RGB byte/int values means there is rounding.<br/>
        ///   This provides you the correct values, but the conversion back to doubles might not be what you passed in.
        /// </summary>
        /// <param name="c">Cyan component (0.0 - 1.0)</param>
        /// <param name="m">Magenta component (0.0 - 1.0)</param>
        /// <param name="y">Yellow component (0.0 - 1.0)</param>
        /// <param name="k">Black (Key) component (0.0 - 1.0)</param>
        /// <returns>(int) RGB "Color" decimal value w/o alpha.</returns>
        public static int CmykToRgb(double c, double m, double y, double k) => CmykToColor(c, m, y, k).ToRgb();
        /// <summary>
        /// Converts CMYK color values to RGB color.<br/>
        /// CMYK values are expected to be in the range [0.0, 1.0].<br/>
        /// CONVERSIONS: There is not always a direct 1 to 1 correlation, however by using CymkSpace,<br/>
        ///              it use better precision decimal points by using RawKey property instead of Key.<br/>
        /// - When converting doubles to create RGB byte/int values means there is rounding.<br/>
        ///   This provides you the correct values, but the conversion back to doubles might not be what you passed in.
        /// </summary>
        /// <param name="cmyk">CmykSpace struct</param>
        /// <returns>Color object</returns>
        public static Color CmykToColor(CmykSpace cmyk) => CmykToColor(cmyk.Cyan, cmyk.Magenta, cmyk.Yellow, cmyk.RawKey);
        /// <summary>
        /// Converts CMYK color values to RGB color.<br/>
        /// CMYK values are expected to be in the range [0.0, 1.0].<br/>
        /// CONVERSIONS: There is not always a direct 1 to 1 correlation.<br/>
        /// - When converting doubles to create RGB byte/int values means there is rounding.<br/>
        ///   This provides you the correct values, but the conversion back to doubles might not be what you passed in.
        /// </summary>
        /// <param name="c">Cyan component (0.0 - 1.0)</param>
        /// <param name="m">Magenta component (0.0 - 1.0)</param>
        /// <param name="y">Yellow component (0.0 - 1.0)</param>
        /// <param name="k">Black (Key) component (0.0 - 1.0)</param>
        /// <returns>Color object</returns>
        public static Color CmykToColor(double c, double m, double y, double k)
        {
            // Validate and normalize CMYK values to the 0-1 range
            if (c < 0 || c > 100 || m < 0 || m > 100 || y < 0 || y > 100 || k < 0 || k > 100)
                throw new ArgumentException("CMYK values must be between 0 and 100.");

            // Ensure if they passed whole percentages, make them decimal percentages.
            if (c > 1.0) c /= 100;
            if (y > 1.0) y /= 100;
            if (m > 1.0) m /= 100;
            if (k > 1.0) k /= 100;

            // Ensure CMYK values are within the valid range
            c = Math.Max(0.0, Math.Min(1.0, c));
            m = Math.Max(0.0, Math.Min(1.0, m));
            y = Math.Max(0.0, Math.Min(1.0, y));
            k = Math.Max(0.0, Math.Min(1.0, k));

            // Convert CMYK to RGB
            int r = (int)Math.Round(255 * (1 - c) * (1 - k));
            int g = (int)Math.Round(255 * (1 - m) * (1 - k));
            int b = (int)Math.Round(255 * (1 - y) * (1 - k));

            // Ensure RGB values are within the valid range [0, 255]
            r = Math.Max(0, Math.Min(255, r));
            g = Math.Max(0, Math.Min(255, g));
            b = Math.Max(0, Math.Min(255, b));

            return Color.FromArgb(r, g, b);
        }
        #endregion

        #region HSV Conversions
        /// <summary>
        /// Converts HSV to ARGB decimal value. (w/ alpha)<br/>
        /// NOTE: There are only 3,600,000 unique HSV combinations.  In 24bit color there are 16,777,216 RGB combinations.<br/>
        /// - This means that each HSV combinations will match with four differnt RGB values.<br/>
        /// CONVERSIONS: There is not always a direct 1 to 1 correlation.<br/>
        /// - When converting doubles to create RGB byte/int values means there is rounding.<br/>
        ///   This provides you the correct values, but the conversion back to doubles might not be what you passed in.
        /// </summary>
        /// <param name="h">Hue Decimal Value</param>
        /// <param name="s">Saturation Decimal Value</param>
        /// <param name="v">Value Decimal Value</param>
        /// <returns>(int) One of four possible ARGB "Color" decimal value with alpha.</returns>
        public static int HsvToArgb(double h, double s, double v) => HsvToColor(h, s, v).ToArgb();
        /// <summary>
        /// Converts HSV to RGB decimal value. (w/o alpha)<br/>
        /// NOTE: There are only 3,600,000 unique HSV combinations.  In 24bit color there are 16,777,216 RGB combinations.<br/>
        /// - This means that each HSV combinations will match with four differnt RGB values.<br/>
        /// CONVERSIONS: There is not always a direct 1 to 1 correlation.<br/>
        /// - When converting doubles to create RGB byte/int values means there is rounding.<br/>
        ///   This provides you the correct values, but the conversion back to doubles might not be what you passed in.
        /// </summary>
        /// <param name="h">Hue Decimal Value</param>
        /// <param name="s">Saturation Decimal Value</param>
        /// <param name="v">Value Decimal Value</param>
        /// <returns>(int) One of four possible ARGB "Color" decimal value w/o alpha.</returns>
        public static int HsvToRgb(double h, double s, double v) => HsvToColor(h, s, v).ToRgb();
        /// <summary>
        /// Converts HSV (Hue, Saturation, Value) color to RGB (Red, Green, Blue).<br/>
        /// NOTE: There are only 3,600,000 unique HSV combinations.  There are 16,777,216 RGB combinations.<br/>
        /// - This means that each HSV combinations will match with 4 differnt RGB values.<br/>
        /// CONVERSIONS: There is not always a direct 1 to 1 correlation.<br/>
        /// - When converting doubles to create RGB byte/int values means there is rounding.<br/>
        ///   This provides you the correct values, but the conversion back to doubles might not be what you passed in.
        /// </summary>
        /// <param name="hsv">HsvSpace struct</param>
        /// <returns>One of four possible color object</returns>
        public static Color HsvToColor(HsvSpace hsv) => HsvToColor(hsv.Hue, hsv.Saturation, hsv.RawValue);
        /// <summary>
        /// Converts HSV (Hue, Saturation, Value) color to RGB (Red, Green, Blue).<br/>
        /// NOTE: There are only 3,600,000 unique HSV combinations.  There are 16,777,216 RGB combinations.<br/>
        /// - This means that each HSV combinations will match with 4 differnt RGB values.<br/>
        /// CONVERSIONS: There is not always a direct 1 to 1 correlation.<br/>
        /// - When converting doubles to create RGB byte/int values means there is rounding.<br/>
        ///   This provides you the correct values, but the conversion back to doubles might not be what you passed in.
        /// </summary>
        /// <param name="h">Hue component (0-360 degrees).</param>
        /// <param name="s">Saturation component (0-1, or 0% to 100%).</param>
        /// <param name="v">Value component (0-1, or 0% to 100%).</param>
        /// <returns>One of four possible color object</returns>
        public static Color HsvToColor(double h, double s, double v)
        {
            // Ensure if they passed whole percentages, make them decimal percentages.
            if (s > 1.0) s /= 100;
            if (v > 1.0) v /= 100;

            // Ensure H, S, V are within valid ranges
            h = h % 360; // Wrap hue around if it exceeds 360
            while (h < 0) h += 360; // Handle negative hue values

            s = Math.Max(0, Math.Min(1, s)); // Clamp saturation to 0-1
            v = Math.Max(0, Math.Min(1, v)); // Clamp value to 0-1

            double r = 0, g = 0, b = 0;
            double c = v * s; // Chroma - Moved this declaration outside the if/else for scope

            // If saturation is 0, the color is a shade of gray
            if (s == 0)
            {
                r = v;
                g = v;
                b = v;
            }
            else
            {
                double hPrime = h / 60.0; // H'
                double x = c * (1 - Math.Abs((hPrime % 2) - 1)); // X

                // Determine the base RGB values based on H' segment
                if (hPrime >= 0 && hPrime < 1)
                {
                    r = c;
                    g = x;
                    b = 0;
                }
                else if (hPrime >= 1 && hPrime < 2)
                {
                    r = x;
                    g = c;
                    b = 0;
                }
                else if (hPrime >= 2 && hPrime < 3)
                {
                    r = 0;
                    g = c;
                    b = x;
                }
                else if (hPrime >= 3 && hPrime < 4)
                {
                    r = 0;
                    g = x;
                    b = c;
                }
                else if (hPrime >= 4 && hPrime < 5)
                {
                    r = x;
                    g = 0;
                    b = c;
                }
                else if (hPrime >= 5 && hPrime < 6)
                {
                    r = c;
                    g = 0;
                    b = x;
                }
            }

            // Lightness adjustment
            double m = v - c;

            // Final RGB values, scaled to 0-255 and clamped
            byte red = (byte)Math.Round(Math.Max(0, Math.Min(255, (r + m) * 255)));
            byte blue = (byte)Math.Round(Math.Max(0, Math.Min(255, (b + m) * 255)));
            byte green = (byte)Math.Round(Math.Max(0, Math.Min(255, (g + m) * 255)));

            return Color.FromArgb(red, blue, green);
        }
        #endregion

        #region HSL Conversions
        /// <summary>
        /// Converts HSL color values to RGB color values.<br/>
        /// HSL values are expected as follows:<br/>
        /// Hue (h): 0.0 - 360.0 degrees<br/>
        /// Saturation (s): 0.0 - 1.0 (0% - 100%)<br/>
        /// Lightness (l): 0.0 - 1.0 (0% - 100%)<br/>
        /// RGB values will be returned in the range [0, 255].<br/>
        /// CONVERSIONS: There is not always a direct 1 to 1 correlation.<br/>
        /// - When converting doubles to create RGB byte/int values means there is rounding.<br/>
        ///   This provides you the correct values, but the conversion back to doubles might not be what you passed in.
        /// </summary>
        /// <param name="h">Hue component (0.0 - 360.0 degrees).</param>
        /// <param name="s">Saturation component (0.0 - 1.0).</param>
        /// <param name="l">Lightness component (0.0 - 1.0).</param>
        /// <returns>(int) One of four possible ARGB "Color" decimal value with alpha.</returns>
        public static int HslToArgb(double h, double s, double l) => HslToColor(h, s, l).ToArgb();
        /// <summary>
        /// Converts HSL color values to RGB color values.<br/>
        /// HSL values are expected as follows:<br/>
        /// Hue (h): 0.0 - 360.0 degrees<br/>
        /// Saturation (s): 0.0 - 1.0 (0% - 100%)<br/>
        /// Lightness (l): 0.0 - 1.0 (0% - 100%)<br/>
        /// RGB values will be returned in the range [0, 255].<br/>
        /// CONVERSIONS: There is not always a direct 1 to 1 correlation.<br/>
        /// - When converting doubles to create RGB byte/int values means there is rounding.<br/>
        ///   This provides you the correct values, but the conversion back to doubles might not be what you passed in.
        /// </summary>
        /// <param name="h">Hue component (0.0 - 360.0 degrees).</param>
        /// <param name="s">Saturation component (0.0 - 1.0).</param>
        /// <param name="l">Lightness component (0.0 - 1.0).</param>
        /// <returns>(int) One of four possible ARGB "Color" decimal value w/o alpha.</returns>
        public static int HslToRgb(double h, double s, double l) => HslToColor(h, s, l).ToRgb();
        /// <summary>
        /// Converts HSL color values to RGB color values.<br/>
        /// HSL values are expected as follows:<br/>
        /// Hue (h): 0.0 - 360.0 degrees<br/>
        /// Saturation (s): 0.0 - 1.0 (0% - 100%)<br/>
        /// Lightness (l): 0.0 - 1.0 (0% - 100%)<br/>
        /// RGB values will be returned in the range [0, 255].<br/>
        /// CONVERSIONS: There is not always a direct 1 to 1 correlation.<br/>
        /// - When converting doubles to create RGB byte/int values means there is rounding.<br/>
        ///   This provides you the correct values, but the conversion back to doubles might not be what you passed in.
        /// </summary>
        /// <param name="hsl">HslSpace struct</param>
        /// <returns>One of four possible color object</returns>
        public static Color HslToColor(HslSpace hsl) => HsvToColor(hsl.Hue, hsl.Saturation, hsl.Lightness);
        /// <summary>
        /// Converts HSL color values to RGB color values.<br/>
        /// HSL values are expected as follows:<br/>
        /// Hue (h): 0.0 - 360.0 degrees<br/>
        /// Saturation (s): 0.0 - 1.0 (0% - 100%)<br/>
        /// Lightness (l): 0.0 - 1.0 (0% - 100%)<br/>
        /// RGB values will be returned in the range [0, 255].<br/>
        /// CONVERSIONS: There is not always a direct 1 to 1 correlation.<br/>
        /// - When converting doubles to create RGB byte/int values means there is rounding.<br/>
        ///   This provides you the correct values, but the conversion back to doubles might not be what you passed in.
        /// </summary>
        /// <param name="h">Hue component (0.0 - 360.0 degrees).</param>
        /// <param name="s">Saturation component (0.0 - 1.0).</param>
        /// <param name="l">Lightness component (0.0 - 1.0).</param>
        /// <returns>One of four possible color object</returns>
        public static Color HslToColor(double h, double s, double l)
        {
            // Ensure if they passed whole percentages, make them decimal percentages.
            if (s > 1.0) s /= 100;
            if (l > 1.0) l /= 100;

            // Clamp HSL values to their valid ranges
            h = h % 360.0; // Ensure hue wraps around 360 degrees
            if (h < 0) h += 360.0;

            s = Math.Max(0.0, Math.Min(1.0, s));
            l = Math.Max(0.0, Math.Min(1.0, l));

            double r = 0, g = 0, b = 0; // Initialize RGB components

            // If saturation is 0, the color is a shade of gray
            if (s == 0)
            {
                r = l;
                g = l;
                b = l;
            }
            else
            {
                // Calculate chroma (C)
                double c = (1 - Math.Abs(2 * l - 1)) * s;
                // Calculate H' (H prime) for sector determination
                double hPrime = h / 60.0;
                // Calculate x (intermediate value for RGB calculation)
                double x = c * (1 - Math.Abs(hPrime % 2 - 1));

                // Determine RGB based on H' sector
                if (hPrime >= 0 && hPrime < 1)
                {
                    r = c;
                    g = x;
                    b = 0;
                }
                else if (hPrime >= 1 && hPrime < 2)
                {
                    r = x;
                    g = c;
                    b = 0;
                }
                else if (hPrime >= 2 && hPrime < 3)
                {
                    r = 0;
                    g = c;
                    b = x;
                }
                else if (hPrime >= 3 && hPrime < 4)
                {
                    r = 0;
                    g = x;
                    b = c;
                }
                else if (hPrime >= 4 && hPrime < 5)
                {
                    r = x;
                    g = 0;
                    b = c;
                }
                else if (hPrime >= 5 && hPrime < 6)
                {
                    r = c;
                    g = 0;
                    b = x;
                }

                // Calculate m (match lightness)
                double m = l - c / 2;

                // Add m to RGB components
                r += m;
                g += m;
                b += m;
            }

            // Scale RGB values to 0-255 and round
            int oRed = (int)Math.Round(r * 255);
            int oGreen = (int)Math.Round(g * 255);
            int oBlue = (int)Math.Round(b * 255);

            // Ensure RGB values are within the valid range [0, 255]
            byte red = (byte)Math.Max(0, Math.Min(255, oRed));
            byte green = (byte)Math.Max(0, Math.Min(255, oGreen));
            byte blue = (byte)Math.Max(0, Math.Min(255, oBlue));

            return Color.FromArgb(red, green, blue);
        }
        #endregion
    }
}
