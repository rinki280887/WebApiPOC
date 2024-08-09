using Microsoft.AspNetCore.Identity;

namespace WebApiPOC.DataBaseModel
{
    public class UserRole : IdentityRole
    {
        public string Descreption { get; set; }
    }
}
