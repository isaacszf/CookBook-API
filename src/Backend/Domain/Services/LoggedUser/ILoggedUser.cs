using Domain.Entities;

namespace Domain.Services.LoggedUser;

public interface ILoggedUser
{
    public Task<User> User();
}