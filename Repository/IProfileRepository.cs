using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using MyHub.Entities;

namespace MyHub.Repository
{
    public interface IProfileRepository<TEntity, TKey> : IRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
       //METODOS ESPECIFICOS DO REPOSITORIO DE PROFILE
    }
}
