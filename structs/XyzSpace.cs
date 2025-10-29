using System;
using System.Drawing;

namespace Chizl.ColorExtension
{
    /// <include file="../docs/XYZ.xml" path='extradoc/class[@name="XYZ"]/*' />
    public readonly struct XyzSpace
    {
        // --- Bradford Transformation Matrix ---
        private static readonly double[,] M_Bradford = {
            {  0.8951,  0.2664, -0.1614 },
            { -0.7502,  1.7135,  0.0367 },
            {  0.0389, -0.0685,  1.0296 }
        };

        // --- Inverse Bradford Transformation Matrix ---
        private static readonly double[,] M_Bradford_Inverse = {
            {  0.9869929, -0.1470543, 0.1599627 },
            {  0.4323053,  0.5183603, 0.0492912 },
            { -0.0085287,  0.0400428, 0.9684867 }
        };

        /// <summary>
        /// Constructors with no parameters are not supported for netstandard2.0
        /// </summary>
        private XyzSpace(bool _)
        {
            this.X = 0.0;
            this.Y = 0.0;
            this.Z = 0.0;
            this.ValueString = "";
            this.ValueRaw = "";
            this.IsEmpty = true;
        }

        internal XyzSpace(double x, double y, double z)
        {
            // Scale XYZ to 0-100 range for consistency with common representations
            // If Y is expected to be 100 for white, then X and Z should be scaled accordingly.
            // The coefficients already produce values in a range where Y for white is ~1.0,
            // so multiplying by 100 makes Y for white = 100.
            this.X = x * 100;
            this.Y = y * 100;
            this.Z = z * 100;

            this.ValueString = $"XYZ[X={X:F2}, Y={Y:F2}, Z={Z:F2}]";
            this.ValueRaw = $"{X:F2},{Y:F2},{Z:F2}";
            this.IsEmpty = false;
        }

        /// <include file="../docs/XYZ.xml" path='extradoc/class[@name="XYZ"]/interfaces/interface[@name="XYZInt"]/*' />
        public XyzSpace(int decValue) : this(Color.FromArgb(decValue)) { }

        /// <include file="../docs/XYZ.xml" path='extradoc/class[@name="XYZ"]/interfaces/interface[@name="XYZColor"]/*' />
        public XyzSpace(Color clr) 
        {
            // Normalize R, G, B to the range [0, 1]
            double r = clr.R / 255.0;
            double g = clr.G / 255.0;
            double b = clr.B / 255.0;

            // Apply gamma correction (sRGB to linear RGB)
            r = (r > 0.04045) ? Math.Pow((r + 0.055) / 1.055, 2.4) : r / 12.92;
            g = (g > 0.04045) ? Math.Pow((g + 0.055) / 1.055, 2.4) : g / 12.92;
            b = (b > 0.04045) ? Math.Pow((b + 0.055) / 1.055, 2.4) : b / 12.92;

            // Convert linear RGB to XYZ using sRGB-specific transformation matrix
            // These coefficients are for sRGB with D65 illuminant
            double x = r * 0.4124564 + g * 0.3575761 + b * 0.1804375;
            double y = r * 0.2126729 + g * 0.7151522 + b * 0.0721750;
            double z = r * 0.0193339 + g * 0.1191920 + b * 0.9503041;

            // Scale XYZ to 0-100 range for consistency with common representations
            // If Y is expected to be 100 for white, then X and Z should be scaled accordingly.
            // The coefficients already produce values in a range where Y for white is ~1.0,
            // so multiplying by 100 makes Y for white = 100.
            this.X = x * 100;
            this.Y = y * 100;
            this.Z = z * 100;

            this.ValueString = $"XYZ[X={X:F2}, Y={Y:F2}, Z={Z:F2}]";
            this.ValueRaw = $"{X:F2},{Y:F2},{Z:F2}";
            this.IsEmpty = false;
        }

        #region Public Static Methods
        /// <include file="../docs/XYZ.xml" path='extradoc/class[@name="XYZ"]/methods/method[@name="AdaptXyz"]/*' />
        public static XyzSpace AdaptXyz(XyzSpace xyz, (double X, double Y, double Z) sourceWhite, (double X, double Y, double Z) destWhite)
        {
            // Convert source and destination white points to cone response (LMS)
            double[] sourceLMS = TransformXyz(M_Bradford, sourceWhite.X, sourceWhite.Y, sourceWhite.Z);
            double[] destLMS = TransformXyz(M_Bradford, destWhite.X, destWhite.Y, destWhite.Z);

            // Calculate cone response for the source color
            double[] sourceLMS_color = TransformXyz(M_Bradford, xyz.X, xyz.Y, xyz.Z);

            // Calculate the adapted cone response
            double[] destLMS_color = {
                sourceLMS_color[0] * (destLMS[0] / sourceLMS[0]),
                sourceLMS_color[1] * (destLMS[1] / sourceLMS[1]),
                sourceLMS_color[2] * (destLMS[2] / sourceLMS[2])
            };

            // Convert adapted cone response back to XYZ
            double[] finalXYZ = TransformXyz(M_Bradford_Inverse, destLMS_color[0], destLMS_color[1], destLMS_color[2]);

            // You might want to add an XyzSpace(double, double, double) constructor
            return new XyzSpace(finalXYZ[0], finalXYZ[1], finalXYZ[2]); // Quick way to get an XyzSpace object
        }
        #endregion

        #region Public Properties
        /// <include file="../docs/Shared.xml" path='extradoc/class[@name="Shared"]/properties/property[@name="Empty"]/*' />
        public static XyzSpace Empty => new XyzSpace(true);

        /// <include file="../docs/Shared.xml" path='extradoc/class[@name="Shared"]/properties/property[@name="IsEmpty"]/*' />
        public bool IsEmpty { get; }

        /// <include file="../docs/XYZ.xml" path='extradoc/class[@name="XYZ"]/properties/property[@name="X"]/*' />
        public double X { get; }

        /// <include file="../docs/XYZ.xml" path='extradoc/class[@name="XYZ"]/properties/property[@name="Y"]/*' />
        public double Y { get; }

        /// <include file="../docs/XYZ.xml" path='extradoc/class[@name="XYZ"]/properties/property[@name="Z"]/*' />
        public double Z { get; }

        /// <include file="../docs/Shared.xml" path='extradoc/class[@name="Shared"]/properties/property[@name="ValueString"]/*' />
        public string ValueString { get; }

        /// <include file="../docs/Shared.xml" path='extradoc/class[@name="Shared"]/properties/property[@name="ValueRaw"]/*' />
        public string ValueRaw { get; }
        #endregion

        #region Private Helper Methods
        /// <summary>
        /// Helper function for matrix multiplication.
        /// </summary>
        private static double[] TransformXyz(double[,] matrix, double x, double y, double z)
        {
            return new double[]
            {
                matrix[0, 0] * x + matrix[0, 1] * y + matrix[0, 2] * z,
                matrix[1, 0] * x + matrix[1, 1] * y + matrix[1, 2] * z,
                matrix[2, 0] * x + matrix[2, 1] * y + matrix[2, 2] * z
            };
        }
        #endregion

        #region Public Override Methods
        public override bool Equals(object obj) => obj is XyzSpace other && this.ValueString.Equals(other.ValueString);
        public override int GetHashCode() => this.ValueString.GetHashCode();
        public override string ToString() => this.ValueString;
        #endregion
    }
}
