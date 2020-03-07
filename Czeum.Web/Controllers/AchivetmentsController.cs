using Czeum.Web.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Czeum.Web.Controllers
{
    [Route(ApiResources.Achivements.BasePath)]
    [ApiController]
    [Authorize]
    public class AchivetmentsController : ControllerBase
    {
    }
}
