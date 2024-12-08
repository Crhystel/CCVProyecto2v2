using CCVProyecto2v2.ViewLogin;

namespace CCVProyecto2v2.ViewsGeneral;

public partial class InicioView : ContentPage
{
    public InicioView()
    {
        InitializeComponent();
    }
    public void Ingresar_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new LoginView());
    }
}