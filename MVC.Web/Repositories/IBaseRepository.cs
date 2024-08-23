namespace MVC.Web.Repositories
{
    public interface IBaseRepository<T>
    {
        Task IncluirAsync(T entity);

        Task AlterarAsync(T entity);

        Task<T> SelecionarAsync(int id);

        Task<List<T>> SelecionarTudoAsync();

        Task ExcluirAsync(int id);
    }
}
