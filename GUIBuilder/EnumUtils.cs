using System;

namespace GUIBuilder
{
    public static class EnumUtils
    {
        public static T ToEnum<T>(string value)
        {
            if (string.IsNullOrEmpty(value) || !Enum.IsDefined(typeof(T), value))
                throw new ArgumentException(string.Format("{0} is not defined", value));

            return (T)Enum.Parse(typeof(T), value);
        }

        public static T ToEnum<T>(string value, T default_value)
        {
            if (string.IsNullOrEmpty(value))
                return default_value;

            foreach (T item in Enum.GetValues(typeof(T)))
            {
                if (string.Equals(value, item.ToString(), StringComparison.OrdinalIgnoreCase))
                    return item;
            }

            return default_value;
        }

        public static bool TryParse<T>(string svalue, out T out_value)
        {
            out_value = default(T);
            if (string.IsNullOrEmpty(svalue) || !Enum.IsDefined(typeof(T), svalue))
                return false;

            out_value = (T)Enum.Parse(typeof(T), svalue);
            return true;
        }

        public static void Foreach<T>(Action<T> action)
        {
            foreach (T item in Enum.GetValues(typeof(T)))
                action(item);
        }
    }
}
