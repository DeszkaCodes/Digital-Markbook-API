using System.Globalization;

namespace SchoolAPI.Classes;

public static class Extensions
{
    public static IEnumerable<T> SelectManyRandom<T>(this IEnumerable<T> array, int amount)
    {
        bool Contains(T[] array, T item)
        {
            for(int i = 0; i < array.Length; i++)
            {
                if(EqualityComparer<T>.Default.Equals(array[i], item))
                    return true;
            }
            return false;
        }

        T[] items = new T[amount];

        for(int i = 0; i < amount; i++)
        {
            T item;
            bool contains = false;
            do
            {
                item = array.SelectRandom();

                if(!Contains(items, item))
                    contains = false;

            } while(contains);
        }

        return items;
    }

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