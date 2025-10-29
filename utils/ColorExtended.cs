using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace Chizl.ColorExtension
{
    //synonyms vs acronyms
    public static class ColorExtended
    {
        #region Public Methods
        /// <include file="../docs/Shared.xml" path='extradoc/class[@name="Shared"]/properties/property[@name="ToRgb"]/*' />
        public static int ToRgb(this Color @this) => int.Parse(@this.ToHexRgb().TrimStart('#'), NumberStyles.HexNumber);

        /// <include file="../docs/Shared.xml" path='extradoc/class[@name="Shared"]/properties/property[@name="IsDark"]/*' />
        public static bool IsDark(this Color @this, double threshold = 0.5)
        {
            threshold = Math.Max(0, Math.Min(1, threshold));

            //##########################################
            // Assuming you have R, G, B values (0-255)
            double r_norm = @this.R / 255.0;
            double g_norm = @this.G / 255.0;
            double b_norm = @this.B / 255.0;

            // Calculate luminance
            double luminance = (0.2126 * r_norm) + (0.7152 * g_norm) + (0.0722 * b_norm);

            return luminance < threshold;
        }

        /// <include file="../docs/Shared.xml" path='extradoc/class[@name="Shared"]/properties/property[@name="IsLight"]/*' />
        public static bool IsLight(this Color c, double threshold = 0.5) => !c.IsDark(threshold);

        /// <include file="../docs/HEX.xml" path='extradoc/class[@name="HEX"]/interfaces/interface[@name="HEXColor"]/*' />
        public static HEX ToHex(this Color @this) => new HEX(@this);

        /// <include file="../docs/CMYK.xml" path='extradoc/class[@name="CMYK"]/interfaces/interface[@name="CMYKColor"]/*' />
        public static CmykSpace ToCMYK(this Color @this) => new CmykSpace(@this);

        /// <include file="../docs/HSVB.xml" path='extradoc/class[@name="HSVB"]/interfaces/interface[@name="HSVBColor"]/*' />
        public static HsbSpace ToHsb(this Color @this) => new HsbSpace(@this);

        /// <include file="../docs/HSL.xml" path='extradoc/class[@name="HSL"]/interfaces/interface[@name="HSLColor"]/*' />
        public static HslSpace ToHsl(this Color @this) => new HslSpace(@this);

        /// <include file="../docs/HSVB.xml" path='extradoc/class[@name="HSVB"]/interfaces/interface[@name="HSVBColor"]/*' />
        public static HsvSpace ToHsv(this Color @this) => new HsvSpace(@this);

        /// <include file="../docs/Shared.xml" path='extradoc/class[@name="Shared"]/properties/property[@name="HSVBDoublesOut"]/*' />
        public static bool ToHSV(this Color @this, out double hue, out double saturation, out double value)
        {
            var retVal = false;

            try
            {
                var hsv = @this.ToHsv();

                hue = hsv.Hue;
                saturation = hsv.Saturation;
                value = hsv.Value;

                retVal = true;
            }
            catch
            {
                hue = 0;
                saturation = 0;
                value = 0;
            }

            return retVal;
        }

        /// <include file="../docs/LAB.xml" path='extradoc/class[@name="LAB"]/interfaces/interface[@name="LABColor"]/*' />
        public static LabSpace ToLab(this Color @this) => new LabSpace(@this);

        /// <include file="../docs/LCH.xml" path='extradoc/class[@name="LCH"]/interfaces/interface[@name="LCHColor"]/*' />
        public static LchSpace ToLch(this Color @this) => new LchSpace(@this);

        /// <include file="../docs/LUV.xml" path='extradoc/class[@name="LUV"]/interfaces/interface[@name="LUVColor"]/*' />
        public static LuvSpace ToLov(this Color @this) => new LuvSpace(@this);

        /// <include file="../docs/XYZ.xml" path='extradoc/class[@name="XYZ"]/interfaces/interface[@name="XYZColor"]/*' />
        public static XyzSpace ToXyz(this Color @this) => new XyzSpace(@this);

        /// <summary>
        /// Creates a 7 byte, include hash, hex response.
        /// </summary>
        /// <param name="this">System.Color Object</param>
        /// <returns>HEX string without the Alpha channel.  e.g. #CC00CC</returns>
        public static string ToHexRgb(this Color @this) => $"#{@this.R:X2}{@this.G:X2}{@this.B:X2}";
        /// <summary>
        /// Creates a 9 byte, include hash, hex response.
        /// </summary>
        /// <param name="this">System.Color Object</param>
        /// <returns>HEX string with the Alpha channel.  e.g. #FFCC00CC</returns>
        public static string ToHexArgb(this Color @this) => $"#{@this.A:X2}{@this.R:X2}{@this.G:X2}{@this.B:X2}";
        /// <summary>
        /// Creates an Ascii string for text color within a Console windows.  Supports 24bit color.
        /// </summary>
        /// <param name="this">System.Color Object</param>
        /// <returns>Ascii string representing text color for consoles use.  e.g. "\x1b[38;5;255;255;0m" - True 24bit Yellow</returns>
        public static string FGAscii(this Color @this, bool forDisplay = false) => Common.GetAsciiEscape(@this, true, forDisplay);
        /// <summary>
        /// Creates an Ascii string for background color behind text within a Console windows.  Supports 24bit color.
        /// </summary>
        /// <param name="this">System.Color Object</param>
        /// <returns>Ascii string representing background color for consoles use.  e.g. "\x1b[48;5;255;0;0m" - True 24bit Red</returns>
        public static string BGAscii(this Color @this, bool forDisplay = false) => Common.GetAsciiEscape(@this, false, forDisplay);
        /// <summary>
        /// Color Extension: returns a Ascii Esape string that can be used in Console windows during Console.Write/WriteLine.<br/>
        /// This string will reset all foreground and background console colors back to default from the time of write.
        /// </summary>
        /// <param name="this">Any Color object</param>
        /// <returns>Ascii Escape string with value of: "\x1b[0m"</returns>
        public static string ResetAscii(this Color @this, bool performWrite = false)
        {
            var retVal = "\x1b[0m";
            if (performWrite) Console.Write(retVal);

            return retVal;
        }
        /// <summary>
        /// Uses existing color as foreground, overlay it on top of arguement background color, and return a new RGB color.<br/>
        /// If foreground color doesn't have any transparency, it will return foreground color as the new color.
        /// </summary>
        /// <param name="bgColor">Color object to use as Background Color</param>
        /// <returns>new System.Drawing.Color object</returns>
        public static Color ApplyBgColor(this Color @this, Color bgColor) => new OverlayColors(@this, bgColor).MergedColor;
        /// <summary>
        /// Uses existing color as background, overlay arguement foreground color, and return a new RGB color.<br/>
        /// If foreground color doesn't have any transparency, it will return foreground color as the new color.
        /// </summary>
        /// <param name="fgColor">Color object to use as Foreground Color</param>
        /// <returns>new System.Drawing.Color object</returns>
        public static Color ApplyFgColor(this Color @this, Color fgColor) => new OverlayColors(fgColor, @this).MergedColor;
        /// <summary>
        /// NOT READY !! - Uses existing color as the Background and will over lay the parameter foreground with Alpha to return a new color RGB.
        /// </summary>
        /// <param name="fgColor">Color object to use as Foreground Color</param>
        /// <returns></returns>
        //public static string NameCreator(this Color @this) => GetColorName(@this);

        /// <include file="../docs/Shared.xml" path='extradoc/class[@name="Shared"]/properties/property[@name="Tint"]/*' />
        public static Color Tint(this Color color, double percentage)
        {
            if (percentage < 0 || percentage > 100)
                throw new ArgumentOutOfRangeException(nameof(percentage), "Value must be between 0 and 100.");

            double factor = percentage / 100.0;

            int r = ((int)(color.R + (255 - color.R) * factor)).ClampTo(0, 255);
            int g = ((int)(color.G + (255 - color.G) * factor)).ClampTo(0, 255);
            int b = ((int)(color.B + (255 - color.B) * factor)).ClampTo(0, 255);

            return Color.FromArgb(color.A, r, g, b);
        }

        /// <include file="../docs/Shared.xml" path='extradoc/class[@name="Shared"]/properties/property[@name="Shade"]/*' />
        public static Color Shade(this Color color, double percentage)
        {
            if (percentage < 0 || percentage > 100)
                throw new ArgumentOutOfRangeException(nameof(percentage), "Value must be between 0 and 100.");

            double factor = percentage / 100.0;

            int r = ((int)(color.R * (1 - factor))).ClampTo(0, 255);
            int g = ((int)(color.G * (1 - factor))).ClampTo(0, 255);
            int b = ((int)(color.B * (1 - factor))).ClampTo(0, 255);

            return Color.FromArgb(color.A, r, g, b);
        }

        /// <include file="../docs/Shared.xml" path='extradoc/class[@name="Shared"]/properties/property[@name="Tone"]/*' />
        public static ToneRule GetTone(this Color @this) => Common.GetToneModifier(@this);

        /// <include file="../docs/Shared.xml" path='extradoc/class[@name="Shared"]/properties/property[@name="Temperature"]/*' />
        public static string GetTemperature(this Color @this) => Common.GetTemperatureModifier(@this);

        /// <include file="../docs/Shared.xml" path='extradoc/class[@name="Shared"]/properties/property[@name="Complementary"]/*' />
        public static Color GetComplementary(this Color @this) => Common.GetComplementary(@this);

        /// <include file="../docs/Shared.xml" path='extradoc/class[@name="Shared"]/properties/property[@name="Contrast"]/*' />
        public static bool GetContrast(this Color background, out Color bestContrastColor, out double contrastRatio, double minThreshold = 0) => 
            ColorContrastHelper.GetContrast(background, out bestContrastColor, out contrastRatio, minThreshold);        

        /// <summary>
        /// One object with CMYK, HSV, HSL, and HEX within it.
        /// </summary>
        public static ColorDetails Details(this Color @this) => new ColorDetails(@this);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="this"></param>
        /// <param name="noSpaces"></param>
        /// <returns></returns>
        public static string HueName(this Color @this, bool noSpaces = true) => HueName(@this.GetHue(), noSpaces);
        public static string HueName(this double @this, bool noSpaces = true) => Common.InterpolateDescriptor(@this, HsValueType.Hue, noSpaces);
        /// <summary>
        /// Dump of information about 
        /// </summary>
        /// <param name="this"></param>
        /// <param name="useAsciiEscapeColorCodes"></param>
        /// <returns></returns>
        public static string About(this Color @this, bool useAsciiEscapeColorCodes = false)
        {
            StringBuilder sbDetails = new StringBuilder();
            var details = @this.Details();

            if(!@this.GetContrast(out Color contrast, out double constrastRatio))
                contrast = Color.Empty;

            var hsv = details.HSV;
            var hsl = details.HSL;
            var hex = details.HEX;
            var cmyk = details.CMYK;

            var lab = details.LAB;
            var lch = details.LCH;
            var luv = details.LUV;
            var xyz = details.XYZ;

            var dark = @this.IsDark();

            var fgAscii = "";
            var bgAscii = "";
            var resetAscii = "";
            var bgFgAscii = "";
            var spResetAscii = "";
            var fgGreen = "";
            var additionalInfo = "";

            var hueInfo = Common.InterpolateDescriptor(hsv.Hue, HsValueType.Hue, true);
            var satHsvInfo = Common.InterpolateDescriptor(hsv.Saturation, HsValueType.Saturation_Hsv, true);
            var valInfo = Common.InterpolateDescriptor(hsv.Value, HsValueType.Value, true);
            var satHslInfo = Common.InterpolateDescriptor(hsl.Saturation, HsValueType.Saturation_Hsl, true);
            var lightInfo = Common.InterpolateDescriptor(hsl.Lightness, HsValueType.Lightness, true);

            var cmykName = Constants.GetCMYKModifier(cmyk.Cyan, cmyk.Magenta, cmyk.Yellow, cmyk.Key);

            var name = CheckKnownColorNames(hex.ToArgb, hex.ToArgbHex);
            var isKnown = !Common.IsHex(name);

            if (useAsciiEscapeColorCodes)
            {
                fgAscii = useAsciiEscapeColorCodes ? Common.GetAsciiEscape((dark ? Color.White : Color.Black), true, false) : "";
                bgAscii = useAsciiEscapeColorCodes ? Common.GetAsciiEscape(@this, false, false) : "";
                resetAscii = useAsciiEscapeColorCodes ? ResetAscii(@this, false) : "";
                bgFgAscii = $"{bgAscii}{fgAscii} ";
                spResetAscii = $" {resetAscii}";
                fgGreen = isKnown ? Color.Lime.FGAscii() : Color.FromArgb(255, 255, 0).FGAscii();
            }

            if (hsv.Saturation.Equals(100.0) && hsv.Value.Equals(100.0))
            {
                if (!isKnown)
                    name = hueInfo;  //overwrite
                else if (!hueInfo.ToLower().Equals(name.ToLower()))
                    additionalInfo = $"Suggested Name:{bgFgAscii}{hueInfo}{spResetAscii}";
            }

            //format it.
            name = $"{bgFgAscii}{name}{spResetAscii}";

            sbDetails.AppendLine($"Color: '{name}' - {fgGreen}Is{(!isKnown ? "n't" : "")} a known system{resetAscii} named color.");
            if (!string.IsNullOrWhiteSpace(additionalInfo))
                sbDetails.AppendLine(additionalInfo);
            var compl = @this.GetComplementary();
            var thisBgColor = @this.BGAscii();
            var thisFgColor = @this.IsDark() ? Color.White.FGAscii() : Color.Black.FGAscii();
            var conBgColor = contrast.BGAscii();
            var conFgColor = contrast.IsDark() ? Color.White.FGAscii() : Color.Black.FGAscii();
            var comBgColor = compl.BGAscii();
            var comFgColor = compl.IsDark() ? Color.White.FGAscii() : Color.Black.FGAscii();

            sbDetails.AppendLine($"{thisBgColor}{thisFgColor}Original {@this}{resetAscii} - {@this.ToHex()}");
            sbDetails.AppendLine($"{conBgColor}{conFgColor}Contrast {contrast}{resetAscii} (Ratio: {constrastRatio}) - {contrast.ToHex()}");
            sbDetails.AppendLine($"{comBgColor}{comFgColor}Complementary {compl}{resetAscii} - {compl.ToHex()}");

            sbDetails.AppendLine($"{hex.ValueString}");
            sbDetails.AppendLine($"{hsv.ValueString}");
            sbDetails.AppendLine($"{hsl.ValueString}");
            sbDetails.AppendLine($"{cmyk.ValueString}");

            if (!string.IsNullOrWhiteSpace(cmykName.Modifier))
                sbDetails.AppendLine($" - CMYK Rule #{cmykName.RuleNo} is described as {bgFgAscii}'{cmykName}'{spResetAscii}\n   - {cmykName.RulesDisplay.Replace("\n", "\n   - ")}");
            else
                sbDetails.AppendLine();

            sbDetails.AppendLine($"{lch.ValueString}");
            sbDetails.AppendLine($"{lab.ValueString}");
            sbDetails.AppendLine($"{luv.ValueString}");
            sbDetails.AppendLine($"{xyz.ValueString}");
            var tone = @this.GetTone();
            sbDetails.AppendLine($"Tone: {tone}");
            sbDetails.AppendLine($" - Tone Rule #{tone.RuleNo}");
            sbDetails.AppendLine($"   - {tone.RulesDisplay.Replace("\n", "\n   - ")}");
            //sbDetails.AppendLine($"Hex Value: v6 {@this.ToHexRgb()},  v8 {@this.ToHexArgb()}");
            //sbDetails.AppendLine($"IsDark: {dark}, Temperature: {hsv.Temperature}, Tone: {tone}");
            //sbDetails.AppendLine($"Tone {tone.RuleNo}: {tone}, S:{tone.Saturation}, V: {tone.Value}, Rules:{tone.RulesDisplay}");

            //line = AddToLine(line, "Color Wheel", hueInfo);
            //line = AddToLine(line, "Purity", satHsvInfo);
            //line = AddToLine(line, "Brightness", valInfo);

            //if (!string.IsNullOrWhiteSpace(line))
            //    sbDetails.AppendLine(line); line = "";

            //line = AddToLine(line, "Intensity", satHslInfo);
            //line = AddToLine(line, "Lightness", lightInfo);

            //if (!string.IsNullOrWhiteSpace(line))
            //    sbDetails.AppendLine(line); line = "";


            //sbDetails.AppendLine($"Hue: {@this)}");
            //sbDetails.AppendLine($"Tone: {@this.GetTone()}");
            //sbDetails.AppendLine($"Tone: {@this.GetTone()}");

            return sbDetails.ToString();
        }
        #endregion

        #region Private Helper Methods
        //private static string GetColorName(Color clr)
        //{
        //    var retVal = clr.Name;

        //    if (Common.IsHex(retVal))
        //        retVal = CheckKnownColorNames(clr.ToRgb(), retVal);

        //    if (Common.IsHex(retVal))
        //    {
        //        var hsv = new HSV(clr);
        //        var cmyk = new CMYK(clr);
        //        var builder = new List<string>();

        //        //retVal = InterpolateDescriptor(hsv.Hue, Constants._hueInterpolations).Replace(" ", "");
        //        retVal = GetCombinedColorName(hsv.Hue, hsv.Saturation, hsv.Value);
        //        //retVal = GetCombinedColorName(hsv.Hue, hsv.Saturation, hsv.Value).Replace(" ","");
        //        //var hueName = InterpolateDescriptor(hsv.Hue, Constants._hueInterpolations);
        //        //AddToBuilder(hueName, ref builder);

        //        //var satName = InterpolateDescriptor(hsv.Saturation, Constants._hsvSaturationInterpolations);
        //        //AddToBuilder(satName, ref builder);

        //        //var valName = InterpolateDescriptor(hsv.Value, Constants._valueInterpolations);
        //        //AddToBuilder(valName, ref builder);

        //        //var cmykName = Constants.GetCMYKModifier(cmyk.C, cmyk.M, cmyk.Y, cmyk.K);
        //        //AddToBuilder(cmykName, ref builder);

        //        //retVal = string.Join("", builder);
        //        //double hPrime = hsv.Hue / 60.0; // H'
        //    }

        //    return retVal;
        //}
        internal static string GetCombinedColorName(double hue, double saturation, double value)
        {
            var hueHsvModifier = Common.InterpolateDescriptor(hue, HsValueType.Hue);
            var satHsvModifier = Common.InterpolateDescriptor(saturation, HsValueType.Saturation_Hsv);
            var valHsvModifier = Common.InterpolateDescriptor(value, HsValueType.Value);
            var valHsvModifierArray = valHsvModifier.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            // 1. Handle Achromatic Extremes (Black, White, Grays)
            //if (value <= 10.0 && saturation <= 10.0) // Very low value, very low saturation
            //    return "Absolute Black";  // Or lookup from a specific "AchromaticDarkness" list

            //Very low or very high value
            if ((value <= 5.0 || value >= 95.0) && saturation <= 5.0)  // very low saturation
                return valHsvModifier;              // Or lookup from a specific "AchromaticDarkness" list

            //if (value >= 96.0 && saturation <= 10.0) // Very high value, very low saturation
            //{
            //    // These are true whites/off-whites
            //    if (value >= 99.0) return "Pure White";
            //    if (value >= 98.0) return "Crisp White";
            //    if (value >= 97.0) return "Off-White";
            //    return "Near White"; // Or lookup from a specific "AchromaticLightness" list
            //}

            //if (saturation <= 10.0) // Low saturation, but not extreme black/white
            //{
            //    // This is a grayscale value (different from black/white)
            //    // You'd need a separate lookup for grayscale names if you want them
            //    // For example: if value is around 50, "Neutral Gray"; if value is 20, "Charcoal Gray"
            //    // For now, let's just combine value modifier with "Gray"
            //    //string valMod = GetValueModifier(value); // Assuming it returns terms like "Muted Dark", "Balanced", "Light Tone"
            //    return valMod + " Gray"; // e.g., "Muted Dark Gray", "Balanced Gray", "Light Tone Gray"
            //}

            if (saturation <= 10.0)
            {
                var hasLightBright = valHsvModifier.Contains("Light") ? true : valHsvModifier.Contains("Bright");
                var hasDarkShadow = valHsvModifier.Contains("Dark") ? true : valHsvModifier.Contains("Shadow");

                if (hasLightBright)
                    return valHsvModifier + " Gray";   // e.g., "Light Gray", "Bright Gray"
                else if (hasDarkShadow)
                    return valHsvModifier + " Gray";   // e.g., "Deep Dark Gray", "Shadowed Gray"
                else
                    return "Neutral Gray";          // For mid-value, low saturation
            }

            // 2. Get the base Hue Name
            //var hueHsvModifier = InterpolateDescriptor(hue, Constants._hueInterpolations);
            // 3. Get Saturation and Value modifiers
            //string satHsvModifier = GetSaturationModifier(saturation);
            //var satHsvModifier = InterpolateDescriptor(saturation, Constants._hsvSaturationInterpolations);
            //string valModifier = GetValueModifier(value);
            //var valModifier = InterpolateDescriptor(value, Constants._valueInterpolations);
            // 4. Combine them intelligently (simplified logic, you'd add more nuances)
            // Prioritize high/low saturation or value modifiers if they are strong

            // Covers "Deep Dark", "Murky", "Twilight", "Dimmed"
            if (value <= 20.0)
            {
                // Combine darkness modifier with hue. Saturation might be less prominent here.
                if (saturation >= 70.0) // Deep, rich jewel tones
                    return $"{valHsvModifier} {satHsvModifier} {hueHsvModifier}"; // e.g., "Deep Dark Rich Ruby Flare" (if Ruby Flare is a hue)
                else
                    return $"{valHsvModifier} {hueHsvModifier}"; // e.g., "Midnight Blue", "Murky Forest Green"
            }

            //*
            // If the color is at its absolute brightest (Value 90-100)
            if (value >= 90.0)
            {
                // If also highly saturated, emphasize purity/vibrancy and brightness
                if (saturation >= 90.0)
                {
                    // For true primaries like Red (0), Yellow (60), Blue (180/240), etc., just the hue name might suffice
                    // or "Pure/Vivid Hue"
                    // Adjust these ranges to match your specific primary points
                    if ((hue >= 355 || hue <= 5) ||
                        (hue >= 55 && hue <= 65) ||
                        (hue >= 175 && hue <= 185) ||
                        (hue >= 235 && hue <= 245) ||
                        (hue >= 295 && hue <= 305))
                        return $"Pure {hueHsvModifier}"; // e.g., "Pure Red", "Pure Yellow", "Pure Blue"
                    else
                        return $"{satHsvModifier} {valHsvModifier} {hueHsvModifier}"; // e.g., "Vivid Full Chroma Brightness Azure Blue"
                }
                // If high value but medium saturation:
                else
                    return $"{valHsvModifier} {hueHsvModifier}"; // e.g., "Brilliant Tone Sky Blue", "Luminescent Orange"
            }
            // Rule 2: Prioritize extreme Saturation (very desaturated or very vibrant)
            // If very desaturated (but not gray/black/white, handled above) || If very highly saturated (but not extreme value, handled above)
            // Covers "Muted", "Washed Out", "Dusty", "Pale" ||  Covers "Vibrant", "Bold", "Intense"
            if (saturation <= 30.0 || saturation >= 70.0)
                return $"{satHsvModifier} {hueHsvModifier}"; // e.g., "Dusty Rose Red", "Muted Sea Green" || "Vibrant Emerald Green", "Bold Magenta"

            // Rule 3: General Combination for Mid-Ranges (most common scenario)
            // Typically, Value modifier first, then Saturation modifier, then Hue.
            // However, if the modifier is "Balanced", "Standard", "Normal", it can often be omitted for conciseness.

            List<string> parts = new List<string>();

            // Add Value modifier if it's significant (not "Balanced", "Standard", etc.)
            if ("Balanced,Standard,Typical,Normal,Moderate,Average Brightness".IndexOf(valHsvModifier).Equals(-1))
                parts.Add(valHsvModifier);

            // Add Saturation modifier if it's significant (not "Balanced", "Standard", etc.)
            if ("Balanced,Standard,Typical,Normal,Moderate,Regular".IndexOf(satHsvModifier).Equals(-1))
                parts.Add(satHsvModifier);

            parts.Add(hueHsvModifier);

            return string.Join(" ", parts); // Joins parts with spaces, e.g., "Light Soft Rose"
            /**/

            //if (saturation >= 90.0) return $"{satHsvModifier} {hueHsvModifier}"; // e.g., "Pure Emerald Green", "Vivid Ruby"
            //if (value >= 90.0) return $"{valHsvModifier} {hueHsvModifier}"; // e.g., "Full Chroma Yellow", "Absolute Brightness Cyan"
            //if (saturation <= 20.0) return $"{satHsvModifier} {hueHsvModifier}"; // e.g., "Dusty Rose", "Faded Blue"
            //if (value <= 20.0) return $"{valHsvModifier} {hueHsvModifier}"; // e.g., "Deep Dark Violet", "Midnight Forest"

            // For mid-ranges, typically value then saturation
            //if (value >= 60.0) // Lighter shades
            //{
            //    if (saturation >= 70.0) return $"{valHsvModifier} {satHsvModifier} {hueHsvModifier}"; // e.g., "Bright Vibrant Blue"
            //    return $"{valHsvModifier} {hueHsvModifier}"; // e.g., "Light Tone Green"
            //}
            //if (value <= 40.0) // Darker shades
            //{
            //    if (saturation >= 70.0) return $"{valHsvModifier} {satHsvModifier} {hueHsvModifier}"; // e.g., "Deep Rich Burgundy"
            //    return $"{valHsvModifier} {hueHsvModifier}"; // e.g., "Muted Dark Indigo"
            //}

            //// Default: Combine saturation and hue, or just hue if all are typical
            //if (saturation >= 50.0) return $"{satHsvModifier} {hueHsvModifier}"; // e.g., "Balanced Sky Blue"
            //return hueHsvModifier; // e.g., "Red"
        }

        //private static void AddToBuilder(string verify, ref List<string> builder)
        //{
        //    var vert = verify.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        //    foreach(var v in vert)
        //    {
        //        if(!builder.Contains(v))
        //            builder.Add($"{v.Substring(0,1).ToUpper()}{v.Substring(1).ToLower()}");
        //    }                    
        //}
        private static string CheckKnownColorNames(int rgb, string defName)
        {
            Type colorType = typeof(Color);
            PropertyInfo[] propInfos = colorType.GetProperties(BindingFlags.Static | BindingFlags.DeclaredOnly | BindingFlags.Public);
            foreach (PropertyInfo propInfo in propInfos)
            {
                var clrProp = (Color)propInfo.GetValue(propInfo);
                var clrVal = clrProp.ToArgb();
                if (clrVal.Equals(rgb))
                    return propInfo.Name;
            }

            return defName;
        }
        #endregion
    }
}
