using RentIt.Domain.Abstractions;

namespace RentIt.Domain.Bookings.Events;

public sealed record BookingCompletedDomainEvent(Guid BookingId) : IDomainEvent;
