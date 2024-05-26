using MediatR;
using RentIt.Application.Abstractions.Email;
using RentIt.Domain.Bookings;
using RentIt.Domain.Bookings.Events;
using RentIt.Domain.Users;

namespace RentIt.Application.Bookings.ReserveBooking;
internal sealed class BookingReservedDomainEventHandler : INotificationHandler<BookingReservedDomainEvent>
{
    private readonly IBookingRepository m_BookingRepository;
    private readonly IUserRepository m_UserRepository;
    private readonly IEmailService m_EmailService;

    public BookingReservedDomainEventHandler(
        IBookingRepository bookingRepository,
        IUserRepository userRepository,
        IEmailService emailService)
    {
        m_BookingRepository = bookingRepository;
        m_UserRepository = userRepository;
        m_EmailService = emailService;
    }

    public async Task Handle(BookingReservedDomainEvent notification, CancellationToken cancellationToken)
    {
        var booking =
            await m_BookingRepository.GetByIdAsync(notification.BookingId, cancellationToken);

        if (booking is null)
        {
            return;
        }

        var user =
            await m_UserRepository.GetByIdAsync(booking.UserId, cancellationToken);

        if (user is null)
        {
            return;
        }

        await m_EmailService.SendAsync(
            user.Email,
            "Booking reserved!",
            "You have 10 minutes to confirm this booking");
    }
}
