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
    public partial class ClaseViewModel : ObservableObject, IQueryAttributable
    {
        private readonly DbbContext _dbContext;

        [ObservableProperty]
        private ClaseDto claseDto = new();

        [ObservableProperty]
        private string tituloPagina;

        private int idClase;

        [ObservableProperty]
        private bool loadingClase;
        public ClaseViewModel()
        {

        }
        public ClaseViewModel(DbbContext context)
        {
            _dbContext = context;
            MainThread.BeginInvokeOnMainThread(async () => await CargarDatos());
        }

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("id") && int.TryParse(query["id"].ToString(), out var id))
            {
                idClase = id;

                if (idClase == 0)
                {
                    TituloPagina = "Nueva Clase";
                }
                else
                {
                    TituloPagina = "Editar Clase";
                    LoadingClase = true;

                    var encontrado = await _dbContext.Clase
                        .Include(c => c.Profesor)
                        .FirstOrDefaultAsync(c => c.Id == idClase);

                    if (encontrado != null)
                    {
                        ClaseDto = new ClaseDto
                        {
                            Id = encontrado.Id,
                            ProfesorId = encontrado.ProfesorId,
                        };


                        {
                            ClaseDto.Profesor = new ProfesorDto
                            {
                                Id = encontrado.Profesor.Id,
                                Nombre = encontrado.Profesor.Nombre,
                                Materia = encontrado.Profesor.Materia
                            };
                        }
                    }
                }

            }
            MainThread.BeginInvokeOnMainThread(() => { LoadingClase = false; });
        }



        [RelayCommand]
        public async Task Guardar()
        {
            LoadingClase = true;

            var mensaje = new CuerpoC();

            if (idClase == 0)
            {
                var nuevaClase = new Clase
                {
                    ProfesorId = ClaseDto.ProfesorId
                };

                _dbContext.Clase.Add(nuevaClase);
                await _dbContext.SaveChangesAsync();

                ClaseDto.Id = nuevaClase.Id;

                mensaje = new CuerpoC
                {
                    EsCrear = true,
                    ClaseDto = ClaseDto
                };
            }
            else
            {
                var encontrado = await _dbContext.Clase.FirstOrDefaultAsync(c => c.Id == idClase);

                if (encontrado != null)
                {
                    encontrado.ProfesorId = ClaseDto.ProfesorId;

                    await _dbContext.SaveChangesAsync();

                    mensaje = new CuerpoC
                    {
                        EsCrear = false,
                        ClaseDto = ClaseDto
                    };
                }
            }

            LoadingClase = false;

            WeakReferenceMessenger.Default.Send(new MensajeriaC(mensaje));
            await Shell.Current.Navigation.PopAsync();
        }

        [ObservableProperty]
        private ObservableCollection<ProfesorDto> profesoresDisponibles = new();

        [ObservableProperty]
        private ObservableCollection<EstudianteDto> estudiantesDisponibles = new();

        public async Task CargarDatos()
        {
            var profesores = await _dbContext.Profesor.ToListAsync();
            ProfesoresDisponibles = new ObservableCollection<ProfesorDto>(
                profesores.Select(p => new ProfesorDto
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    Cedula = p.Cedula,
                    Materia = p.Materia
                }));

           
        }


    }
}
