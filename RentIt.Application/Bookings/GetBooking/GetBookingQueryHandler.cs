﻿using Dapper;
using RentIt.Application.Abstractions.Data;
using RentIt.Application.Abstractions.Messaging;
using RentIt.Domain.Abstractions;

namespace RentIt.Application.Bookings.GetBooking;
public sealed class GetBookingQueryHandler :
    IQueryHandler<GetBookingQuery, BookingResponse>
{
    private readonly ISqlConnectionFactory m_SqlConnectionFactory;

    public GetBookingQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        m_SqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<Result<BookingResponse>> Handle(GetBookingQuery request, CancellationToken cancellationToken)
    {
        using var connection = m_SqlConnectionFactory.CreateConnection();

        const string sql = """
            SELECT
                id AS Id,
                apartment_id AS ApartmentId,
                user_id AS UserId,
                status AS Status,
                price_for_period_amount AS PriceAmount,
                price_for_period_currency AS PriceCurrency,
                cleaning_fee_amount AS CleaningFeeAmount,
                cleaning_fee_currency AS CleaningFeeCurrency,
                amenities_up_charge_amount AS AmenitiesUpChargeAmount,
                amenities_up_charge_currency AS AmenitiesUpChargeCurrency,
                total_price_amount AS TotalPriceAmount,
                total_price_currency AS TotalPriceCurrency,
                duration_start AS DurationStart,
                duration_end AS DurationEnd,
                created_on_utc AS CreatedOnUtc
            FROM bookings
            WHERE id = @BookingId
            """;

        var booking = await connection.QueryFirstOrDefaultAsync<BookingResponse>(
            sql,
            new { request.BookingId });

        return booking;
    }
}
