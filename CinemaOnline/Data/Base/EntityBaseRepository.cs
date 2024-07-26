namespace CinemaOnline.Data.Base
{
    public class EntityBaseRepository<T> : IEntityBaseRepository<T> where T : class, IEntityBase,new()
    {
    }
}
