using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCVProyecto2v2.Dto
{
    public partial class ClaseEstudianteDto:ObservableObject
    {
        [ObservableProperty]
        public int id;
        [ObservableProperty]
        public int? claseId;

        [ObservableProperty]
        public ClaseDto clase;

        [ObservableProperty]
        public int estudianteId;

        [ObservableProperty]
        public EstudianteDto estudiante;
    }
}
