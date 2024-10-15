using System.Collections.Generic;
using System.Threading.Tasks;
using CaseItau.API.src.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using CaseItau.API.src.Domain.Interfaces;
using CaseItau.API.src.Infrastructure.Persistence.Data;

namespace CaseItau.API.src.Infrastructure.Repositories
{
    public class FundoRepository : IFundoRepository
    {
        private readonly AppDbContext _context;

        public FundoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Fundo>> GetAllAsync()
        {
            return await _context.Fundos.ToListAsync();
        }

        public async Task<Fundo> GetByCodigoAsync(string codigo)
        {
            return await _context.Fundos.FirstOrDefaultAsync(f => f.Codigo == codigo);
        }

        public async Task CreateFundoAsync(Fundo fundo)
        {
            await _context.Fundos.AddAsync(fundo);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateFundoAsync(string codigo, Fundo fundoAtualizado)
        {
            var fundoExistente = await _context.Fundos.FirstOrDefaultAsync(f => f.Codigo == codigo);

            if (fundoExistente != null)
            {
                fundoExistente.Nome = fundoAtualizado.Nome;
                fundoExistente.Cnpj = fundoAtualizado.Cnpj;
                fundoExistente.Codigo_Tipo = fundoAtualizado.Codigo_Tipo;
                fundoExistente.Patrimonio = fundoAtualizado.Patrimonio;

                _context.Fundos.Update(fundoExistente);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteFundoAsync(string codigo)
        {
            var fundo = await _context.Fundos.FirstOrDefaultAsync(f => f.Codigo == codigo);

            if (fundo != null)
            {
                _context.Fundos.Remove(fundo);
                await _context.SaveChangesAsync();
            }
        }

        public async Task MovimentarPatrimonioAsync(string codigo, decimal valorMovimentado)
        {
            var fundo = await _context.Fundos.FirstOrDefaultAsync(f => f.Codigo == codigo);

            if (fundo != null)
            {
                fundo.Patrimonio += valorMovimentado;
                await _context.SaveChangesAsync();
            }
        }
    }
}
