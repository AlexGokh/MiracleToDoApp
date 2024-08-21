using Microsoft.AspNetCore.Identity;

namespace MiracleToDoApp.Models
{
    public class Step
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public bool isDone { get; set; }
        public virtual IdentityUser? User { get; set; }
        
    }
}
