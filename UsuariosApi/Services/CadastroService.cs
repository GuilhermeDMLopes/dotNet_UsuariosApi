//Classe de serviços para cadastrar usuarios
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using UsuariosApi.Data.Dtos;
using UsuariosApi.Models;

namespace UsuariosApi.Services;

public class CadastroService
{
    private IMapper _mapper;
    private UserManager<Usuario> _userManager;
    public CadastroService(IMapper mapper, UserManager<Usuario> userManager)
    {
        _mapper = mapper;
        _userManager = userManager;
    }

    public async Task Cadastra(CreateUsuarioDto dto)
    {
        Usuario usuario = _mapper.Map<Usuario>(dto);
        //Identity nos fornece um método de cadastro de usuarios no banco. Passamos o usuário e a senha do mesmo
        //Retornaremos o resultado da operação para ver se ela foi bem sucedida ou não.
        //Como é uma operação assincrona, devemos esperar o resultado
        //A senha deve possuir letra maiuscula, minuscula, numeros e caracteres especiais. SE NÃO HOUVER ESSES REUQUISITOS ELE GERARA UM ERRO NO TASK
        IdentityResult resultado = await _userManager.CreateAsync(usuario, dto.Password);

        if (!resultado.Succeeded)
        {
            throw new ApplicationException("Falha ao cadastrar usuario");
        }

        
    }
}
