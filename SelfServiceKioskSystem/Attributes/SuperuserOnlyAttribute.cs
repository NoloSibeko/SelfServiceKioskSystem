using Microsoft.AspNetCore.Authorization;
using System;

namespace SelfServiceKioskSystem.Attributes
{
    public class SuperuserOnlyAttribute : AuthorizeAttribute
    {
        public SuperuserOnlyAttribute()
        {
            Roles = "Superuser";
        }
    }
}
