using System;
using System.Collections.Generic;
using System.Linq;
using SudokuService.Services;

namespace SudokuService.Shared
{
    public class TokenUtils
    {
        public static List<Token> TokenizeGrid(List<int> grid)
        {
            if (grid == null || grid.Count == 0)
                throw new ArgumentException("The provided grid is either null or empty", nameof(grid));

            var tokens = new List<Token> { CreateIslandToken(grid[0], 0) };

            for (var index = 1; index < grid.Count; index++)
            {
                var newestIsland = tokens.Last();

                if (newestIsland.Type == GetCellType(grid[index]))
                {
                    newestIsland.Values.Add(grid[index]);
                    newestIsland.Length++;
                }
                else
                {
                    tokens.Add(CreateIslandToken(grid[index], index));
                }
            }

            return tokens;
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
    }
}
