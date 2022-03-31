using System.ComponentModel.DataAnnotations;

namespace SchoolAPI.Models;

public class Mark
{
    public Guid Id { get; set; }
    
    [Required]
    public Subject? Subject { get; set; }
    
    [Required]
    public Student? Student { get; set; }
}
