using Microsoft.AspNetCore.Mvc;
using nhsapp.sample.web.integration.Extensions;
using nhsapp.sample.web.integration.NhsLogin;
using nhsapp.sample.web.integration.ServiceFilter;
using nhsapp.sample.web.integration.ViewModels;

namespace nhsapp.sample.web.integration.Controllers
{
    [ServiceFilter(typeof(ConfigSettingsAttribute))]
    public class HomeController : Controller
    {
        private readonly INhsLoginUriService _nhsLoginUriService;

        public HomeController(INhsLoginUriService loginUriService)
        {
            _nhsLoginUriService = loginUriService;
        }

        [Route("")]
        [HttpGet]
        public IActionResult Index()
        {
            if (TempData.Get<UserInfoViewModel>() is null)
            {
                return View();
            }
            return RedirectToAction("Profile", "Login", new { area = "" });
        }

        [Route("sso")]
        [HttpGet]
        public IActionResult Sso(string assertedLoginIdentity)
        {
            if (string.IsNullOrEmpty(assertedLoginIdentity))
            {
                return BadRequest();
            }

            var redirectUri = _nhsLoginUriService.BuildSsoLoginUri(assertedLoginIdentity);
            return new RedirectResult(redirectUri);
        }

        [HttpPost]
        [Route("")]
        public IActionResult Login()
        {
            return new RedirectResult(_nhsLoginUriService.BuildLoginUri());
        }
    }
}
