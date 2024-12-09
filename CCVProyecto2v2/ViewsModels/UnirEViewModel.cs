using CCVProyecto2v2.DataAccess;
using CCVProyecto2v2.Dto;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCVProyecto2v2.ViewsModels
{
    public partial class UnirEViewModel : ObservableObject, IQueryAttributable
    {
        private readonly DbbContext _dbContext;
        [ObservableProperty]
        private ClaseDto claseDto = new();
        [ObservableProperty]
        private string tituloPagina;
        private int IdClase;
        [ObservableProperty]
        private bool loadingClase = false;
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
            IdClase = id;
            if(IdClase==0)
            {
                TituloPagina = "Unir Estudiante";
            }
            else
            {
                TituloPagina = "Editar";
                LoadingClase = false;
                var encontrado = await _dbContext.Clase.Include(c=>c.Estudiantes).FirstOrDefaultAsync(c=>c.Id==id);
                if (encontrado != null)
                {
                    ClaseDto=new ClaseDto
                    {
                        Id=encontrado.Id,
                        Estud
                    }
                }
            }
        }
    }
}
