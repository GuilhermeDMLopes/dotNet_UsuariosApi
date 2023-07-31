using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UsuariosApi.Data.Dtos;
using UsuariosApi.Models;
using UsuariosApi.Services;

namespace UsuariosApi.Controllers;

[ApiController]
[Route("[Controller]")]
public class UsuarioController : ControllerBase{
   
    private CadastroService _cadastroService;

    public UsuarioController(CadastroService cadastroService)
    {
       _cadastroService = cadastroService;
    }

    [HttpPost]
    //Precisamo tornar o metodo asincrono para receber o resultado da operação. Para isso devemos tranformar a função em uma Task (uma operação que pode ou nao retornar um valor)
    public async Task<IActionResult> CadastraUsuario(CreateUsuarioDto dto)
    {
        //Como temos muitas funcionalidades dentro do controlador, iremos tranferir a logica para uma Service
        await _cadastroService.Cadastra(dto);
        return Ok("Usuario Cadastrado!");
    }
}
