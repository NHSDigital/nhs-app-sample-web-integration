using Microsoft.AspNetCore.Mvc;
using nhsapp.sample.web.integration.NhsLogin;
using nhsapp.sample.web.integration.ServiceFilter;

namespace nhsapp.sample.web.integration.Controllers
{
    [Route("Api")]
    [ServiceFilter(typeof(ConfigSettingsAttribute))]
    public class ApiController : Controller
    {
        private readonly INhsLoginUriService _nhsLoginUriService;

        public ApiController(INhsLoginUriService loginUriService)
        {
            _nhsLoginUriService = loginUriService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [Route("addToCalendar")]
        [HttpGet]
        public ActionResult AddToCalendar()
        {
            return View();
        }

        [Route("downloadFromBytes")]
        [HttpGet]
        public ActionResult DownloadFromBytes()
        {
            return View();
        }

        [Route("goToPage")]
        [HttpGet]
        public ActionResult GoToPage()
        {
            return View();
        }

        [Route("isNativeApp")]
        [HttpGet]
        public ActionResult IsNativeApp()
        {
            return View();
        }

        [Route("openBrowserOverlay")]
        [HttpGet]
        public ActionResult OpenBrowserOverlay()
        {
            return View();
        }

        [Route("showHideHeaderFooter")]
        [HttpGet]
        public ActionResult ShowHideHeaderFooter()
        {
            return View();
        }

    }
}
