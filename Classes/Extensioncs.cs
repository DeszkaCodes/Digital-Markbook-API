using System.Globalization;

namespace SchoolAPI.Classes;

public static class Extensions
{
    /// <summary>
    /// Selects the specified number of random elements from the start of a sequence.
    /// </summary>
    /// <typeparam name="T">Type of the IEnumerable</typeparam>
    /// <param name="array">The IEnumerable array</param>
    /// <param name="amount">The amount of requested random items</param>
    /// <returns>The given amount of random items from the array</returns>
    /// <exception cref="ArgumentException">If the given amount is more than the length of the array</exception>
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

        if(array.Count() < amount)
            throw new ArgumentException("Amount is greater than the IEnumerable<T> size");

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

    /// <summary>
    /// Selects a random element from the IEnumerable
    /// </summary>
    /// <typeparam name="T">The type of the IEnumerable</typeparam>
    /// <param name="array">The IEnumerable array</param>
    /// <returns>A random T element from the array</returns>
    /// <exception cref="ArgumentException">If the array is empty</exception>
    public static T SelectRandom<T>(this IEnumerable<T> array)
    {
        var rnd = new Random();

        if(array.Count() == 0)
            throw new ArgumentException("IEnumerable<T> length cannot be less than 1");

        if(array.Count() == 1)
            return array.ElementAt(0);

        return array.ElementAt(rnd.Next(0, array.Count() - 1));
    }

    /// <summary>
    /// Capitalizes the first letter of every word in the given string
    /// </summary>
    /// <param name="title">The string you want to change</param>
    /// <returns>Capitalized string</returns>
    public static string ToTitleCase(this string title)
    {
        return CultureInfo.CreateSpecificCulture("en-US").TextInfo.ToTitleCase(title.ToLower());
    }
}