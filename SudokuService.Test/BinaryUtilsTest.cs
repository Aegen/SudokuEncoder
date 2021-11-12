using System;
using System.Linq;
using SudokuService.Shared;
using Xunit;

namespace SudokuService.Test
{
    public class BinaryUtilsTest
    {
        /*
         *  SplitInto6BitStrings
         */

        [Fact]
        public void SplitInto6BitStrings_DivisibleBySix()
        {
            var inputBits = "010101101010111000";

            var subStrings = BinaryUtils.SplitInto6BitStrings(inputBits);

            Assert.Equal(3, subStrings.Count);
            Assert.Equal("010101", subStrings[0]);
            Assert.Equal("101010", subStrings[1]);
            Assert.Equal("111000", subStrings[2]);
        }

        [Fact]
        public void SplitInto6BitStrings_NotDivisibleBySix()
        {
            var inputBits = "010101101010111";

            var subStrings = BinaryUtils.SplitInto6BitStrings(inputBits);

            Assert.Equal(3, subStrings.Count);
            Assert.Equal("010101", subStrings[0]);
            Assert.Equal("101010", subStrings[1]);
            Assert.Equal("111000", subStrings[2]);
        }

        [Fact]
        public void SplitInto6BitStrings_SingleDivisibleBySix()
        {
            var inputBits = "010101";

            var subStrings = BinaryUtils.SplitInto6BitStrings(inputBits);

            Assert.Single(subStrings);
            Assert.Equal("010101", subStrings[0]);
        }

        [Fact]
        public void SplitInto6BitStrings_SingleNotDivisibleBySix()
        {
            var inputBits = "0101";

            var subStrings = BinaryUtils.SplitInto6BitStrings(inputBits);

            Assert.Single(subStrings);
            Assert.Equal("010100", subStrings[0]);
        }

        /*
         *  RightPadToSize
         */

        [Fact]
        public void RightPadToSize_LargerSize()
        {
            var inputString = "1";

            var paddedString = BinaryUtils.RightPadToSize(inputString, 3);

            Assert.Equal("100", paddedString);
        }

        [Fact]
        public void RightPadToSize_SameSize()
        {
            var inputString = "111";

            var paddedString = BinaryUtils.RightPadToSize(inputString, 3);

            Assert.Equal("111", paddedString);
        }

        [Fact]
        public void RightPadToSize_SmallerSize()
        {
            var inputString = "111";

            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                BinaryUtils.RightPadToSize(inputString, 2);
            });
        }

        /*
         *  LeftPadToSize
         */

        [Fact]
        public void LeftPadToSize_LargerSize()
        {
            var inputString = "1";

            var paddedString = BinaryUtils.LeftPadToSize(inputString, 3);

            Assert.Equal("001", paddedString);
        }

        [Fact]
        public void LeftPadToSize_SameSize()
        {
            var inputString = "111";

            var paddedString = BinaryUtils.LeftPadToSize(inputString, 3);

            Assert.Equal("111", paddedString);
        }

        [Fact]
        public void LeftPadToSize_SmallerSize()
        {
            var inputString = "111";

            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                BinaryUtils.LeftPadToSize(inputString, 2);
            });
        }

        /*
         *  NumBitsToStore
         */

        [Fact]
        public void NumBitsToStore_Zero()
        {
            var sizes = new[] { 0, 1 };
            var bits = sizes.Select(size => BinaryUtils.NumBitsToStore(size)).ToList();
            bits.ForEach(bit => Assert.Equal(0, bit));
        }

        [Fact]
        public void NumBitsToStore_One()
        {
            var sizes = new[] { 2 };
            var bits = sizes.Select(size => BinaryUtils.NumBitsToStore(size)).ToList();
            bits.ForEach(bit => Assert.Equal(1, bit));
        }

        [Fact]
        public void NumBitsToStore_Two()
        {
            var sizes = new[] { 3, 4 };
            var bits = sizes.Select(size => BinaryUtils.NumBitsToStore(size)).ToList();
            bits.ForEach(bit => Assert.Equal(2, bit));
        }

        [Fact]
        public void NumBitsToStore_Three()
        {
            var sizes = new[] { 5, 6, 7, 8 };
            var bits = sizes.Select(size => BinaryUtils.NumBitsToStore(size)).ToList();
            bits.ForEach(bit => Assert.Equal(3, bit));
        }

        [Fact]
        public void NumBitsToStore_Four()
        {
            var sizes = new[] { 9, 10, 11, 12, 13, 14, 15, 16 };
            var bits = sizes.Select(size => BinaryUtils.NumBitsToStore(size)).ToList();
            bits.ForEach(bit => Assert.Equal(4, bit));
        }

        /*
         *  IntToBinaryString
         */

        [Fact]
        public void IntToBinary()
        {
            Assert.Equal("0", BinaryUtils.IntToBinary(0));
            Assert.Equal("1", BinaryUtils.IntToBinary(1));
            Assert.Equal("10", BinaryUtils.IntToBinary(2));
            Assert.Equal("11", BinaryUtils.IntToBinary(3));
            Assert.Equal("100", BinaryUtils.IntToBinary(4));
            Assert.Equal("101", BinaryUtils.IntToBinary(5));
            Assert.Equal("110", BinaryUtils.IntToBinary(6));
            Assert.Equal("111", BinaryUtils.IntToBinary(7));
            Assert.Equal("1000", BinaryUtils.IntToBinary(8));
            Assert.Equal("1001", BinaryUtils.IntToBinary(9));
            Assert.Equal("1010", BinaryUtils.IntToBinary(10));
            Assert.Equal("1011", BinaryUtils.IntToBinary(11));
            Assert.Equal("1100", BinaryUtils.IntToBinary(12));
            Assert.Equal("1101", BinaryUtils.IntToBinary(13));
            Assert.Equal("1110", BinaryUtils.IntToBinary(14));
            Assert.Equal("1111", BinaryUtils.IntToBinary(15));
            Assert.Equal("10000", BinaryUtils.IntToBinary(16));
            Assert.Equal("1010000", BinaryUtils.IntToBinary(80));
        }
    }
}
