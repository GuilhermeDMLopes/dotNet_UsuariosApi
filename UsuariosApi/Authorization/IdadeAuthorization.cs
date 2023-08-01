//Essa classe deve ser reconhecida pelo dotNet como um gerenciador, como algo que consiga lidar com a questão de autorização e interceptação

using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace UsuariosApi.Authorization;

public class IdadeAuthorization : AuthorizationHandler<IdadeMinima>

{
    //Atraves desse context, conseguimos acessar as informações do usuario que esta tentando acessar algum recurso que esta sendo protegido
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IdadeMinima requirement)
    {
        //Pegando a claim do token que possui a data de nascimento
        var dataNascimentoClaim = context.User.FindFirst(claim => claim.Type == ClaimTypes.DateOfBirth);

        if(dataNascimentoClaim is null)
        {
            return Task.CompletedTask;
        }

        //Se não for nulo, fazer conversão para datetime
        var dataNascimento = Convert.ToDateTime(dataNascimentoClaim.Value);

        //Calculando idade do usuario
        var idadeUsuario = DateTime.Today.Year - dataNascimento.Year;

        //Se a data de nascimento do nosso usuario for maior que a diferença de ano da nossa idade, preciso subtrair a idade da pessoa em 1 (Ex, pessoa nasceu em 2002 mas so faz aniversario depois da data de busca)
        if(dataNascimento > DateTime.Today.AddYears(-idadeUsuario))
        {
            idadeUsuario--;
        }

        //Se a idade for maior do que a que passamos por parametro em Program.cs (No caso, 18) o acesso é permitido
        if(idadeUsuario >= requirement.Idade)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
        
    }
}
