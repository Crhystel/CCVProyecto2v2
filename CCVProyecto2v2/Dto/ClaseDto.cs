using CCVProyecto2v2.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCVProyecto2v2.Dto
{
    public partial class ClaseDto:ObservableObject
    {
        [ObservableProperty]
        public int id;
        [ObservableProperty]
        public string nombre;
        [ObservableProperty]
        public int profesorId;
        [ObservableProperty]
        public ProfesorDto profesor;
        [ObservableProperty]
        public ObservableCollection<EstudianteDto> estudiantes;
    }
}
