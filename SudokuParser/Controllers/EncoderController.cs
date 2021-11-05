using System;

using Microsoft.AspNetCore.Mvc;
using SudokuService.Controllers.Utilities;
using SudokuService.DTOs.Requests;
using SudokuService.DTOs.Responses;
using SudokuService.Services;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SudokuService.Controllers
{
    [ApiController]
    public class EncoderController : Controller
    {

        private EncoderService encoderService = new EncoderService();

        [HttpPost]
        [Route("/api/v1/sudoku/encode")]
        public ActionResult<EncodeResponseDTO> Encode(EncodeRequestDTO encodeRequestDTO)
        {
            var binaryString = encoderService.EncodeGrid(encodeRequestDTO.grid);
            var code = Convert.ToBase64String(BinaryConversionUtil.GetBytes(binaryString));

            return new EncodeResponseDTO
            {
                grid = encodeRequestDTO.grid,
                binary = binaryString,
                code = RemoveExtraCharacters(code, binaryString.Length)
            };
        }

        public static String RemoveExtraCharacters(String code, int binaryStringLength)
        {
            var neededLength = (int)Math.Ceiling(binaryStringLength / 6.0);

            return code.Substring(0, neededLength);
        }

    }
}
