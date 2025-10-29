using System;
using System.Drawing;

namespace Chizl.ColorExtension
{
    /// <include file="../docs/HSVB.xml" path='extradoc/class[@name="HSVB"]/*' />
    public readonly struct HsbSpace
    {
        /// <summary>
        /// Constructors with no parameters are not supported for netstandard2.0
        /// </summary>
        private HsbSpace(bool _)
        {
            this.Hue = 0.0;
            this.Saturation = 0.0;
            this.Brightness = 0.0;
            this.RawBrightness = 0.0;
            this.ValueString = "";
            this.ValueRaw = "";
            this.IsEmpty = true;
        }

        /// <include file="../docs/HSVB.xml" path='extradoc/class[@name="HSVB"]/interfaces/interface[@name="HSVBInt"]/*' />
        public HsbSpace(int decValue) : this(Color.FromArgb(decValue)) { }

        /// <include file="../docs/HSVB.xml" path='extradoc/class[@name="HSVB"]/interfaces/interface[@name="HSVBDoubles"]/*' />
        public HsbSpace(double h, double s, double b) : this(ColorConverterEx.HsvToColor(h, s, b)) { }

        /// <include file="../docs/HSVB.xml" path='extradoc/class[@name="HSVB"]/interfaces/interface[@name="HSVBColor"]/*' />
        public HsbSpace(Color clr)
        {
            this.IsEmpty = clr.Equals(Color.Empty);

            if (!this.IsEmpty)
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
                    hue = 0;  // grayscale
                else if (max == r)
                    hue = (g - b) / delta + (g < b ? 6 : 0);
                else if (max == g)
                    hue = (b - r) / delta + 2;
                else
                    hue = (r - g) / delta + 4;

                hue /= 6;     //normalize to 0-1
                hue *= 360;   //convert to degrees (0-360)

                //hsv calculations ---
                this.RawBrightness = max;
                double value = max * 100; //value and/or brightness
                double satur = (max == 0) ? 0 : (delta / max) * 100; //saturation

                //verify and set properties
                this.Hue = hue.SetBoundary(0.0, 360.0, 2);
                this.Saturation = satur.SetBoundary(0.0, 100.0, 2);
                this.Brightness = value.SetBoundary(0.0, 100.0, 2);                
                this.ValueString = $"HSV[H={Hue:F2}º, S={Saturation:F2}%, B={Brightness:F2}%]";
                this.ValueRaw = $"{Hue:F2}º,{Saturation:F2},{Brightness:F2}";
            }
            else
            {
                this.Hue = 0.0;
                this.Saturation = 0.0;
                this.Brightness = 0.0;
                this.RawBrightness = 0.0;
                this.ValueString = "";
                this.ValueRaw = "";
            }
        }

        #region Public Properties
        /// <include file="../docs/Shared.xml" path='extradoc/class[@name="Shared"]/properties/property[@name="Empty"]/*' />
        public static HsbSpace Empty => new HsbSpace(true);

        /// <include file="../docs/Shared.xml" path='extradoc/class[@name="Shared"]/properties/property[@name="IsEmpty"]/*' />
        public bool IsEmpty { get; }

        /// <include file="../docs/Shared.xml" path='extradoc/class[@name="Shared"]/properties/property[@name="HSVBL_Hue"]/*' />
        public double Hue { get; }

        /// <include file="../docs/HSVB.xml" path='extradoc/class[@name="HSVB"]/properties/property[@name="HSVB_Saturation"]/*' />
        public double Saturation { get; }

        /// <include file="../docs/HSVB.xml" path='extradoc/class[@name="HSVB"]/properties/property[@name="HSVB_Value"]/*' />
        public double Brightness { get; }

        /// <include file="../docs/HSVB.xml" path='extradoc/class[@name="HSVB"]/properties/property[@name="HSVB_RawValue"]/*' />
        public double RawBrightness { get; }

        /// <include file="../docs/Shared.xml" path='extradoc/class[@name="Shared"]/properties/property[@name="ValueString"]/*' />
        public string ValueString { get; }

        /// <include file="../docs/Shared.xml" path='extradoc/class[@name="Shared"]/properties/property[@name="ValueRaw"]/*' />
        public string ValueRaw { get; }
        #endregion

        #region Public Override Methods
        public override bool Equals(object obj) => obj is HsbSpace other && this.ValueString.Equals(other.ValueRaw);
        public override int GetHashCode() => this.ValueString.GetHashCode();
        public override string ToString() => this.ValueString;
        #endregion
    }
}
