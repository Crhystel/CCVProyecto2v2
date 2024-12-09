

using CCVProyecto2v2.DataAccess;
using CCVProyecto2v2.Dto;
using CCVProyecto2v2.Utilidades;
using CCVProyecto2v2.ViewsAdmin;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace CCVProyecto2v2.ViewsModels
{
    public partial class UEMainViewModel: ObservableObject
    {
        private readonly DbbContext _dbContext;
        [ObservableProperty]
        private ObservableCollection<ClaseEstudianteDto> listaClaseEstudiantes = new ObservableCollection<ClaseEstudianteDto>();
        public UEMainViewModel(DbbContext context)
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
            var lista = await _dbContext.ClaseEstudiantes.Include(c => c.Estudiante).Include(c=>c.Clase).ThenInclude(c=>c.Profesor).ToListAsync();
            //ListaClases.Clear();
            ListaClaseEstudiantes.Clear();
            
                foreach (var clase in lista)
                {
                    ListaClaseEstudiantes.Add(new ClaseEstudianteDto
                    {
                        Id = clase.Id,
                        EstudianteId = clase.Id,
                        ClaseId = clase.ClaseId,
                        Estudiante = new EstudianteDto
                        {
                            Id = clase.Estudiante.Id,
                            Nombre = clase.Estudiante.Nombre,
                            Cedula = clase.Estudiante.Cedula,
                            Edad = clase.Estudiante.Edad,
                            Grado = clase.Estudiante.Grado
                        },
                        Clase = new ClaseDto
                        {
                            Id = clase.Clase.Id,
                            Nombre = clase.Clase.Nombre,
                            Profesor = new ProfesorDto
                            {
                                Id = clase.Clase.Profesor.Id,
                                Nombre = clase.Clase.Profesor.Nombre
                            }
                        }
                    });
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
                var encontrada = ListaClaseEstudiantes.First(c => c.Id == claseEstudianteDto.Id);

                encontrada.EstudianteId = claseEstudianteDto.EstudianteId;
                encontrada.Estudiante = claseEstudianteDto.Estudiante;
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
                var encontrada = await _dbContext.Clase
                    .FirstAsync(c => c.Id == claseDto.Id);

                _dbContext.Clase.Remove(encontrada);
                await _dbContext.SaveChangesAsync();

                ListaClaseEstudiantes.Remove(claseDto);
            }
        }

    }
}
