using CCVProyecto2v2.DataAccess;
using CCVProyecto2v2.ViewsModels;

namespace CCVProyecto2v2.ViewsClase;

public partial class CMainPage : ContentPage
{
    public CMainPage()
    {
        InitializeComponent();
        BindingContext = new CMainViewModel(new DbbContext());
    }
}