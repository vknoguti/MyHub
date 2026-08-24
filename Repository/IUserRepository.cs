
using MyHub.Entities;

namespace MyHub.Repository
{
    public interface IUserRepository<TEntity, TKey> : IRepository<TEntity, TKey> 
        where TEntity : User<TKey>, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        //METODOS ESPECIFICOS DO USER REPOSITORY
    }
}
