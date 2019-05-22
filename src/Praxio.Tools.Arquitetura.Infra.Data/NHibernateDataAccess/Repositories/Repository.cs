using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using NHibernate;
using NHibernate.Linq;
using Praxio.Tools.Arquitetura.Application.Interfaces.Repositories;
using Praxio.Tools.Arquitetura.Domain.Entities.Base;
using Praxio.Tools.Arquitetura.Infra.Data.NHibernateDataAccess.DataModels;

namespace Praxio.Tools.Arquitetura.Infra.Data.NHibernateDataAccess.Repositories
{
    public class Repository<TDomainModel, TDataModel> : IRepository<TDomainModel>
        where TDomainModel : DomainModel
        where TDataModel : DataModel
    {
        protected readonly ISessionFactory _sessionFactory;
        protected readonly IMapper _mapper;

        public Repository(ISessionFactory sessionFactory, IMapper mapper)
        {
            _sessionFactory = sessionFactory;
            _mapper = mapper;
        }

        public async Task<TDomainModel> Inserir(TDomainModel domainModel)
        {
            var dataModel = _mapper.Map<TDataModel>(domainModel);

            using (var session = _sessionFactory.OpenSession())
            {
                await session.SaveAsync(dataModel);
                await session.FlushAsync();
                domainModel.Id = dataModel.Id;
                return domainModel;
            }
        }

        public async Task<TDomainModel> Alterar(TDomainModel domainModel)
        {
            var dataModel = _mapper.Map<TDataModel>(domainModel);

            using (var session = _sessionFactory.OpenSession())
            {
                await session.UpdateAsync(dataModel);
                await session.FlushAsync();
                return domainModel;
            }
        }

        public async Task Excluir(int id)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                await session.DeleteAsync(await session.LoadAsync<TDataModel>(id));
                await session.FlushAsync();
            }
        }

        public async Task<TDomainModel> ObterPorId(int id)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                var dataModel = await session.GetAsync<TDataModel>(id);
                return _mapper.Map<TDomainModel>(dataModel);
            }
        }

        public async Task<IEnumerable<TDomainModel>> ObterLista()
        {
            using (var session = _sessionFactory.OpenSession())
            {
                var lista = await session.Query<TDataModel>().ToListAsync();
                return _mapper.Map<IEnumerable<TDomainModel>>(lista);
            }
        }

        public async Task<bool> Existe(int id)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                return await session.Query<TDataModel>().AnyAsync(a => a.Id == id);
            }
        }
    }
}