namespace BB.App.Github.Controllers
{
    using BB.App.Github.Constants;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : ControllerBase
    {
        [HttpGet("", Name = HomeControllerRoute.GetIndex)]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Index() => this.RedirectPermanent("/swagger");
    }
}
