using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuService.Services
{
    public class EncoderService
    {
        public EncoderService()
        {

        }

        public String EncodeGrid(List<int> grid)
        {
            var tokens = TokenizeGrid(grid);

            var longestJumpBits = GetLongestJumpBits(tokens);
            var binaryStringBuilder = new StringBuilder();

            // Add max jump section
            binaryStringBuilder.Append(PadToSize(IntToBinaryString(longestJumpBits), 3));

            // Add chain bit for first value
            if (tokens[0].Type == CellTypeEnum.EMPTY)
            {
                binaryStringBuilder.Append('0');
            } else
            {
                binaryStringBuilder.Append('1');
            }

            var tokenBinaryStrings = GetTokenBinaryStrings(tokens, longestJumpBits);

            tokenBinaryStrings.ForEach(tokenBinaryString => binaryStringBuilder.Append(tokenBinaryString));

            return binaryStringBuilder.ToString();
        }

        private List<String> GetTokenBinaryStrings(List<Token> tokens, int longestJumpBits) => tokens.Select(token => TokenToBitString(token, longestJumpBits)).ToList();

        private String TokenToBitString(Token token, int longestJumpBits)
        {
            var bitString = new StringBuilder();
            if (token.Type == CellTypeEnum.VALUE)
            {
                for (var i = 0; i < token.Values.Count - 1; i++)
                {
                    var valueBits = PadToSize(IntToBinaryString(token.Values[i]), 4);

                    bitString.Append(valueBits);
                    bitString.Append('1');
                }

                var finalValueBits = PadToSize(IntToBinaryString(token.Values[^1]), 4);

                bitString.Append(finalValueBits);
                bitString.Append('0');
            }
            else if (token.Type == CellTypeEnum.EMPTY)
            {
                var valueBits = PadToSize(IntToBinaryString(token.Size - 1), longestJumpBits);
                bitString.Append(valueBits);
            }

            return bitString.ToString();
        }



        private List<Token> TokenizeGrid(List<int> grid)
        {
            if (grid.Count == 0)
                return null; // TODO: Make this throw an exception

            var islands = new List<Token> { CreateIslandToken(grid[0], 0) };

            for (var index = 1; index < grid.Count; index++)
            {
                var newestIsland = islands[^1];

                if (newestIsland.Type == GetCellType(grid[index]))
                {
                    newestIsland.Values.Add(grid[index]);
                    newestIsland.Size++;
                }
                else
                {
                    islands.Add(CreateIslandToken(grid[index], index));
                }
            }

            return islands;
        }

        private static Token CreateIslandToken(int cellValue, int index)
        {
            var token = new Token
            {
                StartIndex = index,
                Size = 1,
                Values = new List<int>(),
                Type = GetCellType(cellValue)
            };

            token.Values.Add(cellValue);

            return token;
        }

        private static CellTypeEnum GetCellType(int cellValue) => (cellValue == 0) ? CellTypeEnum.EMPTY : CellTypeEnum.VALUE;

        private static int GetLongestJumpBits(List<Token> tokens) => tokens
                .Where(token => token.Type == CellTypeEnum.EMPTY)
                .Select(token => GetNumBitsToRepresent(token.Size))
                .Max();

        private static int GetNumBitsToRepresent(int size) => (int)Math.Ceiling(Math.Log2(size));

        private static String IntToBinaryString(int value) => Convert.ToString(value, 2);

        private static String PadToSize(String binary, int size)
        {
            if (binary.Length > size)
                throw new ArgumentOutOfRangeException(nameof(size), "The requested size is smaller than the input length");

            if (binary.Length == size)
                return binary;

            var difference = size - binary.Length;

            return new string('0', difference) + binary;
        }
    }
}
