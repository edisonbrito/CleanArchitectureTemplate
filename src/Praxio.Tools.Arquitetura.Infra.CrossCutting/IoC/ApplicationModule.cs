using Autofac;
using Praxio.Tools.Arquitetura.Application.Interfaces.Repositories;
using Praxio.Tools.Arquitetura.Infra.Data.NHibernateDataAccess.Repositories;

namespace Praxio.Tools.Arquitetura.Infra.CrossCutting.IoC
{
    public class ApplicationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
               .RegisterAssemblyTypes(typeof(IRepository<>).Assembly)
               .AsImplementedInterfaces().InstancePerLifetimeScope();            
        }
    }
}