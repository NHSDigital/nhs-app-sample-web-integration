using System;
using nhsapp.sample.web.integration.ViewModels;

namespace nhsapp.sample.web.integration.Models
{
    internal sealed class UserSessionData
    {
        public UserSessionData()
        {
            Id = new Guid();
        }

        public Guid Id { get; set; }
    }
}
