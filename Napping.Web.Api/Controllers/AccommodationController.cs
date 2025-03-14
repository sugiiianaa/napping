using MediatR;
using Microsoft.AspNetCore.Mvc;
using Napping.Application.Features.Accommodation.Commands.AddAccommodation;
using Napping.Web.Api.ViewModels.Accommodation.AddAccommodation;

namespace Napping.Web.Api.Controllers
{
    [ApiController]
    [Route("api/v1/accommodation")]
    public class AccommodationController : ControllerBase
    {
        private readonly ISender _sender;

        public AccommodationController(ISender sender)
        {
            _sender = sender;
        }


        [HttpPost("add-accommodation")]
        public async Task<IActionResult> AddAccommodation(AddAccommodationRequestViewModel request)
        {
            var command = new AddAccommodationCommand(
                Name: request.Name,
                Latitude: request.Latitude,
                Longitude: request.Longitude,
                Address: request.Address,
                Rating: request.Rating);

            var accommodationId = await _sender.Send(command);

            return NoContent();
        }
    }
}
