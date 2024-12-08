using Newtonsoft.Json;

namespace CCVProyecto2v2.ViewsGeneral;

public partial class EstudianteView : ContentPage
{
    public EstudianteView()
    {
        InitializeComponent();
        CargarActividades();
    }
    private void CargarActividades()
    {
        string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "actividades.txt");

        if (File.Exists(path))
        {
            var actividades = JsonConvert.DeserializeObject<List<Models.Actividad>>(File.ReadAllText(path)) ?? new List<Models.Actividad>();
            ActividadesCollection.ItemsSource = actividades;
        }
    }
}