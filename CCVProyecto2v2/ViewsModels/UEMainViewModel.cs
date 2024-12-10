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

        public UEMainViewModel(DbbContext context)
        {
            _dbContext = context;

            MainThread.BeginInvokeOnMainThread(async () => await ObtenerClases());

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

            ListaClaseEstudiantes.Clear();

            var clasesAgrupadas = lista.GroupBy(c => c.Clase.Id) .Select(grupo => new ClaseEstudianteDto
           {
               ClaseId = grupo.Key,
               Clase = new ClaseDto
               {
                   Id = grupo.First().Clase.Id,
                   Nombre = grupo.First().Clase.Nombre,
                   Profesor = new ProfesorDto
                   {
                       Id = grupo.First().Clase.Profesor.Id,
                       Nombre = grupo.First().Clase.Profesor.Nombre
                   }
               },
               Estudiantes = grupo.Select(g => new EstudianteDto
               {
                   Id = g.Estudiante.Id,
                   Nombre = g.Estudiante.Nombre,
                   Cedula = g.Estudiante.Cedula,
                   Edad = g.Estudiante.Edad,
                   Grado = g.Estudiante.Grado
               }).ToList()
           })
           .ToList();

                ListaClaseEstudiantes.Clear();

                foreach (var clase in clasesAgrupadas)
                {
                    ListaClaseEstudiantes.Add(clase);
                }
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
                WeakReferenceMessenger.Default.Send(new Mensajeria(mensaje));
            }

            await Shell.Current.GoToAsync(".."); 
        }
    }
}
