using AutoMapper;
using ControleCinema.Dominio.ModuloSala;
using ControleCinema.WebApp.Models;

namespace ControleCinema.WebApp.Mapping
{
    public class SalaProfile : Profile
    {
        public SalaProfile()
        {
            CreateMap<InserirSalaViewModel, Sala>();
            CreateMap<EditarSalaViewModel, Sala>();
            CreateMap<Sala, ListarSalaViewModel>();
            CreateMap<Sala, DetalhesSalaViewModel>();
            CreateMap<Sala, EditarSalaViewModel>();
        }
    }
}
