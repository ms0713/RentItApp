using MediatR;
using Microsoft.AspNetCore.Mvc;
using RentIt.Application.Bookings.GetBooking;
using RentIt.Application.Bookings.ReserveBooking;

namespace RentIt.Api.Controllers.Bookings;

[ApiController]
[Route("api/bookings")]
public class BookingsController : ControllerBase
{
    private readonly ISender m_Sender;

    public BookingsController(ISender sender)
    {
        m_Sender = sender;
    }

    [HttpGet("id")]
    public async Task<IActionResult> GetBooking(
        Guid id,
        CancellationToken cancellationToken)
    {
        var query = new GetBookingQuery(id);

        var result = await m_Sender.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : NotFound();

    }

    [HttpPost]
    public async Task<IActionResult> ReserveBooking(
        ReserveBookingRequest request,
        CancellationToken cancellationToken)
    {
        var command = new ReserveBookingCommand(
            request.ApartmentId,
            request.UserId,
            request.StartDate,
            request.EndDate);

        var result = await m_Sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return CreatedAtAction(nameof(GetBooking), new { id = result.Value }, result.Value);
    }


}
