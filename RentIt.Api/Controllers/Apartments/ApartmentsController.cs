using Bookify.Application.Apartments.SearchApartments;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace RentIt.Api.Controllers.Apartments;

[ApiController]
[Route("api/apartments")]
public class ApartmentsController : ControllerBase
{
    private readonly ISender m_Sender;

    public ApartmentsController(ISender sender)
    {
        m_Sender = sender;
    }

    [HttpGet]
    public async Task<IActionResult> SearchApartment(
        DateOnly startDate,
        DateOnly endDate,
        CancellationToken cancellationToken)
    {
        var query = new SearchApartmentsQuery(startDate,endDate);

        var result = await m_Sender.Send(query, cancellationToken);

        return Ok(result.Value);
    }
}
