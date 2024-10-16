using Microsoft.EntityFrameworkCore;
using MVC.Web.Data;
using MVC.Web.Models.Entities;

namespace MVC.Web.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class, IEntity
    {
        protected readonly Contexto _contexto;

        public BaseRepository(Contexto contexto)
        {
            _contexto = contexto;
        }

        public async Task IncluirAsync(T entity)
        {
            await _contexto.Set<T>().AddAsync(entity);
            await _contexto.SaveChangesAsync();
        }

        public async Task AlterarAsync(T entity)
        {
            _contexto.Set<T>().Update(entity);
            await _contexto.SaveChangesAsync();
        }

        public async Task<T> SelecionarAsync(int id)
        {
            return await _contexto.Set<T>().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<T>> SelecionarTudoAsync()
        {
            return await _contexto.Set<T>().ToListAsync();
        }

        public async Task ExcluirAsync(int id)
        {
            var entity = await SelecionarAsync(id);
            _contexto.Set<T>().Remove(entity);
            await _contexto.SaveChangesAsync();
        }

        public void Dispose()
        {
            _contexto.Dispose();
        }
    }

}