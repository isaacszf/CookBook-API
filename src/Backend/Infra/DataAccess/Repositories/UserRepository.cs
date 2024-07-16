using Domain.Entities;
using Domain.Repositories.User;
using Microsoft.EntityFrameworkCore;

namespace Infra.DataAccess.Repositories;

public class UserRepository : IUserReadOnlyRepository, IUserWriteOnlyRepository, IUserUpdateOnlyRepository
{
    private readonly CookBookDbContext _dbContext;

    public UserRepository(CookBookDbContext dbContext) => _dbContext = dbContext;

    public async Task Add(User user)
    { 
        await _dbContext.Users.AddAsync(user);
    } 
    
    public async Task<User?> GetByEmailAndPassword(string email, string password)
    {
        return await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(user =>
                    user.Active &&
                    user.Email.Equals(email) &&
                    user.Password.Equals(password)
                );
    }
    
    public async Task<bool> ExistsActiveUserWithEmail(string email) =>
        await _dbContext.Users.AnyAsync(user => user.Email.Equals(email) && user.Active);

    public async Task<bool> ExistsActiveUserWithIdentifier(Guid userIdentifier)
    {
        return await _dbContext.Users
            .AsNoTracking()
            .AnyAsync(user =>
                user.Active &&
                user.UserIdentifier.Equals(userIdentifier));
    }
    
    // Sem o "AsNoTracking", é possível editar o registro deste usuário
    public async Task<User> GetById(long id)
    {
        return await _dbContext.Users
            .FirstAsync(user =>
                user.Active &&
                user.Id == id);
    }
    public void Update(User user) => _dbContext.Users.Update(user);
}