using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using nhsapp.sample.web.integration.Extensions;
using nhsapp.sample.web.integration.ViewModels;
using nhsapp.sample.web.integration.NhsLogin;
using nhsapp.sample.web.integration.NhsLogin.Models;

namespace nhsapp.sample.web.integration.Controllers.Login
{
    public class LoginController : Controller
    {
        private readonly INhsLoginService _nhsLoginService;
        private readonly IAppWebConfiguration _webConfiguration;
        private readonly INhsLoginJwtHelper _nhsLoginJwtHelper;

        public LoginController(
            INhsLoginService nhsLoginService,
            IAppWebConfiguration webConfiguration,
            INhsLoginJwtHelper nhsLoginJwtHelper
            )
        {
            _nhsLoginService = nhsLoginService;
            _webConfiguration = webConfiguration;
            _nhsLoginJwtHelper = nhsLoginJwtHelper;
        }

        [Route("auth-return")]
        [HttpGet]
        public async Task<IActionResult> AuthReturn(AuthReturnResponse authReturnResponse)
        {
            if (!string.IsNullOrWhiteSpace(authReturnResponse.Error))
            {
                if (authReturnResponse.ErrorDescription.Equals("ConsentNotGiven"))
                {
                    return RedirectToAction("TermsAndConditions");
                }

                var nhsLoginErrorViewModel = new NhsLoginErrorViewModel()
                {
                    Error = authReturnResponse.Error,
                    ErrorDescription = authReturnResponse.ErrorDescription
                };
                return View("LoginError",nhsLoginErrorViewModel);
            }

            var redirectUri = _webConfiguration.BaseAddress;
            var userProfileResult = await _nhsLoginService.GetUserProfile(authReturnResponse.Code, redirectUri);

            var userProfileOption = userProfileResult.UserProfile;

            var userProfile = userProfileOption.ValueOrFailure();

            var userInfo = new UserInfoViewModel()
            {
                NhsNumber = userProfile.NhsNumber,
                Birthdate = userProfile.DateOfBirth,
                FamilyName = userProfile.FamilyName,
                GivenName = userProfile.GivenName,
                IdTokenJti = userProfileResult.IdTokenJti,
            };
            TempData.Set(userInfo);
            return RedirectToAction("Profile");
        }

        [Route("Patient")]
        [HttpGet]
        public IActionResult Profile()
        {
            var userInfo = TempData.Get<UserInfoViewModel>();

            return View(userInfo);
        }

        [Route("TermsAndConditions")]
        [HttpGet]
        public IActionResult TermsAndConditions()
        {
            return View();
        }

        [Route("GenerateSso")]
        [HttpPost]
        public IActionResult GenerateSso()
        {
            var userInfo = TempData.Get<UserInfoViewModel>();

            var token = _nhsLoginJwtHelper.CreateAssertedLoginIdentityJwt(userInfo.IdTokenJti);

            userInfo.SsoToken = token;
            TempData.Set(userInfo);

            return RedirectToAction("Profile");
        }

        [Route("Logout")]
        [HttpPost]
        public ActionResult Logout()
        {
            TempData.Clear();
            return RedirectToAction("Index", "Home", new { area = "" });
        }
    }
}