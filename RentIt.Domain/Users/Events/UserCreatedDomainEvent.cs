using RentIt.Domain.Abstractions;

namespace RentIt.Domain.Users.Events;
public sealed record UserCreatedDomainEvent(Guid UserId) : IDomainEvent;