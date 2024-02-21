using Microsoft.AspNetCore.Identity;

namespace Cinema.Web.Models.Tables
{
    public class Employee : IdentityUser
    {
        public String Name { get; set; } = null!;
    }
}
