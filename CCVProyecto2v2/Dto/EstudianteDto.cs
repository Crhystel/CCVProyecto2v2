﻿using CCVProyecto2v2.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCVProyecto2v2.Dto
{
    public partial class EstudianteDto : ObservableObject
    {
        [ObservableProperty]
        public int id;
        [ObservableProperty]
        public string cedula;
        [ObservableProperty]
        public string nombre;
        [ObservableProperty]
        public int edad;
        [ObservableProperty]
        private GradoEnum grado;
    }
}
