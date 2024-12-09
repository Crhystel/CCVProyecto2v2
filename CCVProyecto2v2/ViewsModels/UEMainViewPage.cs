

using CCVProyecto2v2.DataAccess;
using CCVProyecto2v2.Dto;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace CCVProyecto2v2.ViewsModels
{
    public partial class UEMainViewPage: ObservableObject
    {
        private readonly DbbContext _dbContext;
        [ObservableProperty]
        private ObservableCollection<EstudianteDto> listaEstudiantes = new ObservableCollection<EstudianteDto>();
        public UEMainViewPage(DbbContext context)
        {
            _dbContext = context;

        }
        
    }
}
