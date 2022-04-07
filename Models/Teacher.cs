using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using SchoolAPI.Classes;

namespace SchoolAPI.Models;

public class Teacher : Human
{
    [NotMapped]
    public const int MinimumAge = 25;
    [NotMapped]
    public const int MaximumAge = 60;

    [Required]
    public School? School { get; set; }

    [JsonIgnore]
    public ICollection<Subject>? Subjects { get; set; }
    
    [JsonIgnore]
    public SchoolClass? Class { get; set; }

    public Teacher()
    {
        this.Subjects = new List<Subject>();
    }
}