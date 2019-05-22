using Praxio.Tools.Arquitetura.Domain.Entities.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Praxio.Tools.Arquitetura.Application.Interfaces.Repositories
{
    public interface IRepository<TDomainModel> where TDomainModel : IDomainModel
    {
        Task<TDomainModel> Inserir(TDomainModel obj);
        Task<TDomainModel> Alterar(TDomainModel obj);
        Task Excluir(int id);
        Task<TDomainModel> ObterPorId(int id);
        Task<IEnumerable<TDomainModel>> ObterLista();
        Task<bool> Existe(int id);
    }
}