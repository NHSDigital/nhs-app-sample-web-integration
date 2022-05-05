using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace nhsapp.sample.web.integration.ViewModels
{
    public class AdobeAnalyticsViewModel
    {
        public AdobeAnalyticsViewModel(HttpContext context, IConfiguration configuration)
        {
            ScriptUrl = configuration["AdobeAnalyticsScriptUrl"];

            string url = context.Request.PathBase + context.Request.Path;

            List<string> urlFragments =
                url?
                    .Trim()
                    .Split('/', StringSplitOptions.RemoveEmptyEntries)
                    .ToList()
                ?? new List<string>();

            PageName = "nhs:web:" + string.Join(":", urlFragments);

            Dictionary<string, string> categories = new Dictionary<string, string>
            {
                { "primaryCategory", urlFragments.Any() ? urlFragments[0] : "" }
            };

            var subCategoryUrlFragments = urlFragments.Skip(1).Select((f, i) => (f, i+1));
            foreach ((string urlFragment, int index) in subCategoryUrlFragments)
            {
                categories.Add($"subCategory{index}", urlFragment);
            }

            Categories = JsonConvert.SerializeObject(categories);
        }

        public string ScriptUrl { get; }

        public string PageName { get; }

        public string Categories { get; }
    }
}
