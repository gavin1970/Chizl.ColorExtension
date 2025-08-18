using System;
using System.Linq; // For Color.FromArgb to parse Hex and easy RGB access (Windows Forms/GDI+ specific)
using System.Collections.Generic;

// If not using System.Drawing, you'd parse Hex to RGB bytes manually.
// For more advanced color conversions (CIELAB, etc.), you'd look into other classes..

public record HSLColor(double Hue, double Saturation, double Lightness);
public record ColorInfo(string Hex, (double Min, double Max) HueRange, string Description)
{
    // A computed property to get the HSL for the given Hex,
    // useful for direct lookup or debugging.
    public HSLColor Hsl
    {
        get
        {
            return HexToHsl(Hex);
        }
    }

    // Static helper method to convert a hex string to HSL.
    // This is a common implementation based on standard algorithms.
    public static HSLColor HexToHsl(string hexColor)
    {
        // Remove '#' if present
        if (hexColor.StartsWith("#"))
            hexColor = hexColor.Substring(1);

        // Parse hex to R, G, B components
        int r = Convert.ToInt32(hexColor.Substring(0, 2), 16);
        int g = Convert.ToInt32(hexColor.Substring(2, 2), 16);
        int b = Convert.ToInt32(hexColor.Substring(4, 2), 16);

        // Convert RGB to HSL
        double r_f = r / 255f;
        double g_f = g / 255f;
        double b_f = b / 255f;

        double max = Math.Max(Math.Max(r_f, g_f), b_f);
        double min = Math.Min(Math.Min(r_f, g_f), b_f);
        double h = 0, s, l;

        l = (max + min) / 2f;

        if (max == min)
        {
            h = 0; // achromatic
            s = 0; // achromatic
        }
        else
        {
            double d = max - min;
            s = l > 0.5f ? d / (2f - max - min) : d / (max + min);

            if (max == r_f)
            {
                h = (g_f - b_f) / d + (g_f < b_f ? 6f : 0f);
            }
            else if (max == g_f)
            {
                h = (b_f - r_f) / d + 2f;
            }
            else // max == b_f
            {
                h = (r_f - g_f) / d + 4f;
            }
            h /= 6f;
        }

        return new HSLColor(
            (double)Math.Round(h * 360),
            (double)Math.Round(s * 100),
            (double)Math.Round(l * 100)
        );
    }
}
public class ColorDatabase
{
    private Dictionary<string, Dictionary<string, ColorInfo>> _fruitColorsDb;
    public ColorDatabase()
    {
        _fruitColorsDb = new Dictionary<string, Dictionary<string, ColorInfo>>(StringComparer.OrdinalIgnoreCase)
        {
            ["apple"] = new Dictionary<string, ColorInfo>(StringComparer.OrdinalIgnoreCase)
            {
                ["ripe_red"] = new ColorInfo("#DC143C", (340, 10), "Classic bright red apple"),
                ["grannysmith_green"] = new ColorInfo("#4CAF50", (110, 130), "Vibrant, slightly yellow-green"),
                ["chlorophyll_green"] = new ColorInfo("#ACCD24", (60, 80), "Vibrant, yellow-green"),
                ["dried_brownish_red"] = new ColorInfo("#8B4513", (10, 20), "Muted, earthy red-brown of dried apple"),
                ["golden_yellow"] = new ColorInfo("#FFD700", (50, 70), "Rich, golden yellow apple")
            },
            ["banana"] = new Dictionary<string, ColorInfo>(StringComparer.OrdinalIgnoreCase)
            {
                ["ripe_yellow"] = new ColorInfo("#FFE135", (50, 60), "Common bright banana yellow"),
                ["unripe_green"] = new ColorInfo("#9ACD32", (80, 100), "Greenish-yellow unripe banana"),
                ["overripe_brown"] = new ColorInfo("#5C4033", (20, 40), "Dark, desaturated brown spots")
            },
            ["strawberry"] = new Dictionary<string, ColorInfo>(StringComparer.OrdinalIgnoreCase)
            {
                ["classic_red"] = new ColorInfo("#C5101A", (0, 10), "Deep, rich red of a ripe strawberry"),
                ["light_pinkish_red"] = new ColorInfo("#FF69B4", (330, 350), "Lighter, more pinkish berry hue")
            },
            ["lemon"] = new Dictionary<string, ColorInfo>(StringComparer.OrdinalIgnoreCase)
            {
                ["bright_yellow"] = new ColorInfo("#FFF700", (50, 70), "Pure, vibrant lemon yellow")
            },
            ["lime"] = new Dictionary<string, ColorInfo>(StringComparer.OrdinalIgnoreCase)
            {
                ["vibrant_green"] = new ColorInfo("#32CD32", (100, 120), "Bright, zesty lime green")
            },
            ["orange"] = new Dictionary<string, ColorInfo>(StringComparer.OrdinalIgnoreCase)
            {
                ["classic_orange"] = new ColorInfo("#FFA500", (30, 40), "Standard orange fruit color"),
                ["dark_orange"] = new ColorInfo("#FF8C00", (25, 35), "Deeper, richer orange")
            },
            ["blueberry"] = new Dictionary<string, ColorInfo>(StringComparer.OrdinalIgnoreCase)
            {
                ["classic_blue"] = new ColorInfo("#464196", (230, 250), "Typical deep blueberry blue")
            },
            ["grape"] = new Dictionary<string, ColorInfo>(StringComparer.OrdinalIgnoreCase)
            {
                ["purple"] = new ColorInfo("#6A0DAD", (270, 290), "Common purple grape color"),
                ["green"] = new ColorInfo("#9ACD32", (90, 110), "Green grape color")
            },
            ["peach"] = new Dictionary<string, ColorInfo>(StringComparer.OrdinalIgnoreCase)
            {
                ["light_peach"] = new ColorInfo("#FFE5B4", (30, 50), "Soft, light peach color"),
                ["pinkish_peach"] = new ColorInfo("#FFCBA4", (10, 30), "Peach with a noticeable pink tint")
            }
        };
    }

    public ColorInfo[] GetColorByDegree(double degree) => 
        _fruitColorsDb.Values.Select(s => s.Values
                                            .FirstOrDefault(s2 => s2.HueRange.Min < degree && s2.HueRange.Max > degree))
                                            .Where(s => s != null).ToArray();
    public string[] GetKeys() => _fruitColorsDb.Keys.ToArray();
    public string[] GetVariations(string key)
    {
        if (_fruitColorsDb.TryGetValue(key, out var variationName))
            return variationName.Keys.ToArray();
        else
            return new string[0];
    }
    public (string, ColorInfo) GetItemColorInfo(string itemName, string variationName = null)
    {
        if (_fruitColorsDb.TryGetValue(itemName, out var itemData))
        {
            if (variationName != null)
            {
                if (itemData.TryGetValue(variationName, out var variationInfo))
                    return (variationName, variationInfo);
                else
                {
                    // You might want to throw an exception or return null/default based on your error handling
                    Console.WriteLine($"Variation '{variationName}' not found for '{itemName}'. Available variations: {string.Join(", ", itemData.Keys)}");
                    return (null, null); // Or throw new ArgumentException
                }
            }
            else
            {
                // If no variation specified, you could return the "most common" or a list of all variations.
                // For simplicity, let's return the first entry if no specific variation is requested.
                // In a real app, you might define a "default" variation or return a list of all.
                if (itemData.Count > 0)
                {
                    Console.WriteLine($"No variation specified for '{itemName}'. Returning first available variation.");
                    return (itemData.Keys.First(), itemData.Values.First()); // Requires System.Linq
                }

                Console.WriteLine($"No variations defined for '{itemName}'.");
                return (null, null);
            }
        }

        Console.WriteLine($"No color data found for '{itemName}'.");
        return (null, null);
    }
    public Dictionary<string, ColorInfo> GetAllVariationsForItem(string itemName)
    {
        if (_fruitColorsDb.TryGetValue(itemName, out var itemData))
            return itemData;

        return null; // Or an empty dictionary
    }
}
