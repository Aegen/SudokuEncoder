using System;
using System.Collections.Generic;

namespace SudokuService.Services
{
    public class Token
    {
        public CellTypeEnum Type { get; set; }
        public List<int> Values { get; set; }
        public int Size { get; set; }
        public int StartIndex { get; set; }
    }
}
