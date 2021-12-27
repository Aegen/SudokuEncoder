using System;
using System.Collections.Generic;
using System.Linq;

namespace SudokuService.Shared
{
    public class SudokuGrid
    {
        private List<List<int>> grid = new() { // TODO: This may not be necessary, I was just trying things to fix an IndexOutOfBoundsException
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0 }
        };

        // Sets for all 27 zones (rows, columns, and 3x3 regions) where the numbers [1-9] can only appear once
        // Each set will contain all values that have already been placed in that zone
        private readonly List<HashSet<int>> rows = InitializeZoneSets();
        private readonly List<HashSet<int>> columns = InitializeZoneSets();
        private readonly List<HashSet<int>> regions = InitializeZoneSets();

        private static List<HashSet<int>> InitializeZoneSets() => new() { new HashSet<int>(), new HashSet<int>(), new HashSet<int>(), new HashSet<int>(), new HashSet<int>(), new HashSet<int>(), new HashSet<int>(), new HashSet<int>(), new HashSet<int>() };

        public SudokuGrid(List<int> grid)
        {
            if (!IsValidGrid(grid))
                throw new ArgumentException("Grid was not valid");

            for(var i = 0; i < 81; i++)
            {
                if (grid[i] != 0)
                {
                    this.SetCellValue(i, grid[i]);
                }
            }
        }

        public SudokuGrid(){}

        public int GetCellValue(int x, int y)
        {
            return this.grid[x][y];
        }

        public int GetCellValue(int address)
        {
            var (x, y) = GetCoordinatesFromAddress(address);
            return GetCellValue(x, y);
        }

        public void SetCellValue(int x, int y, int value)
        {
            if (value < 1 || value > 9)
                throw new ArgumentOutOfRangeException(nameof(value), "Value must be in the range [1-9]");

            this.columns[x].Add(value);
            this.rows[y].Add(value);
            var regionIndex = (y / 3 * 3) + (x / 3);
            this.regions[regionIndex].Add(value);

            this.grid[x][y] = value;
        }

        public void SetCellValue(int address, int value)
        {
            var (x, y) = GetCoordinatesFromAddress(address);

            SetCellValue(x, y, value);
        }

        public bool SetCellByIndex(int x, int y, int index)
        {
            var possibleValues = GetPossibleCellValues(x, y);

            if(possibleValues.Count-1 < index || index < 0)
                return false;

            this.SetCellValue(x, y, possibleValues[index]);
            return true;
        }

        public void SetCellByIndex(int address, int index)
        {
            var (x, y) = GetCoordinatesFromAddress(address);
            this.SetCellByIndex(x, y, index);
        }

        public List<int> GetPossibleCellValues(int x, int y)
        {
            var regionIndex = (y / 3 * 3) + (x / 3);

            var column = this.columns[x];
            var row = this.rows[y];
            var region = this.regions[regionIndex];

            var possibleValues = new HashSet<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            var remainingValues = RemoveSetsFromBaseSet(possibleValues, column, row, region);

            return new List<int>(remainingValues);
        }

        public List<int> GetPossibleCellValues(int address)
        {
            var (x, y) = GetCoordinatesFromAddress(address);

            return GetPossibleCellValues(x, y);
        }

        private static HashSet<int> RemoveSetsFromBaseSet(HashSet<int> baseSet, params HashSet<int>[] setsToRemove)
        {
            foreach (var set in setsToRemove)
            {
                baseSet.ExceptWith(set);
            }

            return baseSet;
        }

        private static bool IsValidGrid(List<int> rawGrid)
        {
            if(rawGrid.Count != 81)
                return false;

            var validNumbers = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            foreach(var cell in rawGrid)
            {
                if (!validNumbers.Contains(cell))
                    return false;
            }

            return true;
        }

        private static (int x, int y) GetCoordinatesFromAddress(int address)
        {
            if (address > 80 || address < 0)
                throw new ArgumentOutOfRangeException(nameof(address), "Address must be in the range [0-80]");

            return (address % 9, address / 9);
        }

        public override bool Equals(object obj)
        {
            return obj is SudokuGrid grid &&
                   EqualityComparer<List<List<int>>>.Default.Equals(this.grid, grid.grid);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(grid);
        }

        public List<int> Get1DGrid()
        {
            var resultGrid = new List<int>();

            for(var i = 0; i < 81; i++)
            {
                resultGrid.Add(this.GetCellValue(i));
            }

            return resultGrid;
        }
    }
}
