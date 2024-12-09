

using CCVProyecto2v2.DataAccess;
using CCVProyecto2v2.Dto;
using CCVProyecto2v2.Utilidades;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace CCVProyecto2v2.ViewsModels
{
    public partial class UEMainViewPage: ObservableObject
    {
        private readonly DbbContext _dbContext;
        [ObservableProperty]
        private ObservableCollection<ClaseEstudianteDto> listaClaseEstudiantes = new ObservableCollection<ClaseEstudianteDto>();
        public UEMainViewPage(DbbContext context)
        {
            _dbContext = context;
            MainThread.BeginInvokeOnMainThread(new Action(async () => await ObtenerClases()));

            WeakReferenceMessenger.Default.Register<Mensajeria>(this, (r, m) =>
            {
                ClaseMensajeRecibido(m.Value);
            });

        }
        public async Task ObtenerClases()
        {
            var lista = await _dbContext.ClaseEstudiantes.Include(c => c.Estudiante).Include(c=>c.Clase).ToListAsync();
            //ListaClases.Clear();

            if (lista.Any())
            {
                foreach (var clase in lista)
                {
                    ListaClaseEstudiantes.Add(new ClaseEstudianteDto
                    {
                        Id = clase.Id,
                        EstudianteId = clase.Id,

                        Estudiante = new EstudianteDto
                        {
                            Id = clase.Estudiante.Id,
                            Nombre = clase.Estudiante.Nombre,
                            Cedula = clase.Estudiante.Cedula,
                            Edad = clase.Estudiante.Edad,
                            Grado = clase.Estudiante.Grado
                        }
                    });
                }
            }
        }

    }
}
