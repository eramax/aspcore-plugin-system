using System.Linq;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Models;

namespace SharedKernel.Data
{
    public interface IEmoContext
    {
        int SaveChanges();
        DbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity;
        string GenerateCreateScript();

        int ExecuteSqlCommand(RawSqlString sql, bool doNotEnsureTransaction = false, int? timeout = null,
            params object[] parameters);

        void Detach<TEntity>(TEntity entity) where TEntity : BaseEntity;
        IQueryable<TQuery> QueryFromSql<TQuery>(string sql, params object[] parameters) where TQuery : class;
    }
}
