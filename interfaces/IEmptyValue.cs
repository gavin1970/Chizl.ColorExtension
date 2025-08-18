namespace Chizl.ColorExtension
{
    public interface IEmptyValue
    {
        bool IsEmpty { get; }
    }

    public static class StructEmptyExtension
    {
        // It's a generic static method that works with any struct implementing IEmptyValue.
        // public static bool IsEmpty<T>(T value) where T : struct, IEmptyValue => value.IsEmpty;
        // public static bool IsEmpty<T>(this T value) where T : struct, IEmptyValue => value.IsEmpty;

        /// <summary>
        /// Returns true if sturct is initialized by the Empty property or if the struct is null.
        /// </summary>
        /// <typeparam name="T">Pickup struct</typeparam>
        /// <param name="value">Value of struct</param>
        /// <returns>
        /// true : If sturct is initialized by Empty property or if the struct is null.<br/>
        /// false: Has value and is not empty.
        /// </returns>
        public static bool IsEmpty<T>(this T? value) where T : struct, IEmptyValue => !value.HasValue || value.Value.IsEmpty;

        /// <summary>
        /// Checks if a nullable struct has value is not null or empty.
        /// </summary>
        public static bool IsValid<T>(this T? value) where T : struct, IEmptyValue => value.HasValue && !value.Value.IsEmpty;
    }
}
