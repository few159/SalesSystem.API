using Domain.Enitities;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Infrastructure.Persistence;
using Infrastructure.Repositories.Base;

namespace Infrastructure.Repositories;

public class SaleRepository(SalesDbContext context) : Repository<Sale>(context), ISaleRepository
{
    
}