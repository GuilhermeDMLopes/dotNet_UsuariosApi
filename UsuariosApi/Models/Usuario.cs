using Microsoft.AspNetCore.Identity;

namespace UsuariosApi.Models;

//Para mostrar que o modelo Usuario pode ser utilizado pelo Identity extendemos de IdentityUser
//O IdentityUser é uma classe que ja realiza diversas operações de usuário e que podemos reaproveitar (Validação de email, hash de senha, etc)
//Porque nao utilizar o identityUser então? Ao criarmos nossa propria classe de usuario, conseguimos personalizar nosso usuario colocando data de nascimento por ex (não tem no identityUser)
public class Usuario : IdentityUser
{
    public DateTime DataNascimento { get; set; }
    //Para utilizar o que ja tem pronto do identityUser, usamos o construtor
    public Usuario() : base()
    {
        
    }
}
