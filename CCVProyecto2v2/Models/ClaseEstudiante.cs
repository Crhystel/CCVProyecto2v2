using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCVProyecto2v2.Models
{
    public class ClaseEstudiante
    {
        public int Id { get; set; }
        public int ClaseId { get; set; }
        public Clase Clase { get; set; }
        public int EstudianteId { get; set; }
        public Estudiante Estudiante { get; set; }
    }
}
