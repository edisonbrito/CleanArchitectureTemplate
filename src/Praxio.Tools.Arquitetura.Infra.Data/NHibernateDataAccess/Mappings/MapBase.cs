using FluentNHibernate.Mapping;
using Praxio.Tools.Arquitetura.Infra.Data.NHibernateDataAccess.DataModels;

namespace Praxio.Tools.Arquitetura.Infra.Data.NHibernateDataAccess.Mappings
{
    /// <summary>
    /// Classe base para o mapeamento do banco de dados pelo fluent hibernate. Geralmente representa o mapeamento de uma tabela no banco de dados.
    /// </summary>
    /// <typeparam name="T">Tipo de dados</typeparam>
    public class MapBase<T> : ClassMap<T> where T : DataModel
    {
        /// <summary>
        ///     Construtor
        /// </summary>
        /// <param name="lazyLoad">
        ///     Especifica o comportamento de carregamento padrão de sub propriedades (relacionamentos).
        ///     O valor padrão é o não carregamento de sub propriedades (relacionamentos)
        /// </param>
        protected MapBase(bool lazyLoad = true)
        {
            DynamicUpdate();
            if (lazyLoad)
                LazyLoad();
        }

        /// <summary>
        ///     Mapeia a coluna da chave primária de valores automaticamente gerados.
        ///     No caso do MS SQL utiliza o sistema identity para geração de valores de chave primária automaticamente gerados.
        ///     No caso do Oracle utiliza uma convenção de nome do gerador de valores (sequência com o nome "SEQ_" + pTableName)
        /// </summary>
        /// <param name="pTableName">Nome da tabela</param>
        /// <param name="pColumnName">Nome da coluna da chave primária não composta</param>
        public virtual void CreateIdColumn(string pTableName, string pColumnName)
        {
            Table(pTableName);
            Id(x => x.Id).Column(pColumnName).GeneratedBy
                .Native(builder => builder.AddParam("sequence", "SEQ_" + pTableName));
        }
    }
}