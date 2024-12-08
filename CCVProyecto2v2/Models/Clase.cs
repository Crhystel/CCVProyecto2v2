using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCVProyecto2v2.Models
{
    public class Clase
    {
        public int Id { get; set; }
        public Grado Grado { get; set; }
        public MateriaEnum Materia { get; set; }

        public int EstudianteId { get; set; }
        public Estudiante Estudiante { get; set; }
        public int ProfesorId { get; set; }
        public Profesor Profesor { get; set; }
    }
}
