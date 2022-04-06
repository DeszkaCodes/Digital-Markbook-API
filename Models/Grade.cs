using System.ComponentModel.DataAnnotations;

namespace SchoolAPI.Models;

public class Grade
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    public byte Value { get; set; }

    [Required]
    public Subject? Subject { get; set; }
    
    [Required]
    public Student? Student { get; set; }
}
