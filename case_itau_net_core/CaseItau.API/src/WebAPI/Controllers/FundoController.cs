using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CaseItau.API.src.Application.DTOs;
using CaseItau.API.src.Application.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace CaseItau.API.src.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FundoController : ControllerBase
    {
        private readonly FundoUseCase _fundoUseCase;

        public FundoController(FundoUseCase fundoUseCase)
        {
            _fundoUseCase = fundoUseCase;
        }

        // [HttpGet("test-exception")]
        // public IActionResult TestException()
        // {
        //     throw new Exception("Erro de teste.");
        // }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FundoDTO>>> Get()
        {
            var fundos = await _fundoUseCase.GetAllFundosAsync();
            return Ok(fundos);
        }

        [HttpGet("{codigo}")]
        public async Task<ActionResult<FundoDTO>> Get(string codigo)
        {
            var fundo = await _fundoUseCase.GetFundoByCodigoAsync(codigo);
            if (fundo == null)
                return NotFound();

            return Ok(fundo);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] FundoDTO fundoDto)
        {
            var result = await _fundoUseCase.CreateFundoAsync(fundoDto);

            if (!result.Success && result.FundoJaExiste)
            {
                return Conflict(new { message = result.ErrorMessage });
            }

            if (!result.Success)
            {
                return StatusCode(500, result.ErrorMessage);
            }

            return CreatedAtAction(nameof(Post), new { codigo = result.Fundo.Codigo }, result.Fundo);
        }

        [HttpPut("{codigo}")]
        public async Task<IActionResult> Put(string codigo, [FromBody] FundoDTO fundoDto)
        {
            var updated = await _fundoUseCase.UpdateFundoAsync(codigo, fundoDto);
            if (!updated)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{codigo}")]
        public async Task<IActionResult> Delete(string codigo)
        {
            var deleted = await _fundoUseCase.DeleteFundoAsync(codigo);
            if (!deleted)
                return NotFound();

            return NoContent();
        }

        [HttpPut("{codigo}/patrimonio")]
        public async Task<IActionResult> MovimentarPatrimonio(string codigo, [FromBody] decimal value)
        {
            var updated = await _fundoUseCase.MovimentarPatrimonioAsync(codigo, value);
            if (!updated)
                return NotFound();

            return NoContent();
        }
    }
}
