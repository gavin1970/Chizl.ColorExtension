using System;
using System.Drawing;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Chizl.ColorExtension
{
    /// <include file="../docs/HEX.xml" path='extradoc/class[@name="HEX"]/*' />
    public readonly struct HEX
    {
        #region Constructor
        private HEX(bool _) 
        {
            this.AHex = string.Empty;
            this.RHex = string.Empty;
            this.GHex = string.Empty;
            this.BHex = string.Empty;
            this.A = 0;
            this.R = 0;
            this.G = 0;
            this.B = 0;
            this.ToRgbHex = string.Empty;
            this.ToRgb = 0;
            this.ColorRgb = Color.Empty;
            this.ValueRaw = string.Empty;
            this.ValueString = string.Empty;
            this.ToArgbHex = string.Empty;
            this.ToArgb = 0;
            this.ColorArgb = Color.Empty;
            this.IsWebSafe216Color = false;
            this.IsWebSafe16Color = false;
            this.IsEmpty = true;
        }

        /// <include file="../docs/HEX.xml" path='extradoc/class[@name="HEX"]/interfaces/interface[@name="HEXInt"]/*' />
        public HEX(int decValue) : this(Color.FromArgb(decValue)) { }

        /// <include file="../docs/HEX.xml" path='extradoc/class[@name="HEX"]/interfaces/interface[@name="HEXColor"]/*' />
        public HEX(Color clr) : this(clr.Equals(Color.Empty) ? "" : $"#{clr.A:X2}{clr.R:X2}{clr.G:X2}{clr.B:X2}") { }

        /// <include file="../docs/HEX.xml" path='extradoc/class[@name="HEX"]/interfaces/interface[@name="HEXString"]/*' />
        public HEX(string hex)
        {
            //force trimmed and uppercase
            var cleanHex = hex?.Trim().ToUpper();
            if (!string.IsNullOrWhiteSpace(cleanHex))
            {
                // Yes, this allows users to pass in garbage, but it also guarantee validation.
                // removing everything not related.
                cleanHex = Regex.Replace(cleanHex, "[^A-F0-9$]", "");
            }

            // if hex still exists and validated.
            if (!string.IsNullOrWhiteSpace(cleanHex) && Common.IsHex(cleanHex))
            {
                // when string passed in as an example: '#C0C'
                if (cleanHex.Length == 3)
                {
                    this.AHex = "FF";
                    this.RHex = $"{cleanHex.Substring(0, 1)}{cleanHex.Substring(0, 1)}";
                    this.GHex = $"{cleanHex.Substring(1, 1)}{cleanHex.Substring(1, 1)}";
                    this.BHex = $"{cleanHex.Substring(2, 1)}{cleanHex.Substring(2, 1)}";
                }
                // when string passed in as an example: '#CC00CC'
                else if (cleanHex.Length == 6)
                {
                    this.AHex = "FF";
                    this.RHex = $"{cleanHex.Substring(0, 2)}";
                    this.GHex = $"{cleanHex.Substring(2, 2)}";
                    this.BHex = $"{cleanHex.Substring(4, 2)}";
                }
                // when string or Color.class passed in as an example: '#FFCC00CC'
                else if (cleanHex.Length == 8)
                {
                    this.AHex = $"{cleanHex.Substring(0, 2)}";
                    this.RHex = $"{cleanHex.Substring(2, 2)}";
                    this.GHex = $"{cleanHex.Substring(4, 2)}";
                    this.BHex = $"{cleanHex.Substring(6, 2)}";
                }
                // something went wrong, this should never occur as RegEx match should of caught it, but just in case.
                else
                    throw new ArgumentException($"Parameter '{nameof(hex)}' has an invalid length.  With a value of '{hex}' ({hex.Length}) is the wrong size.  It must be 3, 6, or 8 bytes in length.");

                //get byte representing Alpha/Transparency
                this.A = byte.Parse(AHex, NumberStyles.HexNumber);
                //get byte representing Red
                this.R = byte.Parse(RHex, NumberStyles.HexNumber);
                //get byte representing Green
                this.G = byte.Parse(GHex, NumberStyles.HexNumber);
                //get byte representing Blue
                this.B = byte.Parse(BHex, NumberStyles.HexNumber);

                //setting values for only RGB
                this.ToRgbHex = $"#{RHex}{GHex}{BHex}";
                this.ToRgb = int.Parse(ToRgbHex.TrimStart('#'), NumberStyles.HexNumber);
                this.ColorRgb = Color.FromArgb(ToRgb);

                this.ValueRaw = $"{AHex}{RHex}{GHex}{BHex}";
                this.ValueString = $"HEX[#{ValueRaw}]";

                //setting values for only ARGB
                this.ToArgbHex = $"#{ValueRaw}";
                this.ToArgb = int.Parse(ToArgbHex.TrimStart('#'), NumberStyles.HexNumber);
                this.ColorArgb = Color.FromArgb(ToArgb);

                var ws216 = Constants.WebSafe216Colors;
                var ws16 = Constants.WebSafe16Colors;

                //set WebSafe properties
                this.IsWebSafe216Color = ws216.Contains(RHex) &&
                                         ws216.Contains(GHex) &&
                                         ws216.Contains(BHex);

                this.IsWebSafe16Color = ws16.Contains(RHex) &&
                                        ws16.Contains(GHex) &&
                                        ws16.Contains(BHex);

                //clean fully set model
                this.IsEmpty = false;
            }
            else
                throw new ArgumentException($"Parameter '{nameof(hex)}' has an invalid character.  Optional: #, Required: 0-9A-Za-z and must be 3, 6, or 8 bytes in length, not counting #. '{hex}' is not a hex value.");
        }
        #endregion

        #region Public Properties
        /// <include file="../docs/Shared.xml" path='extradoc/class[@name="Shared"]/properties/property[@name="Empty"]/*' />
        public static HEX Empty { get { return new HEX(true); } }

        /// <include file="../docs/Shared.xml" path='extradoc/class[@name="Shared"]/properties/property[@name="IsEmpty"]/*' />
        public bool IsEmpty { get; }

        /// <include file="../docs/HEX.xml" path='extradoc/class[@name="HEX"]/properties/property[@name="ToRgb"]/*' />
        public int ToRgb { get; }

        /// <include file="../docs/HEX.xml" path='extradoc/class[@name="HEX"]/properties/property[@name="ToArgb"]/*' />
        public int ToArgb { get; }

        /// <include file="../docs/HEX.xml" path='extradoc/class[@name="HEX"]/properties/property[@name="AHex"]/*' />
        public string AHex { get; }

        /// <include file="../docs/HEX.xml" path='extradoc/class[@name="HEX"]/properties/property[@name="A"]/*' />
        public byte A { get; }

        /// <include file="../docs/HEX.xml" path='extradoc/class[@name="HEX"]/properties/property[@name="RHex"]/*' />
        public string RHex { get; }

        /// <include file="../docs/HEX.xml" path='extradoc/class[@name="HEX"]/properties/property[@name="R"]/*' />
        public byte R { get; }

        /// <include file="../docs/HEX.xml" path='extradoc/class[@name="HEX"]/properties/property[@name="GHex"]/*' />
        public string GHex { get; }

        /// <include file="../docs/HEX.xml" path='extradoc/class[@name="HEX"]/properties/property[@name="G"]/*' />
        public byte G { get; } 

        /// <include file="../docs/HEX.xml" path='extradoc/class[@name="HEX"]/properties/property[@name="BHex"]/*' />
        public string BHex { get; } 

        /// <include file="../docs/HEX.xml" path='extradoc/class[@name="HEX"]/properties/property[@name="B"]/*' />
        public byte B { get; } 

        /// <include file="../docs/HEX.xml" path='extradoc/class[@name="HEX"]/properties/property[@name="ToRgbHex"]/*' />
        public string ToRgbHex { get; } 

        /// <include file="../docs/HEX.xml" path='extradoc/class[@name="HEX"]/properties/property[@name="ToArgbHex"]/*' />
        public string ToArgbHex { get; } 

        /// <include file="../docs/HEX.xml" path='extradoc/class[@name="HEX"]/properties/property[@name="IsWebSafeColor"]/*' />
        public bool IsWebSafeColor => IsWebSafe216Color || IsWebSafe16Color;

        /// <include file="../docs/HEX.xml" path='extradoc/class[@name="HEX"]/properties/property[@name="IsWebSafe216Color"]/*' />
        public bool IsWebSafe216Color { get; }

        /// <include file="../docs/HEX.xml" path='extradoc/class[@name="HEX"]/properties/property[@name="IsWebSafe16Color"]/*' />
        public bool IsWebSafe16Color { get; }

        /// <include file="../docs/Shared.xml" path='extradoc/class[@name="Shared"]/properties/property[@name="ValueString"]/*' />
        public string ValueString { get; }

        /// <include file="../docs/Shared.xml" path='extradoc/class[@name="Shared"]/properties/property[@name="ValueRaw"]/*' />
        public string ValueRaw { get; }

        /// <include file="../docs/Shared.xml" path='extradoc/class[@name="Shared"]/properties/property[@name="ToColor"]/*' />
        public Color ColorRgb { get; }

        /// <include file="../docs/Shared.xml" path='extradoc/class[@name="Shared"]/properties/property[@name="ToColor"]/*' />
        public Color ColorArgb { get; }
        #endregion

        #region Public Override Methods
        public override bool Equals(object obj) => obj is HEX other && ToArgbHex.Equals(other.ToArgbHex);
        public override int GetHashCode() => ToArgbHex.GetHashCode();
        public override string ToString() => ToArgbHex;
        #endregion
    }
}
