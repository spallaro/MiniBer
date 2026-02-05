/*
 * © 2026 Sebastiano Pallaro
 * Released under the terms of MIT license.
 * Please see LICENSE.md for more details.
 */
namespace MiniBer
{
    public enum ContentTypes
    {
        /// <summary>
        /// Data elements whose content is raw, uninterpreted octets.
        /// </summary>
        Primitive = 0b0,
        /// <summary>
        /// Data elements whose content is itself a sequence of nested TLV structures.
        /// </summary>
        Constructed = 0b1
    }
}
