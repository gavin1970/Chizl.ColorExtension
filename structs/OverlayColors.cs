using System.Drawing;

namespace Chizl.ColorExtension
{
    /// <include file="../docs/OverlayColors.xml" path='extradoc/class[@name="OverlayColors"]/*' />
    public readonly struct OverlayColors
    {
        private OverlayColors(bool _) 
        {
            this.FGColor = Color.Empty;
            this.BGColor = Color.Empty;
            this.MergedColor = Color.Empty;
            this.ValueStringV6 = string.Empty;
            this.ValueStringV8 = string.Empty;
            this.IsEmpty = true;
        }

        /// <include file="../docs/OverlayColors.xml" path='extradoc/class[@name="OverlayColors"]/interfaces/interface[@name="ColorColor"]/*' />
        internal OverlayColors(Color fgColor, Color bgColor)
        {
            if (!fgColor.IsEmpty && !bgColor.IsEmpty)
            {
                byte r;
                byte g;
                byte b;

                //set properties based arguments
                this.FGColor = fgColor;
                this.BGColor = bgColor;

                if (fgColor.A.Equals(255))
                {
                    r = fgColor.R;
                    g = fgColor.G;
                    b = fgColor.B;
                }
                else
                {
                    //normalize alpha
                    double alpha = fgColor.A / 255.0;

                    //get overlay foreground with Alpha on top of background to create new R, G, and B values
                    var Rr = (alpha * fgColor.R) + ((1 - alpha) * bgColor.R);
                    var Gr = (alpha * fgColor.G) + ((1 - alpha) * bgColor.G);
                    var Br = (alpha * fgColor.B) + ((1 - alpha) * bgColor.B);

                    //round or force results to 0-255 range.
                    r = (byte)Rr.SetBoundary(0, 255, 0);
                    g = (byte)Gr.SetBoundary(0, 255, 0);
                    b = (byte)Br.SetBoundary(0, 255, 0);

                }

                //build color from new RGB
                this.MergedColor = Color.FromArgb(r, g, b);

                //setup string that can be pulled for display.
                this.ValueStringV6 = $"[R={r}, G={g}, B={b}]";
                this.ValueStringV8 = $"[A=255, R={r}, G={g}, B={b}]";

                //this object is no longer considered Empty.
                this.IsEmpty = false;
            }
            else
            {
                this.FGColor = Color.Empty;
                this.BGColor = Color.Empty;
                this.MergedColor = Color.Empty;
                this.ValueStringV6 = string.Empty;
                this.ValueStringV8 = string.Empty;
                this.IsEmpty = true;
            }
        }

        #region Public Properties
        /// <include file="../docs/OverlayColors.xml" path='extradoc/class[@name="OverlayColors"]/properties/property[@name="Empty"]/*' />
        public static OverlayColors Empty { get; } = new OverlayColors(true);

        /// <include file="../docs/OverlayColors.xml" path='extradoc/class[@name="OverlayColors"]/properties/property[@name="IsEmpty"]/*' />
        public bool IsEmpty { get; }

        /// <include file="../docs/OverlayColors.xml" path='extradoc/class[@name="OverlayColors"]/properties/property[@name="FGColor"]/*' />
        public Color FGColor { get; }

        /// <include file="../docs/OverlayColors.xml" path='extradoc/class[@name="OverlayColors"]/properties/property[@name="BGColor"]/*' />
        public Color BGColor { get; }

        /// <include file="../docs/OverlayColors.xml" path='extradoc/class[@name="OverlayColors"]/properties/property[@name="MergedColor"]/*' />
        public Color MergedColor { get; }

        /// <include file="../docs/OverlayColors.xml" path='extradoc/class[@name="OverlayColors"]/properties/property[@name="ValueStringV6"]/*' />
        public string ValueStringV6 { get; }

        /// <include file="../docs/OverlayColors.xml" path='extradoc/class[@name="OverlayColors"]/properties/property[@name="ValueStringV8"]/*' />
        public string ValueStringV8 { get; }
        #endregion

        #region Public Override Methods
        public override bool Equals(object obj) => obj is OverlayColors other && ValueStringV6.Equals(other.ValueStringV6);
        public override int GetHashCode() => ValueStringV6.GetHashCode();
        public override string ToString() => ValueStringV6;
        #endregion
    }
}