using RentIt.Application.Abstractions.Clock;
using RentIt.Application.Abstractions.Messaging;
using RentIt.Application.Exceptions;
using RentIt.Domain.Abstractions;
using RentIt.Domain.Apartments;
using RentIt.Domain.Bookings;
using RentIt.Domain.Users;

namespace RentIt.Application.Bookings.ReserveBooking;
public class ReserveBookingCommandHandler
    : ICommandHandler<ReserveBookingCommand, Guid>
{
    private readonly IUserRepository m_UserRepository;
    private readonly IApartmentRepository m_ApartmentRepository;
    private readonly IBookingRepository m_BookingRepository;
    private readonly IUnitOfWork m_UnitOfWork;
    private readonly IDateTimeProvider m_DateTimeProvider;
    private readonly PricingService m_PricingService;

    public ReserveBookingCommandHandler(
        IUserRepository userRepository,
        IApartmentRepository apartmentRepository,
        IBookingRepository bookingRepository,
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider,
        PricingService pricingService)
    {
        m_UserRepository = userRepository;
        m_ApartmentRepository = apartmentRepository;
        m_BookingRepository = bookingRepository;
        m_UnitOfWork = unitOfWork;
        m_DateTimeProvider = dateTimeProvider;
        m_PricingService = pricingService;
    }

    public async Task<Result<Guid>> Handle(ReserveBookingCommand request, CancellationToken cancellationToken)
    {
        var user = await m_UserRepository.GetByIdAsync(request.UserId, cancellationToken);

        if (user is null)
        {
            return Result.Failure<Guid>(UserErrors.NotFound);
        }

        var apartment = await m_ApartmentRepository.GetByIdAsync(request.ApartmentId, cancellationToken);

        if (apartment is null)
        {
            return Result.Failure<Guid>(ApartmentErrors.NotFound);
        }

        var duration = DateRange.Create(request.StartDate, request.EndDate);

        if (await m_BookingRepository.IsOverlappingAsync(apartment, duration, cancellationToken))
        {
            return Result.Failure<Guid>(ApartmentErrors.NotFound);
        }

        try
        {
            var booking = Booking.Reserve(
                apartment,
                user.Id,
                duration,
                m_DateTimeProvider.UtcNow,
                m_PricingService);

            m_BookingRepository.Add(booking);

            await m_UnitOfWork.SaveChangesAsync(cancellationToken);

            return booking.Id;
        }
        catch (ConcurrencyException)
        {

            return Result.Failure<Guid>(BookingErrors.Overlap);
        }
    }
}
