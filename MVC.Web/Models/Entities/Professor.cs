namespace MVC.Web.Models.Entities
{
    public class Professor : IEntity
    {
        public int Id { get; set; }
        public string Nome{ get; set; }
        public string Email { get; set; }
        public List<Curso> Cursos { get; set; }
    }
}
