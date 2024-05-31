namespace RentIt.Domain.Reviews;

public interface IReviewRepository
{
    void Add(Review booking);
}