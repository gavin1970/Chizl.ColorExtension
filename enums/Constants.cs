using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Chizl.ColorExtension
{
    public enum ContrastPassLevel
    {
        /// <summary>
        /// No WCAG level passes (too low contrast).
        /// </summary>
        None,
        /// <summary>
        /// Only passes for large text (≥18pt or 14pt bold).
        /// </summary>
        LargeTextOnly,
        /// <summary>
        /// Passes for all text sizes (normal and large).
        /// </summary>
        AllText
    }

    public enum ToneCategory
    {
        Neutral,
        Bright,
        Midtone,
        Dark,
        VeryDark
    }

    /// <summary>
    /// HSB and HSV are the same.<br/>
    /// Within HsValueType, both Brightness and Value exists with the same value of 3 since they are interchanable.
    /// </summary>
    internal enum HsValueType
    {
        Hue = 1,
        Saturation_Hsv = 2,
        Saturation_Hsl = 3,
        /// <summary>
        /// Also known as Value
        /// </summary>
        Brightness = 4,
        /// <summary>
        /// Also known as Brightness
        /// </summary>
        Value = 5,
        Lightness = 6,
        Temperature = 7,
    }

    internal enum AsciiColorType
    {
        /// <summary>
        /// Foreground Identifier
        /// </summary>
        FG_Color = 38,
        /// <summary>
        /// Background Identifier
        /// </summary>
        BG_Color = 48,
    }

    internal enum AsciiColorPalette
    {
        /// <summary>
        /// 8-bit: 256 colors
        /// </summary>
        Color_Palette_8Bit = 5,
        /// <summary>
        /// 24-bit: 16,777,216 colors
        /// </summary>
        Color_Palette_24Bit = 2,
    }
        
    internal static class Constants
    {
        internal static readonly List<string> WebSafe16Colors = new List<string>() { "00", "80", "C0", "FF" };
        internal static readonly List<string> WebSafe216Colors = new List<string>() { "00", "33", "66", "99", "CC", "FF" };

        private static ConcurrentDictionary<double, string> CreateHueInterpolations()
        {
            var dict = new ConcurrentDictionary<double, string>();

            // REDS (0-19) - Warm, Fiery
            dict.TryAdd(0.00, "Red");
            dict.TryAdd(1.00, "Crimson Fire");
            dict.TryAdd(2.00, "Ember Red");
            dict.TryAdd(3.00, "Blazing Red");
            dict.TryAdd(4.00, "Ruby Red");
            dict.TryAdd(5.00, "Tomato Red");
            dict.TryAdd(6.00, "Scarlet Red");
            dict.TryAdd(7.00, "Cardinal Red");
            dict.TryAdd(8.00, "Chili Red");
            dict.TryAdd(9.00, "Cherry Red");
            dict.TryAdd(10.00, "Vermilion");
            dict.TryAdd(11.00, "Firebrick Red");
            dict.TryAdd(12.00, "Persian Red");
            dict.TryAdd(13.00, "Brick Red");
            dict.TryAdd(14.00, "Terra Cotta");
            dict.TryAdd(15.00, "Indian Red");
            dict.TryAdd(16.00, "Rusty Red");
            dict.TryAdd(17.00, "Cinnabar Red");
            dict.TryAdd(18.00, "Vivid Orange Red");
            dict.TryAdd(19.00, "Bright Red Orange");

            // RED-ORANGES (20-39) - Warm, Earthy
            dict.TryAdd(20.00, "Red Orange");
            dict.TryAdd(21.00, "Crimson Orange");
            dict.TryAdd(22.00, "Copper Orange");
            dict.TryAdd(23.00, "Fiery Amber");
            dict.TryAdd(24.00, "Lava Orange");
            dict.TryAdd(25.00, "Persimmon");
            dict.TryAdd(26.00, "Pumpkin Orange");
            dict.TryAdd(27.00, "Zesty Orange");
            dict.TryAdd(28.00, "Burnt Orange");
            dict.TryAdd(29.00, "Honey Orange");
            dict.TryAdd(30.00, "Orange");
            dict.TryAdd(31.00, "Tangerine");
            dict.TryAdd(32.00, "Apricot Orange");
            dict.TryAdd(33.00, "Marigold Orange");
            dict.TryAdd(34.00, "Golden Orange");
            dict.TryAdd(35.00, "Saffron Orange");
            dict.TryAdd(36.00, "Flame Orange");
            dict.TryAdd(37.00, "Sunset Orange");
            dict.TryAdd(38.00, "Bronze Orange");
            dict.TryAdd(39.00, "Coral Orange");

            // ORANGES (40-59) - Bright, Sunny
            dict.TryAdd(40.00, "Golden Yellow Orange");
            dict.TryAdd(41.00, "Bright Orange");
            dict.TryAdd(42.00, "Honey Gold Orange");
            dict.TryAdd(43.00, "Dandelion Orange");
            dict.TryAdd(44.00, "Vivid Orange");
            dict.TryAdd(45.00, "Mandarin Orange");
            dict.TryAdd(46.00, "Sunny Orange");
            dict.TryAdd(47.00, "Sunray Orange");
            dict.TryAdd(48.00, "Butterscotch");
            dict.TryAdd(49.00, "Cantaloupe Orange");
            dict.TryAdd(50.00, "Sunshine Orange");
            dict.TryAdd(51.00, "Amber Orange");
            dict.TryAdd(52.00, "Light Amber");
            dict.TryAdd(53.00, "Soft Orange");
            dict.TryAdd(54.00, "Peach Orange");
            dict.TryAdd(55.00, "Creme Brulee");
            dict.TryAdd(56.00, "Apricot Blush");
            dict.TryAdd(57.00, "Melon Orange");
            dict.TryAdd(58.00, "Fawn Orange");
            dict.TryAdd(59.00, "Bisque Orange");

            // YELLOW-GREENS (60-79) - Warm Yellows moving to green
            dict.TryAdd(60.00, "Yellow");
            dict.TryAdd(61.00, "Goldenrod");
            dict.TryAdd(62.00, "Honey Gold");
            dict.TryAdd(63.00, "Light Gold");
            dict.TryAdd(64.00, "Bright Marigold");
            dict.TryAdd(65.00, "Sunburst Yellow");
            dict.TryAdd(66.00, "Amber Yellow");
            dict.TryAdd(67.00, "Desert Gold");
            dict.TryAdd(68.00, "Mustard Yellow");
            dict.TryAdd(69.00, "Antique Gold");
            dict.TryAdd(70.00, "Harvest Gold");
            dict.TryAdd(71.00, "Dijon Yellow");
            dict.TryAdd(72.00, "Chartreuse Yellow");
            dict.TryAdd(73.00, "Lime Yellow");
            dict.TryAdd(74.00, "Olive Yellow");
            dict.TryAdd(75.00, "Moss Yellow");
            dict.TryAdd(76.00, "Khaki Yellow");
            dict.TryAdd(77.00, "Pistachio Yellow");
            dict.TryAdd(78.00, "Sage Yellow");
            dict.TryAdd(79.00, "Asparagus Yellow");

            // GREENS (80-119) - Diverse Greens
            dict.TryAdd(80.00, "Spring Green");
            dict.TryAdd(81.00, "Lime Green");
            dict.TryAdd(82.00, "Bright Green");
            dict.TryAdd(83.00, "Leaf Green");
            dict.TryAdd(84.00, "Emerald Green");
            dict.TryAdd(85.00, "Grass Green");
            dict.TryAdd(86.00, "Forest Green");
            dict.TryAdd(87.00, "Pine Green");
            dict.TryAdd(88.00, "Hunter Green");
            dict.TryAdd(89.00, "Deep Green");
            dict.TryAdd(90.00, "Kelly Green");
            dict.TryAdd(91.00, "Jade Green");
            dict.TryAdd(92.00, "Sea Green");
            dict.TryAdd(93.00, "Ocean Green");
            dict.TryAdd(94.00, "Teal Green");
            dict.TryAdd(95.00, "Aqua Green");
            dict.TryAdd(96.00, "Mint Green");
            dict.TryAdd(97.00, "Celadon");
            dict.TryAdd(98.00, "Sage Green");
            dict.TryAdd(99.00, "Olive Green");
            dict.TryAdd(100.00, "Dark Olive Green");
            dict.TryAdd(101.00, "Army Green");
            dict.TryAdd(102.00, "Evergreen");
            dict.TryAdd(103.00, "Spruce Green");
            dict.TryAdd(104.00, "Jungle Green");
            dict.TryAdd(105.00, "Shamrock Green");
            dict.TryAdd(106.00, "Malachite");
            dict.TryAdd(107.00, "Viridian");
            dict.TryAdd(108.00, "Myrtle Green");
            dict.TryAdd(109.00, "Bottle Green");
            dict.TryAdd(110.00, "Cactus Green");
            dict.TryAdd(111.00, "Laurel Green");
            dict.TryAdd(112.00, "Pea Green");
            dict.TryAdd(113.00, "Asparagus Green");
            dict.TryAdd(114.00, "Avocado Green");
            dict.TryAdd(115.00, "Fern Green");
            dict.TryAdd(116.00, "Moss Green");
            dict.TryAdd(117.00, "Bay Leaf Green");
            dict.TryAdd(118.00, "Artichoke Green");
            dict.TryAdd(119.00, "Forest Moss");

            // GREEN-CYANS (120-179) - Transition from Green to Cyan
            dict.TryAdd(120.00, "Lime");
            dict.TryAdd(121.00, "Lime Cyan");
            dict.TryAdd(122.00, "Bright Teal Green");
            dict.TryAdd(123.00, "Aqua Green Blue");
            dict.TryAdd(124.00, "Sea Foam Green");
            dict.TryAdd(125.00, "Lagoon Green");
            dict.TryAdd(126.00, "Emerald Cyan");
            dict.TryAdd(127.00, "Jade Cyan");
            dict.TryAdd(128.00, "Forest Cyan");
            dict.TryAdd(129.00, "Pine Cyan");
            dict.TryAdd(130.00, "Hunter Cyan");
            dict.TryAdd(131.00, "Deep Cyan Green");
            dict.TryAdd(132.00, "Viridian Cyan");
            dict.TryAdd(133.00, "Bottle Cyan");
            dict.TryAdd(134.00, "Cactus Cyan");
            dict.TryAdd(135.00, "Laurel Cyan");
            dict.TryAdd(136.00, "Pea Cyan");
            dict.TryAdd(137.00, "Asparagus Cyan");
            dict.TryAdd(138.00, "Avocado Cyan");
            dict.TryAdd(139.00, "Fern Cyan");
            dict.TryAdd(140.00, "Seafoam Green");
            dict.TryAdd(141.00, "Marine Green");
            dict.TryAdd(142.00, "Caribbean Aqua");
            dict.TryAdd(143.00, "Tropical Teal");
            dict.TryAdd(144.00, "Vivid Turquoise");
            dict.TryAdd(145.00, "Bright Aqua");
            dict.TryAdd(146.00, "Electric Teal");
            dict.TryAdd(147.00, "Deep Sea Green");
            dict.TryAdd(148.00, "Pacific Aqua");
            dict.TryAdd(149.00, "Sky Green Blue");
            dict.TryAdd(150.00, "Azure Green");
            dict.TryAdd(151.00, "Cerulean Green");
            dict.TryAdd(152.00, "Powder Aqua");
            dict.TryAdd(153.00, "Baby Aqua");
            dict.TryAdd(154.00, "Cornflower Aqua");
            dict.TryAdd(155.00, "Periwinkle Aqua");
            dict.TryAdd(156.00, "Steel Aqua");
            dict.TryAdd(157.00, "Denim Aqua");
            dict.TryAdd(158.00, "Slate Aqua");
            dict.TryAdd(159.00, "Cobalt Aqua");
            dict.TryAdd(160.00, "Royal Aqua");
            dict.TryAdd(161.00, "Sapphire Aqua");
            dict.TryAdd(162.00, "Navy Aqua");
            dict.TryAdd(163.00, "Midnight Aqua");
            dict.TryAdd(164.00, "Indigo Aqua");
            dict.TryAdd(165.00, "Ultramarine Aqua");
            dict.TryAdd(166.00, "Prussian Aqua");
            dict.TryAdd(167.00, "Aegean Aqua");
            dict.TryAdd(168.00, "Lake Aqua");
            dict.TryAdd(169.00, "River Aqua");
            dict.TryAdd(170.00, "True Cyan");
            dict.TryAdd(171.00, "Pure Cyan");
            dict.TryAdd(172.00, "Vivid Cyan");
            dict.TryAdd(173.00, "Electric Cyan");
            dict.TryAdd(174.00, "Neon Cyan");
            dict.TryAdd(175.00, "Deep Cyan");
            dict.TryAdd(176.00, "Rich Cyan");
            dict.TryAdd(177.00, "Bright Cyan");
            dict.TryAdd(178.00, "Shimmering Cyan");
            dict.TryAdd(179.00, "Gleaming Cyan");

            // BLUE-CYANS (180-239) - Transition from Cyan to Blue
            dict.TryAdd(180.00, "Aqua");
            dict.TryAdd(181.00, "Sky Blue");
            dict.TryAdd(182.00, "Light Blue");
            dict.TryAdd(183.00, "Powder Blue");
            dict.TryAdd(184.00, "Baby Blue");
            dict.TryAdd(185.00, "Cornflower Blue");
            dict.TryAdd(186.00, "Periwinkle Blue");
            dict.TryAdd(187.00, "Steel Blue");
            dict.TryAdd(188.00, "Denim Blue");
            dict.TryAdd(189.00, "Slate Blue");
            dict.TryAdd(190.00, "Cobalt Blue");
            dict.TryAdd(191.00, "Royal Blue");
            dict.TryAdd(192.00, "Sapphire Blue");
            dict.TryAdd(193.00, "Navy Blue");
            dict.TryAdd(194.00, "Midnight Blue");
            dict.TryAdd(195.00, "Indigo Blue");
            dict.TryAdd(196.00, "Ultramarine Blue");
            dict.TryAdd(197.00, "Prussian Blue");
            dict.TryAdd(198.00, "Azure Blue");
            dict.TryAdd(199.00, "Cerulean Blue");
            dict.TryAdd(200.00, "Ocean Blue");
            dict.TryAdd(201.00, "Lake Blue");
            dict.TryAdd(202.00, "River Blue");
            dict.TryAdd(203.00, "Sea Blue");
            dict.TryAdd(204.00, "Dark Blue");
            dict.TryAdd(205.00, "Deep Blue");
            dict.TryAdd(206.00, "Rich Blue");
            dict.TryAdd(207.00, "Vivid Blue Tint");
            dict.TryAdd(208.00, "Electric Blue");
            dict.TryAdd(209.00, "Cobalt Deep Blue");
            dict.TryAdd(210.00, "Sapphire Deep Blue");
            dict.TryAdd(211.00, "Indigo Deep Blue");
            dict.TryAdd(212.00, "Ultramarine Deep Blue");
            dict.TryAdd(213.00, "Midnight Deep Blue");
            dict.TryAdd(214.00, "Navy Deep Blue");
            dict.TryAdd(215.00, "Royal Deep Blue");
            dict.TryAdd(216.00, "Twilight Blue");
            dict.TryAdd(217.00, "Dusk Blue");
            dict.TryAdd(218.00, "Shadow Blue");
            dict.TryAdd(219.00, "Ink Blue");
            dict.TryAdd(220.00, "Abyss Blue");
            dict.TryAdd(221.00, "Dark Denim Blue");
            dict.TryAdd(222.00, "Dark Slate Blue");
            dict.TryAdd(223.00, "Stone Blue");
            dict.TryAdd(224.00, "Blue Violet");
            dict.TryAdd(225.00, "Periwinkle Violet");
            dict.TryAdd(226.00, "Lavender Blue");
            dict.TryAdd(227.00, "Amethyst Blue");
            dict.TryAdd(228.00, "Iris Blue");
            dict.TryAdd(229.00, "Orchid Blue");
            dict.TryAdd(230.00, "Plum Blue");
            dict.TryAdd(231.00, "Grape Blue");
            dict.TryAdd(232.00, "Indigo Violet");
            dict.TryAdd(233.00, "Electric Blue Violet");
            dict.TryAdd(234.00, "Deep Violet Blue");
            dict.TryAdd(235.00, "Royal Blue Violet");
            dict.TryAdd(236.00, "Byzantine Blue Violet");
            dict.TryAdd(237.00, "Tyrian Blue Violet");
            dict.TryAdd(238.00, "Mulberry Blue Violet");
            dict.TryAdd(239.00, "Wine Blue Violet");

            // BLUES (240-269) - Pure Blue to Blue-Purple
            dict.TryAdd(240.00, "Blue");
            dict.TryAdd(241.00, "True Blue");
            dict.TryAdd(242.00, "Vivid Blue Shade");
            dict.TryAdd(243.00, "Bright Blue");
            dict.TryAdd(244.00, "Electric Sapphire");
            dict.TryAdd(245.00, "Midnight Sapphire");
            dict.TryAdd(246.00, "Regal Blue");
            dict.TryAdd(247.00, "Rich Cobalt");
            dict.TryAdd(248.00, "Ultramarine");
            dict.TryAdd(249.00, "Indigo");
            dict.TryAdd(250.00, "Deep Violet");
            dict.TryAdd(251.00, "Amethyst Violet");
            dict.TryAdd(252.00, "Lavender Violet");
            dict.TryAdd(253.00, "Thistle Violet");
            dict.TryAdd(254.00, "Lilac Violet");
            dict.TryAdd(255.00, "Orchid Violet");
            dict.TryAdd(256.00, "Plum Violet");
            dict.TryAdd(257.00, "Grape Violet");
            dict.TryAdd(258.00, "Electric Violet");
            dict.TryAdd(259.00, "Deep Purple");
            dict.TryAdd(260.00, "Royal Purple");
            dict.TryAdd(261.00, "Byzantine Purple");
            dict.TryAdd(262.00, "Tyrian Purple");
            dict.TryAdd(263.00, "Mulberry Purple");
            dict.TryAdd(264.00, "Wine Purple");
            dict.TryAdd(265.00, "Burgundy Purple");
            dict.TryAdd(266.00, "Eggplant");
            dict.TryAdd(267.00, "Mauve");
            dict.TryAdd(268.00, "Iris Purple");
            dict.TryAdd(269.00, "Violet Red");

            // MAGENTAS (270-329) - Purple-Red to Pure Magenta
            dict.TryAdd(270.00, "Magenta Violet");
            dict.TryAdd(271.00, "Fuchsia Violet");
            dict.TryAdd(272.00, "Rose Violet");
            dict.TryAdd(273.00, "Pink Violet");
            dict.TryAdd(274.00, "Hot Pink Violet");
            dict.TryAdd(275.00, "Cerise Violet");
            dict.TryAdd(276.00, "Raspberry Violet");
            dict.TryAdd(277.00, "Ruby Violet");
            dict.TryAdd(278.00, "Crimson Violet");
            dict.TryAdd(279.00, "Scarlet Violet");
            dict.TryAdd(280.00, "True Magenta");
            dict.TryAdd(281.00, "Vivid Magenta");
            dict.TryAdd(282.00, "Bright Magenta");
            dict.TryAdd(283.00, "Electric Magenta");
            dict.TryAdd(284.00, "Neon Magenta");
            dict.TryAdd(285.00, "Deep Magenta");
            dict.TryAdd(286.00, "Rich Magenta");
            dict.TryAdd(287.00, "Plum Magenta");
            dict.TryAdd(288.00, "Grape Magenta");
            dict.TryAdd(289.00, "Wine Magenta");
            dict.TryAdd(290.00, "Bright Pink");
            dict.TryAdd(291.00, "Hot Pink");
            dict.TryAdd(292.00, "Rose Red");
            dict.TryAdd(293.00, "Cerise");
            dict.TryAdd(294.00, "Raspberry");
            dict.TryAdd(295.00, "Ruby Pink");
            dict.TryAdd(296.00, "Crimson Pink");
            dict.TryAdd(297.00, "Deep Pink");
            dict.TryAdd(298.00, "Shocking Pink");
            dict.TryAdd(299.00, "Barbie Pink");
            dict.TryAdd(300.00, "Fuchsia");
            dict.TryAdd(301.00, "Bubblegum Pink");
            dict.TryAdd(302.00, "Carnation Pink");
            dict.TryAdd(303.00, "Coral Pink");
            dict.TryAdd(304.00, "Salmon Pink");
            dict.TryAdd(305.00, "Peach Pink");
            dict.TryAdd(306.00, "Blush Pink");
            dict.TryAdd(307.00, "Rose Quartz");
            dict.TryAdd(308.00, "Dusty Rose");
            dict.TryAdd(309.00, "Thulian Pink");
            dict.TryAdd(310.00, "Cerise Red");
            dict.TryAdd(311.00, "Light Rose Red");
            dict.TryAdd(312.00, "Baby Rose Red");
            dict.TryAdd(313.00, "Cherry Rose");
            dict.TryAdd(314.00, "Strawberry Red");
            dict.TryAdd(315.00, "Apple Red");
            dict.TryAdd(316.00, "Watermelon Red");
            dict.TryAdd(317.00, "Cranberry Red");
            dict.TryAdd(318.00, "Raspberry Red Tinge");
            dict.TryAdd(319.00, "Currant Red");
            dict.TryAdd(320.00, "Wine Red");
            dict.TryAdd(321.00, "Burgundy Red");
            dict.TryAdd(322.00, "Maroon Red");
            dict.TryAdd(323.00, "Garnet Red");
            dict.TryAdd(324.00, "Terracotta Brick");
            dict.TryAdd(325.00, "Rust Red");
            dict.TryAdd(326.00, "Copper Red");
            dict.TryAdd(327.00, "Bronze Red");
            dict.TryAdd(328.00, "Mahogany Red");
            dict.TryAdd(329.00, "Sienna Red");

            // REDS (330-359) - Returning to Red
            dict.TryAdd(330.00, "Terracotta Red");
            dict.TryAdd(331.00, "Raspberry Punch");
            dict.TryAdd(332.00, "Electric Pink Red");
            dict.TryAdd(333.00, "Vivid Cerise");
            dict.TryAdd(334.00, "Dragon Fruit Red");
            dict.TryAdd(335.00, "Melon Red");
            dict.TryAdd(336.00, "Sunset Red");
            dict.TryAdd(337.00, "Tangerine Red");
            dict.TryAdd(338.00, "Pumpkin Red");
            dict.TryAdd(339.00, "Flame Red");
            dict.TryAdd(340.00, "Fiery Red");
            dict.TryAdd(341.00, "Blazing Red Hue");
            dict.TryAdd(342.00, "Volcanic Red");
            dict.TryAdd(343.00, "Lava Red");
            dict.TryAdd(344.00, "Hot Red");
            dict.TryAdd(345.00, "Vivid Red");
            dict.TryAdd(346.00, "Bright Red");
            dict.TryAdd(347.00, "Signal Red");
            dict.TryAdd(348.00, "Post Office Red");
            dict.TryAdd(349.00, "Fire Engine Red");
            dict.TryAdd(350.00, "Blood Red");
            dict.TryAdd(351.00, "Pillarbox Red");
            dict.TryAdd(352.00, "Barn Red");
            dict.TryAdd(353.00, "Cedar Red");
            dict.TryAdd(354.00, "Russet Red");
            dict.TryAdd(355.00, "Cinnamon Red");
            dict.TryAdd(356.00, "Spice Red");
            dict.TryAdd(357.00, "Burnt Chili");
            dict.TryAdd(358.00, "Pepper Red");
            dict.TryAdd(359.00, "Salsa Red");
            dict.TryAdd(360.00, "Deep Crimson");
            return dict;
        }

        /// <summary>
        /// HSV/HSB/HSL - HUE
        /// HUE INTERPOLATION ENTRIES (0 to 360) DEGREES
        /// </summary>
        private static readonly Dictionary<double, string> _hueInterpolations = new Dictionary<double, string>() 
        {
            // REDS (0-19) - Warm, Fiery
            { 0.00, "Red" }, { 1.00, "Crimson Fire" }, { 2.00, "Ember Red" }, { 3.00, "Blazing Red" }, { 4.00, "Ruby Red" },
            { 5.00, "Tomato Red" }, { 6.00, "Scarlet Red" }, { 7.00, "Cardinal Red" }, { 8.00, "Chili Red" }, { 9.00, "Cherry Red" },
            { 10.00, "Vermilion" }, { 11.00, "Firebrick Red" }, { 12.00, "Persian Red" }, { 13.00, "Brick Red" }, { 14.00, "Terra Cotta" },
            { 15.00, "Indian Red" }, { 16.00, "Rusty Red" }, { 17.00, "Cinnabar Red" }, { 18.00, "Vivid Orange Red" }, { 19.00, "Bright Red Orange" },

            // RED-ORANGES (20-39) - Warm, Earthy
            { 20.00, "Red Orange" }, { 21.00, "Crimson Orange" }, { 22.00, "Copper Orange" }, { 23.00, "Fiery Amber" }, { 24.00, "Lava Orange" },
            { 25.00, "Persimmon" }, { 26.00, "Pumpkin Orange" }, { 27.00, "Zesty Orange" }, { 28.00, "Burnt Orange" }, { 29.00, "Honey Orange" },
            { 30.00, "Orange" }, { 31.00, "Tangerine" }, { 32.00, "Apricot Orange" }, { 33.00, "Marigold Orange" }, { 34.00, "Golden Orange" },
            { 35.00, "Saffron Orange" }, { 36.00, "Flame Orange" }, { 37.00, "Sunset Orange" }, { 38.00, "Bronze Orange" }, { 39.00, "Coral Orange" },

            // ORANGES (40-59) - Bright, Sunny
            { 40.00, "Golden Yellow Orange" }, { 41.00, "Bright Orange" }, { 42.00, "Honey Gold Orange" }, { 43.00, "Dandelion Orange" }, { 44.00, "Vivid Orange" },
            { 45.00, "Mandarin Orange" }, { 46.00, "Sunny Orange" }, { 47.00, "Sunray Orange" }, { 48.00, "Butterscotch" }, { 49.00, "Cantaloupe Orange" },
            { 50.00, "Sunshine Orange" }, { 51.00, "Amber Orange" }, { 52.00, "Light Amber" }, { 53.00, "Soft Orange" }, { 54.00, "Peach Orange" },
            { 55.00, "Creme Brulee" }, { 56.00, "Apricot Blush" }, { 57.00, "Melon Orange" }, { 58.00, "Fawn Orange" }, { 59.00, "Bisque Orange" },

            // YELLOW-GREENS (60-79) - Warm Yellows moving to green
            { 60.00, "Yellow" }, { 61.00, "Goldenrod" }, { 62.00, "Honey Gold" }, { 63.00, "Light Gold" }, { 64.00, "Bright Marigold" },
            { 65.00, "Sunburst Yellow" }, { 66.00, "Amber Yellow" }, { 67.00, "Desert Gold" }, { 68.00, "Mustard Yellow" }, { 69.00, "Antique Gold" },
            { 70.00, "Harvest Gold" }, { 71.00, "Dijon Yellow" }, { 72.00, "Chartreuse Yellow" }, { 73.00, "Lime Yellow" }, { 74.00, "Olive Yellow" },
            { 75.00, "Moss Yellow" }, { 76.00, "Khaki Yellow" }, { 77.00, "Pistachio Yellow" }, { 78.00, "Sage Yellow" }, { 79.00, "Asparagus Yellow" },

            // GREENS (80-119) - Diverse Greens
            { 80.00, "Spring Green" }, { 81.00, "Lime Green" }, { 82.00, "Bright Green" }, { 83.00, "Leaf Green" }, { 84.00, "Emerald Green" },
            { 85.00, "Grass Green" }, { 86.00, "Forest Green" }, { 87.00, "Pine Green" }, { 88.00, "Hunter Green" }, { 89.00, "Deep Green" },
            { 90.00, "Kelly Green" }, { 91.00, "Jade Green" }, { 92.00, "Sea Green" }, { 93.00, "Ocean Green" }, { 94.00, "Teal Green" },
            { 95.00, "Aqua Green" }, { 96.00, "Mint Green" }, { 97.00, "Celadon" }, { 98.00, "Sage Green" }, { 99.00, "Olive Green" },
            { 100.00, "Dark Olive Green" }, { 101.00, "Army Green" }, { 102.00, "Evergreen" }, { 103.00, "Spruce Green" }, { 104.00, "Jungle Green" },
            { 105.00, "Shamrock Green" }, { 106.00, "Malachite" }, { 107.00, "Viridian" }, { 108.00, "Myrtle Green" }, { 109.00, "Bottle Green" },
            { 110.00, "Cactus Green" }, { 111.00, "Laurel Green" }, { 112.00, "Pea Green" }, { 113.00, "Asparagus Green" }, { 114.00, "Avocado Green" },
            { 115.00, "Fern Green" }, { 116.00, "Moss Green" }, { 117.00, "Bay Leaf Green" }, { 118.00, "Artichoke Green" }, { 119.00, "Forest Moss" },

            // GREEN-CYANS (120-179) - Transition from Green to Cyan
            { 120.00, "Lime" }, { 121.00, "Lime Cyan" }, { 122.00, "Bright Teal Green" }, { 123.00, "Aqua Green Blue" }, { 124.00, "Sea Foam Green" }, { 125.00, "Lagoon Green" },
            { 126.00, "Emerald Cyan" }, { 127.00, "Jade Cyan" }, { 128.00, "Forest Cyan" }, { 129.00, "Pine Cyan" }, { 130.00, "Hunter Cyan" },
            { 131.00, "Deep Cyan Green" }, { 132.00, "Viridian Cyan" }, { 133.00, "Bottle Cyan" }, { 134.00, "Cactus Cyan" }, { 135.00, "Laurel Cyan" },
            { 136.00, "Pea Cyan" }, { 137.00, "Asparagus Cyan" }, { 138.00, "Avocado Cyan" }, { 139.00, "Fern Cyan" }, { 140.00, "Seafoam Green" },
            { 141.00, "Marine Green" }, { 142.00, "Caribbean Aqua" }, { 143.00, "Tropical Teal" }, { 144.00, "Vivid Turquoise" }, { 145.00, "Bright Aqua" },
            { 146.00, "Electric Teal" }, { 147.00, "Deep Sea Green" }, { 148.00, "Pacific Aqua" }, { 149.00, "Sky Green Blue" }, { 150.00, "Azure Green" },
            { 151.00, "Cerulean Green" }, { 152.00, "Powder Aqua" }, { 153.00, "Baby Aqua" }, { 154.00, "Cornflower Aqua" }, { 155.00, "Periwinkle Aqua" },
            { 156.00, "Steel Aqua" }, { 157.00, "Denim Aqua" }, { 158.00, "Slate Aqua" }, { 159.00, "Cobalt Aqua" }, { 160.00, "Royal Aqua" },
            { 161.00, "Sapphire Aqua" }, { 162.00, "Navy Aqua" }, { 163.00, "Midnight Aqua" }, { 164.00, "Indigo Aqua" }, { 165.00, "Ultramarine Aqua" },
            { 166.00, "Prussian Aqua" }, { 167.00, "Aegean Aqua" }, { 168.00, "Lake Aqua" }, { 169.00, "River Aqua" }, { 170.00, "True Cyan" },
            { 171.00, "Pure Cyan" }, { 172.00, "Vivid Cyan" }, { 173.00, "Electric Cyan" }, { 174.00, "Neon Cyan" }, { 175.00, "Deep Cyan" },
            { 176.00, "Rich Cyan" }, { 177.00, "Bright Cyan" }, { 178.00, "Shimmering Cyan" }, { 179.00, "Gleaming Cyan" },

            // BLUE-CYANS (180-239) - Transition from Cyan to Blue
            { 180.00, "Aqua" }, { 181.00, "Sky Blue" }, { 182.00, "Light Blue" }, { 183.00, "Powder Blue" }, { 184.00, "Baby Blue" },
            { 185.00, "Cornflower Blue" }, { 186.00, "Periwinkle Blue" }, { 187.00, "Steel Blue" }, { 188.00, "Denim Blue" }, { 189.00, "Slate Blue" },
            { 190.00, "Cobalt Blue" }, { 191.00, "Royal Blue" }, { 192.00, "Sapphire Blue" }, { 193.00, "Navy Blue" }, { 194.00, "Midnight Blue" },
            { 195.00, "Indigo Blue" }, { 196.00, "Ultramarine Blue" }, { 197.00, "Prussian Blue" }, { 198.00, "Azure Blue" }, { 199.00, "Cerulean Blue" },
            { 200.00, "Ocean Blue" }, { 201.00, "Lake Blue" }, { 202.00, "River Blue" }, { 203.00, "Sea Blue" }, { 204.00, "Dark Blue" },
            { 205.00, "Deep Blue" }, { 206.00, "Rich Blue" }, { 207.00, "Vivid Blue Tint" }, { 208.00, "Electric Blue" }, { 209.00, "Cobalt Deep Blue" },
            { 210.00, "Sapphire Deep Blue" }, { 211.00, "Indigo Deep Blue" }, { 212.00, "Ultramarine Deep Blue" }, { 213.00, "Midnight Deep Blue" }, { 214.00, "Navy Deep Blue" },
            { 215.00, "Royal Deep Blue" }, { 216.00, "Twilight Blue" }, { 217.00, "Dusk Blue" }, { 218.00, "Shadow Blue" }, { 219.00, "Ink Blue" },
            { 220.00, "Abyss Blue" }, { 221.00, "Dark Denim Blue" }, { 222.00, "Dark Slate Blue" }, { 223.00, "Stone Blue" }, { 224.00, "Blue Violet" },
            { 225.00, "Periwinkle Violet" }, { 226.00, "Lavender Blue" }, { 227.00, "Amethyst Blue" }, { 228.00, "Iris Blue" }, { 229.00, "Orchid Blue" },
            { 230.00, "Plum Blue" }, { 231.00, "Grape Blue" }, { 232.00, "Indigo Violet" }, { 233.00, "Electric Blue Violet" }, { 234.00, "Deep Violet Blue" },
            { 235.00, "Royal Blue Violet" }, { 236.00, "Byzantine Blue Violet" }, { 237.00, "Tyrian Blue Violet" }, { 238.00, "Mulberry Blue Violet" }, { 239.00, "Wine Blue Violet" },

            // BLUES (240-269) - Pure Blue to Blue-Purple
            { 240.00, "Blue" }, { 241.00, "True Blue" }, { 242.00, "Vivid Blue Shade" }, { 243.00, "Bright Blue" }, { 244.00, "Electric Sapphire" },
            { 245.00, "Midnight Sapphire" }, { 246.00, "Regal Blue" }, { 247.00, "Rich Cobalt" }, { 248.00, "Ultramarine" }, { 249.00, "Indigo" },
            { 250.00, "Deep Violet" }, { 251.00, "Amethyst Violet" }, { 252.00, "Lavender Violet" }, { 253.00, "Thistle Violet" }, { 254.00, "Lilac Violet" },
            { 255.00, "Orchid Violet" }, { 256.00, "Plum Violet" }, { 257.00, "Grape Violet" }, { 258.00, "Electric Violet" }, { 259.00, "Deep Purple" },
            { 260.00, "Royal Purple" }, { 261.00, "Byzantine Purple" }, { 262.00, "Tyrian Purple" }, { 263.00, "Mulberry Purple" }, { 264.00, "Wine Purple" },
            { 265.00, "Burgundy Purple" }, { 266.00, "Eggplant" }, { 267.00, "Mauve" }, { 268.00, "Iris Purple" }, { 269.00, "Violet Red" },

            // MAGENTAS (270-329) - Purple-Red to Pure Magenta
            { 270.00, "Magenta Violet" }, { 271.00, "Fuchsia Violet" }, { 272.00, "Rose Violet" }, { 273.00, "Pink Violet" }, { 274.00, "Hot Pink Violet" },
            { 275.00, "Cerise Violet" }, { 276.00, "Raspberry Violet" }, { 277.00, "Ruby Violet" }, { 278.00, "Crimson Violet" }, { 279.00, "Scarlet Violet" },
            { 280.00, "True Magenta" }, { 281.00, "Vivid Magenta" }, { 282.00, "Bright Magenta" }, { 283.00, "Electric Magenta" }, { 284.00, "Neon Magenta" },
            { 285.00, "Deep Magenta" }, { 286.00, "Rich Magenta" }, { 287.00, "Plum Magenta" }, { 288.00, "Grape Magenta" }, { 289.00, "Wine Magenta" },
            { 290.00, "Bright Pink" }, { 291.00, "Hot Pink" }, { 292.00, "Rose Red" }, { 293.00, "Cerise" }, { 294.00, "Raspberry" },
            { 295.00, "Ruby Pink" }, { 296.00, "Crimson Pink" }, { 297.00, "Deep Pink" }, { 298.00, "Shocking Pink" }, { 299.00, "Barbie Pink" },
            { 300.00, "Fuchsia" }, { 301.00, "Bubblegum Pink" }, { 302.00, "Carnation Pink" }, { 303.00, "Coral Pink" }, { 304.00, "Salmon Pink" },
            { 305.00, "Peach Pink" }, { 306.00, "Blush Pink" }, { 307.00, "Rose Quartz" }, { 308.00, "Dusty Rose" }, { 309.00, "Thulian Pink" },
            { 310.00, "Cerise Red" }, { 311.00, "Light Rose Red" }, { 312.00, "Baby Rose Red" }, { 313.00, "Cherry Rose" }, { 314.00, "Strawberry Red" },
            { 315.00, "Apple Red" }, { 316.00, "Watermelon Red" }, { 317.00, "Cranberry Red" }, { 318.00, "Raspberry Red Tinge" }, { 319.00, "Currant Red" },
            { 320.00, "Wine Red" }, { 321.00, "Burgundy Red" }, { 322.00, "Maroon Red" }, { 323.00, "Garnet Red" }, { 324.00, "Terracotta Brick" },
            { 325.00, "Rust Red" }, { 326.00, "Copper Red" }, { 327.00, "Bronze Red" }, { 328.00, "Mahogany Red" }, { 329.00, "Sienna Red" },

            // REDS (330-359) - Returning to Red
            { 330.00, "Terracotta Red" }, { 331.00, "Raspberry Punch" }, { 332.00, "Electric Pink Red" }, { 333.00, "Vivid Cerise" }, { 334.00, "Dragon Fruit Red" },
            { 335.00, "Melon Red" }, { 336.00, "Sunset Red" }, { 337.00, "Tangerine Red" }, { 338.00, "Pumpkin Red" }, { 339.00, "Flame Red" },
            { 340.00, "Fiery Red" }, { 341.00, "Blazing Red Hue" }, { 342.00, "Volcanic Red" }, { 343.00, "Lava Red" }, { 344.00, "Hot Red" },
            { 345.00, "Vivid Red" }, { 346.00, "Bright Red" }, { 347.00, "Signal Red" }, { 348.00, "Post Office Red" }, { 349.00, "Fire Engine Red" },
            { 350.00, "Blood Red" }, { 351.00, "Pillarbox Red" }, { 352.00, "Barn Red" }, { 353.00, "Cedar Red" }, { 354.00, "Russet Red" },
            { 355.00, "Cinnamon Red" }, { 356.00, "Spice Red" }, { 357.00, "Burnt Chili" }, { 358.00, "Pepper Red" }, { 359.00, "Salsa Red" },
            { 360.00, "Deep Crimson" }
        };

        /// <summary>
        /// HSV/HSB - Saturation
        /// SATURATION INTERPOLATION ENTRIES (0 to 100) PERCENTAGE
        /// </summary>
        private static readonly Dictionary<double, string> _hsvSaturationInterpolations = new Dictionary<double, string>()
        {
	        // VERY LOW SATURATION (0-10) - Achromatic to barely perceptible color
	        { 0.00, "Achromatic" }, { 1.00, "Monochromatic" }, { 2.00, "Colorless" }, { 3.00, "Neutral Tint" }, { 4.00, "Subtle Gray" },
            { 5.00, "Barely Tinted" }, { 6.00, "Faint Tone" }, { 7.00, "Whisper of Color" }, { 8.00, "Ghostly" }, { 9.00, "Ashy" }, { 10.00, "Smoky" },

	        // LOW SATURATION (11-25) - Muted, desaturated, soft pastels
	        { 11.00, "Desaturated" }, { 12.00, "Muted" }, { 13.00, "Washed Out" }, { 14.00, "Faded" }, { 15.00, "Pale" },
            { 16.00, "Dusty" }, { 17.00, "Grayed" }, { 18.00, "Softened" }, { 19.00, "Subdued" }, { 20.00, "Tarnished" },
            { 21.00, "Gentle" }, { 22.00, "Mild" }, { 23.00, "Tranquil" }, { 24.00, "Delicate" }, { 25.00, "Hazy" },

	        // MEDIUM-LOW SATURATION (26-45) - More color present, but still soft/diluted
	        { 26.00, "Pastel" }, { 27.00, "Soft Hue" }, { 28.00, "Light Tone" }, { 29.00, "Creamy" }, { 30.00, "Ethereal" },
            { 31.00, "Dreamy" }, { 32.00, "Mellow" }, { 33.00, "Restrained" }, { 34.00, "Blended" }, { 35.00, "Subtle Hue" },
            { 36.00, "Undertoned" }, { 37.00, "Dusky Hue" }, { 38.00, "Faintly Colored" }, { 39.00, "Desaturated Tone" }, { 40.00, "Reduced Chroma" },
            { 41.00, "Faded Chroma" }, { 42.00, "Low Chroma" }, { 43.00, "Controlled" }, { 44.00, "Dampened" }, { 45.00, "Subtle Color" },

	        // MEDIUM SATURATION (46-70) - Standard, balanced color intensity
	        { 46.00, "Balanced" }, { 47.00, "Standard" }, { 48.00, "Moderate" }, { 49.00, "Typical" }, { 50.00, "Regular" },
            { 51.00, "Normal" }, { 52.00, "Mid-Chroma" }, { 53.00, "Classic" }, { 54.00, "Crisp" }, { 55.00, "Clean" },
            { 56.00, "Clear" }, { 57.00, "True Color" }, { 58.00, "Solid" }, { 59.00, "Lively" }, { 60.00, "Expressive" },
            { 61.00, "Defining" }, { 62.00, "Pronounced" }, { 63.00, "Distinct" }, { 64.00, "Vibrant Tone" }, { 65.00, "Rich Tone" },
            { 66.00, "Saturated Tone" }, { 67.00, "Bold Tone" }, { 68.00, "Deep Tone" }, { 69.00, "Intense Tone" }, { 70.00, "Strong Tone" },

	        // HIGH SATURATION (71-90) - Bright, intense, powerful
	        { 71.00, "Vibrant" }, { 72.00, "Bold" }, { 73.00, "Bright Chroma" }, { 74.00, "Intense" }, { 75.00, "Strong" },
            { 76.00, "Rich" }, { 77.00, "Deep Chroma" }, { 78.00, "Brilliant Chroma" }, { 79.00, "Radiant Chroma" }, { 80.00, "Luminous Chroma" },
            { 81.00, "Electric Chroma" }, { 82.00, "Energetic" }, { 83.00, "Dynamic" }, { 84.00, "Powerful Chroma" }, { 85.00, "Glowing Chroma" },
            { 86.00, "Shining Chroma" }, { 87.00, "Resplendent Chroma" }, { 88.00, "Incandescent Chroma" }, { 89.00, "Fiery Chroma" }, { 90.00, "Explosive Chroma" },

	        // VERY HIGH SATURATION (91-100) - Maximum color, pure hue
	        { 91.00, "Vivid" }, { 92.00, "Flamboyant" }, { 93.00, "Dazzling Chroma" }, { 94.00, "Blazing Chroma" }, { 95.00, "Spectacular Chroma" },
            { 96.00, "Gleaming Chroma" }, { 97.00, "Pristine Chroma" }, { 98.00, "Unadulterated" }, { 99.00, "Max Saturation" }, { 100.00, "" }
        };

        /// <summary>
        /// HSL - Saturation
        /// SATURATION INTERPOLATION ENTRIES (0 to 100) PERCENTAGE
        /// </summary>
        private static readonly Dictionary<double, string> _hslSaturationInterpolations = new Dictionary<double, string>()
        {
	        // 0-10: Almost gray
	        { 0.00, "Achromatic" }, { 1.00, "Grayscale" }, { 2.00, "Monotone" }, { 3.00, "Colorless" }, { 4.00, "Muted Gray" },
            { 5.00, "Neutral Tint" }, { 6.00, "Subtle Tone" }, { 7.00, "Pale Gray" }, { 8.00, "Tinted Neutral" }, { 9.00, "Dusty" }, { 10.00, "Soft Gray" },

	        // 11-25: Slight tint or pastel tone
	        { 11.00, "Subtle Pastel" }, { 12.00, "Soft Tint" }, { 13.00, "Washed" }, { 14.00, "Hazy Tone" }, { 15.00, "Barely There" },
            { 16.00, "Cloudy Hue" }, { 17.00, "Powdery" }, { 18.00, "Blanched" }, { 19.00, "Light Mist" }, { 20.00, "Airy" },
            { 21.00, "Pale Mist" }, { 22.00, "Muted Pastel" }, { 23.00, "Gentle Hue" }, { 24.00, "Dust-Tint" }, { 25.00, "Feathered Color" },

	        // 26–45: Noticeable but soft colors
	        { 26.00, "Soft Color" }, { 27.00, "Light Hue" }, { 28.00, "Tinted Hue" }, { 29.00, "Delicate" }, { 30.00, "Creamy Tint" },
            { 31.00, "Pastel" }, { 32.00, "Subdued Hue" }, { 33.00, "Gentle Chroma" }, { 34.00, "Whisper Hue" }, { 35.00, "Balanced Tint" },
            { 36.00, "Powder Hue" }, { 37.00, "Dreamlike" }, { 38.00, "Even Hue" }, { 39.00, "Controlled Hue" }, { 40.00, "Moderated" },
            { 41.00, "Toned" }, { 42.00, "Colorwashed" }, { 43.00, "Quiet Chroma" }, { 44.00, "Soft Blend" }, { 45.00, "Subtle Glow" },

	        // 46–70: Mid-tone richness, not yet vivid
	        { 46.00, "Balanced" }, { 47.00, "Present Hue" }, { 48.00, "Typical Hue" }, { 49.00, "Colorful" }, { 50.00, "Moderate Chroma" },
            { 51.00, "Toneful" }, { 52.00, "Real Hue" }, { 53.00, "Standard Hue" }, { 54.00, "True Tone" }, { 55.00, "Crisp Tint" },
            { 56.00, "Clear Hue" }, { 57.00, "Distinct" }, { 58.00, "Defined" }, { 59.00, "Fresh" }, { 60.00, "Clean Hue" },
            { 61.00, "Polished" }, { 62.00, "Tinted Bold" }, { 63.00, "Steady Hue" }, { 64.00, "Chroma Balance" }, { 65.00, "Color Integrity" },
            { 66.00, "Hue Forward" }, { 67.00, "Visible Hue" }, { 68.00, "Stable Chroma" }, { 69.00, "Full Tint" }, { 70.00, "Well-Colored" },

	        // 71–90: Strong hue presence
	        { 71.00, "Color-Rich" }, { 72.00, "Strong Hue" }, { 73.00, "Lively" }, { 74.00, "Vibrant" }, { 75.00, "Bright Hue" },
            { 76.00, "Intense Hue" }, { 77.00, "Rich Hue" }, { 78.00, "Dynamic" }, { 79.00, "Robust" }, { 80.00, "Powerful Hue" },
            { 81.00, "Fiery" }, { 82.00, "Electric Hue" }, { 83.00, "Exuberant" }, { 84.00, "Blazing" }, { 85.00, "Color-Packed" },
            { 86.00, "Resonant Hue" }, { 87.00, "Piercing Chroma" }, { 88.00, "Zinging Hue" }, { 89.00, "Eye-Catching" }, { 90.00, "Explosive Tint" },

	        // 91–100: Full-blown saturation
	        { 91.00, "Hypercolor" }, { 92.00, "Prismatic" }, { 93.00, "Chromatic Max" }, { 94.00, "Unfiltered Hue" }, { 95.00, "Unrestrained" },
            { 96.00, "Maximum Hue" }, { 97.00, "Vivid Peak" }, { 98.00, "Full Chroma" }, { 99.00, "Pure Hue" }, { 100.00, "Absolute Saturation" }
        };

        /// <summary>
        /// HSV/HSB - Value/Brightness
        /// SATURATION INTERPOLATION ENTRIES (0 to 100) PERCENTAGE
        /// </summary>
        private static readonly Dictionary<double, string> _valueInterpolations = new Dictionary<double, string>()
        {
            // VERY LOW VALUE (0-10) - Deepest Dark to near Black
            { 0.00, "Absolute Black" }, { 1.00, "Pitch Dark" }, { 2.00, "Inky Black" }, { 3.00, "Deepest Shadow" }, { 4.00, "Abyssal" },
            { 5.00, "Midnight" }, { 6.00, "Coal Black" }, { 7.00, "Raven Dark" }, { 8.00, "Obsidian" }, { 9.00, "Nightfall" }, { 10.00, "Velvet Dark" },

            // LOW VALUE (11-25) - Very Dark to Dark
            { 11.00, "Deep Dark" }, { 12.00, "Profound Shadow" }, { 13.00, "Murky" }, { 14.00, "Subdued Dark" }, { 15.00, "Twilight" },
            { 16.00, "Somber" }, { 17.00, "Gloomy" }, { 18.00, "Dusk" }, { 19.00, "Faint Shadow" }, { 20.00, "Misty Dark" },
            { 21.00, "Low Tone" }, { 22.00, "Shadowed" }, { 23.00, "Dimmed" }, { 24.00, "Hushed" }, { 25.00, "Subtle Dark" },

            // MEDIUM-LOW VALUE (26-40) - Dark to Medium-Dark
            { 26.00, "Deep Tone" }, { 27.00, "Rich Dark" }, { 28.00, "Warm Dark" }, { 29.00, "Cool Dark" }, { 30.00, "Muted Dark" },
            { 31.00, "Earthy Tone" }, { 32.00, "Subdued Tone" }, { 33.00, "Darkened" }, { 34.00, "Solemn" }, { 35.00, "Deepened" },
            { 36.00, "Shaded" }, { 37.00, "Subtly Dark" }, { 38.00, "Low Light" }, { 39.00, "Cloaked" }, { 40.00, "Veiled" },

            // MEDIUM VALUE (41-60) - Mid-range Brightness
            { 41.00, "Balanced" }, { 42.00, "Standard" }, { 43.00, "Mid-Value" }, { 44.00, "Typical" }, { 45.00, "Normal" },
            { 46.00, "Moderate" }, { 47.00, "Even Tone" }, { 48.00, "Natural" }, { 49.00, "Clear Tone" }, { 50.00, "Average Brightness" },
            { 51.00, "Neutral Tone" }, { 52.00, "Pleasant" }, { 53.00, "Clean Tone" }, { 54.00, "Defined" }, { 55.00, "Open Tone" },
            { 56.00, "Gentle Brightness" }, { 57.00, "Smooth" }, { 58.00, "Well-Lit" }, { 59.00, "Ambient" }, { 60.00, "Crisp Tone" },

            // MEDIUM-HIGH VALUE (61-80) - Medium-Bright to Bright
            { 61.00, "Light Tone" }, { 62.00, "Brightened" }, { 63.00, "Luminous" }, { 64.00, "Radiant" }, { 65.00, "Clear Light" },
            { 66.00, "Glow" }, { 67.00, "Shimmering" }, { 68.00, "Gleaming" }, { 69.00, "Sunlit" }, { 70.00, "Vivid Light" },
            { 71.00, "Bright" }, { 72.00, "Brilliant" }, { 73.00, "Shiny" }, { 74.00, "Sparkling" }, { 75.00, "Dazzling" },
            { 76.00, "Lustrous" }, { 77.00, "Incandescent" }, { 78.00, "Vibrant Light" }, { 79.00, "Beaming" }, { 80.00, "Radiant Glow" },

            // HIGH VALUE (81-95) - Very Bright (acknowledging hue presence)
            { 81.00, "High Key" }, { 82.00, "Very Bright" }, { 83.00, "Pure Light" }, { 84.00, "Blinding Light" }, { 85.00, "Flashing Light" },
            { 86.00, "Intense Brightness" }, { 87.00, "Powerful Light" }, { 88.00, "Piercing Light" }, { 89.00, "Electric Brightness" }, { 90.00, "Vivid Brilliance" },
            { 91.00, "Full Brightness" }, { 92.00, "Peak Brightness" }, { 93.00, "Luminescent Hue" }, { 94.00, "Brilliant Tone" }, { 95.00, "Radiant Tone" },

            // VERY HIGH VALUE (96-100) - Maximum Brightness for a color (or pure white if S=0)
            { 96.00, "Maximum Luminosity" }, { 97.00, "Absolute Brightness" }, { 98.00, "Utmost Radiance" }, { 99.00, "Vibrant Apex" }, { 100.00, "Full Chroma Brightness" }
        };

        /// <summary>
        /// HSL - Lightness
        /// SATURATION INTERPOLATION ENTRIES (0 to 100) PERCENTAGE
        /// </summary>
        private static readonly Dictionary<double, string> _lightnessInterpolations = new Dictionary<double, string>()
        {
	        // 0–10: Pure darkness
	        { 0.00, "Black" }, { 1.00, "Absolute Shadow" }, { 2.00, "Pitch Black" }, { 3.00, "Opaque Shadow" }, { 4.00, "Deep Black" },
            { 5.00, "Midnight Depth" }, { 6.00, "Dark Matter" }, { 7.00, "Raven Black" }, { 8.00, "Ink Shadow" }, { 9.00, "Charred" }, { 10.00, "Onyx" },

	        // 11–25: Very dark tones
	        { 11.00, "Dusky" }, { 12.00, "Low Light" }, { 13.00, "Shaded" }, { 14.00, "Subdued" }, { 15.00, "Gloomy" },
            { 16.00, "Dimmed Hue" }, { 17.00, "Muted Dark" }, { 18.00, "Faint Glow" }, { 19.00, "Smoky Tint" }, { 20.00, "Twilight" },
            { 21.00, "Shadowed Color" }, { 22.00, "Dust-Dark" }, { 23.00, "Darkened Tint" }, { 24.00, "Hushed Tone" }, { 25.00, "Low-Glow" },

	        // 26–40: Transition from shadow to midtones
	        { 26.00, "Deep Tone" }, { 27.00, "Rich Darkness" }, { 28.00, "Dull Light" }, { 29.00, "Sooty Tint" }, { 30.00, "Dusky Glow" },
            { 31.00, "Dim Hue" }, { 32.00, "Browned Tone" }, { 33.00, "Shadow Hue" }, { 34.00, "Warm Gray" }, { 35.00, "Faded Hue" },
            { 36.00, "Misty Hue" }, { 37.00, "Low Contrast" }, { 38.00, "Overcast Hue" }, { 39.00, "Subdued Mid" }, { 40.00, "Muted Glow" },

	        // 41–60: True midtones and full-color range
	        { 41.00, "Balanced Tone" }, { 42.00, "Even Hue" }, { 43.00, "Typical Lightness" }, { 44.00, "Stable Color" }, { 45.00, "True Chroma" },
            { 46.00, "Core Hue" }, { 47.00, "Base Light" }, { 48.00, "Middle Tone" }, { 49.00, "Neutral Tone" }, { 50.00, "Pure Tone" },
            { 51.00, "Full Hue" }, { 52.00, "True Lightness" }, { 53.00, "Standard Tint" }, { 54.00, "Color Peak" }, { 55.00, "Color Zenith" },
            { 56.00, "Mid-Chroma" }, { 57.00, "Vivid Base" }, { 58.00, "Rich Hue" }, { 59.00, "Unclouded" }, { 60.00, "Center Brightness" },

	        // 61–80: Brighter tones approaching white
	        { 61.00, "Brightened" }, { 62.00, "Soft Glow" }, { 63.00, "Gentle Light" }, { 64.00, "Tinted Light" }, { 65.00, "Blushed Hue" },
            { 66.00, "Light Touch" }, { 67.00, "High Key" }, { 68.00, "Soft Radiance" }, { 69.00, "Mild Glow" }, { 70.00, "Sunlit Hue" },
            { 71.00, "Pastel Tone" }, { 72.00, "Silken Tint" }, { 73.00, "Ethereal Hue" }, { 74.00, "Airy Tint" }, { 75.00, "Clouded Color" },
            { 76.00, "Dreamlight" }, { 77.00, "Pale Wash" }, { 78.00, "Lightened Hue" }, { 79.00, "Snow-Tint" }, { 80.00, "Powder Hue" },

	        // 81–95: Near-white with tint
	        { 81.00, "Frosted Tint" }, { 82.00, "Bright Wash" }, { 83.00, "Blanched Hue" }, { 84.00, "Pearl Tone" }, { 85.00, "Faint Pastel" },
            { 86.00, "Barely Tinted" }, { 87.00, "Ghost Hue" }, { 88.00, "Mist Hue" }, { 89.00, "Paper Tone" }, { 90.00, "White Tint" },
            { 91.00, "Washed White" }, { 92.00, "Tinted White" }, { 93.00, "Bright Mist" }, { 94.00, "Filtered White" }, { 95.00, "Bleached Hue" },

	        // 96–100: Approaching full white
	        { 96.00, "Pale Glow" }, { 97.00, "Luminous White" }, { 98.00, "Radiant White" }, { 99.00, "Blinding White" }, { 100.00, "Pure White" }
        };

        /// <summary>
        /// Hue - Temperature
        /// TEMPERATURE INTERPOLATION ENTRIES (0 to 360) PERCENTAGE
        /// if (h < 0.04 || h > 0.92) return "Hot";             // Reds
        /// if (h >= 0.04 && h < 0.17) return "Warm";           // Oranges, Yellows
        /// if (h >= 0.17 && h < 0.42) return "Neutral-Warm";   // Yellow-green to green
        /// if (h >= 0.42 && h < 0.58) return "Cool";           // Cyan to blue
        /// if (h >= 0.58 && h < 0.75) return "Cold";           // Indigo range
        /// return "Neutral-Cool";                              // Magenta-violet transition
        /// 
        /// </summary>
        private static readonly Dictionary<double, string> _temperatureInterpolations = new Dictionary<double, string>()
        {
            {   0.0, "Hot" },              // Red (0°)
            {  30.0, "Warm" },             // Orange (~30°)
            {  60.0, "Warm" },             // Yellow (~60°)
            {  90.0, "Neutral-Warm" },     // Yellow-Green (~90°)
            { 120.0, "Neutral" },          // Green (120°)
            { 150.0, "Neutral-Cool" },     // Green-Cyan (~150°)
            { 180.0, "Cool" },             // Cyan (180°)
            { 210.0, "Cool" },             // Blue-Cyan (~210°)
            { 240.0, "Cold" },             // Blue (240°)
            { 270.0, "Cold" },             // Indigo/Violet (~270°)
            { 300.0, "Neutral-Warm" },     // Magenta (~300°)
            { 330.0, "Hot" },              // Red-Magenta (~330°)
            { 360.0, "Hot" }               // Red (360°, wraparound)
        };

        /// <summary>
        /// HSV/HSB/HSL - Hue
        /// HUE INTERPOLATION ENTRIES (0 to 360) DEGREES
        /// </summary>
        internal static Dictionary<double, string> HueMapper { get; } = _hueInterpolations;
        internal static Dictionary<double, string> SaturationHSVMapper { get; } = _hsvSaturationInterpolations;
        internal static Dictionary<double, string> SaturationHSLMapper { get; } = _hslSaturationInterpolations;
        internal static Dictionary<double, string> ValueMapper { get; } = _valueInterpolations;
        internal static Dictionary<double, string> LightnessMapper { get; } = _lightnessInterpolations;
        internal static Dictionary<double, string> TemperatureMapper { get; } = _temperatureInterpolations;

        /// <summary>
        /// Hue - Tone
        ///     - possibly add Hue in the future.
        ///       new ToneRule(15, (s, v, h) => s < 20 && v > 85 && h > 30 && h < 60, "Warm Light Neutral", ...)
        /// </summary>
        private static readonly List<ToneRule> _toneRules = new List<ToneRule>()
        {
            // NEUTRALS (Lowest Saturation: s < 20%)
            new ToneRule(1, (s, v) => s < 5.0 && v >= 95.0, "Near White", "Sat < 5, Val >= 95"),
            new ToneRule(2, (s, v) => s < 20.0 && v > 85.0, "Light Neutral", "Sat < 20, Val > 85"),
            new ToneRule(3, (s, v) => s < 20.0 && v > 55.0, "Mid Neutral", "Sat < 20, Val > 55"),
            new ToneRule(4, (s, v) => s < 20.0 && v > 20.0, "Dark Neutral", "Sat < 20, Val > 20"),
            new ToneRule(5, (s, v) => s < 5.0 && v <= 5.0, "Black", "Sat < 5, Val <= 5"),
            new ToneRule(6, (s, v) => s < 20.0 && v <= 20.0, "Near Black", "Sat < 20, Val <= 20"),

            // HIGH BRIGHTNESS (Value > 85%)
            new ToneRule(7, (s, v) => v > 85.0 && s > 75.0, "Vivid", "Val > 85, Sat > 75"),
            new ToneRule(8, (s, v) => v > 85.0 && s >= 20.0, "Pastel", "Val > 85, Sat >= 20"),
            new ToneRule(9, (s, v) => v > 85.0, "Bright", "Val > 85"),

            // MID BRIGHTNESS (Value > 55%)
            new ToneRule(10, (s, v) => v > 55.0 && s > 75.0, "Strong", "Val > 55, Sat > 75"),
            new ToneRule(11, (s, v) => v > 55.0 && s >= 20.0, "Moderate", "Val > 55, Sat >= 20"),
            new ToneRule(12, (s, v) => v > 55.0, "Muted Light", "Val > 55"),

            // LOW BRIGHTNESS (Value > 20%)
            new ToneRule(13, (s, v) => v > 20.0 && s > 60.0, "Deep", "Val > 20, Sat > 60"),
            new ToneRule(14, (s, v) => v > 20.0 && s >= 20.0, "Dull", "Val > 20, Sat >= 20"),
            new ToneRule(15, (s, v) => v > 20.0, "Washed Out", "Val > 20"),

            // VERY DARK (Value <= 20%)
            new ToneRule(16, (s, v) => v <= 10.0, "Black", "Val <= 10"),
            new ToneRule(17, (s, v) => v > 10.0 && v <= 20.0, "Very Dark", "10 < Val <= 20"),
        };

        /// <summary>
        /// CMYK - Rules Modifier to get back Text
        /// </summary>
        private static readonly List<CmykRule> _cmykRules = new List<CmykRule>()
        {
            // High Black (K) values
            new CmykRule ( 1, (c, m, y, k) => k > 95, "Deep Inky Black", "Key > 95" ),
            new CmykRule ( 2, (c, m, y, k) => k > 85, "Rich Blackened", "Key > 85" ),
            new CmykRule ( 3, (c, m, y, k) => k > 70, "Darkened", "Key > 70" ),

            // High single CMY values with low others
            new CmykRule ( 4, (c, m, y, k) => c > 90 && m < 15 && y < 15, "Vivid Cyan", "Cyan > 90, Magenta < 15, Yellow < 15" ),
            new CmykRule ( 5, (c, m, y, k) => m > 90 && c < 15 && y < 15, "Vivid Magenta", "Magenta > 90, Cyan < 15, Yellow < 15" ),
            new CmykRule ( 6, (c, m, y, k) => y > 90 && c < 15 && m < 15, "Vivid Yellow", "Yellow > 90, Cyan < 15, Magenta < 15" ),

            new CmykRule ( 7, (c, m, y, k) => c > 70 && m < 20 && y < 20, "Strong Cyan", "Cyan > 70, Magenta < 20, Yellow < 20" ),
            new CmykRule ( 8, (c, m, y, k) => m > 70 && c < 20 && y < 20, "Strong Magenta", "Magenta > 70, Cyan < 20, Yellow < 20" ),
            new CmykRule ( 9, (c, m, y, k) => y > 70 && c < 20 && m < 20, "Strong Yellow", "Yellow > 70, Cyan < 20, Magenta < 20" ),

            // Dominant CMY combinations
            new CmykRule ( 10, (c, m, y, k) => c > 60 && m > 60 && y < 20, "Deep Blue Violet", "Cyan > 60, Magenta > 60, Yellow < 20" ),   //High Cyan + Magenta
            new CmykRule ( 11, (c, m, y, k) => c > 60 && y > 60 && m < 20, "Rich Green", "Cyan > 60, Yellow > 60, Magenta < 20" ),         // High Cyan + Yellow
            new CmykRule ( 12, (c, m, y, k) => m > 60 && y > 60 && c < 20, "Fiery Red Orange", "Magenta > 60, Yellow > 60, Cyan < 20" ),   // High Magenta + Yellow

            // Muted or balanced tones
            new CmykRule ( 13, (c, m, y, k) => Math.Abs(c - m) < 15 && Math.Abs(m - y) < 15 && k < 30, "Neutral Gray", "Abs(Cyan - Magenta) < 15, Abs(Magenta - Yellow) < 15, Key < 30" ),
            new CmykRule ( 14, (c, m, y, k) => k > 40 && Math.Abs(c - m) < 20 && Math.Abs(m - y) < 20, "Smoky Taupe", "Key > 40, Abs(Cyan - Magenta) < 20, Abs(Magenta - Yellow) < 20" ),

            // Light or pastel tones (low CMY, low K)
            new CmykRule ( 15, (c, m, y, k) => Math.Max(c, Math.Max(m, y)) < 30 && k < 10 && Math.Abs(c - m) < 10 && Math.Abs(m - y) < 10, "Pale Gray Tint", "Max(Cyan, Max(Magenta, Yellow)) < 30, Key < 10, Abs(Cyan - Magenta) < 10, Abs(Magenta - y) < 10" ),
            new CmykRule ( 16, (c, m, y, k) => Math.Max(c, Math.Max(m, y)) < 30 && k < 10 && (c > 5 || m > 5 || y > 5), "Pale Tint", "Max(Cyan, Max(Magenta, Yellow)) < 30, Key < 10, (Cyan > 5 || Magenta > 5 || Yellow > 5)" ),

            // Almost white, full CMY mix
            new CmykRule ( 17, (c, m, y, k) => c > 60 && m > 60 && y > 60 && k < 20, "Bright Composite Hue", "Cyan > 60, Magenta > 60, Yellow > 60, Key < 20" ),
            // Or low-K browns
            new CmykRule ( 18, (c, m, y, k) => m > 40 && y > 40 && c < 30 && k > 30, "Burnished Umber", "Magenta > 40, Yellow > 40, Cyan < 30, Key > 30" ),
        };

        /// <summary>
        /// OPTIONAL CMYK-BASED MODIFIER STRATEGY
        /// </summary>
        /// <param name="cmykSpace"></param>
        /// <returns>CMYK Rule, holding details on Rule if found.</returns>
        internal static CmykRule GetCMYKModifier(CmykSpace cmykSpace) => GetCMYKModifier(cmykSpace.Cyan, cmykSpace.Magenta, cmykSpace.Yellow, cmykSpace.RawKey);

        /// <summary>
        /// OPTIONAL CMYK-BASED MODIFIER STRATEGY
        /// </summary>
        /// <param name="c">Cyan decimal value</param>
        /// <param name="m">Magenta decimal value</param>
        /// <param name="y">Yellow decimal value</param>
        /// <param name="k">Key decimal value</param>
        /// <returns>CmykRule object with modifier and rule information.</returns>
        internal static CmykRule GetCMYKModifier(double c, double m, double y, double k)
        {
            foreach (var rule in _cmykRules)
            {
                if (rule.Condition(c, m, y, k))
                {
                    return new CmykRule(rule.RuleNo, rule.Condition, rule.Modifier, rule.RulesDisplay)
                    {
                        Cyan = c,
                        Magenta = m,
                        Yellow = y,
                        Key = k
                    };
                }
            }

            return CmykRule.Empty;
        }

        /// <summary>
        /// TONE-BASED MODIFIER STRATEGY
        /// </summary>
        /// <param name="s"></param>
        /// <param name="v"></param>
        /// <returns>ToneRule object with modifier and rule information.</returns>
        internal static ToneRule GetToneModifier(double s, double v)
        {
            foreach (var rule in _toneRules)
            {
                if (rule.Condition(s, v))
                {
                    return new ToneRule(rule.RuleNo, rule.Condition, rule.Modifier, rule.RulesDisplay)
                    {
                        Saturation = s,
                        Value = v
                    };
                }
            }
            
            return ToneRule.Empty;
        }
    }
}
