using System.ComponentModel.DataAnnotations;

namespace SchoolAPI.Classes;

public class Human
{
    public Guid Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public DateTime DateOfBirth { get; set; }
    public Gender Gender { get; set; }

    public Human(Guid id, string name, DateTime dateofbirth, Gender gender = Gender.NotGiven)
    {
        Id = id;
        Name = name;
        DateOfBirth = dateofbirth;
        Gender = gender;
    }
}