using System.Globalization;

namespace SchoolAPI.Classes;

public static class Extensions
{
    public static T SelectRandom<T>(this T[] array)
    {
        var rnd = new Random();

        return array[rnd.Next(0, array.Length)];
    }

    public static string ToTitleCase(this string title)
    {
        return CultureInfo.CreateSpecificCulture("en-US").TextInfo.ToTitleCase(title.ToLower());
    }
}