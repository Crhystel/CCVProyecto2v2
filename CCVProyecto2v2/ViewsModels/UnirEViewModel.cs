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
            MainThread.BeginInvokeOnMainThread(async () => await CargarDatos());
        }

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            var id= int.Parse(query["id"].ToString());
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
                        EstudianteId = encontrado.Id,
                    };
                    ClaseEstudianteDto.Estudiante = new EstudianteDto
                    {
                        Id = encontrado.Estudiante.Id,
                        Nombre = encontrado.Estudiante.Nombre,
                        Grado = encontrado.Estudiante.Grado,
                    };
                }
            }
            MainThread.BeginInvokeOnMainThread(() => { LoadingClaseEstudiante = false; });
        }
        [RelayCommand]
        public async Task Guardar()
        {
            LoadingClaseEstudiante = true;
            var mensaje = new Cuerpo();

            await Task.Run(async () =>
            {
                if (IdClaseEstudiante == 0)
                {
                    var nuevaClase = new ClaseEstudiante
                    {
                        EstudianteId = ClaseEstudianteDto.EstudianteId,
                        ClaseId = ClaseEstudianteDto.Id,
                    };

                    _dbContext.ClaseEstudiantes.Add(nuevaClase);
                    await _dbContext.SaveChangesAsync();

                    ClaseEstudianteDto.Id = nuevaClase.Id;

                    mensaje = new Cuerpo
                    {
                        EsCrear = true,
                        ClaseEstudianteDto = ClaseEstudianteDto
                    };
                }
                else
                {
                    var encontrado = await _dbContext.ClaseEstudiantes.FirstOrDefaultAsync(c => c.Id == IdClaseEstudiante);

                    if (encontrado != null)
                    {
                        encontrado.EstudianteId = ClaseEstudianteDto.Id;
                        encontrado.ClaseId = ClaseEstudianteDto.Id;

                        await _dbContext.SaveChangesAsync();

                        mensaje = new Cuerpo
                        {
                            EsCrear = false,
                            ClaseEstudianteDto = ClaseEstudianteDto
                        };
                    }
                }
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    LoadingClaseEstudiante = false;
                    WeakReferenceMessenger.Default.Send(new Mensajeria(mensaje));
                    Shell.Current.Navigation.PopAsync();
                });
            });
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
                    Cedula = p.Cedula,
                    Grado = p.Grado
                }));


        }

    }
}
