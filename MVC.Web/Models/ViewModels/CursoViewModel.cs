namespace MVC.Web.Models.ViewModels
{
    public class CursoViewModel
    {
        public int? Id { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataInicio { get; set; }
        public int IdProfessor { get; set; }
    }
}
