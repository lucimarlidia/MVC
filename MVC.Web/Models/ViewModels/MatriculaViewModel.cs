namespace MVC.Web.Models.ViewModels
{
    public class MatriculaViewModel
    {
        public int? Id { get; set; }
        public int IdAluno { get; set; }
        public int IdCurso { get; set; }
        public StatusMatricula StatusMatricula { get; set; }
    }
}
