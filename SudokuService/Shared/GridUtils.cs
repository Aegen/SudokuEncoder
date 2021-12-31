using System;
namespace SudokuService.Shared
{
    public class GridUtils
    {
        public static int GetJumpBitsPerSchedule(int address, int maxJumpBits)
        {
            var indexBits = BinaryUtils.NumBitsToStore(81 - address);
            return (indexBits < maxJumpBits) ? indexBits : maxJumpBits;
        }
    }
}
