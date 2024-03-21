using Microsoft.AspNetCore.Identity;

namespace Cinema.Data.Models.Tables
{
    public class Employee : IdentityUser
    {
        public String Name { get; set; } = null!;
    }
}
