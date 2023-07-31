using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UsuariosApi.Models;

namespace UsuariosApi.Data;

//Como estaremos utilizando o Identity para autenciar/validar o usuario no banco. Ao inves de usarmos apenas DbContext, usaremos IdentityDbContext
public class UsuarioDbContext : IdentityDbContext<Usuario>
{
	public UsuarioDbContext(DbContextOptions<UsuarioDbContext> options) : base(options)
	{

	}
}
