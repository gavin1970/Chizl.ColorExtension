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

        /// <include file="../docs/LCH.xml" path='extradoc/class[@name="LCH"]/methods/method[@name="GetDeltaE2000"]/*' />
        public static double GetDeltaE2000(LchSpace lch1, LchSpace lch2, double kL = 1.0, double kC = 1.0, double kH = 1.0)
        {
            if (lch1.IsEmpty || lch2.IsEmpty)
                return 0;

            // --- Step 1: Get L*, C*, h* values ---
            // Note: The algorithm's trig functions require radians.
            double L1 = lch1.L;
            double C1 = lch1.C;
            double H1 = lch1.H * (Math.PI / 180.0); // Convert H1 to radians

            double L2 = lch2.L;
            double C2 = lch2.C;
            double H2 = lch2.H * (Math.PI / 180.0); // Convert H2 to radians

            // --- Step 2: Calculate C' and h' primes ---
            double C_bar = (C1 + C2) / 2.0;
            double G = 0.5 * (1 - Math.Sqrt(Math.Pow(C_bar, 7) / (Math.Pow(C_bar, 7) + Math.Pow(25, 7))));

            double a1_prime = (1 + G) * (C1 * Math.Cos(H1));
            double a2_prime = (1 + G) * (C2 * Math.Cos(H2));

            double b1_prime = (1 + G) * (C1 * Math.Sin(H1));
            double b2_prime = (1 + G) * (C2 * Math.Sin(H2));

            double C1_prime = Math.Sqrt(a1_prime * a1_prime + b1_prime * b1_prime);
            double C2_prime = Math.Sqrt(a2_prime * a2_prime + b2_prime * b2_prime);

            double h1_prime = (Math.Atan2(b1_prime, a1_prime) + 2 * Math.PI) % (2 * Math.PI);
            double h2_prime = (Math.Atan2(b2_prime, a2_prime) + 2 * Math.PI) % (2 * Math.PI);

            // --- Step 3: Calculate L', C', H' means and deltas ---
            double L_bar_prime = (L1 + L2) / 2.0;
            double C_bar_prime = (C1_prime + C2_prime) / 2.0;

            // Mean Hue (h_bar_prime) calculation
            double h_bar_prime;
            double h_diff = Math.Abs(h1_prime - h2_prime);
            if (h_diff <= Math.PI)
            {
                h_bar_prime = (h1_prime + h2_prime) / 2.0;
            }
            else
            {
                h_bar_prime = (h_diff > Math.PI)
                    ? (h1_prime + h2_prime + 2 * Math.PI) / 2.0
                    : (h1_prime + h2_prime) / 2.0;
            }

            // Deltas (ΔL', ΔC', ΔH')
            double delta_L_prime = L2 - L1;
            double delta_C_prime = C2_prime - C1_prime;

            double delta_h_prime;
            if (C1_prime * C2_prime == 0)
            {
                delta_h_prime = 0;
            }
            else if (h_diff <= Math.PI)
            {
                delta_h_prime = h2_prime - h1_prime;
            }
            else
            {
                delta_h_prime = (h2_prime <= h1_prime)
                    ? (h2_prime - h1_prime + 2 * Math.PI)
                    : (h2_prime - h1_prime - 2 * Math.PI);
            }

            double delta_H_prime = 2 * Math.Sqrt(C1_prime * C2_prime) * Math.Sin(delta_h_prime / 2.0);

            // --- Step 4: Calculate Weighting Functions (SL, SC, SH) ---
            double T = 1
                - 0.17 * Math.Cos(h_bar_prime - (30 * Math.PI / 180.0))
                + 0.24 * Math.Cos(2 * h_bar_prime)
                + 0.32 * Math.Cos(3 * h_bar_prime + (6 * Math.PI / 180.0))
                - 0.20 * Math.Cos(4 * h_bar_prime - (63 * Math.PI / 180.0));

            double L_bar_prime_minus_50_sq = (L_bar_prime - 50) * (L_bar_prime - 50);
            double SL = 1 + (0.015 * L_bar_prime_minus_50_sq) / Math.Sqrt(20 + L_bar_prime_minus_50_sq);
            double SC = 1 + 0.045 * C_bar_prime;
            double SH = 1 + 0.015 * C_bar_prime * T;

            // --- Step 5: Calculate Rotation Term (RT) ---
            double delta_theta = (30 * Math.PI / 180.0)
                * Math.Exp(-Math.Pow((h_bar_prime * 180.0 / Math.PI - 275) / 25.0, 2));

            double C_bar_prime_pow7 = Math.Pow(C_bar_prime, 7);
            double RC = 2 * Math.Sqrt(C_bar_prime_pow7 / (C_bar_prime_pow7 + Math.Pow(25, 7)));
            double RT = -RC * Math.Sin(2 * delta_theta);

            // --- Step 6: Calculate Final Delta E 2000 ---
            double L_term = delta_L_prime / (kL * SL);
            double C_term = delta_C_prime / (kC * SC);
            double H_term = delta_H_prime / (kH * SH);

            double dE2000 = Math.Sqrt(
                L_term * L_term +
                C_term * C_term +
                H_term * H_term +
                RT * C_term * H_term
            );

            return dE2000;
        }

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
