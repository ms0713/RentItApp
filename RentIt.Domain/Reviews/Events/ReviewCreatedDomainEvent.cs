using RentIt.Domain.Abstractions;

namespace RentIt.Domain.Reviews.Events;
public sealed record ReviewCreatedDomainEvent(Guid ReviewId) : IDomainEvent;
