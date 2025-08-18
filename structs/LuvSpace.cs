using System;
using System.Drawing;

namespace Chizl.ColorExtension
{
    /// <include file="../docs/LUV.xml" path='extradoc/class[@name="LUV"]/*' />
    public readonly struct LuvSpace
    {
        /// <summary>
        /// Constructors with no parameters are not supported for netstandard2.0
        /// </summary>
        private LuvSpace(bool _)
        {
            this.L = 0.0;
            this.U = 0.0;
            this.V = 0.0;
            this.ValueString = "";
            this.ValueRaw = "";
            this.IsEmpty = true;
        }

        /// <include file="../docs/LUV.xml" path='extradoc/class[@name="LUV"]/interfaces/interface[@name="LUVInt"]/*' />
        public LuvSpace(int decValue, bool precisionD65 = false) : this(Color.FromArgb(decValue), precisionD65) { }

        /// <include file="../docs/LUV.xml" path='extradoc/class[@name="LUV"]/interfaces/interface[@name="LUVColor"]/*' />
        public LuvSpace(Color clr, bool precisionD65 = false) : this(new XyzSpace(clr), precisionD65) { }

        /// <include file="../docs/LUV.xml" path='extradoc/class[@name="LUV"]/interfaces/interface[@name="LUVXYZ"]/*' />
        public LuvSpace(XyzSpace xyz, bool precisionD65 = false)
        {
            double X = xyz.X;
            double Y = xyz.Y;
            double Z = xyz.Z;

            var D65 = precisionD65 ? Common.FullPrecisionD65 : Common.D65;

            // Calculate reference white point chromaticity coordinates (u'n, v'n)
            double un_prime = (4 * D65.X) / (D65.X + (15 * D65.Y) + (3 * D65.Z));
            double vn_prime = (9 * D65.Y) / (D65.X + (15 * D65.Y) + (3 * D65.Z));

            // Calculate sample chromaticity coordinates (u', v')
            double divisor = (X + (15 * Y) + (3 * Z));
            double u_prime = (divisor == 0) ? 0 : (4 * X) / divisor;
            double v_prime = (divisor == 0) ? 0 : (9 * Y) / divisor;

            // Calculate L*
            const double delta = 6.0 / 29.0;
            const double deltaCubed = (delta * delta * delta); // (6/29)^3

            if ((Y / D65.Y) > deltaCubed)
                this.L = 116 * Math.Pow(Y / D65.Y, 1.0 / 3.0) - 16;
            else
                this.L = (29.0 / 6.0) * (29.0 / 6.0) * (29.0 / 6.0) * (Y / D65.Y);

            // Calculate u* and v*
            this.U = 13 * L * (u_prime - un_prime);
            this.V = 13 * L * (v_prime - vn_prime);

            this.ValueString = $"LUV[L={L:F2}, U={U:F2}, V={V:F2}]";
            this.ValueRaw = $"{L:F2},{U:F2},{V:F2}";
            this.IsEmpty = false;
        }

        #region Public Properties
        /// <include file="../docs/Shared.xml" path='extradoc/class[@name="Shared"]/properties/property[@name="Empty"]/*' />
        public static LuvSpace Empty => new LuvSpace(true);

        /// <include file="../docs/Shared.xml" path='extradoc/class[@name="Shared"]/properties/property[@name="IsEmpty"]/*' />
        public bool IsEmpty { get; }

        /// <include file="../docs/LUV.xml" path='extradoc/class[@name="LUV"]/properties/property[@name="L"]/*' />
        public double L { get; }

        /// <include file="../docs/LUV.xml" path='extradoc/class[@name="LUV"]/properties/property[@name="U"]/*' />
        public double U { get; }

        /// <include file="../docs/LUV.xml" path='extradoc/class[@name="LUV"]/properties/property[@name="V"]/*' />
        public double V { get; }

        /// <include file="../docs/Shared.xml" path='extradoc/class[@name="Shared"]/properties/property[@name="ValueString"]/*' />
        public string ValueString { get; }

        /// <include file="../docs/Shared.xml" path='extradoc/class[@name="Shared"]/properties/property[@name="ValueRaw"]/*' />
        public string ValueRaw { get; }
        #endregion

        #region Public Override Methods
        public override bool Equals(object obj) => obj is LuvSpace other && this.ValueString.Equals(other.ValueString);
        public override int GetHashCode() => this.ValueString.GetHashCode();
        public override string ToString() => this.ValueString;
        #endregion
    }
}
