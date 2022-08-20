using DevIO.App.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DevIO.App.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Route("error/{idlength(3,3)}")]
        public IActionResult Errors(int id)
        {
            var modelError = new ErrorViewModel();

            if (id == 500)
            {
                modelError.Message = "Ocorreu um erro! Tente novamente mais tarde ou contate nosso suporte.";
                modelError.Titulo = "Ocorreu um erro!";
                modelError.ErrorCode = id;
            }
            else if (id == 404)
            {
                modelError.Message = "A página que está procurando não existe <br/> Em caso de dúvidas ente em contato com nosso suporte.";
                modelError.Titulo = "Ops! Págin não encontrada.";
                modelError.ErrorCode = id;
            }
            else if (id == 403)
            {
                modelError.Message = "Você não tem premissão para fazer isto.";
                modelError.Titulo = "Acesso Negado";
                modelError.ErrorCode = id;
            }
            else 
            {
                return StatusCode(404);
            }

            return View("Error", modelError);
        }
    }
}