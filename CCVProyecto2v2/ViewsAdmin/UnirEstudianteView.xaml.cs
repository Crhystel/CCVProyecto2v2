using CCVProyecto2v2.DataAccess;
using CCVProyecto2v2.Dto;
using CCVProyecto2v2.ViewsModels;

namespace CCVProyecto2v2.ViewsAdmin;

public partial class UnirEstudianteView : ContentPage
{
	public UnirEstudianteView()
	{
		InitializeComponent();
        BindingContext = new UnirEViewModel(new DbbContext());

    }
    private void OnClaseSelected(object sender, EventArgs e)
    {
        var picker = sender as Picker;
        if (picker?.SelectedItem is ClaseDto claseSeleccionada)
        {
            var viewModel = BindingContext as UnirEViewModel;
            if (viewModel != null)
            {
                viewModel.ClaseEstudianteDto.ClaseId = claseSeleccionada.Id;
            }
        }
    }

    private void OnEstudianteSelected(object sender, EventArgs e)
    {
        var picker = sender as Picker;
        if (picker != null && picker.SelectedItem is EstudianteDto estudianteSeleccionado)
        {
            var viewModel = BindingContext as UnirEViewModel;
            if (viewModel != null)
            {
                viewModel.ClaseEstudianteDto.EstudianteId = estudianteSeleccionado.Id;
            }
        }
    }

}