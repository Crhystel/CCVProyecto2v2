using CCVProyecto2v2.DataAccess;
using CCVProyecto2v2.Dto;
using CCVProyecto2v2.Models;
using CCVProyecto2v2.Utilidades;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCVProyecto2v2.ViewsModels
{
    public partial class UnirEViewModel : ObservableObject, IQueryAttributable
    {
        private readonly DbbContext _dbContext;
        [ObservableProperty]
        private ClaseEstudianteDto claseEstudianteDto = new();
        [ObservableProperty]
        private ObservableCollection<ClaseEstudianteDto> listaClaseEstudiantes = new();
        [ObservableProperty]
        private ObservableCollection<ClaseDto> clasesDisponibles = new();
        [ObservableProperty]
        private ObservableCollection<EstudianteDto> estudiantesSeleccionados = new();
        [ObservableProperty]
        private string tituloPagina;
        private int IdClaseEstudiante;
        [ObservableProperty]
        private bool loadingClaseEstudiante = false;
        public UnirEViewModel()
        {
            
        }
        public UnirEViewModel(DbbContext context)
        {
            _dbContext = context;
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await CargarDatos();
                await CargarClases();
                await CargarEstudiantesPorClase();
            });
        }

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            var id= int.Parse(query["id"].ToString());
            LoadingClaseEstudiante = true;
            IdClaseEstudiante = id;
            if(IdClaseEstudiante==0)
            {
                TituloPagina = "Unir Estudiante";
            }
            else
            {
                TituloPagina = "Editar";
                LoadingClaseEstudiante = false;
                var encontrado = await _dbContext.ClaseEstudiantes.Include(c=> c.Clase).Include(c=>c.Estudiante).FirstOrDefaultAsync(c=>c.Id==id);
                if (encontrado != null)
                {
                    ClaseEstudianteDto = new ClaseEstudianteDto
                    {
                        Id = encontrado.Id,
                        ClaseId = encontrado.ClaseId,
                        EstudianteId = encontrado.EstudianteId,
                        Estudiante = new EstudianteDto
                        {
                            Id = encontrado.Estudiante.Id,
                            Nombre = encontrado.Estudiante.Nombre,
                            Grado = encontrado.Estudiante.Grado
                        },
                        Clase = new ClaseDto
                        {
                            Id = encontrado.Clase.Id,
                            Nombre = encontrado.Clase.Nombre
                        }
                    };
                }
                LoadingClaseEstudiante = false;
            }
            MainThread.BeginInvokeOnMainThread(() => { LoadingClaseEstudiante = false; });
        }
        [RelayCommand]
        public async Task Guardar()
        {
            LoadingClaseEstudiante = true;
            var mensaje = new Cuerpo();

            try
            {
                if (IdClaseEstudiante == 0)
                {
                    var nuevaClaseEstudiante = new ClaseEstudiante
                    {
                        EstudianteId = ClaseEstudianteDto.EstudianteId,
                        ClaseId = ClaseEstudianteDto.ClaseId
                    };

                    _dbContext.ClaseEstudiantes.Add(nuevaClaseEstudiante);
                    await _dbContext.SaveChangesAsync();

                    ClaseEstudianteDto.Id = nuevaClaseEstudiante.Id;

                    mensaje = new Cuerpo
                    {
                        EsCrear = true,
                        ClaseEstudianteDto = ClaseEstudianteDto
                    };
                }
                else
                {
                    var existente = await _dbContext.ClaseEstudiantes
                        .FirstOrDefaultAsync(c => c.Id == IdClaseEstudiante);

                    if (existente != null)
                    {
                        existente.EstudianteId = ClaseEstudianteDto.EstudianteId;
                        existente.ClaseId = ClaseEstudianteDto.ClaseId;

                        await _dbContext.SaveChangesAsync();

                        mensaje = new Cuerpo
                        {
                            EsCrear = false,
                            ClaseEstudianteDto = ClaseEstudianteDto
                        };
                    }
                }

                WeakReferenceMessenger.Default.Send(new Mensajeria(mensaje));
                await Shell.Current.Navigation.PopAsync();
            }
            finally
            {
                LoadingClaseEstudiante = false;
            }
        }
        [ObservableProperty]
        private ObservableCollection<EstudianteDto> estudiantesDisponibles = new();

        public async Task CargarDatos()
        {
            var estudiantes = await _dbContext.Estudiante.ToListAsync();
            EstudiantesDisponibles = new ObservableCollection<EstudianteDto>(
                estudiantes.Select(p => new EstudianteDto
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    Grado = p.Grado
                }));
        }
        [RelayCommand]
        public async Task GuardarMultiple()
        {
            foreach (var estudiante in EstudiantesSeleccionados)
            {
                var nuevaClaseEstudiante = new ClaseEstudiante
                {
                    EstudianteId = estudiante.Id,
                    ClaseId = ClaseEstudianteDto.ClaseId
                };
                _dbContext.ClaseEstudiantes.Add(nuevaClaseEstudiante);
            }
            await _dbContext.SaveChangesAsync();
            await CargarDatos();
        }
        public async Task CargarClases()
        {
            var clases = await _dbContext.Clase.Include(c => c.Profesor).ToListAsync();
            ClasesDisponibles = new ObservableCollection<ClaseDto>(
                clases.Select(c => new ClaseDto
                {
                    Id = c.Id,
                    Nombre = c.Nombre,
                    ProfesorId = c.ProfesorId,
                    Profesor = new ProfesorDto
                    {
                        Id = c.Profesor.Id,
                        Nombre = c.Profesor.Nombre
                    }
                }));
        }
        public async Task CargarEstudiantesPorClase()
        {
            var claseEstudiantes = await _dbContext.ClaseEstudiantes
                .Include(ce => ce.Clase)
                .Include(ce => ce.Estudiante)
                .ToListAsync();

            ListaClaseEstudiantes = new ObservableCollection<ClaseEstudianteDto>(
                claseEstudiantes.Select(ce => new ClaseEstudianteDto
                {
                    Id = ce.Id,
                    Clase = new ClaseDto
                    {
                        Id = ce.Clase.Id,
                        Nombre = ce.Clase.Nombre,
                        Profesor = new ProfesorDto
                        {
                            Id = ce.Clase.Profesor.Id,
                            Nombre = ce.Clase.Profesor.Nombre
                        }
                    },
                    Estudiante = new EstudianteDto
                    {
                        Id = ce.Estudiante.Id,
                        Nombre = ce.Estudiante.Nombre
                    }
                }));
        }

    }
}
