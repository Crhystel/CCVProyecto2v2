using CCVProyecto2v2.DataAccess;
using CCVProyecto2v2.Dto;
using CCVProyecto2v2.Models;
using CCVProyecto2v2.Utilidades;
using CCVProyecto2v2.ViewsAdmin;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace CCVProyecto2v2.ViewsModels
{
    public partial class UEMainViewModel : ObservableObject
    {
        private readonly DbbContext _dbContext;

        [ObservableProperty]
        private ObservableCollection<ClaseEstudianteDto> listaClaseEstudiantes = new();
        [ObservableProperty]
        private ObservableCollection<ClaseDto> listaClases = new();

        public UEMainViewModel(DbbContext context)
        {
            _dbContext = context;
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await ObtenerClases();
                await ObtenerClasesParaPicker();
            });
            WeakReferenceMessenger.Default.Register<Mensajeria>(this, (r, m) =>
            {
                ClaseMensajeRecibido(m.Value);
            });
        }
        public async Task ObtenerClases()
        {
            var lista = await _dbContext.ClaseEstudiantes
                .Include(c => c.Estudiante)
                .Include(c => c.Clase)
                .ThenInclude(c => c.Profesor)
                .ToListAsync();


            var clasesAgrupadas = lista.GroupBy(c => c.Clase.Id) .Select(c => new ClaseEstudianteDto
           {
               ClaseId = c.Key,
               Clase = new ClaseDto
               {
                   Id = c.First().Clase.Id,
                   Nombre = c.First().Clase.Nombre,
                   Profesor = new ProfesorDto
                   {
                       Id = c.First().Clase.Profesor.Id,
                       Nombre = c.First().Clase.Profesor.Nombre
                   }
               },
               Estudiantes = c.Select(c => new EstudianteDto
               {
                   Id = c.Estudiante.Id,
                   Nombre = c.Estudiante.Nombre,
                   Cedula = c.Estudiante.Cedula,
                   Edad = c.Estudiante.Edad,
                   Grado = c.Estudiante.Grado
               }).ToList()
           })
           .ToList();

                ListaClaseEstudiantes.Clear();

                foreach (var clase in clasesAgrupadas)
                {
                    ListaClaseEstudiantes.Add(clase);
                }
        }
        public async Task ObtenerClasesParaPicker()
        {
            var clases = await _dbContext.Clase.ToListAsync();

            ListaClases = new ObservableCollection<ClaseDto>(
                clases.Select(c => new ClaseDto
                {
                    Id = c.Id,
                    Nombre = c.Nombre,
                    Profesor = c.Profesor != null ? new ProfesorDto
                    {
                        Id = c.Profesor.Id,
                        Nombre = c.Nombre,
                    } : null
                }));
       


        }
        private void ClaseMensajeRecibido(Cuerpo claseCuerpo)
        {
            var claseEstudianteDto = claseCuerpo.ClaseEstudianteDto;

            if (claseCuerpo.EsCrear)
            {
                ListaClaseEstudiantes.Add(claseEstudianteDto);
            }
            else
            {
                var encontrada = ListaClaseEstudiantes.FirstOrDefault(c => c.Id == claseEstudianteDto.Id);
                if (encontrada != null)
                {
                    encontrada.EstudianteId = claseEstudianteDto.EstudianteId;
                    encontrada.Estudiante = claseEstudianteDto.Estudiante;
                }
            }
        }
        [RelayCommand]
        private async Task Crear()
        {
            var uri = $"{nameof(UnirEstudianteView)}?id=0";
            await Shell.Current.GoToAsync(uri);
        }

        [RelayCommand]
        private async Task Editar(ClaseEstudianteDto claseDto)
        {
            var uri = $"{nameof(UnirEstudianteView)}?id={claseDto.Id}";
            await Shell.Current.GoToAsync(uri);
        }

        [RelayCommand]
        private async Task Eliminar(ClaseEstudianteDto claseDto)
        {
            bool anwser = await Shell.Current.DisplayAlert("Mensaje", "¿Desea eliminar esta clase?", "Sí", "No");
            if (anwser)
            {
                var encontrada = await _dbContext.ClaseEstudiantes.FirstOrDefaultAsync(c => c.Id == claseDto.Id);
                if (encontrada != null)
                {
                    _dbContext.ClaseEstudiantes.Remove(encontrada);
                    await _dbContext.SaveChangesAsync();

                    ListaClaseEstudiantes.Remove(claseDto);
                }
            }
        }

        [RelayCommand]
        public async Task Guardar(ClaseEstudianteDto claseEstudianteDto)
        {
            var nuevaClaseEstudiante = new ClaseEstudiante
            {
                EstudianteId = claseEstudianteDto.EstudianteId,
                ClaseId = claseEstudianteDto.ClaseId
            };

            if (claseEstudianteDto.Id == 0)
            {
                _dbContext.ClaseEstudiantes.Add(nuevaClaseEstudiante);
                await _dbContext.SaveChangesAsync();

                claseEstudianteDto.Id = nuevaClaseEstudiante.Id;
                var mensaje = new Cuerpo { EsCrear = true, ClaseEstudianteDto = claseEstudianteDto };
                WeakReferenceMessenger.Default.Send(new Mensajeria(new Cuerpo { EsCrear = true }));

            }
            await ObtenerClases();
            await ObtenerClasesParaPicker();
            await Shell.Current.GoToAsync(".."); 
        }
    }
}
