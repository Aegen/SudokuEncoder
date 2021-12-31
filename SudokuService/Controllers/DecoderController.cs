using Microsoft.AspNetCore.Mvc;
using SudokuService.DTOs.Requests;
using SudokuService.DTOs.Responses;
using SudokuService.Services;
using SudokuService.Shared;

namespace SudokuService.Controllers
{
    [ApiController]
    public class DecoderController : Controller
    {
        [HttpPost]
        [Route("/api/v1/sudoku/decode")]
        public ActionResult<DecodeResponseDTO> Decode(DecodeRequestDTO decodeRequestDTO)
        {
            var binary = Base64Converter.ToBinary(decodeRequestDTO.Code);
            var response = DecoderService.Decode(binary);

            return new DecodeResponseDTO
            {
                Code = decodeRequestDTO.Code,
                Binary = response.Binary,
                Grid = response.Grid
            };
        }
    }
}
