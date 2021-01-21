using Microsoft.AspNetCore.Identity;

namespace BuildingMaterialsStore.Models
{
    public class ApplicationUser: IdentityUser
    {
        public string FullName { get; set; }
    }
}