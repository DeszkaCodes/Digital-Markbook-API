using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SchoolAPI.Models;

public class Class
{
    public Guid Id { get; set; }

    [Required]
    public string? Name { get; set; }
    
    [ForeignKey("Id")][Required]
    public Teacher? HeadMaster { get; set; }

    [Required]
    public School? School { get; set; }

    [JsonIgnore]
    public ICollection<Student>? Students { get; set; }

    [JsonIgnore]
    public ICollection<Subject>? Subjects { get; set; }
}