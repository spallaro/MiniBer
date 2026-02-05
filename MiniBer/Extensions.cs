/*
 * © 2026 Sebastiano Pallaro
 * Released under the terms of MIT license.
 * Please see LICENSE.md for more details.
 */

using System.Text;

namespace MiniBer
{
    internal static class Extensions
    {
        /// <summary>
        /// Returns the status of a bit, on a byte.
        /// </summary>
        /// <param name="value">The byte to check.</param>
        /// <param name="bitNumber">The bit number (from 1 to 8). No checks on the value.</param>
        /// <returns>True if the bit is set to 1.</returns>
        /// <see href="https://stackoverflow.com/questions/4854207/get-a-specific-bit-from-byte"></see>
        internal static bool IsBitSet(this byte value, int bitNumber) =>
            (value & (1 << bitNumber - 1)) != 0;

        internal static string ToHextString(this List<byte> value) => value.ToArray().ToHexString();

        internal static string ToHexString(this byte[]? value)
        {
            if (value == null) return "NULL";
            var ret = new StringBuilder(value.Length * 3);

            foreach (var v in value)
            {
                if (ret.Length > 0) ret.Append(' ');
                ret.Append($"{v:X2}");
            }

            return $"{ret}";
        }

        internal static void Trace(this Exception exception)
        {
            System.Diagnostics.Trace.TraceError(
                $"{exception.GetType().Name}(): {exception.Message}");
            System.Diagnostics.Trace.TraceError(
                exception.StackTrace);
        }
    }
}
