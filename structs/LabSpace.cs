using System;
using System.Drawing;
using System.Runtime.ConstrainedExecution;

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
        public LabSpace(int decValue) : this(Color.FromArgb(decValue), Common.D65) { }
        /// <include file="../docs/LAB.xml" path='extradoc/class[@name="LAB"]/interfaces/interface[@name="LABIntD65"]/*' />
        public LabSpace(int decValue, (double X, double Y, double Z) referenceWhite) : this(Color.FromArgb(decValue), referenceWhite) { }
        /// <include file="../docs/LAB.xml" path='extradoc/class[@name="LAB"]/interfaces/interface[@name="LABColor"]/*' />
        public LabSpace(Color clr) : this(new XyzSpace(clr), Common.D65) { }
        /// <include file="../docs/LAB.xml" path='extradoc/class[@name="LAB"]/interfaces/interface[@name="LABColorD65"]/*' />
        public LabSpace(Color clr, (double X, double Y, double Z) referenceWhite) : this(new XyzSpace(clr), referenceWhite) { }
        /// <include file="../docs/LAB.xml" path='extradoc/class[@name="LAB"]/interfaces/interface[@name="LABXYZ"]/*' />
        public LabSpace(XyzSpace xyz) : this(xyz, Common.D65) { }
        /// <include file="../docs/LAB.xml" path='extradoc/class[@name="LAB"]/interfaces/interface[@name="LABXYZD65"]/*' />
        public LabSpace(XyzSpace xyz, (double X, double Y, double Z) referenceWhite) : this(xyz, Common.D65, referenceWhite) { }
        /// <include file="../docs/LAB.xml" path='extradoc/class[@name="LAB"]/interfaces/interface[@name="LABXYZD50"]/*' />
        public LabSpace(XyzSpace xyz, (double X, double Y, double Z) sourceWhite, (double X, double Y, double Z) destinationWhite)
        {
            // Get the source XYZ values
            double X = xyz.X;
            double Y = xyz.Y;
            double Z = xyz.Z;

            // Adapt the color IF the illuminants are different
            // (Using a simple X-value check for speed. You could make this more robust.)
            if (Math.Abs(sourceWhite.X - destinationWhite.X) > 0.0001)
            {
                var adaptedXyz = XyzSpace.AdaptXyz(xyz, sourceWhite, destinationWhite);
                X = adaptedXyz.X;
                Y = adaptedXyz.Y;
                Z = adaptedXyz.Z;
            }

            // Normalize using the DESTINATION white point
            double x_normalized = X / destinationWhite.X;
            double y_normalized = Y / destinationWhite.Y;
            double z_normalized = Z / destinationWhite.Z;

            // Run the L*a*b* conversion 
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

        /// <include file="../docs/LAB.xml" path='extradoc/class[@name="LAB"]/methods/method[@name="GetDeltaE76"]/*' />
        public static double GetDeltaE76(LabSpace lab1, LabSpace lab2)
        {
            if (lab1.IsEmpty || lab2.IsEmpty)
                return 0;

            double deltaL = lab1.L - lab2.L;
            double deltaA = lab1.A - lab2.A;
            double deltaB = lab1.B - lab2.B;

            // Standard Euclidean distance formula
            return Math.Sqrt(deltaL * deltaL + deltaA * deltaA + deltaB * deltaB);
        }

        /// <include file="../docs/LAB.xml" path='extradoc/class[@name="LAB"]/methods/method[@name="ToXyzSpace"]/*' />
        public XyzSpace ToXyzSpace() => ToXyzSpace(Common.D65);

        /// <include file="../docs/LAB.xml" path='extradoc/class[@name="LAB"]/methods/method[@name="ToXyzSpaceIll"]/*' />
        public XyzSpace ToXyzSpace((double X, double Y, double Z) referenceWhite)
        {
            if (this.IsEmpty)
                return XyzSpace.Empty; // Assuming XyzSpace has an .Empty property

            // Reverse the L*a*b* formulas to find f_x, f_y, f_z
            double f_y = (this.L + 16.0) / 116.0;
            double f_x = this.A / 500.0 + f_y;
            double f_z = f_y - this.B / 200.0;

            // Define the inverse f(t) function
            // This reverses the (t > delta^3) logic
            const double delta = 6.0 / 29.0;
            const double delta_sq = delta * delta;

            Func<double, double> f_inv = (f) =>
            {
                if (f > delta)
                {
                    // f = t^(1/3)  =>  t = f^3
                    return f * f * f;
                }
                else
                {
                    // f = t / (3 * delta^2) + 4/29  =>  t = 3 * delta^2 * (f - 4/29)
                    return 3 * delta_sq * (f - 4.0 / 29.0);
                }
            };

            // Apply inverse f(t) to get normalized ratios
            double x_norm = f_inv(f_x);
            double y_norm = f_inv(f_y);
            double z_norm = f_inv(f_z);

            // De-normalize with the reference white to get absolute XYZ
            double X = x_norm * referenceWhite.X;
            double Y = y_norm * referenceWhite.Y;
            double Z = z_norm * referenceWhite.Z;

            // return XyzSpace
            return new XyzSpace(X, Y, Z);
        }

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
