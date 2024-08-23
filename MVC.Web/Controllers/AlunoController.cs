using Microsoft.AspNetCore.Mvc;
using MVC.Web.Models.ViewModels;
using MVC.Web.Services;

namespace MVC.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlunoController : ControllerBase
    {
        private readonly IAlunoService _service;

        public AlunoController(IAlunoService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AlunoViewModel alunoViewModel)
        {
            if (alunoViewModel == null)
            {
                return BadRequest("Dados inválidos.");
            }

            try
            {
                var alunoId = await _service.Salvar(alunoViewModel);
                return Ok(new { Message = "Aluno salvo com sucesso!", AlunoId = alunoId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao salvar o aluno: {ex.Message}");
            }
        }
    }
}
