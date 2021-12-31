using System;
using SudokuService.Services.Resources;
using SudokuService.Shared;

namespace SudokuService.Services
{
    public class DecoderService
    {
        public static DecoderResponse Decode(string binary)
        {
            var bitsUsed = 0;
            var remainingBits = binary;
            var bits = "";

            // Max Jump Section
            (bits, remainingBits) = BinaryUtils.ClipHead(remainingBits, 3);
            bitsUsed += bits.Length;
            var maxJumpBitsNeeded = BinaryUtils.BinaryToInt(bits);

            //First token value or jump
            (bits, remainingBits) = BinaryUtils.ClipHead(remainingBits, 1);
            bitsUsed += bits.Length;
            var nextTokenType = (bits == "1") ? CellTypeEnum.VALUE : CellTypeEnum.EMPTY;

            var sudokuGrid = new SudokuGrid();
            var address = 0;

            while (address < 81)
            {
                if (nextTokenType == CellTypeEnum.VALUE)
                {
                    for (var nextValueBit = "1"; nextValueBit == "1";) { 
                        var neededSize = sudokuGrid.GetPossibleCellValues(address).Count;
                        var bitsToStore = BinaryUtils.NumBitsToStore(neededSize);
                        (bits, remainingBits) = BinaryUtils.ClipHead(remainingBits, bitsToStore);
                        (nextValueBit, remainingBits) = BinaryUtils.ClipHead(remainingBits, 1);
                        bitsUsed += bits.Length + 1;
                        var index = BinaryUtils.BinaryToInt(bits);
                        sudokuGrid.SetCellByIndex(address, index);
                        address++;
                    }
                    nextTokenType = CellTypeEnum.EMPTY;
                }
                else if (nextTokenType == CellTypeEnum.EMPTY)
                {
                    var bitsNeeded = GridUtils.GetJumpBitsPerSchedule(address, maxJumpBitsNeeded);
                    (bits, remainingBits) = BinaryUtils.ClipHead(remainingBits, bitsNeeded);
                    var jumpLength = BinaryUtils.BinaryToInt(bits);
                    bitsUsed += bits.Length;
                    address += jumpLength + 1;
                    nextTokenType = CellTypeEnum.VALUE;
                }
            }

            return new DecoderResponse()
            {
                Binary = binary.Substring(0, bitsUsed),
                Grid = sudokuGrid.Get1DGrid()
            };
        }

        public void GetNext()
        {
            
        }
    }
}
