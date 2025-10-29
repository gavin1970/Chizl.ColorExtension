using System;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Diagnostics;
using Chizl.ColorExtension;
using System.Collections.Generic;
using System.Security.AccessControl;

namespace NET8Demo
{
    internal class Program
    {
        static readonly string _darkGrayBG = Color.FromArgb(48, 48, 48).BGAscii();
        static readonly string _whiteBG = Color.White.BGAscii();
        static readonly string _blackBG = Color.FromArgb(0, 0, 0).BGAscii();
        static readonly string _whiteFG = Color.FromArgb(255, 255, 255).FGAscii();
        static readonly string _yellowFG = Color.FromArgb(255, 255, 0).FGAscii();
        static readonly string _blackFG = Color.FromArgb(0, 0, 0).FGAscii();
        static readonly string _reset = Color.Transparent.ResetAscii();
        static readonly string _clearBuffer = "\u001bc\x1b[3J";         //clears screen and console buffer

        static void Main(string[] args)
        {
            var titleBgColor = Color.Wheat;
            var titleFgColor = Color.Black;
            var windowBgColor = Color.DarkRed;
            var windowFgColor = Color.White;
            var exitKey = ConsoleKey.Escape;

            var c = Color.White; // Pure Red
            var c2 = Color.FromArgb(c.ToArgb());
            Console.WriteLine($"{c}\t: {c2}");
            Console.WriteLine($"Complementary\t: {c.GetComplementary()}");
            if (c.GetContrast(out Color bestColor, out double ratio))
                Console.WriteLine($"Contrast\t: {bestColor} (Ratio: {ratio:0.000}:1)");

            ReadKey();

            var menuItems = new string[5] 
            { 
                "Display Color Wheel w/ hue name", 
                "Transparency Color for new Color", 
                "CMYK to Color w/ Rules", 
                "Testing Record Class (just test)",
                "Build XML Help Docs"
            };

            var menu = BuildMenu("Make Selection", menuItems, 
                                titleBgColor, titleFgColor,
                                windowBgColor, windowFgColor,
                                exitKey);

            DisplayMenu(menu, exitKey, true);
        }
        static bool DisplayMenu(string[] menuBuffer, ConsoleKey exitKey, bool hCenter = false)
        {
            var retVal = false;
            var consoleWidth = 0;
            var orgMenuBuffer = (string[])menuBuffer.Clone();
            var pad = "";

            if (menuBuffer.Length.Equals(0))
                return retVal;

            var key = ConsoleKey.None;
            while (key != exitKey)
            {
                if (hCenter && !consoleWidth.Equals(Console.WindowWidth))
                {
                    consoleWidth = Console.WindowWidth;
                    var menuWidth = orgMenuBuffer[0].RemoveAsciiEscape(true, true, false).Length;
                    pad = new string(' ', ((consoleWidth / 2) - (menuWidth / 2)));
                }

                foreach (string line in menuBuffer)
                    Console.WriteLine($"{pad}{line}");

                key = ReadKey();

                switch (key)
                {
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:
                        //DumpHueToneModifier();
                        DisplayAllHue();
                        break;
                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        TestBlendMerge(Color.FromArgb(127, 255, 0, 0), Color.FromArgb(255, 0, 128, 0), _whiteFG, _whiteBG, _blackBG);
                        TestBlendMerge(Color.FromArgb(127, 220, 211, 136), Color.FromArgb(255, 0, 128, 0), _blackFG, _darkGrayBG, _blackBG);
                        break;
                    case ConsoleKey.D3:
                    case ConsoleKey.NumPad3:
                        CMYK2Color();
                        break;
                    case ConsoleKey.D4:
                    case ConsoleKey.NumPad4:
                        TestingRecord();
                        break;
                    case ConsoleKey.D5:
                    case ConsoleKey.NumPad5:
                        XMLDocBuilder.CreateDocs(false);    //not working with merge, so leaving it false for now.
                        break;
                }
            }

            return key.Equals(exitKey);
        }
        static string[] BuildMenu(string title, string[] selections,
            Color bgTitleColor, Color fgTitleColor,
            Color bgWindowColor, Color fgWindowColor, ConsoleKey exitKey)
        {
            if (string.IsNullOrWhiteSpace(title) || selections == null || selections.Length.Equals(0))
                return new string[0];
            StringBuilder sb = new StringBuilder();
            title = $" {title.Trim()} ";
            var maxSize = title.Length;

            var itemNo = 0;

            foreach (var sel in selections)
                maxSize = Math.Max(maxSize, $" {itemNo++} - {sel} ".Length);

            var space = new string(' ', maxSize - title.Length);
            var win = $"{bgWindowColor.BGAscii()}{fgWindowColor.FGAscii()}";
            var head = $"{bgTitleColor.BGAscii()}{fgTitleColor.FGAscii()}{title}{space}";
            var bar = new string('═', maxSize);

            sb.Append($"{win}╒{bar}╕{_reset}\n");
            sb.Append($"{win}│{head}{win}│{_reset}\n");
            sb.Append($"{win}╞{bar}╡{_reset}\n");

            itemNo = 0;
            foreach (var sel in selections)
            {
                var selection = $" {itemNo++} - {sel} ";
                space = new string(' ', maxSize - selection.Length);

                //reformat with color, because we don't want
                //to calc length with those strings within.
                selection = $" {_yellowFG}{itemNo}{win} - {sel} ";

                sb.Append($"{win}│{selection}{space}│{_reset}\n");
            }

            var exit = $" {exitKey.Name()} - Exit ";
            space = new string(' ', maxSize - exit.Length);

            //reformat with color, because we don't want
            //to calc length with those strings within.
            exit = $" {_yellowFG}{exitKey.Name()}{win} - Exit{space} ";

            sb.Append($"{win}│{exit}│{_reset}\n");
            sb.Append($"{win}╘{bar}╛{_reset}\n");

            //reason, I did sb.Append and used \n so I didn't have to remove them, for the split.
            //so this should never occure.
            Debug.Assert(sb.ToString().IndexOf('\r') == -1);

            return sb.ToString().Split('\n');
        }

        static void CMYKandBack()
        {
            var clr = Color.FromArgb(-3932476);
            var cmyk = clr.ToCMYK();
            Console.WriteLine($"The following is to illistrate the use of Key vs RawKey property for CMYK to Color.\n" +
                              $"In a lot of cases standard conversions work with Key, but in some they don't.\n" +
                              $"This is because decimal point precision are lost when we round, but isn't when\n" +
                              $"preserving the original key before rouding. By using the CMYK object, it does this by default.\n");
            Console.WriteLine($"Org Color: {clr}");
            Console.WriteLine($"     Full: {cmyk}  (RawKey: {cmyk.RawKey})\n");
            Console.WriteLine(new string('-', 100));
            Console.WriteLine($"    Color: {clr}");
            Console.WriteLine($"   w/ Key: {ColorConverterEx.CmykToColor(cmyk.Cyan, cmyk.Magenta, cmyk.Yellow, cmyk.Key)}");
            Console.WriteLine($"w/ RawKey: {ColorConverterEx.CmykToColor(cmyk.Cyan, cmyk.Magenta, cmyk.Yellow, cmyk.RawKey)}");
            Console.WriteLine($" CMYK Obj: {ColorConverterEx.CmykToColor(cmyk)}");

            ReadKey();
        }
        static Color ConvertCmykToRgb(double c, double m, double y, double k)
        {
            // Validate and normalize CMYK values to the 0-1 range
            if (c < 0 || c > 100 || m < 0 || m > 100 || y < 0 || y > 100 || k < 0 || k > 100)
                throw new ArgumentException("CMYK values must be between 0 and 100.");

            // Ensure CMYK values are in the range of 0-1
            if (c > 1)
                c /= 100.0;
            if (m > 1)
                m /= 100.0;
            if (y > 1)
                y /= 100.0;
            if (k > 1)
                k /= 100.0;

            var r = Convert.ToInt32(255 * (1 - c) * (1 - k));
            var g = Convert.ToInt32(255 * (1 - m) * (1 - k));
            var b = Convert.ToInt32(255 * (1 - y) * (1 - k));

            // Ensure RGB values are within the valid range (0-255)
            r = Math.Max(0, Math.Min(255, r));
            g = Math.Max(0, Math.Min(255, g));
            b = Math.Max(0, Math.Min(255, b));

            return Color.FromArgb(r, g, b);
        }
        static void CMYK2Color()
        {
            CMYKandBack();
            var colorList = new List<Color>() { new HEX("#C3FEC4").ColorArgb, new HEX("#474300").ColorArgb, Color.Azure, Color.DarkTurquoise, Color.Firebrick };

            foreach (var clr in colorList)
            {
                var cmyk = clr.ToCMYK();
                var bgColor = clr.IsDark() ? Color.White.BGAscii() : Color.Black.BGAscii();
                var name = clr.Name.IsHex() ? clr.Name.ToUpper() : clr.Name;
                Console.WriteLine($"{clr.FGAscii()}{bgColor}[ ==== This is {name} ====]{clr.ResetAscii()}");
                ShowColor(ColorConverterEx.CmykToColor(cmyk), true, true, true);
            }
        }
        static bool ShowColor(Color color, bool showAbout, bool wait4Key, bool breakLine)
        {
            bool retVal = false;

            var bgFgAscii = $"{color.BGAscii()}{(color.IsDark() ? _whiteFG : _blackFG)}";

            if (showAbout)
                Console.WriteLine($"{color.About(true).Replace("\n", "\n\t")}");
            else
            {
                var name = color.Name;// color.HueName();
                name = $"{name.Replace(" ", "")}";
                var disp = $"{name}{(new string(' ', 25 - name.Length))}";
                Console.Write($"{bgFgAscii}{disp}{color.ResetAscii()}");

                if(breakLine)
                    Console.WriteLine();    
            }

            if (wait4Key)
                retVal = ReadKey(false) == ConsoleKey.Escape;

            return retVal;
        }
        static void DumpHueToneModifier()
        {
            Console.OutputEncoding = Encoding.UTF8;
            for (int h = 0; h < 360; h += 10)
            {
                for (int s = 0; s <= 100; s += 10)
                {
                    for (int v = 0; v <= 100; v += 10)
                    {
                        var color = ColorConverterEx.HsvToColor(h, s / 100.0, v / 100.0);
                        var tone = color.GetTone();

                        Console.WriteLine($"HSV({h},{s},{v}) → {tone.Modifier}");
                    }
                }
            }
        }
        static void DisplayAllHue()
        {
            bool showAbout = false;
            bool showColorWheel = true;
            int recordDisplay = 60;
            int columnWidth = 5;

            if (showAbout)
                recordDisplay /= 10;

            for (int i = 0; i <= 360; i++)
            {
                if (i > 1 && ((i - 1) % recordDisplay) == 0)
                {
                    Console.WriteLine("\nPress 'Esc' to exit.  Press any other key to continue.");
                    if (ReadKey(false).Equals(ConsoleKey.Escape))
                        return;
                }

                int j = i > 255 ? 255 : i;
                int k = i > 255 ? i - 255 : 0;

                Color color = showColorWheel ? ColorConverterEx.HsvToColor(i, 1.0, 1.0) : Color.FromArgb(255, j, k);

                //ColorConverterEx.HsvToRgb
                //var hex = color.ToHexRgb();
                var bgFgAscii = $"{color.BGAscii()}{(color.IsDark() ? _whiteFG : _blackFG)}";
                if (showAbout)
                    Console.WriteLine($"{bgFgAscii} {i}: {color.ResetAscii()} {color.About(true).Replace("\n", "\n")}");
                else
                {
                    var hueName = color.HueName();
                    //var hueName = color.GetTone().Modifier;//color.HueName();
                    hueName = $" {i}: {hueName.Replace(" ", "")}";
                    var disp = $"{hueName}{(new string(' ', 25 - hueName.Length))}";
                    Console.Write($"{bgFgAscii}{disp}{color.ResetAscii()}");

                    if ((i % columnWidth) == 0)
                        Console.WriteLine();    
                }
            }

            Console.WriteLine("\nPress any key to exit.");
            Console.CursorVisible = false;
            ReadKey(false);
        }
        static void TestBlendMerge(Color fgColor, Color bgColor, string argFGColor, string argBGColor1, string argBGColor2)
        {
            Console.Clear();
            var maxLen = -1;
            // ------------------------------------------------------------
            var fg = fgColor.Details();
            var fgHex = fg.HEX;
            var fgHsv = fg.HSV;
            var fgHsl = fg.HSL;
            var fgCMYK = fg.CMYK;
            var fgLAB = fg.LAB;
            var fgLCH = fg.LCH;
            var fgLUV = fg.LUV;
            var fgXYZ = fg.XYZ;
            var fgAsciiBG = fg.BGAscii;
            // ------------------------------------------------------------
            var bg = bgColor.Details();
            var bgHex = bg.HEX;
            var bgHsv = bg.HSV;
            var bgHsl = bg.HSL;
            var bgCMYK = bg.CMYK;
            var bgLAB = bg.LAB;
            var bgLCH = bg.LCH;
            var bgLUV = bg.LUV;
            var bgXYZ = bg.XYZ;
            var bgAsciiBG = bg.BGAscii;
            // ------------------------------------------------------------
            var blendColors = fgColor.ApplyBgColor(bgColor);
            var bldclr = blendColors.Details();
            var blendHex = bldclr.HEX;
            var blendHsv = bldclr.HSV;
            var blendHsl = bldclr.HSL;
            var blendCMYK = bldclr.CMYK;
            var blendLAB = bldclr.LAB;
            var blendLCH = bldclr.LCH;
            var blendLUV = bldclr.LUV;
            var blendXYZ = bldclr.XYZ;
            var blendAsciiBG = bldclr.BGAscii;
            // ------------------------------------------------------------
            var fgName = fgColor.IsNamedColor ? fgColor.Name : $"#{fgColor.Name.ToUpper()}";
            var bgName = bgColor.IsNamedColor ? bgColor.Name : $"#{bgColor.Name.ToUpper()}";
            var blName = blendColors.IsNamedColor ? blendColors.Name : $"#{blendColors.Name.ToUpper()}";

            var fgStr = $"Foreground: Name[{fgName}]\n" +
                        $"Argb[A:{fgColor.A},R:{fgColor.R},G:{fgColor.G},B:{fgColor.B}], " +
                        $"Rgb[R:{fgColor.R},G:{fgColor.G},B:{fgColor.B}]\n" +
                        $"ArgbDec[{fgColor.ToArgb()}], RgbDec[{fgColor.ToRgb()}]\n" +
                        $"ArgbHex[{fgHex.ToArgbHex}], RgbHex[{fgHex.ToRgbHex}]\n" +
                        $"Temperature[{fg.Temperature}], Tone[{fg.Tone}]\n" +
                        $"{fgCMYK}\n" +
                        $"{fgHsv}\n" +
                        $"{fgHsl}\n" +
                        $"{fgLAB}\n" +
                        $"{fgLCH}\n" +
                        $"{fgLUV}\n" +
                        $"{fgXYZ}";

            var bgStr = $"Background: Name[{bgName}]\n" +
                        $"Argb[A:{bgColor.A},R:{bgColor.R},G:{bgColor.G},B:{bgColor.B}], " +
                        $"Rgb[R:{bgColor.R},G:{bgColor.G},B:{bgColor.B}]\n" +
                        $"ArgbDec[{bgColor.ToArgb()}], RgbDec[{bgColor.ToRgb()}]\n" +
                        $"ArgbHex[{bgHex.ToArgbHex}], RgbHex[{bgHex.ToRgbHex}]\n" +
                        $"Temperature[{bg.Temperature}], Tone[{bg.Tone}]\n" +
                        $"{bgCMYK}\n" +
                        $"{bgHsv}\n" +
                        $"{bgHsl}\n" +
                        $"{bgLAB}\n" +
                        $"{bgLCH}\n" +
                        $"{bgLUV}\n" +
                        $"{bgXYZ}";

            var blStr = $"Blended: Name[{blName}]\n" +
                        $"Argb[A:{blendColors.A},R:{blendColors.R},G:{blendColors.G},B:{blendColors.B}], " +
                        $"Rgb[R:{blendColors.R},G:{blendColors.G},B:{blendColors.B}]\n" +
                        $"ArgbDec[{blendColors.ToArgb()}], RgbDec[{blendColors.ToRgb()}]\n" +
                        $"ArgbHex[{blendHex.ToArgbHex}], RgbHex[{blendHex.ToRgbHex}]\n" +
                        $"Temperature[{bldclr.Temperature}], Tone[{bldclr.Tone}]\n" +
                        $"{blendCMYK}\n" +
                        $"{blendHsv}\n" +
                        $"{blendHsl}\n" +
                        $"{blendLAB}\n" +
                        $"{blendLCH}\n" +
                        $"{blendLUV}\n" +
                        $"{blendXYZ}";

            maxLen = GetMaxLength(fgStr, maxLen, out _);
            maxLen = GetMaxLength(bgStr, maxLen, out _);
            maxLen = GetMaxLength(blStr, maxLen, out _);

            BoxWriteLine(fgStr, argFGColor, fgAsciiBG, true, 2, maxLen);
            BoxWriteLine(bgStr, argFGColor, bgAsciiBG, false, 2, maxLen);
            BoxWriteLine(blStr, argFGColor, blendAsciiBG, false, 2, maxLen);

            ReadKey(false);
            Console.Clear();
        }
        static void BoxWriteLine(string multiLineStr, string fgColor, string bgColor, bool headerBar = true, int padCnt = 2, int maxSize = -1)
        {
            var maxLen = GetMaxLength(multiLineStr, maxSize, out string[] lines);
            if(maxLen < maxSize)
                maxLen = maxSize;

            var colors = $"{fgColor}{bgColor}";
            var padding = new string(' ', padCnt);
            var sep = $"{_reset}{padding}{new string('-', maxLen)}{padding}";

            if (headerBar) 
                Console.WriteLine($"{sep}{colors}");
            else
                Console.Write($"{colors}");

            foreach (var line in lines)
            {
                var addSpace = line.Length < maxLen ? new string(' ', maxLen - line.Length) : "";
                Console.WriteLine($"{padding}{line}{addSpace}{padding}");
            }

            Console.WriteLine($"{sep}");
        }
        static int GetMaxLength(string multiLineStr, int maxSize, out string[] lines)
        {
            lines = multiLineStr.Replace("\r", "").Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines)
            {
                if (line.Length > maxSize)
                    maxSize = line.Length;
            }

            return maxSize;
        }
        static ConsoleKey ReadKey(bool showCursor = false)
        {
            Console.CursorVisible = showCursor;
            var retVal = Console.ReadKey(true).Key;
            Console.Write(_clearBuffer);
            return retVal;
        }

        /// <summary>
        /// Just playing with Record class, which I've never used before.  
        /// Interesting, but I believe the standard List or Array is faster.
        /// </summary>
        static void TestingRecord()
        {
            ColorDatabase db = new ColorDatabase();

            var keys = string.Join(", ", db.GetKeys());

            Console.WriteLine("Welcome to the Fruit Color Query System!");
            Console.WriteLine($"Enter fruit name (e.g., {keys}) or 'quit' to exit.");

            while (true)
            {
                Console.Write("Enter fruit name: ");
                var itemName = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(itemName) || itemName.ToLower() == "quit")
                    break;


                var items = string.Join(", ", db.GetVariations(itemName));
                Console.Write($"Enter variation for {itemName} (e.g., {items}, or leave blank for default/all): ");
                var variationName = Console.ReadLine();

                var resp = db.GetItemColorInfo(itemName, string.IsNullOrWhiteSpace(variationName) ? null : variationName);
                var name = resp.Item1; 
                var info = resp.Item2;

                if (info != null)
                {
                    Console.WriteLine($"\n--- Color Info: {name} ---");
                    Console.WriteLine($"Hex: {info.Hex}");
                    Console.WriteLine($"HSL: H:{info.Hsl.Hue}, S:{info.Hsl.Saturation}%, L:{info.Hsl.Lightness}%");
                    Console.WriteLine($"Description: {info.Description}");
                    Console.WriteLine($"Hue Range: {info.HueRange.Min} to {info.HueRange.Max} degrees");
                    Console.WriteLine("-------------------\n");

                    //others within the same hue..
                    var others = db.GetColorByDegree(info.Hsl.Hue);
                    foreach(var other in others)
                    {
                        if (info.Hex.ToUpper().Equals(other.Hex.ToUpper()))
                            continue;

                        Console.WriteLine($"--- {other.Hsl.Hue.HueName()} ---");
                        Console.WriteLine($"Hex: {other.Hex}");
                        Console.WriteLine($"HSL: H:{other.Hsl.Hue}, S:{other.Hsl.Saturation}%, L:{other.Hsl.Lightness}%");
                        Console.WriteLine($"Description: {other.Description}");
                        Console.WriteLine($"Hue Range: {other.HueRange.Min} to {other.HueRange.Max} degrees");
                        Console.WriteLine("-------------------\n");
                    }

                    Console.WriteLine();
                }
                else if (string.IsNullOrWhiteSpace(variationName)) // If no variation was specified and the default lookup also failed
                {
                    var allVariations = db.GetAllVariationsForItem(itemName);
                    if (allVariations != null && allVariations.Any()) // Using System.Linq.Any()
                    {
                        Console.WriteLine($"\n--- All Variations for {itemName.ToUpper()} ---");
                        foreach (var kvp in allVariations)
                            Console.WriteLine($"  {kvp.Key.Replace("_", " ").PadRight(20)}: Hex: {kvp.Value.Hex}, HSL: H:{kvp.Value.Hsl.Hue}, S:{kvp.Value.Hsl.Saturation}%, L:{kvp.Value.Hsl.Lightness}%");
                        Console.WriteLine("-------------------------------------------\n");
                    }
                    else
                    {
                        // Error message already printed by GetItemColorInfo
                    }
                }
            }
            Console.WriteLine("Exiting color query system. Goodbye!");
        }
    }
}
