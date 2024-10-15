using System.Collections.Generic;
using System.Threading.Tasks;
using CaseItau.API.src.Domain.Entities;

namespace CaseItau.API.src.Domain.Interfaces
{
    public interface IFundoRepository
    {
        Task<IEnumerable<Fundo>> GetAllAsync();

        Task<Fundo> GetByCodigoAsync(string codigo);

        Task CreateFundoAsync(Fundo fundo);

        Task UpdateFundoAsync(string codigo, Fundo fundo);

        Task DeleteFundoAsync(string codigo);

        Task MovimentarPatrimonioAsync(string codigo, decimal valor);
    }
}
