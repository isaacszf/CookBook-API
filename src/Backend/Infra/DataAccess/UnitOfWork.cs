using Domain.Repositories;

namespace Infra.DataAccess;

public class UnitOfWork: IUnitOfWork
{
    private readonly CookBookDbContext _dbContext;

    public UnitOfWork(CookBookDbContext dbContext) => _dbContext = dbContext;

    public async Task Commit() => await _dbContext.SaveChangesAsync();
}