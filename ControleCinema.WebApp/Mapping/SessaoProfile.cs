using AutoMapper;
using ControleCinema.Dominio.ModuloSessao;
using ControleCinema.WebApp.Models;

namespace ControleCinema.WebApp.Mapping;

public class SessaoProfile : Profile
{
    public SessaoProfile()
    {
        CreateMap<InserirSessaoViewModel, Sessao>();

        CreateMap<Sessao, ListarSessaoViewModel>()
            .ForMember(dest => dest.Filme, opt => opt.MapFrom(src => src.Filme.Titulo))
            .ForMember(dest => dest.Sala, opt => opt.MapFrom(src => src.Sala.Numero.ToString()))
            .ForMember(dest => dest.IngressosDisponiveis,
                opt => opt.MapFrom(src => src.ObterQuantidadeIngressosDisponiveis()))
            .ForMember(dest => dest.Inicio, opt => opt.MapFrom(src => src.Inicio.ToString("dd/MM/yyyy HH:mm")))
            .ForMember(dest => dest.Encerrada,
                opt => opt.MapFrom(src => src.Encerrada ? "Encerrada" : "Disponível"));

        CreateMap<Sessao, DetalhesSessaoViewModel>()
            .ForMember(dest => dest.Filme, opt => opt.MapFrom(src => src.Filme.Titulo))
            .ForMember(dest => dest.Sala, opt => opt.MapFrom(src => src.Sala.Numero.ToString()))
            .ForMember(dest => dest.IngressosDisponiveis,
                opt => opt.MapFrom(src => src.ObterQuantidadeIngressosDisponiveis()))
            .ForMember(dest => dest.Inicio, opt => opt.MapFrom(src => src.Inicio.ToString("dd/MM/yyyy HH:mm")))
            .ForMember(dest => dest.Encerrada,
                opt => opt.MapFrom(src => src.Encerrada ? "Encerrada" : "Disponível"));
    }
}