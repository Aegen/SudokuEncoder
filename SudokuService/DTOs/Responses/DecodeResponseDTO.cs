using System.Collections.Generic;

namespace SudokuService.DTOs.Responses
{
    public class DecodeResponseDTO
    {
        public string Code { get; set; }
        public string Binary { get; set; }
        public List<int> Grid { get; set; }
    }
}
