using AutoMapper;
using Contracts;
using Contracts.Dto;

namespace Publisher;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CalculationInputDto, CalculationInput>();
        CreateMap<ComputedOutput, ComputedOutputDto>();
    }
}