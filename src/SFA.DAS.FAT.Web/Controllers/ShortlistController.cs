using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAT.Application.Shortlist.Commands.CreateShortlistItemForUser;
using SFA.DAS.FAT.Application.Shortlist.Queries.GetShortlistForUser;
using SFA.DAS.FAT.Domain.Configuration;
using SFA.DAS.FAT.Domain.Interfaces;
using SFA.DAS.FAT.Web.Infrastructure;
using SFA.DAS.FAT.Web.Models;

namespace SFA.DAS.FAT.Web.Controllers
{
    [Route("[controller]")]
    public class ShortlistController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ICookieStorageService<ShortlistCookieItem> _shortlistCookieService;
        private readonly ICookieStorageService<LocationCookieItem> _locationCookieService;

        public ShortlistController(IMediator mediator,
            ICookieStorageService<ShortlistCookieItem> shortlistCookieService,
            ICookieStorageService<LocationCookieItem> locationCookieService)
        {
            _mediator = mediator;
            _shortlistCookieService = shortlistCookieService;
            _locationCookieService = locationCookieService;
        }

        [HttpGet]
        [Route("", Name = RouteNames.ShortList)]
        public async Task<IActionResult> Index()
        {
            var cookie = _shortlistCookieService.Get(Constants.ShortlistCookieName);

            //todo: what if no cookie yet?

            var result =
                await _mediator.Send(
                    new GetShortlistForUserQuery {ShortlistUserId = cookie.ShortlistUserId});

            var viewModel = new ShortlistViewModel
            {
                Shortlist = result.Shortlist.Select(item => (ShortlistItemViewModel)item)
            };

            return View(viewModel);
        }

        [HttpPost]
        [Route("courses/{id}/providers/{providerId}")]
        public async Task<IActionResult> CreateShortlistItem(CreateShortListItemRequest request)
        {
            var cookie = _shortlistCookieService.Get(Constants.ShortlistCookieName);

            if (cookie == null)
            {
                cookie = new ShortlistCookieItem
                {
                    ShortlistUserId = Guid.NewGuid()
                };
                
                _shortlistCookieService.Create(cookie, Constants.ShortlistCookieName, 30);
            }

            var location = _locationCookieService.Get(Constants.LocationCookieName);

            await _mediator.Send(new CreateShortlistItemForUserCommand
            {
                Lat = location?.Lat,
                Lon = location?.Lon,
                Ukprn = request.Ukprn,
                LocationDescription = string.IsNullOrEmpty(location?.Name) ? null : location.Name,
                TrainingCode = request.TrainingCode,
                ShortlistUserId = cookie.ShortlistUserId,
                SectorSubjectArea = request.SectorSubjectArea
            });
            
            return Accepted();
        }
        
    }
}
