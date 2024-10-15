using Moq;
using AutoMapper;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using CaseItau.API.src.Application.UseCases;
using CaseItau.API.src.Domain.Interfaces;
using CaseItau.API.src.Domain.Entities;
using CaseItau.API.src.Application.DTOs;
using CaseItau.API.src.WebAPI.Controllers;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace CaseItau.API.Tests.ControllersTests
{
    public class FundoControllerTests
    {
        private readonly Mock<IFundoRepository> _fundoRepositoryMock;
        private readonly FundoController _fundoController;
        private readonly Mock<IMapper> _mapperMock;

        private readonly Mock<ILogger<FundoUseCase>> _loggerMock;

        public FundoControllerTests()
        {
            _fundoRepositoryMock = new Mock<IFundoRepository>();
            _loggerMock = new Mock<ILogger<FundoUseCase>>();
            _mapperMock = new Mock<IMapper>();

            // Passando o _mapperMock.Object ao invés de null
            var fundoUseCase = new FundoUseCase(_fundoRepositoryMock.Object, _mapperMock.Object, _loggerMock.Object);
            _fundoController = new FundoController(fundoUseCase);
        }

        [Fact]
        public async Task ObterTodosFundos_DeveRetornarOk_ComFundos()
        {
            // Arrange
            var fundos = new List<Fundo>
            {
                new Fundo { Codigo = "001", Nome = "Fundo 1", Cnpj = "12345678901234", Codigo_Tipo = 1, Patrimonio = 1000 },
                new Fundo { Codigo = "002", Nome = "Fundo 2", Cnpj = "56789012345678", Codigo_Tipo = 2, Patrimonio = 2000 }
            };

            _fundoRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(fundos);

            // Mapeamento manual de Fundo para FundoDTO
            var fundosDto = new List<FundoDTO>
            {
                new FundoDTO { Codigo = "001", Nome = "Fundo 1", Cnpj = "12345678901234", Codigo_Tipo = 1, Patrimonio = 1000 },
                new FundoDTO { Codigo = "002", Nome = "Fundo 2", Cnpj = "56789012345678", Codigo_Tipo = 2, Patrimonio = 2000 }
            };

            _mapperMock.Setup(m => m.Map<IEnumerable<FundoDTO>>(fundos)).Returns(fundosDto);

            // Act
            var result = await _fundoController.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnFundos = Assert.IsType<List<FundoDTO>>(okResult.Value);
            Assert.Equal(fundosDto.Count, returnFundos.Count);
        }

        [Fact]
        public async Task ObterFundoPorCodigo_DeveRetornarOk_SeFundoExistir()
        {
            // Arrange
            var fundo = new Fundo { Codigo = "001", Nome = "Fundo 1", Cnpj = "12345678901234", Codigo_Tipo = 1, Patrimonio = 1000 };

            _fundoRepositoryMock.Setup(repo => repo.GetByCodigoAsync("001")).ReturnsAsync(fundo);

            // Mapeamento manual de Fundo para FundoDTO
            var fundoDto = new FundoDTO
            {
                Codigo = fundo.Codigo,
                Nome = fundo.Nome,
                Cnpj = fundo.Cnpj,
                Codigo_Tipo = fundo.Codigo_Tipo,
                Patrimonio = fundo.Patrimonio
            };

            _mapperMock.Setup(m => m.Map<FundoDTO>(fundo)).Returns(fundoDto);

            // Act
            var result = await _fundoController.Get("001");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnFundo = Assert.IsType<FundoDTO>(okResult.Value);
            Assert.Equal(fundoDto.Codigo, returnFundo.Codigo);
        }

        [Fact]
        public async Task ObterFundoPorCodigo_DeveRetornarNotFound_SeFundoNaoExistir()
        {
            // Arrange
            _fundoRepositoryMock.Setup(repo => repo.GetByCodigoAsync("001")).ReturnsAsync((Fundo)null);

            // Act
            var result = await _fundoController.Get("001");

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task CriarFundo_DeveRetornarCreated_SeFundoForCriado()
        {
            // Arrange
            var fundoDto = new FundoDTO { Codigo = "001", Nome = "Fundo Novo", Cnpj = "12345678901234", Codigo_Tipo = 1, Patrimonio = 1000 };

            _fundoRepositoryMock.Setup(repo => repo.GetByCodigoAsync(fundoDto.Codigo)).ReturnsAsync((Fundo)null);
            _fundoRepositoryMock.Setup(repo => repo.CreateFundoAsync(It.IsAny<Fundo>())).Returns(Task.CompletedTask);

            // Mapeamento manual de FundoDTO para Fundo
            var fundoCriado = new Fundo
            {
                Codigo = fundoDto.Codigo,
                Nome = fundoDto.Nome,
                Cnpj = fundoDto.Cnpj,
                Codigo_Tipo = fundoDto.Codigo_Tipo,
                Patrimonio = fundoDto.Patrimonio ?? 0
            };

            _mapperMock.Setup(m => m.Map<Fundo>(fundoDto)).Returns(fundoCriado);
            _mapperMock.Setup(m => m.Map<FundoDTO>(fundoCriado)).Returns(fundoDto);

            // Act
            var result = await _fundoController.Post(fundoDto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnFundo = Assert.IsType<FundoDTO>(createdResult.Value);
            Assert.Equal(fundoCriado.Codigo, returnFundo.Codigo);
        }

        [Fact]
        public async Task CriarFundo_DeveRetornarConflict_SeFundoJaExistir()
        {
            // Arrange
            var fundoDto = new FundoDTO { Codigo = "001", Nome = "Fundo Existente", Cnpj = "12345678901234", Codigo_Tipo = 1, Patrimonio = 1000 };
            var fundoExistente = new Fundo { Codigo = "001", Nome = "Fundo Existente", Cnpj = "12345678901234", Codigo_Tipo = 1, Patrimonio = 1000 };

            _fundoRepositoryMock.Setup(repo => repo.GetByCodigoAsync(fundoDto.Codigo)).ReturnsAsync(fundoExistente);
            _mapperMock.Setup(m => m.Map<Fundo>(fundoDto)).Returns(fundoExistente); // Mapeamento simulado

            // Act
            var result = await _fundoController.Post(fundoDto);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);

            Assert.NotNull(conflictResult.Value);
            var conflictMessage = conflictResult.Value.GetType().GetProperty("message")?.GetValue(conflictResult.Value, null);
            Assert.NotNull(conflictMessage);
            Assert.Equal("Fundo já existente com o código fornecido.", conflictMessage.ToString());
        }

        [Fact]
        public async Task AtualizarFundo_DeveRetornarNoContent_SeAtualizacaoForSucesso()
        {
            // Arrange
            var fundoDto = new FundoDTO { Codigo = "001", Nome = "Fundo Atualizado", Cnpj = "12345678901234", Codigo_Tipo = 1, Patrimonio = 1000 };
            var fundo = new Fundo { Codigo = "001", Nome = "Fundo Atualizado", Cnpj = "12345678901234", Codigo_Tipo = 1, Patrimonio = 1000 };

            _fundoRepositoryMock.Setup(repo => repo.GetByCodigoAsync(fundoDto.Codigo)).ReturnsAsync(fundo);
            _fundoRepositoryMock.Setup(repo => repo.UpdateFundoAsync(fundoDto.Codigo, It.IsAny<Fundo>())).Returns(Task.CompletedTask);

            _mapperMock.Setup(m => m.Map<Fundo>(fundoDto)).Returns(fundo);

            // Act
            var result = await _fundoController.Put(fundoDto.Codigo, fundoDto);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task AtualizarFundo_DeveRetornarNotFound_SeFundoNaoExistir()
        {
            // Arrange
            var fundoDto = new FundoDTO { Codigo = "001", Nome = "Fundo Atualizado", Cnpj = "12345678901234", Codigo_Tipo = 1, Patrimonio = 1000 };

            _fundoRepositoryMock.Setup(repo => repo.GetByCodigoAsync(fundoDto.Codigo)).ReturnsAsync((Fundo)null);

            // Act
            var result = await _fundoController.Put(fundoDto.Codigo, fundoDto);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeletarFundo_DeveRetornarNoContent_SeDeletarComSucesso()
        {
            // Arrange
            var fundo = new Fundo { Codigo = "001", Nome = "Fundo Para Deletar", Cnpj = "12345678901234", Codigo_Tipo = 1, Patrimonio = 1000 };

            _fundoRepositoryMock.Setup(repo => repo.GetByCodigoAsync("001")).ReturnsAsync(fundo);
            _fundoRepositoryMock.Setup(repo => repo.DeleteFundoAsync("001")).Returns(Task.CompletedTask);

            // Act
            var result = await _fundoController.Delete("001");

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeletarFundo_DeveRetornarNotFound_SeFundoNaoExistir()
        {
            // Arrange
            _fundoRepositoryMock.Setup(repo => repo.GetByCodigoAsync("001")).ReturnsAsync((Fundo)null);

            // Act
            var result = await _fundoController.Delete("001");

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task MovimentarPatrimonio_DeveRetornarNoContent_SeSucesso()
        {
            // Arrange
            var fundo = new Fundo { Codigo = "001", Nome = "Fundo", Cnpj = "12345678901234", Codigo_Tipo = 1, Patrimonio = 1000 };

            _fundoRepositoryMock.Setup(repo => repo.GetByCodigoAsync("001")).ReturnsAsync(fundo);
            _fundoRepositoryMock.Setup(repo => repo.MovimentarPatrimonioAsync("001", 500)).Returns(Task.CompletedTask);

            // Act
            var result = await _fundoController.MovimentarPatrimonio("001", 500);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task MovimentarPatrimonio_DeveRetornarNotFound_SeFundoNaoExistir()
        {
            // Arrange
            _fundoRepositoryMock.Setup(repo => repo.GetByCodigoAsync("001")).ReturnsAsync((Fundo)null);

            // Act
            var result = await _fundoController.MovimentarPatrimonio("001", 500);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
