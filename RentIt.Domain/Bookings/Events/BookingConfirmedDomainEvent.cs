using RentIt.Domain.Abstractions;

namespace RentIt.Domain.Bookings.Events;

public sealed record BookingConfirmedDomainEvent(Guid BookingId) : IDomainEvent;
