/*
 * © 2026 Sebastiano Pallaro
 * Released under the terms of MIT license.
 * Please see LICENSE.md for more details.
 */
namespace MiniBer
{
    [Flags]
    public enum DecodeOptions
    {
        /// <summary>
        /// No options.
        /// </summary>
        None = 0b00000000,
        /// <summary>
        /// Source TLV data doesn't include Values, only Tags and Lengths.
        /// </summary>
        NoData = 0b00000001,
        /// <summary>
        /// Decode nodes data only when required.
        /// </summary>
        SmartDecode = 0b00000010
    }
}
