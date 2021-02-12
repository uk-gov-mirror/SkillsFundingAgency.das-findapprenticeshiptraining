using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAT.Application.Shortlist.Queries.GetShortlistForUser;
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

        public ShortlistController(IMediator mediator, ICookieStorageService<ShortlistCookieItem> shortlistCookieService)
        {
            _mediator = mediator;
            _shortlistCookieService = shortlistCookieService;
        }

        [HttpGet]
        [Route("", Name = RouteNames.ShortList)]
        public async Task<IActionResult> Index()
        {
            var cookie = _shortlistCookieService.Get(nameof(ShortlistCookieItem));

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
    }
}
