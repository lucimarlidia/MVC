namespace MVC.Web.Models.Entities
{
    public class Curso : IEntity
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataInicio { get; set; }
        public int IdProfessor { get; set; }
        public Professor Professor { get; set; }
        public List<Matricula> Matriculas { get; set; }
    }
}
