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
            binaryStringBuilder.Append(BinaryUtils.LeftPadBinaryToSize(IntToBinaryString(longestJumpBits), 3));

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
            var address = 0;
            var output = tokens.Select(token => TokenToBitString(token, longestJumpBits, ref sudokuGrid, ref address)).ToList();
            return output;
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

        private static String TokenToBitString(Token token, int longestJumpBits, ref SudokuGrid sudokuGrid, ref int address)
        {
            var bitString = new StringBuilder();
            if (token.Type == CellTypeEnum.VALUE)
            {
                for (var i = 0; i < token.Values.Count - 1; i++)
                {
                    var possibleValues = sudokuGrid.GetPossibleCellValues(address);
                    var neededBits = GetNumBitsToStore(possibleValues.Count);
                    var rawBits = IntToBinaryString(FindValueIndex(possibleValues, token.Values[i]));
                    var valueBits = BinaryUtils.LeftPadBinaryToSize(
                        rawBits,
                        neededBits
                        );

                    bitString.Append(valueBits);
                    bitString.Append('1');
                    sudokuGrid.SetCellValue(address, token.Values[i]);
                    address++;
                }

                var finalPossibleValues = sudokuGrid.GetPossibleCellValues(address);
                var finalNeededBits = GetNumBitsToStore(finalPossibleValues.Count);
                var finalRawBits = IntToBinaryString(FindValueIndex(finalPossibleValues, token.Values.Last()));
                var finalValueBits = BinaryUtils.LeftPadBinaryToSize(
                    finalRawBits,
                    finalNeededBits
                    );

                bitString.Append(finalValueBits);
                bitString.Append('0');
                sudokuGrid.SetCellValue(address, token.Values.Last());
                address++;
            }
            else if (token.Type == CellTypeEnum.EMPTY)
            {
                var valueBits = BinaryUtils.LeftPadBinaryToSize(IntToBinaryString(token.Length - 1), longestJumpBits);
                bitString.Append(valueBits);
                address += token.Length;
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
                .Select(token => GetNumBitsToStore(token.Length))
                .Max();

        private static int GetNumBitsToStore(int size) => (int)Math.Ceiling(Math.Log2(size));

        private static String IntToBinaryString(int value) => Convert.ToString(value, 2);

        private static int GetJumpBitSize(int index, int longestJumpBits)
        {
            var indexBits = GetNumBitsToStore(index);
            return (indexBits < longestJumpBits) ? indexBits : longestJumpBits;
        }
    }
}
