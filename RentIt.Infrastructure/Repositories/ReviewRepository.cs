using RentIt.Domain.Reviews;

namespace RentIt.Infrastructure.Repositories;
internal sealed class ReviewRepository : Repository<Review>, IReviewRepository
{
    public ReviewRepository(ApplicationDbContext dbContext) 
        : base(dbContext)
    {
    }
}
