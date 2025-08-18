using System;
using System.Drawing;

namespace Chizl.ColorExtension
{
    /// <include file="../docs/LCH.xml" path='extradoc/class[@name="LCH"]/*' />
    public readonly struct LchSpace
    {
        /// <summary>
        /// Constructors with no parameters are not supported for netstandard2.0
        /// </summary>
        private LchSpace(bool _)
        {
            this.L = 0.0;
            this.C = 0.0;
            this.H = 0.0;
            this.ValueString = "";
            this.ValueRaw = "";
            this.IsEmpty = true;
        }

        /// <include file="../docs/LCH.xml" path='extradoc/class[@name="LCH"]/interfaces/interface[@name="LCHInt"]/*' />
        public LchSpace(int decValue) : this(Color.FromArgb(decValue)) { }

        /// <include file="../docs/LCH.xml" path='extradoc/class[@name="LCH"]/interfaces/interface[@name="LCHColor"]/*' />
        public LchSpace(Color clr) : this(new LabSpace(clr)) { }

        /// <include file="../docs/LCH.xml" path='extradoc/class[@name="LCH"]/interfaces/interface[@name="LCHLAB"]/*' />
        public LchSpace(LabSpace lab)
        {
            var h = Math.Atan2(lab.B, lab.A);

            // convert from radians to degrees
            if (h > 0)
                h = (h / Math.PI) * 180.0;
            else
                h = 360 - (Math.Abs(h) / Math.PI) * 180.0;

            if (h < 0)
                h += 360.0;
            else if (h >= 360)
                h -= 360.0;

            this.L = lab.L;
            this.C = Math.Sqrt(lab.A * lab.A + lab.B * lab.B);
            this.H = h;

            this.ValueString = $"LCH[L={L:F2}, C={C:F2}, H={H:F2}]";
            this.ValueRaw = $"{L:F2},{C:F2},{H:F2}";
            this.IsEmpty = false;
        }

        #region Public Properties
        /// <include file="../docs/Shared.xml" path='extradoc/class[@name="Shared"]/properties/property[@name="Empty"]/*' />
        public static LchSpace Empty => new LchSpace(true);

        /// <include file="../docs/Shared.xml" path='extradoc/class[@name="Shared"]/properties/property[@name="IsEmpty"]/*' />
        public bool IsEmpty { get; }

        /// <include file="../docs/LCH.xml" path='extradoc/class[@name="LCH"]/properties/property[@name="L"]/*' />
        public double L { get; }

        /// <include file="../docs/LCH.xml" path='extradoc/class[@name="LCH"]/properties/property[@name="C"]/*' />
        public double C { get; }

        /// <include file="../docs/LCH.xml" path='extradoc/class[@name="LCH"]/properties/property[@name="H"]/*' />
        public double H { get; }

        /// <include file="../docs/Shared.xml" path='extradoc/class[@name="Shared"]/properties/property[@name="ValueString"]/*' />
        public string ValueString { get; }

        /// <include file="../docs/Shared.xml" path='extradoc/class[@name="Shared"]/properties/property[@name="ValueRaw"]/*' />
        public string ValueRaw { get; }
        #endregion

        #region Public Override Methods
        public override bool Equals(object obj) => obj is LchSpace other && this.ValueString.Equals(other.ValueString);
        public override int GetHashCode() => base.GetHashCode();
        public override string ToString() => base.ToString();
        #endregion
    }
}
