//Classe de serviços para cadastrar usuarios
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UsuariosApi.Models;

namespace UsuariosApi.Services
{
    public class TokenService
    {
        public string GenerateToken(Usuario usuario)
        {
            //Informações que serão usadas para gerar o token do usuario
            Claim[] claims = new Claim[]
            {
                new Claim("Username", usuario.UserName),
                new Claim("id", usuario.Id),
                new Claim(ClaimTypes.DateOfBirth, usuario.DataNascimento.ToString())
            };

            //Criando palavra chave para geração do nosso token
            var chave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SA99123KAJSD1Las1"));

            //Criando credenciais. Chave secreta e algoritmo de encriptografia
            var signingCredentials = new SigningCredentials(chave, SecurityAlgorithms.HmacSha256);

            //Precisaremos instalar o pacote System.IdentityModel.Tokens.Jwt
            //gerando token em si
            var token = new JwtSecurityToken
                (
                //Tempo de expiração do token (a partir da geração, 10 minutos)
                expires: DateTime.Now.AddMinutes(10),
                //As claims usadas (As que criamos acima)
                claims: claims,
                //Credenciais
                signingCredentials: signingCredentials
                );

            //Convertendo Token em uma string para mostrar ao usuario
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}