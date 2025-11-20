using AutoMapper;
using SparkUpSolution.Application.DTOs;
using SparkUpSolution.Application.Requests;
using SparkUpSolution.Domain.Entities;
using SparkUpSolution.Domain.Enums;

namespace SparkUpSolution.Application.Mapping
{
    public class BonusProfile : Profile
    {
        public BonusProfile()
        {
            CreateMap<Bonus, BonusDTO>();
            CreateMap<CreateBonusRequest, Bonus>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Player, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.MapFrom(_ => BonusStatus.Pending));
        }
    }
}
