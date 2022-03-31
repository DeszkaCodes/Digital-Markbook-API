using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using SchoolAPI.Classes;

namespace SchoolAPI.Models;

public class School
{
    public Guid Id { get; set; }

    [Required]
    public string? Name { get; set; }

    [Required]
    public SchoolType Type { get; set; }
    
    [JsonIgnore]
    public ICollection<Student>? Students { get; set; }

    [JsonIgnore]
    public ICollection<Teacher>? Teachers { get; set; }
}