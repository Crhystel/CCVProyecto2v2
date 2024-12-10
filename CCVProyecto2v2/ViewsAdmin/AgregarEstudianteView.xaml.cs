using CCVProyecto2v2.DataAccess;
using CCVProyecto2v2.ViewsModels;

namespace CCVProyecto2v2.ViewsAdmin;

public partial class AgregarEstudianteView : ContentPage
{
    public AgregarEstudianteView()
    {
        InitializeComponent();
        BindingContext = new EstudianteViewModel(new DbbContext());

    }
}