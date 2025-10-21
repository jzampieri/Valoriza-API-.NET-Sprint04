using AutoMapper;
using Valoriza.Application.DTOs;
using Valoriza.Domain.Entities;


namespace Valoriza.Application.Mapping;


public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<TransactionRecord, TransactionViewDto>();
        CreateMap<TransactionCreateDto, TransactionRecord>()
        .ForMember(d => d.Date, opt => opt.MapFrom(s => s.Date ?? DateTime.UtcNow));
        CreateMap<TransactionUpdateDto, TransactionRecord>()
        .ForMember(d => d.Date, opt => opt.MapFrom(s => s.Date ?? DateTime.UtcNow));
    }
}