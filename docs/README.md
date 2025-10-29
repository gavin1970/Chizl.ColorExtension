# `Chizl.ColorExtension` ![`logo`](https://github.com/gavin1970/Chizl.ColorExtension/blob/master/docs/ChizlColorExtension.png)
In the works and no where near complete with auto name generation. That is part of <a href="https://www.colors.dev">colors.dev</a> project.  However, this library has much more to offer.
<hr/>

## Project Information
- What Is It: 
	- ![Class Library](https://img.shields.io/badge/Multi_Platform-Class_Library-orange)
- Written In: 
	- ![Visual Studio](https://badgen.net/badge/icon/VisualStudio?color=blue&icon=visualstudio&label)![v2022](https://badgen.net/badge/visualstudio/2022/red?labelColor=blue&color=red&label)
- Target Frameworks: 
	- ![NetStandard](https://img.shields.io/badge/.NET_Standard-gray)![](https://img.shields.io/badge/v2.0-red)![](https://img.shields.io/badge/v2.1-blue)
- Short Description:
	- Attempt to build libraries to help developers with color. This library supports the following spaces: 
		- CMYK
		- Hsv/Hsv
		- Hsl
		- L\*a\*b
		- Lch
		- Luv
		- Xyz
	- It also handles Xyz illuminant "White" in both Lab and Luv space.
	- Has a feature called Overlay, where you can take two color's with the first having some transparency/alpha and overlay with a solid color to get a new RGB color.
	- Many Color extensions, e.g:
		- public static int ToRgb(this Color @this)
		- public static bool IsDark(this Color @this, double threshold = 0.5)
		- public static bool IsLight(this Color c, double threshold = 0.5)
		- public static HEX ToHex(this Color @this)
		- public static CmykSpace ToCMYK(this Color @this)
		- public static HsbSpace ToHsb(this Color @this)
		- public static HslSpace ToHsl(this Color @this)
		- public static HsvSpace ToHsv(this Color @this)
		- public static bool ToHSV(this Color @this, out double hue, out double saturation, out double value)
		- public static LabSpace ToLab(this Color @this)
		- public static LchSpace ToLch(this Color @this)
		- public static LuvSpace ToLov(this Color @this)
		- public static XyzSpace ToXyz(this Color @this)
		- public static string ToHexRgb(this Color @this)
		- public static string ToHexArgb(this Color @this)
		- public static string FGAscii(this Color @this, bool forDisplay = false)
		- public static string BGAscii(this Color @this, bool forDisplay = false)
		- public static string ResetAscii(this Color @this, bool performWrite = false)
		- public static Color ApplyBgColor(this Color @this, Color bgColor)
		- public static Color ApplyFgColor(this Color @this, Color fgColor)
		- public static Color Tint(this Color color, double percentage)
		- public static Color Shade(this Color color, double percentage)
		- public static ToneRule GetTone(this Color @this)
		- public static string GetTemperature(this Color @this)
		- public static Color GetComplementary(this Color @this)
		- public static bool GetContrast(this Color background, out Color bestContrastColor, out double contrastRatio, double minThreshold = 0)
		- public static string HueName(this Color @this, bool noSpaces = true)


## Demos Included
- NET9.0 Console
- NETFramework 4.8 WinForm


## What is it?
We have been collecting color names from different sources on the web.  There are a lot of unnamed colors in the 24bit color wheel and could use your help to name them.  If you would like to be part of this contribution, please visit the official color name registry at [color-register.org](https://color-register.org/).

We ask that you try to name them based on their color, not names like `Bob's Red`.  Names like `Ricky Red` will still work as it has a understanding of the color in its name and a clean flow.

This project's color classes will be update based these updates.
