using System;
using System.Collections.Generic;

namespace SudokuService.Services.Resources
{
    public class DecoderResponse
    {
        public List<int> Grid { get; set; }
        public string Binary { get; set; }
    }
}
