using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using SudokuService.DTOs.Requests;
using SudokuService.DTOs.Responses;
using SudokuService.Services;

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
            var service = new EncoderService();
            var binaryString = service.EncodeGrid(encodeRequestDTO.grid);
            var code = Convert.ToBase64String(GetBytes(binaryString));

            var encodeResponseDTO = new EncodeResponseDTO
            {
                grid = encodeRequestDTO.grid,
                binary = binaryString,
                code = code
            };


            return encodeResponseDTO;
        }

        public static byte[] GetBytes(string bitString)
        {
            return Enumerable.Range(0, bitString.Length / 8).
                Select(pos => Convert.ToByte(
                    bitString.Substring(pos * 8, 8),
                    2)
                ).ToArray();
        }

    }
}
