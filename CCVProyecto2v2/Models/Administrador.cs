using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCVProyecto2v2.DataAccess;

namespace CCVProyecto2v2.Models
{
    public class Administrador : Usuario
    {
        public async Task CrearUsuario(string nombreUsuario, string contrasenia, RolEnum rol)
        {
            using (var context = new DbbContext())
            {
                Usuario nuevoUsuario;

                switch (rol)
                {
                    case RolEnum.Estudiante:
                        nuevoUsuario = new Estudiante
                        {
                            NombreUsuario = nombreUsuario,
                            Contrasenia = contrasenia,
                            Rol = RolEnum.Estudiante
                        };
                        break;

                    case RolEnum.Profesor:
                        nuevoUsuario = new Profesor
                        {
                            NombreUsuario = nombreUsuario,
                            Contrasenia = contrasenia,
                            Rol = RolEnum.Profesor
                        };
                        break;

                    case RolEnum.Administrador:
                    default:
                        nuevoUsuario = new Usuario
                        {
                            NombreUsuario = nombreUsuario,
                            Contrasenia = contrasenia,
                            Rol = RolEnum.Administrador
                        };
                        break;
                }

                context.Usuarios.Add(nuevoUsuario);
                await context.SaveChangesAsync();
            }
        }

    }

}
