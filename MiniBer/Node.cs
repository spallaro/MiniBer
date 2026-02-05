/*
 * © 2026 Sebastiano Pallaro
 * Released under the terms of MIT license.
 * Please see LICENSE.md for more details.
 */

namespace MiniBer
{
    public class Node
    {
        /// <summary>
        /// The elaborated tag number.
        /// </summary>
        /// <remarks>See X.690 8.1.2.</remarks>
        public int TagNumber { get; set; }

        /// <summary>
        /// The original identifier octects.
        /// </summary>
        /// <remarks>See X.690 8.1.2.</remarks>
        public List<byte>? IdentifierOctets { get; set; }

        /// <summary>
        /// Class of the node.
        /// </summary>
        /// <remarks>See X.690 8.1.2.</remarks>
        public Classes Class { get; set; }

        /// <summary>
        /// Content type of the node.
        /// </summary>
        /// <remarks>See X.690 8.1.2.</remarks>
        public ContentTypes ContentType { get; set; }

        /// <summary>
        /// Inner nodes of the node.
        /// </summary>
        /// <remarks>This property is available if ContentType is Constructed, or after a successful call to TryParseSubNodes().</remarks>
        public Nodes? Nodes { get; set; }

        /// <summary>
        /// Offset on the original data, from where the node data is available.
        /// </summary>
        internal int Offset { get; set; }

        /// <summary>
        /// The calculated length of the contents.
        /// </summary>
        /// <remarks>See X.690 8.1.3.</remarks>
        public int Length { get; set; }

        /// <summary>
        /// Determines if Lenght is indefinitive.
        /// </summary>
        public bool IndefinitiveLength { get => Length == 0b10000000; }

        /// <summary>
        /// The original length octects.
        /// </summary>
        /// <remarks>See X.690 8.1.3.</remarks>
        public List<byte>? LengthOctects { get; set; }
        
        /// <summary>
        /// Data contents.
        /// </summary>
        /// <remarks>See X.690 8.1.4.</remarks>
        public byte[]? Contents { get; set; }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() =>
            $"0x{TagNumber:X2} ({IdentifierOctets?.ToHextString()}): Class={Class}, ContentType={ContentType}, Length={Length}";

        /// <summary>
        /// Try parse Contents to the Nodes property. Same as TryParseSubNodes(DecodeOptions.None).
        /// </summary>
        /// <returns>True if parse succeeded</returns>
        /// <remarks>
        /// Useful if ContentType is Primitive, and Data contains ASN.1 nodes.
        /// </remarks>
        public bool TryParseSubNodes() => TryParseSubNodes(decodeOptions: DecodeOptions.None);

        /// <summary>
        /// Try parse Contents to the Nodes property.
        /// </summary>
        /// <param name="decodeOptions">Options for the parsing process.</param>
        /// <returns>True if parse succeeded</returns>
        /// <remarks>
        /// Useful if ContentType is Primitive, and Data contains ASN.1 nodes.
        /// </remarks>
        public bool TryParseSubNodes(DecodeOptions decodeOptions)
        {
            try
            {
                Nodes = new Nodes(
                    data: Contents, 
                    offset: Offset, 
                    decodeOptions: decodeOptions);
                return true;
            }
            catch (Exception ex)
            {
                ex.Trace();
            }
            return false;
        }
    }
}
