namespace SchoolAPI.Classes;

public static class Extensions
{
    public static T SelectRandom<T>(this T[] array)
    {
        var rnd = new Random();

        return array[rnd.Next(0, array.Length)];
    }
}