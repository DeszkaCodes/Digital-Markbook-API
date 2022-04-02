using System.Globalization;

namespace SchoolAPI.Classes;

public static class Extensions
{
    public static T SelectRandom<T>(this IEnumerable<T> array)
    {
        var rnd = new Random();

        return array.ElementAt(rnd.Next(0, array.Count()));
    }

    public static string ToTitleCase(this string title)
    {
        return CultureInfo.CreateSpecificCulture("en-US").TextInfo.ToTitleCase(title.ToLower());
    }
}