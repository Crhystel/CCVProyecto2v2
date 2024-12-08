using CCVProyecto2v2.DataAccess;
using CCVProyecto2v2.ViewsModels;

namespace CCVProyecto2v2.ViewsAdmin;

public partial class AgregarProfesorView : ContentPage
{
    public AgregarProfesorView()
    {
        InitializeComponent();
        BindingContext = new ProfesorViewModel(new DbbContext());
    }
}