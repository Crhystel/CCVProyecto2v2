using CCVProyecto2v2.DataAccess;
using CCVProyecto2v2.ViewsModels;

namespace CCVProyecto2v2.ViewsAdmin;

public partial class AgregarClaseView : ContentPage
{
    public AgregarClaseView()
    {
        InitializeComponent();
        BindingContext = new ClaseViewModel(new DbbContext());
    }
}