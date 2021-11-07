using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using SudokuService.Shared;

namespace SudokuService.Controllers.Utilities
{
    public class Base64Converter
    {
        private static readonly Dictionary<String, Char> binaryToBase64Map = new Dictionary<String, Char> {
            {"000000", 'A'},
            {"000001", 'B'},
            {"000010", 'C'},
            {"000011", 'D'},
            {"000100", 'E'},
            {"000101", 'F'},
            {"000110", 'G'},
            {"000111", 'H'},
            {"001000", 'I'},
            {"001001", 'J'},
            {"001010", 'K'},
            {"001011", 'L'},
            {"001100", 'M'},
            {"001101", 'N'},
            {"001110", 'O'},
            {"001111", 'P'},
            {"010000", 'Q'},
            {"010001", 'R'},
            {"010010", 'S'},
            {"010011", 'T'},
            {"010100", 'U'},
            {"010101", 'V'},
            {"010110", 'W'},
            {"010111", 'X'},
            {"011000", 'Y'},
            {"011001", 'Z'},
            {"011010", 'a'},
            {"011011", 'b'},
            {"011100", 'c'},
            {"011101", 'd'},
            {"011110", 'e'},
            {"011111", 'f'},
            {"100000", 'g'},
            {"100001", 'h'},
            {"100010", 'i'},
            {"100011", 'j'},
            {"100100", 'k'},
            {"100101", 'l'},
            {"100110", 'm'},
            {"100111", 'n'},
            {"101000", 'o'},
            {"101001", 'p'},
            {"101010", 'q'},
            {"101011", 'r'},
            {"101100", 's'},
            {"101101", 't'},
            {"101110", 'u'},
            {"101111", 'v'},
            {"110000", 'w'},
            {"110001", 'x'},
            {"110010", 'y'},
            {"110011", 'z'},
            {"110100", '0'},
            {"110101", '1'},
            {"110110", '2'},
            {"110111", '3'},
            {"111000", '4'},
            {"111001", '5'},
            {"111010", '6'},
            {"111011", '7'},
            {"111100", '8'},
            {"111101", '9'},
            {"111110", '+'},
            {"111111", '/'}
        };

        private static readonly Dictionary<Char, String> base64ToBinaryMap = new Dictionary<Char, String> {
            {'A', "000000" },
            {'B', "000001" },
            {'C', "000010" },
            {'D', "000011" },
            {'E', "000100" },
            {'F', "000101" },
            {'G', "000110" },
            {'H', "000111" },
            {'I', "001000" },
            {'J', "001001" },
            {'K', "001010" },
            {'L', "001011" },
            {'M', "001100" },
            {'N', "001101" },
            {'O', "001110" },
            {'P', "001111" },
            {'Q', "010000" },
            {'R', "010001" },
            {'S', "010010" },
            {'T', "010011" },
            {'U', "010100" },
            {'V', "010101" },
            {'W', "010110" },
            {'X', "010111" },
            {'Y', "011000" },
            {'Z', "011001" },
            {'a', "011010" },
            {'b', "011011" },
            {'c', "011100" },
            {'d', "011101" },
            {'e', "011110" },
            {'f', "011111" },
            {'g', "100000" },
            {'h', "100001" },
            {'i', "100010" },
            {'j', "100011" },
            {'k', "100100" },
            {'l', "100101" },
            {'m', "100110" },
            {'n', "100111" },
            {'o', "101000" },
            {'p', "101001" },
            {'q', "101010" },
            {'r', "101011" },
            {'s', "101100" },
            {'t', "101101" },
            {'u', "101110" },
            {'v', "101111" },
            {'w', "110000" },
            {'x', "110001" },
            {'y', "110010" },
            {'z', "110011" },
            {'0', "110100" },
            {'1', "110101" },
            {'2', "110110" },
            {'3', "110111" },
            {'4', "111000" },
            {'5', "111001" },
            {'6', "111010" },
            {'7', "111011" },
            {'8', "111100" },
            {'9', "111101" },
            {'+', "111110" },
            {'/', "111111" }
        };

        public static Char BinaryToBase64(String bits)
        {
            if (!Regex.IsMatch(bits, "^[01]{6}$"))
                throw new ArgumentException("Must contain 6 bits", nameof(bits));

            return binaryToBase64Map[bits];
        }

        private static readonly List<Char> validBase64Characters = new() { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '+', '/' };

        public static Boolean IsBase64(String characters)
        {
            foreach(var character in characters)
            {
                //TODO: Can probably be made faster by converting from checking if it's in the array to checking if it's within character ranges
                if (!validBase64Characters.Contains(character)) 
                {
                    return false;
                }
            }

            return true;
        }

        public static String Base64ToBinary(Char base64Char)
        {
            if (!IsBase64(base64Char.ToString()))
                throw new ArgumentException("Must be a valid base64 character", nameof(base64Char));

            return base64ToBinaryMap[base64Char];
        }

        public static String ConvertBinaryStringToBase64(String binaryString)
        {
            if (!Regex.IsMatch(binaryString, "^[01]+$"))
                throw new ArgumentException("Must be composed of only 1s and 0s", nameof(binaryString));

            var binaryStrings = BinaryUtils.SplitInto6BitStrings(binaryString);
            var individualBase64Chars = binaryStrings.Select(binary => BinaryToBase64(binary)).ToArray();

            return String.Join("", individualBase64Chars);
        }

        public static String ConvertBase64ToBinaryString(String base64String)
        {
            if (!IsBase64(base64String))
                throw new ArgumentException("Must be a base64 string", nameof(base64String));

            var binaryStrings = base64String.ToCharArray().Select(character => Base64ToBinary(character)).ToArray();

            return String.Join("", binaryStrings);
        }
    }
}
