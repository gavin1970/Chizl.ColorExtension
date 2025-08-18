using System.Drawing;

namespace Chizl.ColorExtension
{
    public readonly struct ColorDetails
    {
        /// <summary>
        /// Constructors with no parameters are not supported for netstandard2.0
        /// </summary>
        private ColorDetails(bool _)
        {
            FGAscii = string.Empty;
            BGAscii = string.Empty;
            Temperature = string.Empty;
            Tone = ToneRule.Empty;
            CMYK = CmykSpace.Empty;
            HEX = HEX.Empty;
            HSB = HsvSpace.Empty;
            HSL = HslSpace.Empty;
            HSV = HsvSpace.Empty;
            LAB = LabSpace.Empty;
            LCH = LchSpace.Empty;
            LUV = LuvSpace.Empty;
            XYZ = XyzSpace.Empty;
            Argb = 0;
            Rgb = 0;
            IsEmpty = true;
        }

        /// <include file="../docs/ColorDetails.xml" path='extradoc/class[@name="ColorDetails"]/interfaces/interface[@name="ColorDetailsInt"]/*' />
        public ColorDetails(int argb) : this(Color.FromArgb(argb)) { }

        /// <include file="../docs/ColorDetails.xml" path='extradoc/class[@name="ColorDetails"]/interfaces/interface[@name="ColorDetailsColor"]/*' />
        public ColorDetails(Color color) 
        {
            if (!color.IsEmpty && Common.GetCachedColorList(color, out ColorDetails cached))
                this = cached;
            else
            {
                FGAscii = color.FGAscii();
                BGAscii = color.BGAscii();
                Temperature = color.GetTemperature();
                Tone = color.GetTone();

                CMYK = new CmykSpace(color);
                HEX = new HEX(color);
                HSB = new HsvSpace(color);
                HSL = new HslSpace(color);
                HSV = new HsvSpace(color);
                LAB = new LabSpace(color);
                LCH = new LchSpace(color);
                LUV = new LuvSpace(color);
                XYZ = new XyzSpace(color);
                Argb = color.ToArgb();
                Rgb = color.ToRgb();
                IsEmpty = false;

                //cache, to help with speed
                Common.AddCachedColorList(this);
            }
        }

        #region Public Properties
        /// <include file="../docs/Shared.xml" path='extradoc/class[@name="Shared"]/properties/property[@name="Empty"]/*' />
        public static ColorDetails Empty { get { return new ColorDetails(true); } }

        /// <include file="../docs/Shared.xml" path='extradoc/class[@name="Shared"]/properties/property[@name="IsEmpty"]/*' />
        public bool IsEmpty { get; }

        /// <include file="../docs/ColorDetails.xml" path='extradoc/class[@name="ColorDetails"]/properties/property[@name="Argb"]/*' />
        public int Argb { get; }

        /// <include file="../docs/ColorDetails.xml" path='extradoc/class[@name="ColorDetails"]/properties/property[@name="Rgb"]/*' />
        public int Rgb { get; }

        /// <include file="../docs/ColorDetails.xml" path='extradoc/class[@name="ColorDetails"]/properties/property[@name="Tone"]/*' />
        public ToneRule Tone { get; }

        /// <include file="../docs/ColorDetails.xml" path='extradoc/class[@name="ColorDetails"]/properties/property[@name="Temperature"]/*' />
        public string Temperature { get; }

        /// <include file="../docs/ColorDetails.xml" path='extradoc/class[@name="ColorDetails"]/properties/property[@name="FGAscii"]/*' />
        public string FGAscii { get; }

        /// <include file="../docs/ColorDetails.xml" path='extradoc/class[@name="ColorDetails"]/properties/property[@name="BGAscii"]/*' />
        public string BGAscii { get; }

        /// <include file="../docs/CmykSpace.xml" path='extradoc/class[@name="CmykSpace"]/*' />
        public CmykSpace CMYK { get; }

        /// <include file="../docs/HEX.xml" path='extradoc/class[@name="HEX"]/*' />
        public HEX HEX { get; }

        /// <include file="../docs/HsvSpace.xml" path='extradoc/class[@name="HsvSpace"]/*' />
        public HsvSpace HSB { get; }

        /// <include file="../docs/HSL.xml" path='extradoc/class[@name="HSL"]/*' />
        public HslSpace HSL { get; }

        /// <include file="../docs/HSV.xml" path='extradoc/class[@name="HSV"]/*' />
        public HsvSpace HSV { get; }

        /// <include file="../docs/LAB.xml" path='extradoc/class[@name="LAB"]/*' />
        public LabSpace LAB { get; }

        /// <include file="../docs/LCH.xml" path='extradoc/class[@name="LCH"]/*' />
        public LchSpace LCH { get; }

        /// <include file="../docs/LUV.xml" path='extradoc/class[@name="LUV"]/*' />
        public LuvSpace LUV { get; }

        /// <include file="../docs/XYZ.xml" path='extradoc/class[@name="XYZ"]/*' />
        public XyzSpace XYZ { get; }
        #endregion
    }
}
