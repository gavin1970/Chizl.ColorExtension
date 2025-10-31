using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Chizl.ColorExtension
{
    public static class StandardExtensions
    {
        #region Private Arrays
        private static readonly HashSet<Type> _validBoundaryTypes = new HashSet<Type>
        {
            typeof(byte),
            typeof(short),     // Int16
            typeof(int),       // Int32
            typeof(long),      // Int64
            typeof(float),     // Single
            typeof(double),    // Double
            typeof(decimal)    // Decimal
        };

        private static readonly HashSet<Type> _decimalTypes = new HashSet<Type>
        {
            typeof(float),     // Single
            typeof(double),    // Double
            typeof(decimal)    // Decimal
        };
        #endregion

        #region Public Generic Methods
        /// <summary>
        /// Returns the 'string' name of the Enums value being used.<br/>
        /// <code>
        /// Example:
        ///     var e = MyEnum.Property
        ///     Console.WriteLine($"Enum property name: {e.Name()}");
        /// </code>
        /// </summary>
        /// <returns>string name of enum property</returns>
        public static string Name<T>(this T @this) where T : Enum => $"{@this}";
        /// <summary>
		/// Returns the 'int' value of the Enums value being used.<br/>
        /// <code>
        /// Example:
        ///     var e = MyEnum.Property
        ///     Console.WriteLine($"Enum property value: {e.Value()}");
        /// </code>
        /// </summary>
        /// <returns>int value of enum property</returns>
        public static int Value<T>(this T @this) where T : Enum => (int)(object)@this;
        /// <summary>
        /// Responds with numeric value passed in after forcing value to be within range given from Min to Max.<br/>
        /// If less than Min, Min will be the set value.<br/>
        /// If more than Max, Max will be the set value.
        /// </summary>
        /// <param name="min">Minimum value allowed</param>
        /// <param name="max">Maximum value allowed</param>
        /// <param name="decCount">(Range (0-4): Rounds to specific decimal.<br/>
        /// If the decCount value isn't passed in, is less than 0, or more than 4: Default: 0)</param>
        /// <returns>value forced within range.</returns>
        public static T SetBoundary<T>(this T value, T min, T max, byte decCount = 0) where T : IComparable<T>
        {
            if (!IsSupportedNumeric<T>())
                throw new NotSupportedException($"'{typeof(T).Name}' is not a supported numeric type.\n" +
                                                $"Supported types: ({string.Join(", ", _validBoundaryTypes.Select(s=>s.Name))})");

            //if this return type doesn't have decimal values, and decCount is greater than 0, set to 0;
            if (decCount > 0 && _decimalTypes.Where(w => w.Name.Equals(typeof(T).Name)).Count().Equals(0))
                decCount = 0;

            //validate decimal count based on response type.
            var dec = decCount.ClampTo<byte>(0, 4);
            //set range value
            var retVal = (value.CompareTo(min) < 0) ? min : (value.CompareTo(max) > 0) ? max : value;

            //convert to double for rounding, even if Int.  If Int, dec would of been forced to 0.
            //It will be removed later, but required for Math.Round() to be a decimal type value.
            //this allows for function to be generic across all _validBoundaryTypes
            var conv = (double)Convert.ChangeType(retVal, typeof(double));  

            //validate min and max and round if necessary.
            return (T)Convert.ChangeType(Math.Round(conv, dec), typeof(T));
        }
        /// <summary>
        /// Since netstandard2.0 doesn't have Math.Clamp, this will do it.
        /// </summary>
        /// <typeparam name="T">Generic IComparable type</typeparam>
        /// <param name="value">this var</param>
        /// <param name="min">Miniumum value based on type</param>
        /// <param name="max">Maximum value based on type</param>
        /// <returns>Forced bounds value based on type</returns>
        public static T ClampTo<T>(this T value, T min, T max)
            where T : IComparable<T>
        {
            if (value.CompareTo(min) < 0) return min;
            if (value.CompareTo(max) > 0) return max;
            return value;
        }
        /// <summary>
        /// Uses Regex to validate if string as a whole is HexV8, HexV6, or HexV3, allowing # to be optional.<br/>
        /// Pattern: "^([#]?([a-fA-F0-9]{8})|([a-fA-F0-9]{6})|([a-fA-F0-9]{3}))$"
        /// </summary>
        /// <returns>(bool)<br/>
        /// - true : is HEX<br/>
        /// - false: is not HEX</returns>
        public static bool IsHex(this string str) => Common.IsHex(str);
        #endregion

        #region Public Methods
        /// <summary>
        /// <code>
        /// Verifies if a string is numerica, which includes negatives and deciamals.
        /// Pattern: @"^\-?\d{1,10}(\.\d{1,8})?$"
        /// Allows: 1.0, 1234567890, -1234567890, 1234567890.12345678, -1234567890.12345678, -0.12345678
        /// --------------------------------------------------
        /// [\-]?            : Optional negative
        /// [\d]{1,10}       : Required Digit only. Legnth: Min 1, Max 10
        /// (                : Starting optional group 1
        ///     [\.]         : Required decimal
        ///     [\d]{1,8}    : Required Digit only. Legnth: Min 1, Max 8
        /// )?               : End optional group 1
        /// Required: Numeric Only [0-9] {Minlength 1, MaxLength: 10}
        /// Optional Decimal: If exists( [.][0-9] {Minlength 1, MaxLength: 8} )
        /// </code>
        /// </summary>
        public static bool IsNumeric(this string @this) => Regex.IsMatch(@this, @"^\-?\d{1,10}(\.\d{1,8})?$");
        /// <summary>
        /// Removes all Ascii Escape Character for colors and font styling.
        /// </summary>
        public static string RemoveAsciiEscape(this string @this, bool colors, bool styles, bool incSymbols) => Common.CleanString(@this, colors, styles, incSymbols);
        #endregion

        #region Private Helper Methods
        private static bool IsSupportedNumeric<T>() => _validBoundaryTypes.Contains(typeof(T));
        #endregion
    }
}



