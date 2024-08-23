using MVC.Web.Data;
using MVC.Web.Models.Entitidades;

namespace MVC.Web.Repositories
{
    public class AlunoRepository : BaseRepository<Aluno>, IAlunoRepository    // BaseRepository usa Generics
    {
        public AlunoRepository(Contexto contexto) : base(contexto)
        {
        }
    }
};
