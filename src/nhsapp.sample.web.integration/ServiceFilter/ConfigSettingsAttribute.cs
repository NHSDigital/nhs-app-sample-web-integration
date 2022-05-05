namespace nhsapp.sample.web.integration.ServiceFilter
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.Configuration;
    using ViewModels;

    public class ConfigSettingsAttribute : IActionFilter
    {
        private readonly IConfiguration _configuration;

        public ConfigSettingsAttribute(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        void IActionFilter.OnActionExecuted(ActionExecutedContext context)
        {
        }

        void IActionFilter.OnActionExecuting(ActionExecutingContext context)
        {
            if (context.Controller is Controller controller)
            {
                AdobeAnalyticsViewModel AdobeAnalyticsData = new AdobeAnalyticsViewModel(context.HttpContext, _configuration);
                BreadcrumbViewModel BreadcrumbData = new BreadcrumbViewModel(new List<BreadcrumbLink>());

                controller.ViewBag.AdobeAnalytics = AdobeAnalyticsData;
                controller.ViewBag.Breadcrumbs = BreadcrumbData;
            }
        }
    }
}
