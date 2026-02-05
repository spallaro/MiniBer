/*
 * © 2026 Sebastiano Pallaro
 * Released under the terms of MIT license.
 * Please see LICENSE.md for more details.
 */
namespace MiniBer
{
    public class Decoder
    {
        /// <summary>
        /// Decodes provided data.
        /// </summary>
        /// <param name="data">The data to be decoded.</param>
        /// <returns>The decoded nodes.</returns>
        public static Nodes Decode(
            byte[] data) => Decode(
                data: data,
                decodeOptions: DecodeOptions.None);

        /// <summary>
        /// Decodes provided data.
        /// </summary>
        /// <param name="data">The data to be decoded.</param>
        /// <param name="decodeOptions">Optiomns for the decode process.</param>
        /// <returns>The decoded nodes.</returns>
        public static Nodes Decode(
            byte[] data,
            DecodeOptions decodeOptions) =>
            new(
                data: data,
                offset: 0,
                decodeOptions: decodeOptions);
    }
}
