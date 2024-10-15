using AutoMapper;
using Moq;
using Xunit;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using CaseItau.API.src.Application.UseCases;
using CaseItau.API.src.Domain.Interfaces;
using CaseItau.API.src.Domain.Entities;
using CaseItau.API.src.Application.DTOs;

namespace CaseItau.API.Tests.UseCasesTests
{
    public class FundoUseCaseTests
    {
        private readonly Mock<IFundoRepository> _fundoRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<FundoUseCase>> _loggerMock;
        private readonly FundoUseCase _fundoUseCase;

        public FundoUseCaseTests()
        {
            _fundoRepositoryMock = new Mock<IFundoRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<FundoUseCase>>();
            _fundoUseCase = new FundoUseCase(_fundoRepositoryMock.Object, _mapperMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task ObterTodosFundos_DeveRetornarTodosFundos()
        {
            // Arrange
            var fundos = new List<Fundo>
            {
                new Fundo { Codigo = "001", Nome = "Fundo 1", Cnpj = "12345678901234", Codigo_Tipo = 1, Patrimonio = 1000 },
                new Fundo { Codigo = "002", Nome = "Fundo 2", Cnpj = "56789012345678", Codigo_Tipo = 2, Patrimonio = 2000 }
            };

                    var fundosDto = new List<FundoDTO>
            {
                new FundoDTO { Codigo = "001", Nome = "Fundo 1", Cnpj = "12345678901234", Codigo_Tipo = 1, Patrimonio = 1000 },
                new FundoDTO { Codigo = "002", Nome = "Fundo 2", Cnpj = "56789012345678", Codigo_Tipo = 2, Patrimonio = 2000 }
            };

            _fundoRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(fundos);
            _mapperMock.Setup(m => m.Map<IEnumerable<FundoDTO>>(fundos)).Returns(fundosDto);

            // Act
            var result = await _fundoUseCase.GetAllFundosAsync();

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task CriarFundo_DeveRetornarConflict_SeFundoJaExistir()
        {
            // Arrange
            var fundoDto = new FundoDTO { Codigo = "001", Nome = "Fundo Existente", Cnpj = "12345678901234", Codigo_Tipo = 1, Patrimonio = 1000 };
            var fundoExistente = new Fundo { Codigo = "001", Nome = "Fundo Existente", Cnpj = "12345678901234", Codigo_Tipo = 1, Patrimonio = 1000 };

            _fundoRepositoryMock.Setup(repo => repo.GetByCodigoAsync(fundoDto.Codigo)).ReturnsAsync(fundoExistente);
            _mapperMock.Setup(m => m.Map<Fundo>(fundoDto)).Returns(fundoExistente);

            // Act
            var result = await _fundoUseCase.CreateFundoAsync(fundoDto);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.True(result.FundoJaExiste);
            Assert.Equal("Fundo já existente com o código fornecido.", result.ErrorMessage);
        }

        [Fact]
        public async Task CriarFundo_DeveCriarNovoFundo()
        {
            // Arrange
            var fundoDto = new FundoDTO { Codigo = "003", Nome = "Novo Fundo", Cnpj = "98765432109876", Codigo_Tipo = 1, Patrimonio = 5000 };
            var fundoCriado = new Fundo { Codigo = "003", Nome = "Novo Fundo", Cnpj = "98765432109876", Codigo_Tipo = 1, Patrimonio = 5000 };

            _fundoRepositoryMock.Setup(repo => repo.GetByCodigoAsync(fundoDto.Codigo)).ReturnsAsync((Fundo)null);
            _fundoRepositoryMock.Setup(repo => repo.CreateFundoAsync(It.IsAny<Fundo>())).Returns(Task.CompletedTask);

            // Mapeamento simulado
            _mapperMock.Setup(m => m.Map<Fundo>(fundoDto)).Returns(fundoCriado);
            _mapperMock.Setup(m => m.Map<FundoDTO>(fundoCriado)).Returns(fundoDto);

            // Act
            var result = await _fundoUseCase.CreateFundoAsync(fundoDto);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.NotNull(result.Fundo);
            Assert.Equal(fundoDto.Codigo, result.Fundo.Codigo);
        }

        [Fact]
        public async Task AtualizarFundo_DeveRetornarFalse_SeFundoNaoExistir()
        {
            // Arrange
            var fundoDto = new FundoDTO { Codigo = "004", Nome = "Fundo Inexistente", Cnpj = "98765432101234", Codigo_Tipo = 2, Patrimonio = 3000 };

            _fundoRepositoryMock.Setup(repo => repo.GetByCodigoAsync(fundoDto.Codigo)).ReturnsAsync((Fundo)null);

            // Act
            var result = await _fundoUseCase.UpdateFundoAsync(fundoDto.Codigo, fundoDto);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task AtualizarFundo_DeveAtualizarFundo_Existente()
        {
            // Arrange
            var fundoExistente = new Fundo { Codigo = "005", Nome = "Fundo Antigo", Cnpj = "12345678901234", Codigo_Tipo = 1, Patrimonio = 2000 };
            var fundoAtualizadoDto = new FundoDTO { Codigo = "005", Nome = "Fundo Atualizado", Cnpj = "98765432101234", Codigo_Tipo = 1, Patrimonio = 5000 };

            _fundoRepositoryMock.Setup(repo => repo.GetByCodigoAsync(fundoExistente.Codigo)).ReturnsAsync(fundoExistente);

            // Mapeamento manual de FundoDTO para Fundo
            var fundoAtualizado = new Fundo
            {
                Codigo = fundoAtualizadoDto.Codigo,
                Nome = fundoAtualizadoDto.Nome,
                Cnpj = fundoAtualizadoDto.Cnpj,
                Codigo_Tipo = fundoAtualizadoDto.Codigo_Tipo,
                Patrimonio = fundoAtualizadoDto.Patrimonio ?? 0
            };

            _fundoRepositoryMock.Setup(repo => repo.UpdateFundoAsync(fundoAtualizadoDto.Codigo, fundoAtualizado)).Returns(Task.CompletedTask);

            // Act
            var result = await _fundoUseCase.UpdateFundoAsync(fundoAtualizadoDto.Codigo, fundoAtualizadoDto);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeletarFundo_DeveRetornarFalse_SeFundoNaoExistir()
        {
            // Arrange
            var codigoInexistente = "999";

            _fundoRepositoryMock.Setup(repo => repo.GetByCodigoAsync(codigoInexistente)).ReturnsAsync((Fundo)null);

            // Act
            var result = await _fundoUseCase.DeleteFundoAsync(codigoInexistente);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task DeletarFundo_DeveDeletarFundo_Existente()
        {
            // Arrange
            var fundoExistente = new Fundo { Codigo = "006", Nome = "Fundo a Ser Deletado", Cnpj = "12345678901234", Codigo_Tipo = 1, Patrimonio = 1500 };

            _fundoRepositoryMock.Setup(repo => repo.GetByCodigoAsync(fundoExistente.Codigo)).ReturnsAsync(fundoExistente);

            _fundoRepositoryMock.Setup(repo => repo.DeleteFundoAsync(fundoExistente.Codigo)).Returns(Task.CompletedTask);

            // Act
            var result = await _fundoUseCase.DeleteFundoAsync(fundoExistente.Codigo);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task MovimentarPatrimonio_DeveRetornarFalse_SeFundoNaoExistir()
        {
            // Arrange
            var codigoInexistente = "999";
            var valorMovimentacao = 1000m;

            _fundoRepositoryMock.Setup(repo => repo.GetByCodigoAsync(codigoInexistente)).ReturnsAsync((Fundo)null);

            // Act
            var result = await _fundoUseCase.MovimentarPatrimonioAsync(codigoInexistente, valorMovimentacao);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task MovimentarPatrimonio_DeveAtualizarPatrimonio_Existente()
        {
            // Arrange
            var fundoExistente = new Fundo { Codigo = "007", Nome = "Fundo para Movimentacao", Cnpj = "12345678901234", Codigo_Tipo = 1, Patrimonio = 3000 };
            var valorMovimentacao = 500m;

            _fundoRepositoryMock.Setup(repo => repo.GetByCodigoAsync(fundoExistente.Codigo)).ReturnsAsync(fundoExistente);

            _fundoRepositoryMock.Setup(repo => repo.MovimentarPatrimonioAsync(fundoExistente.Codigo, valorMovimentacao)).Returns(Task.CompletedTask);

            // Act
            var result = await _fundoUseCase.MovimentarPatrimonioAsync(fundoExistente.Codigo, valorMovimentacao);

            // Assert
            Assert.True(result);
        }
    }
}
