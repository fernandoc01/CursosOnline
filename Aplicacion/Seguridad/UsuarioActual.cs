using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.Contratos;
using Dominio;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Aplicacion.Seguridad
{
    public class UsuarioActual
    {
        public class Ejecuta : IRequest<UsuarioData>
        {
        }

        public class Manejador : IRequestHandler<Ejecuta, UsuarioData>
        {
            UserManager<Usuario> _userManager;
            IJwtGenerador _ijwtGenerador;
            IUsuarioSesion _iusuarioSesion;

            public Manejador(UserManager<Usuario> userManager, IJwtGenerador ijwtGenerador, IUsuarioSesion iusuarioSesion){
                _userManager = userManager;
                _ijwtGenerador = ijwtGenerador;
                _iusuarioSesion = iusuarioSesion;
            }

            public async Task<UsuarioData> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var usuario = await _userManager.FindByNameAsync(_iusuarioSesion.ObtenerUsuarioSesion());

                var resultadoRoles = await _userManager.GetRolesAsync(usuario);
                var listaRoles = new List<string>(resultadoRoles);

                return new UsuarioData{
                    NombreCompleto = usuario.NombreCompleto,
                    UserName = usuario.UserName,
                    Token = _ijwtGenerador.CrearToken(usuario, listaRoles),
                    Imagen = null,
                    Email = usuario.Email
                };
            }
        }
    }
}