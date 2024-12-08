using CCVProyecto2v2.DataAccess;
using CCVProyecto2v2.Dto;
using CCVProyecto2v2.ViewsModels;

namespace CCVProyecto2v2.ViewsAdmin;

public partial class AgregarClaseView : ContentPage
{
    public AgregarClaseView()
    {
        InitializeComponent();
        BindingContext = new ClaseViewModel(new DbbContext());
    }
    private void OnProfesorSelected(object sender, EventArgs e)
    {
        var picker = sender as Picker;
        if (picker != null && picker.SelectedItem is ProfesorDto profesorSeleccionado)
        {
            var viewModel = BindingContext as ClaseViewModel;
            if (viewModel != null)
            {
                viewModel.ClaseDto.ProfesorId = profesorSeleccionado.Id;
            }
        }
    }
}