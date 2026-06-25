namespace mm.core.flow
{
    public interface IModelProvider<TModel> : IInfrastructure
    {
        TModel GetModel();
    }
}
