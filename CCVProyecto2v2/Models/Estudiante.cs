using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCVProyecto2v2.Models
{
    public class Estudiante : Usuario
    {

        [Required(ErrorMessage = "Este campo es obligatorio.")]

        public GradoEnum Grado { get; set; }
        public RolEnum Rol { get; set; }
        public ICollection<ClaseEstudiante> ClasesEstudiantes { get; set; } = new List<ClaseEstudiante>();

    }
}
