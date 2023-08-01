//Classe para controlar os acessos dos usuarios a partir do token
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UsuariosApi.Controllers;

[ApiController]
[Route("[Controller]")]
public class AcessoController : ControllerBase
{
    [HttpGet]
    //Dada certa condição de autorização, o usuario pode ou não ter acesso
    //No caso iremos usar idade minima
    [Authorize(Policy = "IdadeMinima")]
    public IActionResult Get()
    {
        return Ok("Acesso Permitido");
    }
}
