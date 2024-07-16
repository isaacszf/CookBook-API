namespace Domain.Repositories.User;

public interface IUserReadOnlyRepository
{
    public Task<bool> ExistsActiveUserWithEmail(string email);
    public Task<bool> ExistsActiveUserWithIdentifier(Guid userIdentifier);
    public Task<Entities.User?> GetByEmailAndPassword(string email, string password);
}