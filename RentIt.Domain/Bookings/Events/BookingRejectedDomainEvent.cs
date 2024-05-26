using RentIt.Domain.Abstractions;

namespace RentIt.Domain.Bookings.Events;

public sealed record BookingRejectedDomainEvent(Guid BookingId) : IDomainEvent;
