namespace DataLayer.Contexts
{
    public interface IContext
    {
        void SetModified(object entity);
        void SetAdd(object entity);
    }
}
