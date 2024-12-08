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
        public string Nombre { get; set; }
        public int ProfesorId { get; set; } 
        public Profesor Profesor { get; set; } 
        public ICollection<Estudiante> Estudiantes { get; set; }
    }
}
