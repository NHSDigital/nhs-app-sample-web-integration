﻿using System;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace nhsapp.sample.web.integration.NhsLogin.Models
{
    [Serializable]
    public class ErrorResponse
    {
        public string Error { get; set; }

        [JsonProperty("error_description")]
        public string ErrorDescription { get; set; }

        [JsonProperty("error_uri")]
        [SuppressMessage("Microsoft.Design", "CA1056", Justification = "Uris are not serializable")]
        public string ErrorUrl { get; set; }
    }
}
