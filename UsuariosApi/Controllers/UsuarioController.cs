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
   
    private UsuarioService _usuarioService;

    public UsuarioController(UsuarioService cadastroService)
    {
       _usuarioService = cadastroService;
    }

    [HttpPost("cadastro")]
    //Precisamo tornar o metodo asincrono para receber o resultado da operação. Para isso devemos tranformar a função em uma Task (uma operação que pode ou nao retornar um valor)
    public async Task<IActionResult> CadastraUsuario(CreateUsuarioDto dto)
    {
        //Como temos muitas funcionalidades dentro do controlador, iremos tranferir a logica para uma Service
        await _usuarioService.Cadastra(dto);
        return Ok("Usuario Cadastrado!");
    }

    //Rota de login
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUsuarioDto dto)
    {
        var token = await _usuarioService.Login(dto);
        return Ok(token);
    }
}
