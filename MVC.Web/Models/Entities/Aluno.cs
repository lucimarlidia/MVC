namespace MVC.Web.Models.Entities
{
    public class Aluno : IEntity
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Endereco { get; set; }
        public string Email { get; set; }
        public bool Ativo { get; set; }
        public List<Matricula> Matriculas { get; set; }
    }
}
