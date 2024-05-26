using RentIt.Domain.Abstractions;

namespace RentIt.Domain.Bookings.Events;

public sealed record BookingReservedDomainEvent(Guid BookingId) : IDomainEvent;
