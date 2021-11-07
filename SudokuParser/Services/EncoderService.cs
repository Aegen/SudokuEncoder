using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SudokuService.Shared;

namespace SudokuService.Services
{
    public class EncoderService
    {
        public static String EncodeGrid(List<int> grid)
        {
            var tokens = TokenizeGrid(grid);

            var longestJumpBits = GetLongestJumpBits(tokens);
            var binaryStringBuilder = new StringBuilder();

            // Add max jump section
            binaryStringBuilder.Append(BinaryUtils.LeftPadToSize(BinaryUtils.IntToBinary(longestJumpBits), 3));

            // Add chain bit for first value
            if (tokens[0].Type == CellTypeEnum.EMPTY)
            {
                binaryStringBuilder.Append('0');
            }
            else
            {
                binaryStringBuilder.Append('1');
            }

            var tokenBinaryStrings = GetTokenBinaryStrings(tokens, longestJumpBits);

            tokenBinaryStrings.ForEach(tokenBinaryString => binaryStringBuilder.Append(tokenBinaryString));

            return binaryStringBuilder.ToString();
        }

        private static List<String> GetTokenBinaryStrings(List<Token> tokens, int longestJumpBits)
        {
            var sudokuGrid = new SudokuGrid();
            return tokens.Select(token => TokenToBitString(token, longestJumpBits, ref sudokuGrid)).ToList();
        }

        private static int FindValueIndex(List<int> possibleValue, int valueToFind)
        {
            for (var idx = 0; idx < possibleValue.Count; idx++)
            {
                if (possibleValue[idx] == valueToFind)
                {
                    return idx;
                }
            }

            return -1;
        }

        private static String TokenToBitString(Token token, int longestJumpBits, ref SudokuGrid sudokuGrid)
        {
            var bitString = new StringBuilder();
            if (token.Type == CellTypeEnum.VALUE)
            {
                for (var i = 0; i < token.Values.Count; i++)
                {
                    var possibleValues = sudokuGrid.GetPossibleCellValues(token.StartIndex + i);
                    var neededBitCount = BinaryUtils.NumBitsToStore(possibleValues.Count);
                    var rawBits = BinaryUtils.IntToBinary(FindValueIndex(possibleValues, token.Values[i]));
                    var valueBits = BinaryUtils.LeftPadToSize(rawBits, neededBitCount);

                    var chainBit = (i == token.Values.Count - 1) ? '0' : '1';

                    bitString.Append(valueBits);
                    bitString.Append(chainBit);

                    sudokuGrid.SetCellValue(token.StartIndex + i, token.Values[i]);
                }
            }
            else if (token.Type == CellTypeEnum.EMPTY && token.StartIndex != 80)
            {
                var rawBits = BinaryUtils.IntToBinary(token.Length - 1);
                var neededBitCount = GetJumpBitsPerSchedule(token.StartIndex, longestJumpBits);
                var valueBits = BinaryUtils.LeftPadToSize(rawBits, neededBitCount);
                bitString.Append(valueBits);
            }

            return bitString.ToString();
        }

        private static List<Token> TokenizeGrid(List<int> grid)
        {
            if (grid == null || grid.Count == 0)
                throw new ArgumentException("The provided grid is either null or empty");

            var islands = new List<Token> { CreateIslandToken(grid[0], 0) };

            for (var index = 1; index < grid.Count; index++)
            {
                var newestIsland = islands.Last();

                if (newestIsland.Type == GetCellType(grid[index]))
                {
                    newestIsland.Values.Add(grid[index]);
                    newestIsland.Length++;
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
            return new Token
            {
                StartIndex = index,
                Length = 1,
                Values = new List<int> { cellValue },
                Type = GetCellType(cellValue)
            };
        }

        private static CellTypeEnum GetCellType(int cellValue) => (cellValue == 0) ? CellTypeEnum.EMPTY : CellTypeEnum.VALUE;

        private static int GetLongestJumpBits(List<Token> tokens) => tokens
                .Where(token => token.Type == CellTypeEnum.EMPTY)
                .Select(token => BinaryUtils.NumBitsToStore(token.Length))
                .Max();

        public static int GetJumpBitsPerSchedule(int address, int longestJumpBits)
        {
            var indexBits = BinaryUtils.NumBitsToStore(81 - address);
            return (indexBits < longestJumpBits) ? indexBits : longestJumpBits;
        }
    }
}
