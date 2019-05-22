using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Praxio.Tools.Arquitetura.Api.Controllers
{
    /// <summary/>
    [AllowAnonymous]
    public class HomeController : ControllerBase
    {
        /// <summary/>
        public ActionResult Index() => new RedirectResult("~/swagger");
    }
}