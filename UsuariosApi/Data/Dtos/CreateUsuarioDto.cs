using System.ComponentModel.DataAnnotations;

namespace UsuariosApi.Data.Dtos;

public class CreateUsuarioDto
{
    [Required]
    public string Username { get; set; }
    [Required]
    public DateTime DataNascimento { get; set; }
    //Para senha, podemos passar um tipo de dado específico
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    //Para o cadastro de usuario, temos o campo de confirmar senha para verificar se ele digitou corretamente
    //Esse dado não vai para o banco é apenas uma validação preventiva
    [Required]
    [Compare("Password")]
    public string RePassword { get; set; }

}
