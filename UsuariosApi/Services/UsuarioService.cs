//Classe de serviços para cadastrar usuarios
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using UsuariosApi.Data.Dtos;
using UsuariosApi.Models;

namespace UsuariosApi.Services;

public class UsuarioService
{
    private IMapper _mapper;
    private UserManager<Usuario> _userManager;
    private SignInManager<Usuario> _signInManager;
    private TokenService _tokenService;

    public UsuarioService(IMapper mapper, UserManager<Usuario> userManager, SignInManager<Usuario> signInManager = null, TokenService tokenService = null)
    {
        _mapper = mapper;
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
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

    public async Task<string> Login(LoginUsuarioDto dto)
    {
        //Assim como temos o UserManager na função anterior. Também temos o SignInManager para login
        //A partir do usuario e senha passado por parametro, ele fara a autenticação do usuario
        //PasswordSignInAsync possui 4 parametro, usario, senha, isPersistent(se vamos querer guardar o cookie depois de fechar o navegador), lockoutOnFailure (se a conta deve ser bloqueada se o login falhar)
        //Como a função é uma task e devemos aguardar o resultado, devemos colocar await
        var resultado = await _signInManager.PasswordSignInAsync(dto.Username, dto.Password, false, false);

        if(!resultado.Succeeded)
        {
            throw new ApplicationException("Usuario nao autenticado");
        }

        //Atraves do SignInManager, precisamos recuperar o usuario para termos acesso a seu id e data de nascimento para gerar o token
        var usuario = _signInManager.UserManager.Users.FirstOrDefault(user => user.NormalizedUserName == dto.Username.ToUpper());

        //Validando token do usuario
        var token = _tokenService.GenerateToken(usuario);

        return token;
    }
}
