using System.ComponentModel.DataAnnotations;

namespace SchoolAPI.Classes;

public class Human
{
    public Guid Id { get; set; }

    [Required]
    public string? Name { get; set; }

    [Required]
    public DateTime DateOfBirth { get; set; }
    public Gender Gender { get; set; } = Gender.NotGiven;
}