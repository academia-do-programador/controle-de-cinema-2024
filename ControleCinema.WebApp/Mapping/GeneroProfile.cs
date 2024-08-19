using AutoMapper;
using ControleCinema.Dominio.ModuloGenero;
using ControleCinema.WebApp.Models;

namespace ControleCinema.WebApp.Mapping;

public class GeneroProfile : Profile
{
    public GeneroProfile()
    {
            CreateMap<InserirGeneroViewModel, Genero>();
            CreateMap<EditarGeneroViewModel, Genero>();
            CreateMap<Genero, ListarGeneroViewModel>();
            CreateMap<Genero, DetalhesGeneroViewModel>();
            CreateMap<Genero, EditarGeneroViewModel>();
    }
}