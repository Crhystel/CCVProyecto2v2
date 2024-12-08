using CCVProyecto2v2.DataAccess;
using CCVProyecto2v2.ViewsModels;

namespace CCVProyecto2v2.ViewsProfesor;

public partial class PMainPage : ContentPage
{
    public PMainPage()
    {
        InitializeComponent();
        BindingContext = new PMainViewModel(new DbbContext());
    }
}