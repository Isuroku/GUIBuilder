using System;
using System.Text;

namespace GUIBuilder
{
    public static class ArrayExt
    {
        public static bool Empty<T>(this T[] array) { return array == null || array.Length == 0; }
        public static bool Empty<T>(this T[,] array) { return array == null || array.Length == 0; }

        public static T FindCheck<T>(this T[] array, Predicate<T> match)
        {
            if (array.Empty())
                return default(T);

            return Array.Find(array, match);
        }

        public static bool ContainsCheck<T>(this T[] array, Predicate<T> match)
        {
            if (array.Empty())
                return false;

            for (int i = 0; i < array.Length; i++)
            {
                if (match(array[i]))
                    return true;
            }
            return false;
        }

        public static bool ContainsCheck<T>(this T[] array, T value)
        {
            if (array.Empty())
                return false;

            for (int i = 0; i < array.Length; i++)
            {
                if (array[i].Equals(value))
                    return true;
            }
            return false;
        }

        public static void Foreach<T>(this T[] array, Action<T> action)
        {
            if (array.Empty())
                return;

            for (int i = 0; i < array.Length; i++)
                action(array[i]);
        }

        public static string ToString<T>(this T[] array, string delimeter)
        {
            if (array.Empty())
                return string.Empty;

            var sb = new StringBuilder();
            for (int i = 0; i < array.Length; i++)
                if (i > 0)
                    sb.AppendFormat("{0}{1}", delimeter, array[i].ToString());
                else
                    sb.Append(array[i].ToString());

            return sb.ToString();
        }

        public static string ToString<T>(this T[,] array, string delimeter1, string delimeter2)
        {
            if (array.Empty())
                return string.Empty;

            var sb = new StringBuilder();
            for (int j = 0; j < array.GetLength(0); j++)
            {
                if (j > 0)
                    sb.AppendFormat("{0}", delimeter2);

                for (int i = 0; i < array.GetLength(1); i++)
                {
                    if (i > 0)
                        sb.AppendFormat("{0}{1}", delimeter1, array[j, i].ToString());
                    else
                        sb.Append(array[j, i].ToString());
                }
            }

            return sb.ToString();
        }

        public static T[] CreateLightCopy<T>(this T[] array)
        {
            if (array.Empty())
                return new T[0];

            T[] na = new T[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                na[i] = array[i];
            }

            return na;
        }

        public static int GetCheckedHashCode<T>(this T[] array)
        {
            if (array.Empty())
                return 0;
            int res = array[0].GetHashCode();
            for (int i = 1; i < array.Length; i++)
                res = res * 37 + array[i].GetHashCode();
            return res;
        }

        public static string Join(this string[] array, string separator = null)
        {
            if (string.IsNullOrEmpty(separator))
                separator = ", ";

            return string.Join(separator, array);
        }

        public static bool Any<T>(this T[] array, Predicate<T> match)
        {
            if (array.Empty())
                return false;

            for (int i = 0; i < array.Length; ++i)
            {
                if (match(array[i]))
                    return true;
            }
            return false;
        }

        public static bool All<T>(this T[] array, Predicate<T> match)
        {
            if (array.Empty())
                return true;

            for (int i = 0; i < array.Length; ++i)
            {
                if (!match(array[i]))
                    return false;
            }
            return true;
        }
    }
}
