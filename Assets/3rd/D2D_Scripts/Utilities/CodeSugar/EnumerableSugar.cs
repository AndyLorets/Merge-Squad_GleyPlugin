using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using D2D.Utilities;
using Sirenix.Utilities;
using UnityEngine;

namespace D2D.Utilities
{
    public static class EnumerableSugar
    {
        public static bool IsNullOrEmpty<T>(this T[] arr)
        {
            return arr == null || arr.Length == 0 || arr[0] == null;
        }
        
        public static bool IsNullOrEmpty<T>(this List<T> list)
        {
            return list == null || list.Count == 0 || list[0] == null;
        }
        
        /// <summary>
        /// Get the random element of an array.
        /// </summary>
        public static T GetRandomElement<T>(this T[] arr)
        {
	        if (arr.IsNullOrEmpty())
	        {
		        throw new Exception("You trying to get random element from null or empty array!");
	        }
	        
            return arr[DMath.Random(0, arr.Length-1)];
        }

        /// <summary>
        /// Get the random element of a list.
        /// </summary>
        public static T GetRandomElement<T>(this List<T> list)
        {
	        if (list.IsNullOrEmpty())
		        throw new Exception("You trying to get random element from null or empty list!");
	        
            return list[DMath.Random(0, list.Count-1)];
        }
        
        public static T GetRandomElement<T>(this IList<T> list)
        {
	        if (list.IsNullOrEmpty())
		        throw new Exception("You trying to get random element from null or empty list!");
	        
	        return list[DMath.Random(0, list.Count-1)];
        }
        
        /*public static List<T> Shuffle<T>(this List<T> list, int by)
        {
	        var result = new List<T>(list);
	        for (int i = 0; i < list.Count; i++)
	        {
		        int shuffleIndex = i + by;
		        if (shuffleIndex >= list.Count)
		        {
			        shuffleIndex %= list.Count;
		        }
		        result[i] = list[shuffleIndex];
	        }

	        return result;
        }*/

        public static List<T> Shuffle<T>(this IEnumerable<T> collection)
        {
	        var list = collection.ToList();
	        list.Sort((a, b) => 1 - 2 * DMath.Random(0, 1));
	        return list;
        }
        
        public static T[] Shuffle<T>(this T[] array)
        {
	        var list = array.ToList();
	        list.Sort((a, b) => 1 - 2 * DMath.Random(0, 1));
	        return list.ToArray();
        }

        
        /*public static List<T> Shuffle<T>(this List<T> list, int by)
		{
			var result = new List<T>(list);
			for (int i = 0; i < list.Count; i++)
			{
				int shuffleIndex = i + by;
				if (shuffleIndex >= list.Count)
				{
					shuffleIndex %= list.Count;
				}
				result[i] = list[shuffleIndex];
			}

			return result;
		}
        
        public static T[] Shuffle<T>(this T[] list, int by)
        {
	        var result = new List<T>(list);
	        for (int i = 0; i < list.Length; i++)
	        {
		        int shuffleIndex = i + by;
		        if (shuffleIndex >= list.Length)
		        {
			        shuffleIndex %= list.Length;
		        }
		        result[i] = list[shuffleIndex];
	        }

	        return result.ToArray();
        }*/

        //Foreach, no return
		public static IEnumerable<T> ForEach<T>(this IEnumerable<T> enumerable, Action<T> action) 
		{
			using (var enumerator = enumerable.GetEnumerator())
			{
				while (enumerator.MoveNext())
					action(enumerator.Current);
				return enumerable;
			}
		}

		/// <summary>
		/// Finds the index of a given object in an enumerable.
		/// </summary>
		public static int IndexOf<T>(this IEnumerable<T> enumerable, T instance) 
		{
			int i = 0;
			using (var enumerator = enumerable.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current != null && enumerator.Current.Equals(instance))
						return i;
					i++;
				}

				return -1;
			}
		}
    }
}
