using System;
using System.Drawing;

namespace Chizl.ColorExtension
{

    /// <include file="../docs/CmykSpace.xml" path='extradoc/class[@name="CmykSpace"]/*' />
    public readonly struct CmykSpace
    {
        /// <summary>
        /// Constructors with no parameters are not supported for netstandard2.0
        /// </summary>
        private CmykSpace(bool _)
        {
            this.Cyan = 0.0;
            this.Magenta = 0.0;
            this.Yellow = 0.0;
            this.Key = 0.0;
            this.RawKey = 0.0;
            this.Modifier = CmykRule.Empty;
            this.ValueString = "";
            this.ValueRaw = "";
            this.IsEmpty = true;
        }

        /// <include file="../docs/CmykSpace.xml" path='extradoc/class[@name="CmykSpace"]/interfaces/interface[@name="CmykSpaceInt"]/*' />
        public CmykSpace(int decValue) : this(Color.FromArgb(decValue)) { }

        /// <include file="../docs/CmykSpace.xml" path='extradoc/class[@name="CmykSpace"]/interfaces/interface[@name="CmykSpaceDoubles"]/*' />
        public CmykSpace(double c, double m, double y, double k) : this(ColorConverterEx.CmykToColor(c, m, y, k)) { }

        /// <include file="../docs/CmykSpace.xml" path='extradoc/class[@name="CmykSpace"]/interfaces/interface[@name="CmykSpaceColor"]/*' />
        public CmykSpace(Color clr)
        {
            this.IsEmpty = clr.Equals(Color.Empty);

            if (!this.IsEmpty)
            {
                var r = clr.R / 255.0;
                var g = clr.G / 255.0;
                var b = clr.B / 255.0;

                this.RawKey = 1.0 - Math.Max(r, Math.Max(g, b));
                this.Key = this.RawKey;

                //can not divid by 0, so zero it all out.
                if ((1.0 - this.Key) <= 0.00001)
                {
                    this.Cyan = 0;
                    this.Magenta = 0;
                    this.Yellow = 0;
                }
                else
                {
                    this.Cyan = Math.Round((1.0 - r - this.Key) / (1.0 - this.Key) * 100, 2);
                    this.Magenta = Math.Round((1.0 - g - this.Key) / (1.0 - this.Key) * 100, 2);
                    this.Yellow = Math.Round((1.0 - b - this.Key) / (1.0 - this.Key) * 100, 2);
                    this.Key = Math.Round(this.RawKey * 100, 2);
                }

                this.Modifier = Constants.GetCMYKModifier(Cyan, Magenta, Yellow, Key);
                this.ValueString = $"CMYK[C={this.Cyan}%, M={this.Magenta}%, Y={this.Yellow}%, K={this.Key}%]";
                this.ValueRaw = $"{this.Cyan},{this.Magenta},{this.Yellow},{this.Key}";
            }
            else
            {
                this.Cyan = 0.0;
                this.Magenta = 0.0;
                this.Yellow = 0.0;
                this.Key = 0.0;
                this.RawKey = 0.0;
                this.Modifier = CmykRule.Empty;
                this.ValueString = "";
                this.ValueRaw = "";
            }
        }

        #region Public Properties
        /// <include file="../docs/Shared.xml" path='extradoc/class[@name="Shared"]/properties/property[@name="Empty"]/*' />
        public static CmykSpace Empty => new CmykSpace(true);

        /// <include file="../docs/Shared.xml" path='extradoc/class[@name="Shared"]/properties/property[@name="IsEmpty"]/*' />
        public bool IsEmpty { get; }

        /// <include file="../docs/CmykSpace.xml" path='extradoc/class[@name="CmykSpace"]/properties/property[@name="Cyan"]/*' />
        public double Cyan { get; }

        /// <include file="../docs/CmykSpace.xml" path='extradoc/class[@name="CmykSpace"]/properties/property[@name="Magenta"]/*' />
        public double Magenta { get; }

        /// <include file="../docs/CmykSpace.xml" path='extradoc/class[@name="CmykSpace"]/properties/property[@name="Yellow"]/*' />
        public double Yellow { get; }

        /// <include file="../docs/CmykSpace.xml" path='extradoc/class[@name="CmykSpace"]/properties/property[@name="Key"]/*' />
        public double Key { get; }

        /// <include file="../docs/CmykSpace.xml" path='extradoc/class[@name="CmykSpace"]/properties/property[@name="RawKey"]/*' />
        public double RawKey { get; }

        /// <include file="../docs/CmykSpace.xml" path='extradoc/class[@name="CmykSpace"]/properties/property[@name="Modifier"]/*' />
        public CmykRule Modifier { get; }

        /// <include file="../docs/Shared.xml" path='extradoc/class[@name="Shared"]/properties/property[@name="ValueString"]/*' />
        public string ValueString { get; }
        
        /// <include file="../docs/Shared.xml" path='extradoc/class[@name="Shared"]/properties/property[@name="ValueRaw"]/*' />
        public string ValueRaw { get; }
        #endregion

        #region Public Override Methods
        public override bool Equals(object obj) => obj is CmykSpace other && this.ValueString.Equals(other.ValueRaw);
        public override int GetHashCode() => this.ValueString.GetHashCode();
        public override string ToString() => this.ValueString;
        #endregion
    }
}
