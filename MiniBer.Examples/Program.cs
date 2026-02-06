namespace MiniBer.Examples
{
    internal class Program
    {
        static void Example01()
        {
            Console.WriteLine("Example 01");

            byte[] data = [
                0x30, 0x13,             // Sequence Root (19 bytes)
                0x02, 0x02, 0x03, 0xE8, // Integer 1000
                0x30, 0x0D,             // Sequence Nested (13 bytes)
                0x0C, 0x06,             // UTF8String (6 bytes)
                0x47, 0x65, 0x6D, 0x69, 0x6E, 0x69,
                0x04, 0x03,             // Octet String (3 bytes)
                0xAA, 0xBB, 0xCC
            ];

            MiniBer.Nodes nodes = MiniBer.Decoder.Decode(
                data: data);

            nodes.NodesLog();
        }

        private static void Example02()
        {
            Console.WriteLine("Example 02");

            byte[] data = [
                0x30, 0x2A, 0x0C, 0x0D, 0x43, 0x65, 0x6E,
                0x74, 0x72, 0x6F, 0x20, 0x53, 0x74, 0x6F,
                0x72, 0x69, 0x63, 0x6F, 0x30, 0x19, 0x0A,
                0x01, 0x01, 0x30, 0x14, 0x02, 0x01, 0x65,
                0x16, 0x06, 0x41, 0x43, 0x54, 0x49, 0x56,
                0x45, 0x30, 0x07, 0x18, 0x05, 0x32, 0x30,
                0x32, 0x36, 0x5A
            ];

            MiniBer.Nodes nodes = MiniBer.Decoder.Decode(
                data: data);

            nodes.NodesLog();
        }

        static void Main(string[] args)
        {
            Example01();
            Example02();
        }
    }
}
