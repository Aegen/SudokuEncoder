using System;
using System.Collections.Generic;

namespace SudokuService.Shared
{
    public class SudokuGrid
    {
        private List<List<int>> grid;

        public SudokuGrid(List<int> grid)
        {
            if (!ValidateRawGrid(grid))
            {
                throw new ArgumentException("Grid was not valid");
            }

            var x = 0;
            var y = 0;

            for(var i = 0; i < 81; i++)
            {
                this.grid[x][y] = grid[i];

                if (x < 9)
                {
                    x++;
                }
                else
                {
                    x = 0;
                    y++;
                }
            }
        }

        public int GetCellValue(int x, int y)
        {
            return this.grid[x][y];
        }

        public void SetCellValue(int x, int y, int value)
        {
            if (value < 1 || value > 9)
                throw new ArgumentOutOfRangeException(nameof(value), "Value must be in the range [1-9]");

            this.grid[x][y] = value;
        }

        public List<int> GetPossibleCellValues(int x, int y)
        {
            var possibleValues = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            // Remove values from the same column and row
            for(var i = 0; i < 9; i++)
            {
                if(this.grid[x][i] > 0)
                {
                    possibleValues.Remove(this.grid[x][i]);
                }

                if (this.grid[i][y] > 0)
                {
                    possibleValues.Remove(this.grid[i][y]);
                }
            }

            // Remove values from the same 3x3 region
            var localX = x - (x % 3);
            var localY = y - (y % 3);

            for(var xMod = 0; xMod < 3; xMod++)
            {
                for (var yMod = 0; yMod < 3; yMod++)
                {
                    var cellValue = this.grid[localX + xMod][localY + yMod];
                    if (cellValue > 0)
                    {
                        possibleValues.Remove(cellValue);
                    }
                }
            }

            return possibleValues;
        }

        private static bool ValidateRawGrid(List<int> rawGrid)
        {
            if(rawGrid.Count != 81)
            {
                return false;
            }

            var validNumbers = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            foreach(var cell in rawGrid)
            {
                if (!validNumbers.Contains(cell))
                    return false;
            }

            return true;
        }
    }
}
