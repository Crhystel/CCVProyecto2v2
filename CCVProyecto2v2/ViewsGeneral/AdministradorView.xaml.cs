using CCVProyecto2v2.ViewsClase;
using CCVProyecto2v2.ViewsEstudiante;
using CCVProyecto2v2.ViewsProfesor;

namespace CCVProyecto2v2.ViewsGeneral;

public partial class AdministradorView : ContentPage
{
        public AdministradorView()
        {
            InitializeComponent();
        }
        public void CrearProfesor_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new PMainPage());
        }
        public void CrearEstudiante_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new EMainPage());
        }
        public void CrearCurso_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CMainPage());
        }
        public void UnirEstudiante_Clicked(object sender, EventArgs e)
        {
           Navigation.PushAsync(new CMainPage());
        }
    
}