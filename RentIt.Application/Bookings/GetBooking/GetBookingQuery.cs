using RentIt.Application.Abstractions.Messaging;

namespace RentIt.Application.Bookings.GetBooking;
public sealed record GetBookingQuery(Guid BookingId) : IQuery<BookingResponse>;
