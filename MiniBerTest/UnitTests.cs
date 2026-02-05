using Microsoft.Testing.Platform.Extensions.Messages;

namespace MiniBerTest
{
    [TestClass]
    public sealed class UnitTests
    {
        #region TLV

        [TestMethod]
        [TestCategory("TLV")]
        public void TLV_Correct01()
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
        [TestCategory("TLV")]
        public void TLV_Correct02()
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
        [TestCategory("TLV")]
        public void TLV_Correct03()
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
        [TestCategory("TLV")]
        public void TLV_Correct04()
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
        [TestCategory("TLV")]
        public void TLV_Correct05()
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
        [TestCategory("TLV")]
        public void TLV_Correct06()
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

        #endregion

        [TestMethod]
        public void TestDecode()
        {
            /*
             * Sample data generated with Copilot
             *
             * MyData ::= SEQUENCE {
             *   version INTEGER,
             *   payload OCTET STRING,
             *   active  BOOLEAN
             * }
             * 
             * SEQUENCE (Tag 30)
             *   30: Tag universale per una SEQUENCE 
             *       (Costrutto composto).
             *   0C: Lunghezza totale dei dati 
             *       interni, pari a 12 byte.
             * 
             * INTEGER (Tag 02)
             *   02: Tag dell’INTEGER.
             *   01: Lunghezza (1 byte).
             *   01: Valore 1 (in esadecimale).
             *   
             * OCTET STRING (Tag 04)
             *   04: Tag per l’OCTET STRING.
             *   04: Lunghezza (4 byte).
             *   61 62 63 64: Valori in esadecimale 
             *       corrispondenti ai caratteri ASCII 
             *       "a", "b", "c", "d".
             *       
             * BOOLEAN (Tag 01)
             *   01: Tag per il BOOLEAN.
             *   01: Lunghezza (1 byte).
             *   FF: Valore TRUE (in DER, la codifica 
             *       canonica per TRUE è 0xFF).
             */
            byte[] data = [
                0x30, 0x0C, 0x02, 0x01,
                0x01, 0x04, 0x04, 0x61,
                0x62, 0x63, 0x64, 0x01,
                0x01, 0xFF];

            var nodes = MiniBer.Decoder.Decode(data);

            Assert.IsNotNull(nodes);

            // TODO
        }
    }
}
