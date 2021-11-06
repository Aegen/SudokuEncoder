using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SudokuService.DTOs.Requests
{
    public class EncodeRequestDTO
    {
        [Required, MaxLength(81), MinLength(81)]
        public List<int> grid { get; set; }
    }
}
