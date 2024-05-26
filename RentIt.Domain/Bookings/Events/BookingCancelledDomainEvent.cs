using RentIt.Domain.Abstractions;

namespace RentIt.Domain.Bookings.Events;
public sealed record BookingCancelledDomainEvent(Guid BookingId) : IDomainEvent;
