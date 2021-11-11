using System;
using System.Collections.Generic;

namespace SudokuService.DTOs.Responses
{
    public class EncodeResponseDTO
    {
        public List<int> grid { get; set; }
        public String binary { get; set; } //This is just for testing creating the binary representation
        public String code { get; set; }
    }
}
