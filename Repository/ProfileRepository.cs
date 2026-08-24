using MyHub.Data;
using MyHub.Entities;

namespace MyHub.Repository
{
    public class ProfileRepository<TEntity, TKey> : Repository<TEntity, TKey>, IProfileRepository<TEntity, TKey>
        where TEntity : Profile<TKey>, IEntity<TKey>
        where TKey: IEquatable<TKey>
    {
        public ProfileRepository(ApplicationDbContext context) : base(context) { }
    }
}
