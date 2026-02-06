namespace MiniBer.UnitTests
{
    [TestClass]
    public sealed class UnitTests
    {
        [TestMethod]
        public void Test01()
        {
            MiniBer.Nodes nodes = MiniBer.Decoder.Decode(
                data: null);

            Assert.IsTrue(nodes != null);

            Assert.IsTrue(nodes.Count == 0);
        }

        [TestMethod]
        public void Test02()
        {
            MiniBer.Nodes nodes = MiniBer.Decoder.Decode(data: []);

            Assert.IsTrue(nodes != null);

            Assert.IsTrue(nodes.Count == 0);
        }

        [TestMethod]
        public void Test03()
        {
            byte[] data = [0x01, 0x01, 0xFF];

            var nodes = MiniBer.Decoder.Decode(data);

            Assert.IsNotNull(nodes);

            Assert.IsTrue(nodes.Count == 1);

            var searchResult = nodes.Search([0x01]);

            Assert.IsNotNull(searchResult);

            Assert.IsTrue(searchResult.Count == 1);

            Assert.IsTrue(searchResult[0].TagNumber == 0x01);

            Assert.IsTrue(searchResult[0].Length == 1);

            Assert.IsNotNull(searchResult[0].Contents);

            Assert.IsTrue(searchResult[0].Contents?.Length == 1);

            Assert.IsTrue(searchResult[0].Contents?[0] == 0xFF);

            Assert.IsTrue(searchResult[0].Class == MiniBer.Classes.Universal);

            Assert.IsTrue(searchResult[0].ContentType == MiniBer.ContentTypes.Primitive);
        }

        [TestMethod]
        public void Test04()
        {
            byte[] data = [0x9F, 0x1A, 0x02, 0x12, 0x34];

            var nodes = MiniBer.Decoder.Decode(data);

            Assert.IsNotNull(nodes);

            Assert.IsTrue(nodes.Count == 1);

            var searchResult = nodes.Search([0x9F, 0x1A]);

            Assert.IsNotNull(searchResult);

            Assert.IsTrue(searchResult.Count == 1);

            Assert.IsTrue(searchResult[0].TagNumber == 0x1A);

            Assert.IsTrue(searchResult[0].Length == 2);

            Assert.IsNotNull(searchResult[0].Contents);

            Assert.IsTrue(searchResult[0].Contents?.Length == 2);

            Assert.IsTrue(searchResult[0].Contents?.SequenceEqual<byte>([0x12, 0x34]));

            Assert.IsTrue(searchResult[0].Class == MiniBer.Classes.ContextSpecific);

            Assert.IsTrue(searchResult[0].ContentType == MiniBer.ContentTypes.Primitive);
        }

        [TestMethod]
        public void Test05()
        {
            byte[] data = [0xDF, 0x00, 0x03, 0xAB, 0xCD, 0xEF];

            var nodes = MiniBer.Decoder.Decode(data);

            Assert.IsNotNull(nodes);

            Assert.IsTrue(nodes.Count == 1);

            var searchResult = nodes.Search([0xDF, 0x00]);

            Assert.IsNotNull(searchResult);

            Assert.IsTrue(searchResult.Count == 1);

            Assert.IsTrue(searchResult[0].TagNumber == 0x00);

            Assert.IsTrue(searchResult[0].Length == 3);

            Assert.IsNotNull(searchResult[0].Contents);

            Assert.IsTrue(searchResult[0].Contents?.Length == 3);

            Assert.IsTrue(searchResult[0].Contents?.SequenceEqual<byte>([0xAB, 0xCD, 0xEF]));

            Assert.IsTrue(searchResult[0].Class == MiniBer.Classes.Private);

            Assert.IsTrue(searchResult[0].ContentType == MiniBer.ContentTypes.Primitive);
        }

        [TestMethod]
        public void Test06()
        {
            byte[] data = [0x5C, 0x03, 0x01, 0x02, 0x03];

            var nodes = MiniBer.Decoder.Decode(data);

            Assert.IsNotNull(nodes);

            Assert.IsTrue(nodes.Count == 1);

            var searchResult = nodes.Search([0x5C]);

            Assert.IsNotNull(searchResult);

            Assert.IsTrue(searchResult.Count == 1);

            Assert.IsTrue(searchResult[0].TagNumber == 0x1C);

            Assert.IsTrue(searchResult[0].Length == 3);

            Assert.IsNotNull(searchResult[0].Contents);

            Assert.IsTrue(searchResult[0].Contents?.Length == 3);

            Assert.IsTrue(searchResult[0].Contents?.SequenceEqual<byte>([0x01, 0x02, 0x03]));

            Assert.IsTrue(searchResult[0].Class == MiniBer.Classes.Application);

            Assert.IsTrue(searchResult[0].ContentType == MiniBer.ContentTypes.Primitive);
        }

        [TestMethod]
        public void Test07()
        {
            byte[] data = [0x6F, 0x00];

            var nodes = MiniBer.Decoder.Decode(data);

            Assert.IsNotNull(nodes);

            Assert.IsTrue(nodes.Count == 1);

            var searchResult = nodes.Search([0x6F]);

            Assert.IsNotNull(searchResult);

            Assert.IsTrue(searchResult.Count == 1);

            Assert.IsTrue(searchResult[0].TagNumber == 0x0F);

            Assert.IsTrue(searchResult[0].Length == 0);

            Assert.IsNotNull(searchResult[0].Contents);

            Assert.IsTrue(searchResult[0].Contents?.Length == 0);

            Assert.IsTrue(searchResult[0].Class == MiniBer.Classes.Application);

            Assert.IsTrue(searchResult[0].ContentType == MiniBer.ContentTypes.Constructed);
        }


        [TestMethod]
        public void Test08()
        {
            /*
             
            #	Bytes	Classe	PC	Tag number	Length	Valore / Struttura
            1	01 01 FF	Universal (00₂)	Primitive (0)	0x01 (1)	Short-form 1	0xFF ⇒ BOOLEAN TRUE
            2	9F 1A 02 12 34	Context-Specific (10₂)	Primitive (0)	0x1A (26)	Short-form 2	12 34
            3	5C 00	Application (01₂)	Primitive (0)	0x1C (28)	Short-form 0	valore vuoto
            4	DF 02 81 02 AB CD	Private (11₂)	Primitive (0)	0x02 (2)	Long-form (0x81 02 = 2)	AB CD
            5	E1 05 30 03 02 01 01	Private (11₂)	Constructed (1)	0x01 (1)	Short-form 5	TLV annidato: SEQUENCE 30 03 02 01 01 (INTEGER 1)
             
             */

            byte[] data = [
                0x01, 0x01, 0xFF,                        // ELEMENT #1
                0x9F, 0x1A, 0x02, 0x12, 0x34,            // ELEMENT #2
                0x5C, 0x00,                              // ELEMENT #3
                0xDF, 0x02, 0x81, 0x02, 0xAB, 0xCD,      // ELEMENT #4
                0xE1, 0x05, 0x30, 0x03, 0x02, 0x01, 0x01 // ELEMENT #5
            ];

            var nodes = MiniBer.Decoder.Decode(data);

            Assert.IsNotNull(nodes);

            Assert.IsTrue(nodes.Count == 5);

            List<MiniBer.Node> searchResult;
            MiniBer.Node node;

            // ELEMENT #1

            searchResult = nodes.Search([0x01]);

            Assert.IsNotNull(searchResult);

            Assert.IsTrue(searchResult.Count == 1);

            node = nodes[0];

            Assert.IsTrue(node.TagNumber == 0x01);

            Assert.IsTrue(node.Length == 1);

            Assert.IsNotNull(node.Contents);

            Assert.IsTrue(node.Contents?.Length == 1);

            Assert.IsTrue(node.Contents?.SequenceEqual<byte>([0xFF]));

            Assert.IsTrue(node.Class == MiniBer.Classes.Universal);

            Assert.IsTrue(node.ContentType == MiniBer.ContentTypes.Primitive);

            // ELEMENT #2

            searchResult = nodes.Search([0x9F, 0x1A]);

            Assert.IsNotNull(searchResult);

            Assert.IsTrue(searchResult.Count == 1);

            node = nodes[1];

            Assert.IsTrue(node.TagNumber == 0x1A);

            Assert.IsTrue(node.Length == 2);

            Assert.IsNotNull(node.Contents);

            Assert.IsTrue(node.Contents?.Length == 2);

            Assert.IsTrue(node.Contents?.SequenceEqual<byte>([0x12, 0x34]));

            Assert.IsTrue(node.Class == MiniBer.Classes.ContextSpecific);

            Assert.IsTrue(node.ContentType == MiniBer.ContentTypes.Primitive);

            // ELEMENT #3

            searchResult = nodes.Search([0x5C]);

            Assert.IsNotNull(searchResult);

            Assert.IsTrue(searchResult.Count == 1);

            node = nodes[2];

            Assert.IsTrue(node.TagNumber == 0x1C);

            Assert.IsTrue(node.Length == 0);

            Assert.IsNotNull(node.Contents);

            Assert.IsTrue(node.Contents?.Length == 0);

            Assert.IsTrue(node.Class == MiniBer.Classes.Application);

            Assert.IsTrue(node.ContentType == MiniBer.ContentTypes.Primitive);

            // ELEMENT #4

            searchResult = nodes.Search([0xDF, 0x02]);

            Assert.IsNotNull(searchResult);

            Assert.IsTrue(searchResult.Count == 1);

            node = nodes[3];

            Assert.IsTrue(node.TagNumber == 0x02);

            Assert.IsTrue(node.Length == 2);

            Assert.IsNotNull(node.Contents);

            Assert.IsTrue(node.Contents?.Length == 2);

            Assert.IsTrue(node.Contents?.SequenceEqual<byte>([0xAB, 0xCD]));

            Assert.IsTrue(node.Class == MiniBer.Classes.Private);

            Assert.IsTrue(node.ContentType == MiniBer.ContentTypes.Primitive);

            // ELEMENT #5

            // E1 (Private, Constructed, tag 1, length 5)
            //  └─ 30 (Universal, Constructed, SEQUENCE, length 3)
            //      └─ 02 (Universal, Primitive, INTEGER, length 1) = 0x01

            searchResult = nodes.Search([0xE1]);

            Assert.IsNotNull(searchResult);

            Assert.IsTrue(searchResult.Count == 1);

            node = nodes[4];

            Assert.IsTrue(node.TagNumber == 0x01);

            Assert.IsTrue(node.Length == 5);

            Assert.IsNotNull(node.Contents);

            Assert.IsTrue(node.Contents?.Length == 5);

            Assert.IsTrue(node.Contents?.SequenceEqual<byte>([0x30, 0x03, 0x02, 0x01, 0x01]));

            Assert.IsTrue(node.Class == MiniBer.Classes.Private);

            Assert.IsTrue(node.ContentType == MiniBer.ContentTypes.Constructed);

            // ELEMENT #5 -> SUBNODE

            Assert.IsNotNull(node.Nodes);

            Assert.IsTrue(node.Nodes.Count == 1);

            searchResult = node.Nodes.Search([0x30]);

            Assert.IsNotNull(searchResult);

            Assert.IsTrue(searchResult.Count == 1);

            var node2 = node.Nodes[0];

            Assert.IsTrue(node2.TagNumber == 0x10);

            Assert.IsTrue(node2.Length == 3);

            Assert.IsNotNull(node2.Contents);

            Assert.IsTrue(node2.Contents?.Length == 3);

            Assert.IsTrue(node2.Contents?.SequenceEqual<byte>([0x02, 0x01, 0x01]));

            Assert.IsTrue(node2.Class == MiniBer.Classes.Universal);

            Assert.IsTrue(node2.ContentType == MiniBer.ContentTypes.Constructed);

            // ELEMENT #5 -> SUBNODE -> SUBNODE

            Assert.IsNotNull(node2.Nodes);

            Assert.IsTrue(node2.Nodes.Count == 1);

            searchResult = node2.Nodes.Search([0x02]);

            Assert.IsNotNull(searchResult);

            Assert.IsTrue(searchResult.Count == 1);

            var node3 = node2.Nodes[0];

            Assert.IsTrue(node3.TagNumber == 0x02);

            Assert.IsTrue(node3.Length == 1);

            Assert.IsNotNull(node3.Contents);

            Assert.IsTrue(node3.Contents?.Length == 1);

            Assert.IsTrue(node3.Contents?.SequenceEqual<byte>([0x01]));

            Assert.IsTrue(node3.Class == MiniBer.Classes.Universal);

            Assert.IsTrue(node3.ContentType == MiniBer.ContentTypes.Primitive);
        }

        [TestMethod]
        public void Test09()
        {
            byte[] data = [
                0x02, 0x03, 0x0C, 0x05, 0x04,
                0x01, 0x0A, 0x02, 0x16, 0x04,
                0x03, 0x03, 0x12, 0x05, 0x06,
                0x01, 0x09, 0x04, 0x1B, 0x02
            ];

            MiniBer.Nodes nodes = MiniBer.Decoder.Decode(
                data: data,
                decodeOptions: MiniBer.DecodeOptions.NoData);

            Assert.IsTrue(nodes != null);

            Assert.IsTrue(nodes.Count == 10);

            Test01_Check(nodes[0], 0x02, 3);
            Test01_Check(nodes[1], 0x0C, 5);
            Test01_Check(nodes[2], 0x04, 1);
            Test01_Check(nodes[3], 0x0A, 2);
            Test01_Check(nodes[4], 0x16, 4);
            Test01_Check(nodes[5], 0x03, 3);
            Test01_Check(nodes[6], 0x12, 5);
            Test01_Check(nodes[7], 0x06, 1);
            Test01_Check(nodes[8], 0x09, 4);
            Test01_Check(nodes[9], 0x1B, 2);
        }

        private void Test01_Check(
            MiniBer.Node node,
            int tagNumber,
            int length)
        {
            Assert.IsTrue(node.TagNumber == tagNumber);
            Assert.IsTrue(node.Class == MiniBer.Classes.Universal);
            Assert.IsTrue(node.ContentType == MiniBer.ContentTypes.Primitive);
            Assert.IsTrue(node.Length == length);
            Assert.IsTrue(node.Contents != null);
            Assert.IsTrue(node.Contents?.Length == 0);
        }

        [TestMethod]
        public void Test10()
        {
            // Missing length and value.

            byte[] data = [0x01];

            Assert.ThrowsException<MiniBer.UnexpectedEndOfStreamException>(() =>
            {
                MiniBer.Decoder.Decode(data);
            });
        }

        [TestMethod]
        public void Test11()
        {
            // Declared length 2 but only one value byte.

            byte[] data = [0x9F, 0x1A, 0x02, 0x12];

            Assert.ThrowsException<MiniBer.UnexpectedEndOfStreamException>(() =>
            {
                MiniBer.Decoder.Decode(data);
            });
        }

        [TestMethod]
        public void Test12()
        {
            // Single-octet tag marker 5F requires a following tag octet.

            byte[] data = [0x5F];

            Assert.ThrowsException<MiniBer.UnexpectedEndOfStreamException>(() =>
            {
                MiniBer.Decoder.Decode(data);
            });
        }

        [TestMethod]
        public void Test13()
        {
            byte[] data = [0x04, 0x81];

            Assert.ThrowsException<MiniBer.UnexpectedEndOfStreamException>(() =>
            {
                MiniBer.Decoder.Decode(data);
            });
        }

        [TestMethod]
        public void Test14()
        {
            byte[] data = [0xDF, 0x00, 0x05, 0xAA, 0xBB];

            Assert.ThrowsException<MiniBer.UnexpectedEndOfStreamException>(() =>
            {
                MiniBer.Decoder.Decode(data);
            });
        }
    }
}
