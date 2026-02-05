/*
 * © 2026 Sebastiano Pallaro
 * Released under the terms of MIT license.
 * Please see LICENSE.md for more details.
 */
namespace MiniBer
{
    public enum Classes
    {
        /// <summary>
        /// The universal class is reserved for data types defined by the ASN.1 standard itself, such as BOOLEAN, INTEGER, SEQUENCE, and OCTET STRING.
        /// </summary>
        Universal = 0b00,
        /// <summary>
        /// The application class is intended for data elements defined by a particular application or protocol. These tags carry semantics agreed upon by the sending and receiving applications.
        /// </summary>
        Application = 0b01,
        /// <summary>
        /// Context-specific tags allow protocols to embed optional or variant elements within a larger structure. The meaning of each context-specific tag is determined by its position and usage within that specific context.
        /// </summary>
        ContextSpecific = 0b10,
        /// <summary>
        /// The private class is for use by individual organizations or vendors. Tags in this class have no standardized meaning and are defined in proprietary specifications.
        /// </summary>
        Private = 0b11
    }
}
