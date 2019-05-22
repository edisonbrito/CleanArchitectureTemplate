using Autofac;
using Praxio.Tools.Arquitetura.Infra.Data.NHibernateDataAccess.Repositories;
using Praxio.Tools.Arquitetura.Infra.Data.NHibernateDataAccess;

namespace Praxio.Tools.Arquitetura.Infra.CrossCutting.IoC
{
    public class InfraModule : Module
    {
        private readonly string _connectionString;

        public InfraModule(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder
               .RegisterAssemblyTypes(typeof(Repository<,>).Assembly)
               .AsImplementedInterfaces().InstancePerLifetimeScope();

            var sessionFactory = NHibernateHelper.ConfigureSessionFactory(a =>
            {
                a.ConnectionString = _connectionString;
                a.DatabaseType = DatabaseType.oracle10;
                a.Configuration(c =>
                {
                    c.SetProperty("connection.driver_class", "NHibernate.Driver.OracleManagedDataClientDriver");
                });
#if DEBUG
                a.ShowSql = true;               
#endif
            });

            builder.Register(f => sessionFactory).SingleInstance();
        }
    }
}