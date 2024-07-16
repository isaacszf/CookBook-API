using AutoMapper;
using Communication.Requests;
using Domain.Entities;

namespace Application.Services.AutoMapper;

public class AutoMapping : Profile
{
    public AutoMapping()
    {
        RequestToDomain();
    }

    private void RequestToDomain()
    {
        CreateMap<RequestRegisterUserJson, User>()
            .ForMember(
                dest => dest.Password, 
                opt => opt.Ignore()
                );
    }
}