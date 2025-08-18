using System;
using System.Drawing;

namespace Chizl.ColorExtension
{
    /// <include file="../docs/LAB.xml" path='extradoc/class[@name="LAB"]/*' />
    public readonly struct LabSpace
    {
        /// <summary>
        /// Constructors with no parameters are not supported for netstandard2.0
        /// </summary>
        private LabSpace(bool _)
        {
            this.L = 0.0;
            this.A = 0.0;
            this.B = 0.0;
            this.ValueString = "";
            this.ValueRaw = "";
            this.IsEmpty = true;
        }

        /// <include file="../docs/LAB.xml" path='extradoc/class[@name="LAB"]/interfaces/interface[@name="LABInt"]/*' />
        public LabSpace(int decValue, bool precisionD65 = false) : this(Color.FromArgb(decValue), precisionD65) { }

        /// <include file="../docs/LAB.xml" path='extradoc/class[@name="LAB"]/interfaces/interface[@name="LABColor"]/*' />
        public LabSpace(Color clr, bool precisionD65 = false) : this(new XyzSpace(clr), precisionD65) { }

        /// <include file="../docs/LAB.xml" path='extradoc/class[@name="LAB"]/interfaces/interface[@name="LABXYZ"]/*' />
        public LabSpace(XyzSpace xyz, bool precisionD65 = false)
        {
            double X = xyz.X;
            double Y = xyz.Y;
            double Z = xyz.Z;

            var D65 = precisionD65 ? Common.FullPrecisionD65 : Common.D65;

            // Normalize X, Y, Z by the reference white point (D65)
            double x_normalized = X / D65.X;
            double y_normalized = Y / D65.Y;
            double z_normalized = Z / D65.Z;

            // Define the f(t) function for LAB conversion
            Func<double, double> f = (t) =>
            {
                const double delta = 6.0 / 29.0;
                if (t > Math.Pow(delta, 3))
                    return Math.Pow(t, 1.0 / 3.0);
                else
                    return (t / (3 * Math.Pow(delta, 2))) + (4.0 / 29.0);
            };

            this.L = 116 * f(y_normalized) - 16;
            this.A = 500 * (f(x_normalized) - f(y_normalized));
            this.B = 200 * (f(y_normalized) - f(z_normalized));

            this.ValueString = $"LAB[L={L:F2}, A={A:F2}, B={B:F2}]";
            this.ValueRaw = $"{L:F2},{A:F2},{B:F2}";
            this.IsEmpty = false;
        }

        #region Public Properties
        /// <include file="../docs/Shared.xml" path='extradoc/class[@name="Shared"]/properties/property[@name="Empty"]/*' />
        public static LabSpace Empty => new LabSpace(true);

        /// <include file="../docs/Shared.xml" path='extradoc/class[@name="Shared"]/properties/property[@name="IsEmpty"]/*' />
        public bool IsEmpty { get; }

        /// <include file="../docs/LAB.xml" path='extradoc/class[@name="LAB"]/properties/property[@name="L"]/*' />
        public double L { get; }

        /// <include file="../docs/LAB.xml" path='extradoc/class[@name="LAB"]/properties/property[@name="A"]/*' />
        public double A { get; }

        /// <include file="../docs/LAB.xml" path='extradoc/class[@name="LAB"]/properties/property[@name="B"]/*' />
        public double B { get; }

        /// <include file="../docs/Shared.xml" path='extradoc/class[@name="Shared"]/properties/property[@name="ValueString"]/*' />
        public string ValueString { get; }

        /// <include file="../docs/Shared.xml" path='extradoc/class[@name="Shared"]/properties/property[@name="ValueRaw"]/*' />
        public string ValueRaw { get; }
        #endregion

        #region Public Override Methods
        public override bool Equals(object obj) => obj is LabSpace other && this.ValueString.Equals(other.ValueString);
        public override int GetHashCode() => this.ValueString.GetHashCode();
        public override string ToString() => this.ValueString;
        #endregion
    }
}
