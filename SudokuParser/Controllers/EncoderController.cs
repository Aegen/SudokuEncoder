using Microsoft.AspNetCore.Mvc;
using SudokuService.Controllers.Utilities;
using SudokuService.DTOs.Requests;
using SudokuService.DTOs.Responses;
using SudokuService.Services;
using SudokuService.Shared;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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
            var code = Base64Converter.ConvertBinaryStringToBase64(binaryString);

            return new EncodeResponseDTO
            {
                grid = encodeRequestDTO.grid,
                binary = binaryString,
                code = code
            };
        }
    }
}
