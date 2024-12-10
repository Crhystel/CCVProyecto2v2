using CCVProyecto2v2.DataAccess;
using CCVProyecto2v2.Models;
using CCVProyecto2v2.ViewsGeneral;
using Microsoft.EntityFrameworkCore;

namespace CCVProyecto2v2.ViewLogin;

public partial class LoginView : ContentPage
{
    public LoginView()
    {
        InitializeComponent();
    }
    private async void Ingresar_Clicked(object sender, EventArgs e)
    {
        string usuario = UsuarioEntry.Text;
        string contrasenia = ContraseniaEntry.Text;


        if (usuario == "admin" && contrasenia == "admin")
        {
            await Navigation.PushAsync(new AdministradorView());
        }
        else
        {

            var usuarioAutenticado = await AutenticarUsuarioAsync(usuario, contrasenia);

            if (usuarioAutenticado != null)
            {

                switch (usuarioAutenticado.Rol)
                {
                    case RolEnum.Administrador:
                        await Navigation.PushAsync(new AdministradorView());
                        break;
                    case RolEnum.Profesor:
                        await Navigation.PushAsync(new ProfesorView());
                        break;
                    case RolEnum.Estudiante:
                        await Navigation.PushAsync(new EstudianteView());
                        break;
                    default:
                        await DisplayAlert("Error", "Rol desconocido", "OK");
                        break;
                }
            }
            else
            {
                await DisplayAlert("Error", "Usuario o contraseña incorrectos", "OK");
            }
        }
    }

    private async Task<Usuario> AutenticarUsuarioAsync(string nombreUsuario, string contrasenia)
    {
        using (var context = new DbbContext())
        {
            return await context.Usuarios
                .FirstOrDefaultAsync(u => u.NombreUsuario == nombreUsuario && u.Contrasenia == contrasenia);
        }
    }


}