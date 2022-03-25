using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using SchoolAPI.Classes;

namespace SchoolAPI.Models;

public class Teacher : Human
{
    [Required]
    public School? School { get; set; }
}