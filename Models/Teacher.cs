using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using SchoolAPI.Classes;

namespace SchoolAPI.Models;

public class Teacher : Human
{
    [Required]
    public School? School { get; set; }

    [JsonIgnore]
    public ICollection<Subject>? Subjects { get; set; }
    
    [JsonIgnore]
    public Class? Class { get; set; }

    public Teacher()
    {
        this.Subjects = new List<Subject>();
    }
}