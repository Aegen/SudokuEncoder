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
            var tokens = TokenUtils.TokenizeGrid(grid);
            var maxJumpBitsNeeded = GetLongestJumpBits(tokens);

            var binaryStringBuilder = new StringBuilder();

            binaryStringBuilder.Append(MaxJumpSection(maxJumpBitsNeeded));

            binaryStringBuilder.Append(InitialChainBitSection(tokens));

            var tokenBinaryStrings = GetTokenBinaryStrings(tokens, maxJumpBitsNeeded);

            tokenBinaryStrings.ForEach(tokenBinaryString => binaryStringBuilder.Append(tokenBinaryString));

            return binaryStringBuilder.ToString();
        }

        private static String MaxJumpSection(int bitsNeeded) => BinaryUtils.LeftPadToSize(BinaryUtils.IntToBinary(bitsNeeded), 3);

        private static String InitialChainBitSection(List<Token> tokens) =>(tokens.First().Type == CellTypeEnum.EMPTY) ? "0" : "1";

        private static List<String> GetTokenBinaryStrings(List<Token> tokens, int maxJumpBits)
        {
            var sudokuGrid = new SudokuGrid();
            return tokens.Select(token => TokenToBinary(token, maxJumpBits, ref sudokuGrid)).ToList();
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

            return -1; //TODO: Maybe change this to an exception, happy pathing right now
        }

        private static String TokenToBinary(Token token, int maxJumpBits, ref SudokuGrid sudokuGrid)
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
                var neededBitCount = GridUtils.GetJumpBitsPerSchedule(token.StartIndex, maxJumpBits);
                var valueBits = BinaryUtils.LeftPadToSize(rawBits, neededBitCount);
                bitString.Append(valueBits);
            }

            return bitString.ToString();
        }

        private static int GetLongestJumpBits(List<Token> tokens) => tokens
                .Where(token => token.Type == CellTypeEnum.EMPTY)
                .Select(token => BinaryUtils.NumBitsToStore(token.Length))
                .Max();
    }
}
