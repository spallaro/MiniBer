using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniBer.Examples
{
    internal static class Extensions
    {
        public static void NodesLog(this MiniBer.Nodes nodes) =>
            nodes.NodesLogImpl(level: 1);

        private static void NodesLogImpl(this MiniBer.Nodes nodes, int level)
        {
            foreach (var node in nodes)
            {
                if (level > 1)
                {
                    Console.Write($"{new string(' ', (level - 1) * 2)}└─ ");
                }
                else
                {
                    Console.Write("─ ");
                }
                Console.WriteLine($"0x{node.TagNumber:X2}, {node.IdentifierOctets?.ToHexString(true)} ({node.ContentType}, {node.Class}): {node.Contents?.Length} bytes");
                if (node.Nodes?.Count > 0)
                {
                    node.Nodes.NodesLogImpl(level: level + 1);
                }
            }
        }

        public static string ToHexString(this List<byte> value, bool shortForm = false) =>
            value.ToArray().ToHexString(shortForm);

        public static string ToHexString(this byte[] value, bool shortForm = false)
        {
            var sb = new StringBuilder();
            foreach (var b in value)
            {
                if (!shortForm && sb.Length > 0) { sb.Append(' '); }
                sb.Append(
                    shortForm ?
                    $"{b:X2}" :
                    $"0x{b:X2}");
            }
            return $"{sb}";
        }
    }
}
