using System.Drawing;

namespace Chizl.ColorExtension
{
    public static class ColorAscii
    {
        static ColorAscii()
        {
            Blue = Color.FromArgb(0, 0, 255).FGAscii();                             // #0000FF
            Cyan = Color.FromArgb(0, 255, 255).FGAscii();                           // #00FFFF
            DarkGreen = Color.FromArgb(5, 73, 7).FGAscii();                         // #054907
            Green = Color.FromArgb(0, 255, 0).FGAscii();                            // #00FF00
            LuminousVividAzure = Color.FromArgb(0, 128, 255).FGAscii();             // #0080FF
            LuminousVividChartreuseGreen = Color.FromArgb(128, 255, 0).FGAscii();   // #80FF00
            LuminousVividSpringGreen = Color.FromArgb(0, 255, 128).FGAscii();       // #00FF80
            Maroon = Color.FromArgb(128, 0, 0).FGAscii();                           // #800000
            NavyBlue = Color.FromArgb(0, 0, 128).FGAscii();                         // #0000FF
            Orange = Color.FromArgb(255, 128, 0).FGAscii();                         // #FF8000
            Red = Color.FromArgb(255, 0, 0).FGAscii();                              // #FF0000
            Yellow = Color.FromArgb(255, 255, 0).FGAscii();                         // #FFFF00
        }

        public static string Blue { get; }
        public static string Cyan { get; }
        public static string DarkGreen { get; }
        public static string Green { get; }
        public static string LuminousVividAzure { get; }
        public static string LuminousVividChartreuseGreen { get; }
        public static string LuminousVividSpringGreen { get; }
        public static string Maroon { get; }
        public static string NavyBlue { get; }
        public static string Orange { get; }
        public static string Red { get; }
        public static string Yellow { get; }
    }
}
