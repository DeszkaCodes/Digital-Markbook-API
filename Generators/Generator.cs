using System.Security.AccessControl;
using SchoolAPI.Models;
using SchoolAPI.Classes;
using SchoolAPI.Data;
using System.Diagnostics;

namespace SchoolAPI.Generators;

public static class Generator
{
    private static School[] _schools = new School[0];
    private static Teacher[] _teachers = new Teacher[0];
    private static SchoolClass[] _classes = new SchoolClass[0];
    private static Subject[] _subjects = new Subject[0];
    private static Student[] _students = new Student[0];
    private static Mark[] _marks = new Mark[0];


    public static void StartGenerating(SchoolContext context, int studentAmount, int teacherAmount, int classAmount, int subjectAmount)
    {
        if(studentAmount < 30)
            throw new ArgumentOutOfRangeException("Student amount can't be more than 30 (school amount)");

        if(teacherAmount < 30)
            throw new ArgumentOutOfRangeException("Teacher amount can't be more than 30 (school amount)");

        if(classAmount < 30)
            throw new ArgumentOutOfRangeException("Class amount can't be more than 30 (school amount)");

        if(subjectAmount < 30)
            throw new ArgumentOutOfRangeException("Subject amount can't be more than 30 (school amount)");

        _schools = Schools();

        int studentsInEachSchool = studentAmount / _schools.Length;
        int teachersInEachSchool = teacherAmount / _schools.Length;
        int classesInEachSchool = classAmount / _schools.Length;
        int subjectsInEachSchool = subjectAmount / _schools.Length;

        if(classesInEachSchool > teachersInEachSchool)
            throw new ArgumentOutOfRangeException("Classes amount can't be more than teachers amount");

        if(subjectsInEachSchool > classesInEachSchool)
            throw new ArgumentOutOfRangeException("Subjects amount can't be more than classes amount");

        _teachers = Teacher(_schools, teachersInEachSchool);
        _classes = Class(_schools, _teachers, classesInEachSchool);
        _subjects = Subject(_schools, _teachers, _classes, subjectsInEachSchool);
        _students = Student(_schools, _classes, studentsInEachSchool);
        _marks = Mark(_schools, _subjects, _students, _classes, 5);

        context.Schools.AddRange(_schools);
        context.Teachers.AddRange(_teachers);
        context.Classes.AddRange(_classes);
        context.Marks.AddRange(_marks);
        context.Subjects.AddRange(_subjects);
        context.Students.AddRange(_students);

        context.SaveChanges();
    }

    private static Mark[] Mark(
        School[] schools, Subject[] subjects, Student[] students, SchoolClass[] classes, int marksPerSubject)
    {
        var marks = new List<Mark>();
        var rnd = new Random();

        for(var sch = 0; sch < subjects.Length; sch++)
        {
            var subject = subjects[sch];
            var classInSubject = classes.First(c => c == subject.Class);

            var studentsInSubject = students.Where(s => s.Class == classInSubject).ToArray();

            for(var stud = 0; stud < studentsInSubject.Length; stud++)
            {
                var student = studentsInSubject[stud];
                for(int i = 0; i < marksPerSubject; i++)
                {
                    var mark = new Mark
                    {
                        Id = Guid.NewGuid(),
                        Student = student,
                        Subject = subject,
                        Value = (byte)rnd.Next(1, 6)
                    };

                    marks.Add(mark);

                    student.Marks?.Add(mark);
                    subject.Marks?.Add(mark);
                }
            }
        }

        return marks.ToArray();
    }

    private static School[] Schools()
    {
        var names = File.ReadAllLines(@"./Generators/schoolname.txt");
        var schools = new School[names.Length];

        var rnd = new Random();

        for(int i = 0; i < schools.Length; i++)
        {
            var id = Guid.NewGuid();
            var name = names[i];
            var type = (SchoolType)rnd.Next(0, 5);

            schools[i] = new School()
            {
                Id= id,
                Type = type,
                Name = name
            };
        }

        return schools;
    }

    private static Teacher[] Teacher(School[] schools, int teacherAmount)
    {
        var teachers = new List<Teacher>();

        var rnd = new Random();

        for(var sch = 0; sch < schools.Length; sch++)
        {
            var school = schools[sch];
            for(int i = 0; i < teacherAmount; i++)
            {
                var id = Guid.NewGuid();
                var gender = (Gender)rnd.Next(0, 2);
                var name = GetName(gender);
                var birthdate = GetBetweenAge(24, 60);

                var teacher = new Teacher()
                {
                    Id = id,
                    Name = name,
                    Gender = gender,
                    DateOfBirth = birthdate,
                    School = school
                };

                teachers.Add(teacher);

                school.Teachers?.Add(teacher);
            }
        }
        return teachers.ToArray();
    }

    private static SchoolClass[] Class(School[] schools, Teacher[] teachers, int classAmount)
    {
        var classes = new List<SchoolClass>();

        var rnd = new Random();

        for(var sch = 0; sch < schools.Length; sch++)
        {
            var school = schools[sch];
            var teachersInSchool = teachers.Where(t => t.School == school).ToList();
            
            for(int i = 0; i < classAmount; i++)
            {
                var id = Guid.NewGuid();
                var name = rnd.Next(1, 10).ToString() + " class";
                var teacher = teachersInSchool[0];
                teachersInSchool.RemoveAt(0);

                var lclass = new SchoolClass()
                {
                    Id = id,
                    Name = name,
                    HeadMaster = teacher,
                    School = school
                };

                classes.Add(lclass);

                teacher.Class = lclass;
            }
        }

        return classes.ToArray();
    }

    private static Subject[] Subject(School[] schools, Teacher[] teachers, SchoolClass[] classes, int subjectAmount)
    {
        var subjects = new List<Subject>();

        var rnd = new Random();

        for(var sch = 0; sch < schools.Length; sch++)
        {
            var school = schools[sch];
            var localClasses = classes.Where(c => c.School == school).ToArray();

            for(int i = 0; i < subjectAmount; i++)
            {
                var id = Guid.NewGuid();
                var name = File.ReadAllLines(@"./Generators/subjectname.txt").SelectRandom() + $" {localClasses[i].Name}";
                var forClass = localClasses[i];
                var type = (SubjectType)rnd.Next(0, 1);
                var teaching = teachers.Where(t => t.School?.Id == school.Id).ToArray().SelectManyRandom(rnd.Next(1, 3));

                var subject = new Subject()
                {
                    Id = id,
                    Name = name,
                    Type = type,
                    Class = forClass,
                    Teachers = teaching.ToList()
                };

                subjects.Add(subject);

                forClass.Subjects?.Add(subject);
                Array.ForEach(teaching.ToArray(), t => t.Subjects?.Add(subject));
                school.Subjects?.Add(subject);
            }
        }

        return subjects.ToArray();
    }

    private static Student[] Student(School[] schools, SchoolClass[] classes, int studentAmount)
    {
        var students = new List<Student>();

        var rnd = new Random();

        for(var sch = 0; sch < schools.Length; sch++)
        {
            var school = schools[sch];
            var localClasses = classes.Where(c => c.School == school).ToArray();

            for(int i = 0; i < studentAmount; i++)
            {
                var id = Guid.NewGuid();
                var gender = (Gender)rnd.Next(0, 2);
                var name = GetName(gender);
                var birthdate = GetBetweenAge(7, 20);
                var lclass = localClasses.SelectRandom();

                var student = new Student()
                {
                    Id = id,
                    Name = name,
                    DateOfBirth = birthdate,
                    Gender = gender,
                    School = school,
                    Class = lclass
                };

                students.Add(student);

                school.Students?.Add(student);
                lclass.Students?.Add(student);
            }
        }

        return students.ToArray();
    }

    private static string GetName(Gender gender)
    {
        string firstname = gender switch
        {
            Gender.Male => File.ReadLines(@"./Generators/malename.txt").SelectRandom(),
            Gender.Female => File.ReadLines(@"./Generators/femalename.txt").SelectRandom(),
            Gender.NotGiven => File.ReadLines(@"./Generators/malename.txt").SelectRandom(),
            _ => "Névtelen"
        };

        string lastname = File.ReadAllLines(@"./Generators/surname.txt").SelectRandom();

        return lastname + firstname;
    }

    private static DateTime GetBetweenAge(int minAge, int maxAge)
    {
        var rnd = new Random();
        var age = rnd.Next(minAge, maxAge);

        var today = DateTime.Today;
        var birthDate = today.AddYears(-age);

        return birthDate;
    }

    //private static void Subject(int subjectAmount)
    //{
    //    var names = File.ReadAllLines("./Generators/classnames.txt");

    //    CurrentSubjects = new Subject[subjectAmount];

    //    var rnd = new Random();

    //    for(int i = 0; i < subjectAmount; i++)
    //    {
    //        var name = names.SelectRandom();

    //        var type = (SubjectType)rnd.Next(0, 2);

    //        var school = CurrentSchools.SelectRandom();

    //        var classes = CurrentClasses.Where(c => c.School?.Id == school.Id).SelectRandom();

    //        Teacher[] teachers = CurrentTeachers.Where(t => t.School?.Id == school.Id)
    //            .SelectManyRandom(rnd.Next(0, CurrentTeachers.Count(c => c.School?.Id == school.Id))).ToArray();

    //        CurrentSubjects[i] = new Subject
    //        {
    //            Id = Guid.NewGuid(),
    //            Name = name,
    //            Type = type,
    //            Class = classes,
    //            Teachers = teachers
    //        };
    //    }
    //}

    //private static void Class(int classAmount)
    //{
    //    CurrentClasses = new Class[classAmount];

    //    var rnd = new Random();

    //    for(int i = 0; i < classAmount; i++)
    //    {
    //        Teacher master;
    //        var isUniqueMaster = false;
    //        do
    //        {
    //            master = CurrentTeachers.SelectRandom();

    //            if(!CurrentClasses.Any(c => c?.HeadMaster?.Id == master.Id))
    //                isUniqueMaster = true;

    //        } while(!isUniqueMaster);

    //        var className = rnd.Next(1, 12).ToString();
    //        var school = CurrentSchools.SelectRandom();

    //        CurrentClasses[i] = new Class
    //        {
    //            Id = Guid.NewGuid(),
    //            Name = className,
    //            HeadMaster = master,
    //            School = school
    //        };
    //    }
    //}

    //public static void Schools()
    //{
    //    var names = File.ReadAllLines("./Generators/schoolname.txt");

    //    CurrentSchools = new School[names.Length];

    //    for (int i = 0; i < CurrentSchools.Length; i++)
    //    {
    //        var values = Enum.GetValues(typeof(SchoolType));
    //        var rnd = new Random();
    //        #pragma warning disable CS8605
    //        var type = (SchoolType)values.GetValue(rnd.Next(values.Length));
    //        #pragma warning restore CS8605


    //        CurrentSchools[i] = new School()
    //        {
    //            Id = Guid.NewGuid(),
    //            Name = names[i],
    //            Type = type
    //        };
    //    }
    //}

    //private static void Student(int amount)
    //{
    //    CurrentStudents = new Student[amount];

    //    var rnd = new Random();

    //    for(int i = 0; i < amount; i++)
    //    {
    //        var id = Guid.NewGuid();

    //        var isMale = rnd.Next(0, 2) == 0 ? false : true;

    //        var name = String.Empty;

    //        if(isMale)
    //        {
    //            var datas = File.ReadAllLines("./Generators/malename.txt");

    //            name = datas[rnd.Next(0, datas.Length)];
    //        }
    //        else
    //        {
    //            var datas = File.ReadAllLines("./Generators/femalename.txt");

    //            name = datas[rnd.Next(0, datas.Length)];
    //        }

    //        var surnames = File.ReadAllLines("./Generators/surname.txt");
    //        name += $" {surnames[rnd.Next(0, surnames.Length)]}";

    //        var year = DateTime.Now.Year;

    //        var birthdate = new DateTime(year - rnd.Next(18,22), rnd.Next(1,13), rnd.Next(1,28));

    //        var school = CurrentSchools?.SelectRandom();
    //        var classIn = CurrentClasses?.Where(c => c.School.Id == school?.Id).SelectRandom();

    //        CurrentStudents[i] = new Student()
    //            {
    //                Id = id,
    //                Name = name,
    //                Gender = isMale ? Gender.Male : Gender.Female,
    //                DateOfBirth = birthdate,
    //                School = CurrentSchools?.SelectRandom(),
    //                Class = classIn
    //            };
    //    }
    //}

    //private static void Teacher(int amount)
    //{
    //    CurrentTeachers = new Teacher[amount];

    //    var rnd = new Random();

    //    for(int i = 0; i < amount; i++)
    //    {
    //        var id = Guid.NewGuid();

    //        var isMale = rnd.Next(0, 2) == 0 ? false : true;

    //        var name = String.Empty;

    //        if(isMale)
    //        {
    //            var datas = File.ReadAllLines("./Generators/malename.txt");

    //            name = datas[rnd.Next(0, datas.Length)];
    //        }
    //        else
    //        {
    //            var datas = File.ReadAllLines("./Generators/femalename.txt");

    //            name = datas[rnd.Next(0, datas.Length)];
    //        }

    //        var surnames = File.ReadAllLines("./Generators/surname.txt");
    //        name += $" {surnames[rnd.Next(0, surnames.Length)]}";

    //        var year = DateTime.Now.Year;

    //        var birthdate = new DateTime(year - rnd.Next(26,60), rnd.Next(1,13), rnd.Next(1,28));

    //        CurrentTeachers[i] = new Teacher()
    //            {
    //                Id = id,
    //                Name = name,
    //                Gender = isMale ? Gender.Male : Gender.Female,
    //                DateOfBirth = birthdate,
    //                School = CurrentSchools?.SelectRandom()
    //            };
    //    }
    //}
}