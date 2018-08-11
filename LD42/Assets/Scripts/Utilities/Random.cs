using System.Collections;
using System.Collections.Generic;

public static class RandomUtil
{

    static System.Random random = new System.Random();

    public static T choice<T>(ICollection<T> collection)
    {
        int index = random.Next(collection.Count);
        int count = 0;
        foreach (T elem in collection)
        {
            if (index == count++)
                return elem;
        }

        throw new System.Exception("Random choice called on empty collection");
    }
}
