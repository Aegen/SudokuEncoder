using System;
using System.Linq;

namespace SudokuService.Controllers.Utilities
{
    public class BinaryConversionUtil
    {
        private BinaryConversionUtil() { }

        public static byte[] GetBytes(string bitString)
        {
            return Enumerable.Range(0, bitString.Length / 8).
                Select(pos => Convert.ToByte(
                    bitString.Substring(pos * 8, 8),
                    2)
                ).ToArray();
        }
    }
}
