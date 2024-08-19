using AutoMapper;
using ControleCinema.Dominio.Extensions;
using ControleCinema.Dominio.ModuloFilme;
using ControleCinema.WebApp.Models;

namespace ControleCinema.WebApp.Mapping;

public class FilmeProfile : Profile
{
    public FilmeProfile()
    {
        CreateMap<InserirFilmeViewModel, Filme>();
        CreateMap<EditarFilmeViewModel, Filme>();

        CreateMap<Filme, EditarFilmeViewModel>()
            .ForMember(dest => dest.GeneroId, opt => opt.MapFrom(src => src.Genero.Id));

        CreateMap<Filme, ListarFilmeViewModel>()
            .ForMember(dest => dest.Duracao, opt => opt.MapFrom(src => src.Duracao.FormatarEmHorasEMinutos()))
            .ForMember(dest => dest.Lancamento, opt => opt.MapFrom(src => src.Lancamento ? "Lançamento" : "Re-Exibição"))
            .ForMember(dest => dest.Genero, opt => opt.MapFrom(src => src.Genero.Descricao));

        CreateMap<Filme, DetalhesFilmeViewModel>()
            .ForMember(dest => dest.Duracao, opt => opt.MapFrom(src => src.Duracao.FormatarEmHorasEMinutos()))
            .ForMember(dest => dest.Lancamento, opt => opt.MapFrom(src => src.Lancamento ? "Lançamento" : "Re-Exibição"))
            .ForMember(dest => dest.Genero, opt => opt.MapFrom(src => src.Genero.Descricao));
    }
}