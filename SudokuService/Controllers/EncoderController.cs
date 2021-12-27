using Microsoft.AspNetCore.Mvc;
using SudokuService.DTOs.Requests;
using SudokuService.DTOs.Responses;
using SudokuService.Services;
using SudokuService.Shared;

namespace SudokuService.Controllers
{
    [ApiController]
    public class EncoderController : Controller
    {
        [HttpPost]
        [Route("/api/v1/sudoku/encode")]
        public ActionResult<EncodeResponseDTO> Encode(EncodeRequestDTO encodeRequestDTO)
        {
            var binaryString = EncoderService.EncodeGrid(encodeRequestDTO.grid);
            var code = Base64Converter.FromBinary(binaryString);

            return new EncodeResponseDTO
            {
                Grid = encodeRequestDTO.grid,
                Binary = binaryString,
                Code = code
            };
        }
    }
}
