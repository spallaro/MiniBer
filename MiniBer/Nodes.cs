/*
 * © 2026 Sebastiano Pallaro
 * Released under the terms of MIT license.
 * Please see LICENSE.md for more details.
 */
namespace MiniBer
{
    public class Nodes : List<Node>
    {
        private readonly byte[]? Data;

        private readonly DecodeOptions DecodeOptions;

        private bool Parsed;

        private readonly int Offset;

        public Nodes()
        {
            Parsed = false;
            Data = null;
            DecodeOptions = DecodeOptions.None;
            Offset = 0;
        }

        internal Nodes(
            byte[]? data,
            int offset,
            DecodeOptions decodeOptions)
        {
            Parsed = false;
            Data = data;
            DecodeOptions = decodeOptions;
            Offset = offset;
            if (!decodeOptions.HasFlag(DecodeOptions.SmartDecode))
            {
                Parse();
            }
        }

        internal Nodes(
            List<Node> nodes)
        {
            Parsed = true;
            Data = null;
            DecodeOptions = DecodeOptions.None;
            Offset = 0;
            base.AddRange(nodes);
        }

        public new int Count
        {
            get
            {
                Parse();
                return base.Count;
            }
        }

        public new Node this[int index]
        {
            get
            {
                Parse();
                return base[index];
            }
            set => base[index] = value;
        }

        public Nodes this[params byte[] identifierOctects]
        {
            get => Search(identifierOctects: identifierOctects);

        }

        /// <summary>
        /// Search all nodes based on TagNumber property.
        /// </summary>
        /// <param name="tagNumber">The TagNumber to search for.</param>
        /// <returns>Found nodes. An empty collection if no nodes are found.</returns>
        public Nodes Search(int tagNumber)
        {
            Parse();

            var nodes = from n in this
                        where n.TagNumber == tagNumber
                        select n;

            if (nodes.Any())
            {
                return new Nodes(nodes: [.. nodes]);
            }

            return new Nodes(nodes: []);
        }

        /// <summary>
        /// Search all nodes, based on IdentifierOctects property.
        /// </summary>
        /// <param name="identifierOctects">The IdentifierOctects to search for.</param>
        /// <returns>Found nodes. An empty collection if no nodes are found.</returns>
        public Nodes Search(byte[] identifierOctects)
        {
            Parse();

            var nodes = from n in this
                        where
                            n.IdentifierOctets != null &&
                            n.IdentifierOctets.SequenceEqual(
                                identifierOctects)
                        select n;

            if (nodes.Any())
            {
                return new Nodes(nodes: [.. nodes]);
            }

            return new Nodes(nodes: []);
        }

        /// <summary>
        /// Searches a node on a path of indexes.
        /// </summary>
        /// <param name="path">The indexes path.</param>
        /// <returns>The found node, or null.</returns>
        public Node? SearchPath(params int[] path) => SearchPath(index: 0, path: path);

        private Node? SearchPath(int index, params int[] path)
        {
            Parse();

            if (index == path.Length - 1)
            {
                if (Count > path[index])
                {
                    return this[path[index]];
                }
                return null;
            }

            if (this[path[index]] == null ||
                this[path[index]].ContentType == ContentTypes.Primitive ||
                this[path[index]].Nodes == null)
            {
                return null;
            }

            return this[path[index]]?.Nodes?.SearchPath(index: ++index, path);
        }

        private void Parse() => Parse(offset: Offset);
        private void Parse(int offset)
        {
            if (Parsed) return;
            if (Data == null)
            {
                Parsed = true;
                System.Diagnostics.Trace.TraceWarning("Can't Parse() as Data is NULL.");
                return;
            }
            if (Data.Length == 0)
            {
                Parsed = true;
                System.Diagnostics.Trace.TraceWarning("Can't Parse() as Data is EMPTY.");
                return;
            }
            using (var ms = new MemoryStream(Data))
            {
                do
                {
                    var node = new Node
                    {
                        Offset = offset
                    };

                    // 8.1.2: IDENTIFIER OCTECTS

                    // The first byte contains the tag number.
                    // - Bit 1 to 5: tag number.
                    // - Bit 6: encoding (primitive or constructed).
                    // - Bit 7 to 8: Class.
                    int tag = ms.ReadByte();

                    node.IdentifierOctets =
                    [
                        (byte)tag
                    ];

                    // If bits from 1 to 5 are set to 1, the tag number spans multiple bytes.
                    if ((tag & 0b00011111) == 0b00011111)
                    {
                        // Need to read bytes until an 8th bit set to 1 is found.
                        while (true)
                        {
                            tag = ms.ReadByte();
                            node.IdentifierOctets.Add((byte)tag);

                            if ((tag & 0b10000000) != 0b10000000)
                            {
                                // Reach last byte of tag number.
                                break;
                            }
                        }
                    }

                    // Bits 1 to 5
                    node.TagNumber = node.IdentifierOctets[0] & 0b00011111;

                    // If number is 0b00011111 (0x1F), tag number spans multiple bytes.
                    if (node.TagNumber == 0b00011111)
                    {
                        if (node.IdentifierOctets.Count == 1)
                        {
                            throw new Asn1ParseException("Tag number shall span multiple bytes, but only one byte provided.");
                        }
                        node.TagNumber = 0;
                        for (int i = 1; i < node.IdentifierOctets.Count; i++)
                        {
                            node.TagNumber += node.IdentifierOctets[i] & 0b01111111;
                        }
                    }

                    // Bit 6: content type (Contructed or Primitive).
                    node.ContentType =
                        node.IdentifierOctets[0].IsBitSet(6) ?
                        ContentTypes.Constructed :
                        ContentTypes.Primitive;

                    // Bits 7 and 8: class.
                    node.Class = (Classes)(node.IdentifierOctets[0] >> 6);

                    // 8.1.3: LENGTH OCTECTS

                    // After the tag number bytes, there are the data length bytes.
                    node.LengthOctects =
                    [
                        (byte)ms.ReadByte()
                    ];
                    node.Length = node.LengthOctects[0];
                    if (node.Length > 0b10000000)
                    {
                        int numBytes = node.Length - 0b10000000;
                        node.Length = 0;
                        for (int i = 0; i < numBytes; i++)
                        {
                            node.LengthOctects.Add((byte)ms.ReadByte());
                            node.Length = (node.Length << 8) | node.LengthOctects.Last(); //node.LengthOctects[node.LengthOctects.Count - 1];
                        }
                    }

                    //if (node.IdentifierOctets.Count == 1 &&
                    //    node.IdentifierOctets[0] == 0 &&
                    //    node.Length == 0)
                    //{
                    //    // NULL
                    //}

                    if (!DecodeOptions.HasFlag(DecodeOptions.NoData) &&
                        node.Length > 0)
                    {
                        // 8.1.4 (and 8.1.5): CONTENTS OCTECTS

                        if (node.IndefinitiveLength)
                        {
                            // Length is idefinite, we should read until 2 0x00 bytes are found.

                            var buff = new byte[] { 0xFF, 0xFF };
                            var dataBuffer = new List<byte>();
                            while (true)
                            {
                                buff[0] = buff[1];
                                buff[1] = (byte)ms.ReadByte();
                                if (buff[0] == 0x00 && buff[1] == 0x00)
                                {
                                    break;
                                }
                                dataBuffer.Add(buff[1]);
                            }

                            node.Contents = [.. dataBuffer];
                        }
                        else
                        {
                            var contents = new List<byte>();
                            for (int i = 0; i < node.Length; i++)
                            {
                                contents.Add((byte)ms.ReadByte());
                            }

                            node.Contents = [.. contents];
                        }
                    }
                    else
                    {
                        // Skip data part.
                        node.Contents = [];
                    }

                    Add(node);

                    if (node.ContentType == ContentTypes.Constructed &&
                        node.Contents.Length > 0)
                    {
                        node.Nodes = new Nodes(
                            data: node.Contents,
                            offset: offset + node.IdentifierOctets.Count + node.LengthOctects.Count,
                            decodeOptions: DecodeOptions);
                    }

                    offset += node.Length;

                } while (ms.Position < ms.Length);
            }

            Parsed = true;
        }
    }
}
