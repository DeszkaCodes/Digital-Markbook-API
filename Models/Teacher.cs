using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using SchoolAPI.Classes;

namespace SchoolAPI.Models;

public class Teacher : Human
{
    [Required]
    public School? School { get; set; }

    [JsonIgnore]
    public List<Subject>? Subjects { get; set; }
}