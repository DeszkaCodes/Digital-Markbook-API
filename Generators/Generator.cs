using System.Security.AccessControl;
using SchoolAPI.Models;
using SchoolAPI.Classes;
using SchoolAPI.Data;

namespace SchoolAPI.Generators;

public static class Generator
{
    public static School[] CurrentSchools { get; set; } = new School[0];
    public static Student[] CurrentStudents { get; set; } = new Student[0];

    public static Teacher[] CurrentTeachers { get; set; } = new Teacher[0];
    public static Subject[] CurrentSubjects { get; set; } = new Subject[0];

    public static void StartGenerating(SchoolContext context, int studentAmount, int teacherAmount)
    {
        Schools();
        Student(studentAmount);
        Teacher(teacherAmount);

        context.Schools
            .AddRange(CurrentSchools);

        context.Students
            .AddRange(CurrentStudents);

        context.Teachers
            .AddRange(CurrentTeachers);

        context.SaveChanges();
    }

    public static void Schools()
    {
        var names = File.ReadAllLines("./Generators/schoolname.txt");

        CurrentSchools = new School[names.Length];

        for (int i = 0; i < CurrentSchools.Length; i++)
        {
            var values = Enum.GetValues(typeof(SchoolType));
            var rnd = new Random();
            #pragma warning disable CS8605
            var type = (SchoolType)values.GetValue(rnd.Next(values.Length));
            #pragma warning restore CS8605


            CurrentSchools[i] = new School()
            {
                Id = Guid.NewGuid(),
                Name = names[i],
                Type = type
            };
        }
    }
    private static void Student(int amount)
    {
        CurrentStudents = new Student[amount];

        var rnd = new Random();

        for(int i = 0; i < amount; i++)
        {
            var id = Guid.NewGuid();

            var isMale = rnd.Next(0, 2) == 0 ? false : true;

            var name = String.Empty;

            if(isMale)
            {
                var datas = File.ReadAllLines("./Generators/malename.txt");

                name = datas[rnd.Next(0, datas.Length)];
            }
            else
            {
                var datas = File.ReadAllLines("./Generators/femalename.txt");

                name = datas[rnd.Next(0, datas.Length)];
            }

            var surnames = File.ReadAllLines("./Generators/surname.txt");
            name += $" {surnames[rnd.Next(0, surnames.Length)]}";

            var year = DateTime.Now.Year;

            var birthdate = new DateTime(year - rnd.Next(18,22), rnd.Next(1,13), rnd.Next(1,28));

            CurrentStudents[i] = new Student()
                {
                    Id = id,
                    Name = name,
                    Gender = isMale ? Gender.Male : Gender.Female,
                    DateOfBirth = birthdate,
                    School = CurrentSchools?.SelectRandom()
                };
        }
    }

    private static void Teacher(int amount)
    {
        CurrentTeachers = new Teacher[amount];

        var rnd = new Random();

        for(int i = 0; i < amount; i++)
        {
            var id = Guid.NewGuid();

            var isMale = rnd.Next(0, 2) == 0 ? false : true;

            var name = String.Empty;

            if(isMale)
            {
                var datas = File.ReadAllLines("./Generators/malename.txt");

                name = datas[rnd.Next(0, datas.Length)];
            }
            else
            {
                var datas = File.ReadAllLines("./Generators/femalename.txt");

                name = datas[rnd.Next(0, datas.Length)];
            }

            var surnames = File.ReadAllLines("./Generators/surname.txt");
            name += $" {surnames[rnd.Next(0, surnames.Length)]}";

            var year = DateTime.Now.Year;

            var birthdate = new DateTime(year - rnd.Next(26,60), rnd.Next(1,13), rnd.Next(1,28));

            CurrentTeachers[i] = new Teacher()
                {
                    Id = id,
                    Name = name,
                    Gender = isMale ? Gender.Male : Gender.Female,
                    DateOfBirth = birthdate,
                    School = CurrentSchools?.SelectRandom()
                };
        }
    }
}