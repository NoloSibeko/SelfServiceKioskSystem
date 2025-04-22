using Microsoft.AspNetCore.Authorization;
using System;

namespace SelfServiceKioskSystem.Attributes
{
    public class SuperuserOnlyAttribute : AuthorizeAttribute
    {
        public SuperuserOnlyAttribute()
        {
            // Require superuser role for this action
            Roles = "Superuser";
        }
    }
}
