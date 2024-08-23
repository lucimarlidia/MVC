using Microsoft.AspNetCore.Mvc;
using MVC.Web.Models.ViewModels;
using MVC.Web.Services;

namespace MVC.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatriculaController : ControllerBase
    {
        private readonly IMatriculaService _service;

        public MatriculaController(IMatriculaService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] MatriculaViewModel matriculaViewModel)
        {
            if (matriculaViewModel == null)
            {
                return BadRequest("Dados inválidos.");
            }

            try
            {
                var matriculaId = await _service.Salvar(matriculaViewModel);
                return Ok(new { Message = "Matrícula salva com sucesso!", MatriculaId = matriculaId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao salvar a matrícula: {ex.Message}");
            }
        }
    }
}
