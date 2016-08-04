namespace BusinessLayer.Mappers
{
    public interface IMapToNew<in TSource, out TTarget>
    {
        TTarget Map(TSource source);
        TTarget Map(TSource source, params object[] options);
    }
}
