
using System;
using System.Collections.Generic;
using Sirenix.Utilities;

namespace SRF
{
    public static class SRFIListExtensions
    {
        public static T Random<T>(this IList<T> list)
        {
            if (list.Count == 0)
            {
                throw new IndexOutOfRangeException("List needs at least one entry to call Random()");
            }

            if (list.Count == 1)
            {
                return list[0];
            }

            return list.GetRandomElement();
        }
        
        public static T GetRandomElement<T>(this IList<T> list)
        {
	        if (list.IsNullOrEmpty())
		        throw new Exception("You trying to get random element from null or empty list!");
	        
	        return list[Random(0, list.Count-1)];
        }
        
        public static int Random(int min, int max)
        {
            float randomValue = UnityEngine.Random.Range((float) min, (float) max);
            return (int) Math.Round(randomValue, 0);
        }

        public static T RandomOrDefault<T>(this IList<T> list)
        {
            if (list.Count == 0)
            {
                return default(T);
            }

            return list.Random();
        }

        public static T PopLast<T>(this IList<T> list)
        {
            if (list.Count == 0)
            {
                throw new InvalidOperationException();
            }

            var t = list[list.Count - 1];

            list.RemoveAt(list.Count - 1);

            return t;
        }
    }
}
