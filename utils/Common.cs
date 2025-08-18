using System.Linq;
using System.Drawing;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;
using System.Collections.Concurrent;
using System.Threading;

namespace Chizl.ColorExtension
{
    internal static class Common
    {
        #region Private Vars
        //can increate cache, but it takes more memory.  Evaluate before changing.
        const int _maxCachedObjects = 256;
        //once max is reached, it will trim down 56 of the oldest.   _colorDetailsList is thread safe, so other threads
        //could be write to it, as it's being cleared.  It will continue to remove 1 at a time, until it is at the _trimToCacheSize.
        const int _trimToCacheSize = 200;
        /// <summary>
        /// 24bit, because Color object is using a 24bit palette.
        /// </summary>
        private static readonly int _24BitPalette = AsciiColorPalette.Color_Palette_24Bit.Value();
        private static readonly Regex _regExHex = new Regex("^#?([a-fA-F0-9]{8}|[a-fA-F0-9]{6}|[a-fA-F0-9]{3})$");
        public static readonly (double X, double Y, double Z) D65 = (95.047, 100.000, 108.883);
        public static readonly (double X, double Y, double Z) FullPrecisionD65 = (95.0489, 100.0000, 108.8840);
        private static readonly ConcurrentDictionary<int, ColorDetails> _colorDetailsList = new ConcurrentDictionary<int, ColorDetails>();
        private static readonly ConcurrentQueue<int> _order = new ConcurrentQueue<int>();
        private static readonly object _colorDetailListLock = new object();
        private static readonly HsValueType[] _allowedQuickList = new HsValueType[]
            { HsValueType.Value, HsValueType.Hue, HsValueType.Saturation_Hsl,
              HsValueType.Saturation_Hsv, HsValueType.Brightness, HsValueType.Lightness };
        private static Thread _cleanupThread;
        private static readonly object _cleanupThreadLock = new object();
        #endregion

        #region public methods
        /// <summary>
        /// Used to pull ColorDetails from cache if exists.
        /// </summary>
        /// <param name="color">Color to look for</param>
        /// <param name="colorDetails">Cached version if exists</param>
        /// <returns>true if successfully found, false if not found</returns>
        public static bool GetCachedColorList(Color color, out ColorDetails colorDetails) => _colorDetailsList.TryGetValue(color.ToArgb(), out colorDetails);
        /// <summary>
        /// Add ColorDetails object to Cache if not already there.  if more than max in cached, removed the oldest cached object.
        /// </summary>
        /// <param name="color">ColorDetails object to cache</param>
        public static void AddCachedColorList(ColorDetails color)
        {
            // Try to add to dictionary
            if (_colorDetailsList.TryAdd(color.Argb, color))
            {
                // Track insertion order
                _order.Enqueue(color.Argb);

                // Trim if we go over capacity
                if (_colorDetailsList.Count > _maxCachedObjects)
                {
                    // This lock will return quicly after thread starts, but need lock to ensure other 
                    // threads can't execute the next if statement at the same time. Without lock, that would cause exceptions.
                    lock (_cleanupThreadLock)
                    {
                        // If thread isn't already running
                        if (_cleanupThread == null || !_cleanupThread.IsAlive)
                        {
                            // True multi-threading: Asynchronous, only 1 allowed, but will not hender flow while clean up is executing.
                            // Will exit thread when clean up has caught up.
                            _cleanupThread = new Thread(() =>
                            {
                                lock (_colorDetailListLock)
                                {
                                    //going to trim it down with some to spare, not just _maxCachedObjects - 1
                                    while (_colorDetailsList.Count > _trimToCacheSize && _order.TryDequeue(out var oldest))
                                    {
                                        // Make sure the dequeued key is still present before removing
                                        _colorDetailsList.TryRemove(oldest, out _);
                                    }
                                }
                            });
                            _cleanupThread.Start();
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Uses Regex to validate if string as a whole is HexV8, HexV6, or HexV3, allowing # to be optional.<br/>
        /// Pattern: "^([#]?([a-fA-F0-9]{8})|([a-fA-F0-9]{6})|([a-fA-F0-9]{3}))$"
        /// </summary>
        /// <returns>(bool)<br/>
        /// - true : is HEX<br/>
        /// - false: is not HEX</returns>
        public static bool IsHex(string str) => _regExHex.IsMatch(str);
        /// <summary>
        /// Removes all ConsoleCommands and Escape Character Colors and Symbols
        /// </summary>
        /// <returns></returns>
        public static string CleanString(string msg, bool colors, bool styles, bool incSymbols)
        {
            var retVal = msg;

            if (string.IsNullOrWhiteSpace(retVal))
                return string.Empty;

            // \x1b[1m          - (1) value makes text BOLD and uses no speace on screen, only in string.
            // \u001b[38;5;1m   - (38) = foreground, (1) = DarkRed color
            // \u001b[48;5;1m   - (48) = background, (1) = DarkRed color
            // FG Example: \u001b[38;5;196m"; -- 196m = TrueRed
            // BG Example: \u001b[48;5;16m";  -- 16m = TrueBlack

            var colorsToken = "\u001b[";
            var stytlesToken = "\x1b[";

            if (colors)
                retVal = CleanAC(retVal, colorsToken);
            if (styles)
                retVal = CleanAC(retVal, stytlesToken);

            if (incSymbols)
            {
                var orgStr = retVal;
                foreach (char clrChars in orgStr)
                {
                    //if (clrChars > 255)
                    if (clrChars > 128)
                        retVal = retVal.Replace(clrChars.ToString(), "");
                }
            }

            return retVal;
        }
        /// <summary>
        /// Support for CleanString, based on colors and/or styles set to true.
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static string CleanAC(string msg, string token)
        {
            if (msg.Contains(token))
            {
                //find the start of the color
                var start = msg.IndexOf(token);
                while (start > 0)
                {
                    //required because of the slash escape
                    start -= 1;

                    //find the end of the color
                    var end = msg.IndexOf("m", start + token.Length);
                    //if exists
                    if (end > -1)
                    {
                        //get full string
                        var repToken = msg.Substring(start, ((end + 1) - start));
                        //replace it
                        msg = msg.Replace(repToken, "");
                    }
                    else
                        break;

                    //check to see if another exists, should of been replaced above, lets be sure.
                    start = msg.IndexOf(token);
                }
            }
            return msg;
        }

        /// <summary>
        /// Use to query associated array of values to match up with names.
        /// </summary>
        /// <param name="value">value to lookup</param>
        /// <param name="hsValueType">value type to select array to use.</param>
        /// <param name="noSpaces">to remove spaces on return or not.</param>
        /// <returns>String found or "" if not found or empty value</returns>
        public static string InterpolateDescriptor(double value, HsValueType hsValueType, bool noSpaces = false)
        {
            var dictionary = GetMappedDictionary(hsValueType);
            if (dictionary == null || dictionary.Count == 0) return string.Empty;

            if (_allowedQuickList.Contains(hsValueType))
            {
                double rndValue = Math.Round(value);
                if (dictionary.TryGetValue(rndValue, out string strValue))
                    return strValue;
            }

            // Handle different valid ranges based on type
            double minValid = dictionary.Keys.Min();
            double maxValid = dictionary.Keys.Max();

            // Clamp for types other than cyclic
            if (hsValueType != HsValueType.Hue && hsValueType != HsValueType.Temperature)
            {
                value = value.ClampTo(minValid, maxValid);
            }
            else
            {
                // Normalize cyclic values to [0,360)
                value = (value % 360 + 360) % 360;
            }

            if (dictionary.ContainsKey(value))
                return noSpaces ? dictionary[value].Replace(" ", "") : dictionary[value];

            if (hsValueType == HsValueType.Hue || hsValueType == HsValueType.Temperature)
            {
                // Cyclic lookup with wrap-around
                var keys = dictionary.Keys.OrderBy(k => k).ToList();

                double minDist = double.MaxValue;
                double closestKey = keys[0];

                foreach (var key in keys)
                {
                    double dist = Math.Min(Math.Abs(value - key), 360 - Math.Abs(value - key));
                    if (dist < minDist)
                    {
                        minDist = dist;
                        closestKey = key;
                    }
                }

                var retVal = dictionary[closestKey];
                return noSpaces ? retVal.Replace(" ", "") : retVal;
            }
            else
            {
                // Linear nearest neighbor for other types
                var lower = dictionary.Keys.Where(k => k < value).DefaultIfEmpty(minValid).Max();
                var higher = dictionary.Keys.Where(k => k > value).DefaultIfEmpty(maxValid).Min();

                double distLower = value - lower;
                double distHigher = higher - value;

                var retVal = (distLower < distHigher ? dictionary[lower] : dictionary[higher]).Trim();
                return noSpaces ? retVal.Replace(" ", "") : retVal;
            }
        }

        //public static string InterpolateDescriptor(double value, HsValueType hsValueType, bool noSpaces = false)
        //{
        //    if (value > 360.0 || value < 0.00) return string.Empty;

        //    var retVal = string.Empty;
        //    var dictionary = GetMappedDictionary(hsValueType);

        //    //Dictionary<double, string> dictionary
        //    if (dictionary.ContainsKey(value))
        //        retVal = dictionary[value];
        //    else
        //    {
        //        // Find nearest lower and higher keys for interpolation
        //        var lower = dictionary.Keys.Where(k => k < value).DefaultIfEmpty(dictionary.Keys.Min()).Max();
        //        var higher = dictionary.Keys.Where(k => k > value).DefaultIfEmpty(dictionary.Keys.Max()).Min();

        //        // For descriptive strings, just pick the closest (could improve by using more complex interpolation)
        //        double distLower = value - lower;
        //        double distHigher = higher - value;

        //        retVal = (distLower < distHigher ? dictionary[lower] : dictionary[higher]).Trim();
        //    }

        //    return noSpaces ? retVal?.Replace(" ", "") : retVal;
        //}
        /// <summary>
        /// Used to build foreground and or background colors Ascii Escape codes for consoles coloring.
        /// </summary>
        /// <param name="clr">Color to build Ascii Escape codes from</param>
        /// <param name="isForeground">This is required to build the correct Ascii Escape codes</param>
        /// <param name="asString">true if used for displaying the codes.  false if you want them to be used for coloring. (Default: false)</param>
        /// <returns>Ascii Escape code for display or usage based on asString argument.</returns>
        public static string GetAsciiEscape(Color clr, bool isForeground, bool asString = false)
        {
            var fgBgColor = isForeground ? AsciiColorType.FG_Color : AsciiColorType.BG_Color;
            if (asString)
                return $"\\x1b[{fgBgColor.Value()};{_24BitPalette};{clr.R};{clr.G};{clr.B}m";
            else
                return $"\x1b[{fgBgColor.Value()};{_24BitPalette};{clr.R};{clr.G};{clr.B}m";
        }

        #region Calculations
        /// <summary>
        /// Yes this does the math to create the Hue intead of using Color.GetHue().<br/>
        /// This is because MS Color structure isn't 100% for example:<br/>
        /// GetBrightness() returns HSL Lightness and GetSaturation returns HSL Saturation values instead of HSV as documented and commented within their structure.<br/>
        /// As things change, this method guarantees correct values.
        /// </summary>
        /// <param name="clr">Color to get hue from.</param>
        /// <param name="decPoints">(Optional) How many decimal points to round to. Only accepts 0-4.<br/>Default: 2</param>
        /// <returns>value rounded by set decimal points.</returns>
        public static double CalcHue(Color clr, byte decPoints = 2)
        {
            double r = clr.R / 255.0;
            double g = clr.G / 255.0;
            double b = clr.B / 255.0;

            //find max
            double max = Math.Max(r, Math.Max(g, b));
            //find mind
            double min = Math.Min(r, Math.Min(g, b));
            //set difference
            double delta = max - min;

            //hue calculations
            double hue;
            if (delta == 0)
                hue = 0;  // Grayscale
            else if (max == r)
                hue = (g - b) / delta + (g < b ? 6 : 0);
            else if (max == g)
                hue = (b - r) / delta + 2;
            else
                hue = (r - g) / delta + 4;

            hue /= 6;     //normalize to 0-1
            hue *= 360;   //convert to degrees (0-360)

            return hue.SetBoundary(0.0, 360.0, decPoints);
        }
        /// <summary>
        /// Calculates both HSV and HSL Saturation.
        /// </summary>
        /// <param name="clr">Color to build saturation from.</param>
        /// <param name="decPoints">(Optional) How many decimal points to round to. Only accepts 0-4.<br/>Default: 2</param>
        /// <returns>(HSV.Saturation, HSL.Saturation)</returns>
        public static (double hsbSaturation, double hslSaturation) CalcSaturation(Color clr, byte decPoints = 2)
        {
            double r = clr.R / 255.0;
            double g = clr.G / 255.0;
            double b = clr.B / 255.0;

            //find max
            double max = Math.Max(r, Math.Max(g, b));
            //find mind
            double min = Math.Min(r, Math.Min(g, b));
            //set difference
            double delta = max - min;

            // HSL and HSV has a different calculation for Saturation
            // HSL Saturation Calculations
            double s_hsl = (delta == 0) ? 0 : (delta / (1 - Math.Abs(2 * ((max + min) / 2.0) / 100f - 1))) * 100;
            // HSV/HSB Saturation Calculations
            double s_hsv = (max == 0) ? 0 : (delta / max) * 100;

            //Trim down data and round based on requirements.
            s_hsv = s_hsv.SetBoundary(0.0, 100.0, decPoints);
            s_hsl = s_hsl.SetBoundary(0.0, 100.0, decPoints);

            return (s_hsv, s_hsl);
        }
        /// <summary>
        /// Will calculate to build HSV/HSB Value/Brightness.
        /// </summary>
        /// <param name="clr">Color to get birghtness from.</param>
        /// <param name="decPoints">(Optional) How many decimal points to round to. Only accepts 0-4.<br/>Default: 2</param>
        /// <returns>value rounded by set decimal points.</returns>
        public static double CalcBrightness(Color clr, byte decPoints = 2)
        {
            double r = clr.R / 255.0;
            double g = clr.G / 255.0;
            double b = clr.B / 255.0;

            //find max
            double max = Math.Max(r, Math.Max(g, b));
            //hsv.value, hsb.brightness
            double brightness = max * 100; //value,brightness
            //make sure it's within 0-100 and round by 2 decimal places
            return brightness.SetBoundary(0.0, 100.0, decPoints);
        }
        #endregion

        /// <summary>
        /// Creates a tone level name based on HSV/HSB Saturation and Brightness of the color passed in.
        /// </summary>
        /// <param name="clr">Color to get Tone information from.</param>
        /// <returns>
        /// ToneRule object, with Modifier property as the name.<br/>
        /// Also included 1 of 14 rule # and rule break down on how name was generated.
        /// </returns>
        public static ToneRule GetToneModifier(Color clr)
        {
            //get HSB / HSV information from color.
            var hsb = clr.ToHsb();

            //return rule with value and name.
            return Constants.GetToneModifier(hsb.Saturation, hsb.Brightness);
        }
        /// <summary>
        /// High level temperature string stating how this color is viewed.
        /// </summary>
        /// <param name="clr">Color to get Temperature information from.</param>
        /// <returns>String with name. Available return values: Hot, Warm, Neutral-Warm, Cool, Cold, Neutral-Cool</returns>
        public static string GetTemperatureModifier(Color clr) => InterpolateDescriptor(CalcHue(clr), HsValueType.Temperature, true);
        /// <summary>
        /// Returns the color from the colorwheel 180 degree from itself.
        /// </summary>
        /// <param name="clr">Color to get complementary color for</param>
        /// <returns>new color 180 degrees from the color wheel</returns>
        public static Color GetComplementary(Color clr)
        {
            //get HSB / HSV information from color.
            var hsb = clr.ToHsb();

            //adjust the hue by 180 degrees
            double complementaryHue = (hsb.Hue + 180) % 360;

            //return the complementary color object
            return ColorConverterEx.HsvToColor(complementaryHue, hsb.Saturation, hsb.Brightness);
        }
        #endregion

        #region private helper methods
        private static Dictionary<double, string> GetMappedDictionary(HsValueType hsValueType)
        {
            switch (hsValueType)
            {
                case HsValueType.Saturation_Hsv:
                    return Constants.SaturationHSVMapper;
                case HsValueType.Saturation_Hsl:
                    return Constants.SaturationHSLMapper;
                case HsValueType.Lightness:
                    return Constants.LightnessMapper;
                case HsValueType.Brightness:
                case HsValueType.Value:
                    return Constants.ValueMapper;
                case HsValueType.Temperature:
                    return Constants.TemperatureMapper;
                case HsValueType.Hue:
                default:
                    return Constants.HueMapper;
            }
        }
        #endregion
    }
}
