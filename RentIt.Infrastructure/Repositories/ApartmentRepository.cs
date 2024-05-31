using RentIt.Domain.Apartments;

namespace RentIt.Infrastructure.Repositories;
internal sealed class ApartmentRepository : Repository<Apartment>, IApartmentRepository
{
    public ApartmentRepository(ApplicationDbContext dbContext) 
        : base(dbContext)
    {
    }
}
