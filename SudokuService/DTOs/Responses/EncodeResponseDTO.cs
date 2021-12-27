using System;
using System.Collections.Generic;

namespace SudokuService.DTOs.Responses
{
    public class EncodeResponseDTO
    {
        public List<int> Grid { get; set; }
        public String Binary { get; set; } //This is just for testing creating the binary representation
        public String Code { get; set; }
    }
}
