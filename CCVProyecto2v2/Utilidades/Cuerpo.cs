using CCVProyecto2v2.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCVProyecto2v2.Utilidades
{
    public class Cuerpo
    {
        public bool EsCrear { get; set; }
        public EstudianteDto EstudianteDto { get; set; }
        public ClaseDto ClaseDto { get; set; }
        public ProfesorDto ProfesorDto { get; set; }
    }
}
