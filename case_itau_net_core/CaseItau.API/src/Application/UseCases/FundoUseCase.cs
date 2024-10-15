using AutoMapper;
using CaseItau.API.src.Domain.Entities;
using CaseItau.API.src.Domain.Interfaces;
using CaseItau.API.src.Domain.DTOs;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CaseItau.API.src.Application.UseCases
{
    public class FundoUseCase
    {
        private readonly IFundoRepository _fundoRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<FundoUseCase> _logger;

        public FundoUseCase(IFundoRepository fundoRepository, IMapper mapper, ILogger<FundoUseCase> logger)
        {
            _fundoRepository = fundoRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<FundoDTO>> GetAllFundosAsync()
        {
            var fundos = await _fundoRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<FundoDTO>>(fundos);
        }

        public async Task<FundoDTO> GetFundoByCodigoAsync(string codigo)
        {
            var fundo = await _fundoRepository.GetByCodigoAsync(codigo);
            return fundo == null ? null : _mapper.Map<FundoDTO>(fundo);
        }

        public async Task<CreateFundoResult> CreateFundoAsync(FundoDTO newFundoDto)
        {
            var newFundo = _mapper.Map<Fundo>(newFundoDto);
            var fundo = await _fundoRepository.GetByCodigoAsync(newFundo.Codigo);

            if (fundo != null)
            {
                return new CreateFundoResult
                {
                    Success = false,
                    FundoJaExiste = true,
                    ErrorMessage = "Fundo já existente com o código fornecido."
                };
            }

            await _fundoRepository.CreateFundoAsync(newFundo);
            return new CreateFundoResult
            {
                Success = true,
                Fundo = _mapper.Map<FundoDTO>(newFundo)
            };
        }

        public async Task<bool> UpdateFundoAsync(string codigo, FundoDTO updatedFundoDto)
        {
            var existingFundo = await _fundoRepository.GetByCodigoAsync(codigo);
            if (existingFundo == null)
            {
                return false;
            }

            var updatedFundo = _mapper.Map<Fundo>(updatedFundoDto);
            await _fundoRepository.UpdateFundoAsync(codigo, updatedFundo);
            return true;
        }

        public async Task<bool> DeleteFundoAsync(string codigo)
        {
            var existingFundo = await _fundoRepository.GetByCodigoAsync(codigo);
            if (existingFundo == null)
            {
                return false;
            }

            await _fundoRepository.DeleteFundoAsync(codigo);
            return true;
        }

        public async Task<bool> MovimentarPatrimonioAsync(string codigo, decimal valor)
        {
            var existingFundo = await _fundoRepository.GetByCodigoAsync(codigo);
            if (existingFundo == null)
            {
                return false;
            }

            await _fundoRepository.MovimentarPatrimonioAsync(codigo, valor);
            return true;
        }
    }
}
