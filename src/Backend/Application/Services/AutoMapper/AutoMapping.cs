using AutoMapper;
using Communication.Requests;
using Communication.Responses;
using Domain.Entities;
using Sqids;

namespace Application.Services.AutoMapper;

public class AutoMapping : Profile
{
    private readonly SqidsEncoder<long> _idEncoder;
    
    public AutoMapping(SqidsEncoder<long> idEncoder)
    {
        _idEncoder = idEncoder;
        
        RequestToDomain();
        DomainToResponse();
    }

    private void RequestToDomain()
    {
        CreateMap<RequestRegisterUserJson, User>()
            .ForMember(
                dest => dest.Password, 
                opt => opt.Ignore()
                );

        CreateMap<RequestRecipeJson, Recipe>()
            .ForMember(dest => dest.Instructions, opt => opt.Ignore())
            .ForMember(dest => dest.Ingredients, opt => opt.MapFrom(source => source.Ingredients.Distinct()))
            .ForMember(dest => dest.DishTypes, opt => opt.MapFrom(source => source.DishTypes.Distinct()));

        CreateMap<string, Ingredient>()
            .ForMember(dest => dest.Item, opt => opt.MapFrom(source => source));
        
        CreateMap<Communication.Enums.DishType, DishType>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(source => source));

        CreateMap<RequestInstructionJson, Instruction>();
    }

    private void DomainToResponse()
    {
        CreateMap<Recipe, ResponseCreatedRecipeJson>()
            .ForMember(dest => dest.Id, config => config.MapFrom(source => _idEncoder.Encode(source.Id)));

        CreateMap<Recipe, ResponseShortRecipeJson>()
            .ForMember(dest => dest.Id, config => config.MapFrom(source => _idEncoder.Encode(source.Id)))
            .ForMember(dest => dest.AmountIngredients, config => config.MapFrom(source => source.Ingredients.Count));
    }
}