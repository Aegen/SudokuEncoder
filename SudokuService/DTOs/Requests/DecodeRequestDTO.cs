using System;
using System.ComponentModel.DataAnnotations;
using SudokuService.Constants;

namespace SudokuService.DTOs.Requests
{
    public class DecodeRequestDTO
    {
        [RegularExpression(pattern: ValidationConstants.Base64Pattern)]
        public String Code { get; set; }
    }
}
