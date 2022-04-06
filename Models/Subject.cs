using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using SchoolAPI.Classes;

namespace SchoolAPI.Models;

public class Subject
{
    public Guid Id { get; set; }
    
    [Required]
    public string? Name { get; set; }
    
    [Required]
    public SubjectType Type { get; set; }

    [Required]
    public SchoolClass? Class { get; set; }
    
    public ICollection<Teacher>? Teachers { get; set; }

    [JsonIgnore]
    public ICollection<Mark>? Marks { get; set; }

    public Subject()
    {
        Teachers = new List<Teacher>();
        Marks = new List<Mark>();
    }
}