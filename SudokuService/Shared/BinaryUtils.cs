using System;
using System.Collections.Generic;

namespace SudokuService.Shared
{
    public class BinaryUtils
    {
        private BinaryUtils() { }

        public static List<String> SplitInto6BitStrings(String bits)
        {
            var bitStrings = new List<String>();

            for (var startIndex = 0; startIndex < bits.Length; startIndex += 6)
            {
                var subStringLength = (bits.Length - startIndex > 6) ? 6 : bits.Length - startIndex;

                bitStrings.Add(RightPadToSize(bits.Substring(startIndex, subStringLength), 6));
            }

            return bitStrings;
        }

        public static String RightPadToSize(String binary, int size)
        {
            if (binary.Length > size)
                throw new ArgumentOutOfRangeException(nameof(size), "The requested size is smaller than the input length");

            if (binary.Length == size)
                return binary;

            var difference = size - binary.Length;

            return binary + new string('0', difference);
        }

        public static String LeftPadToSize(String binary, int size)
        {
            if (binary.Length > size)
                throw new ArgumentOutOfRangeException(nameof(size), "The requested size is smaller than the input length");

            if (binary.Length == size)
                return binary;

            var difference = size - binary.Length;

            return new string('0', difference) + binary;
        }

        public static int NumBitsToStore(int size)
        {
            if (size == 0)
                return 0;

            return (int)Math.Ceiling(Math.Log2(size));
        }

        public static String IntToBinary(int value) => Convert.ToString(value, 2);
    }
}
