using System;
using System.Collections;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core
{
    public static class Extentions
    {
        public static bool IsNullOrEmpty(this Array array)
        {
            return array == null || array.Length == 0;
        }
        public static bool IsNullOrEmpty(this IList list)
        {
            return list == null || list.Count == 0;
        }
        public static bool IsNullOrEmpty(this ICollection collection)
        {
            return collection == null || collection.Count == 0;
        }

        public static T Clone<T>(this T prototype)
        {
            var data = JsonUtility.ToJson(prototype);
            try
            {
                var clonedItem = JsonUtility.FromJson<T>(data);
                return clonedItem;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return prototype;
            }
        }
    }
}