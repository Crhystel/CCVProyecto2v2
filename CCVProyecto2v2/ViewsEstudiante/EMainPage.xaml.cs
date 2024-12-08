using CCVProyecto2v2.DataAccess;
using CCVProyecto2v2.ViewsModels;

namespace CCVProyecto2v2.ViewsEstudiante;

public partial class EMainPage : ContentPage
{
    public EMainPage()
    {
        InitializeComponent();
        BindingContext = new EMainViewModel(new DbbContext());
    }
}