namespace WebVella.ERP
{
    public interface IEntityRepository : IRepository
    {
        IEntity Get();
    }
}