using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using SchoolAPI.Classes;

namespace SchoolAPI.Models;

public class Student : Human
{
    [Required]
    public School? School { get; set; }

}