using FluentMigrator;
using Praxio.Tools.Arquitetura.Infra.Migrations.Base;

namespace Praxio.Tools.Arquitetura.Infra.Migrations
{
    [MigrationBase(1, "Tools")]
    public class Baseline : Migration
    {
        public override void Up()
        {
            
        }

        public override void Down()
        {			
			
        }
    }

    /// <summary>
    /// Setup - A.K.A. Migração zero. É um pequeno truque para que tenhamos o 
    /// controle do rollback até o ponto zero da base de dados.
    /// </summary>
    /// <remarks>
    /// Esta classe não deve conter nada nos metodos e up e down
    /// </remarks>
    [Migration(0)]
    public class Setup : Migration
    {
        public override void Up()
        {

        }

        public override void Down()
        {

        }
    }
}
