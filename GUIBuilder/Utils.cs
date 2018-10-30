using CascadeParser;
using System;
using System.Collections.Generic;
using System.Text;

namespace GUIBuilder
{
    internal static class Utils
    {
        public static IKey FindChildByName(this IKey key, string inName, StringComparison comparisonType = StringComparison.InvariantCulture)
        {
            for(int i = 0; i < key.GetChildCount(); i++)
            {
                if (string.Equals(key.GetChild(i).GetName(), inName, comparisonType))
                    return key.GetChild(i);
            }
            return null;
        }

        public static void CopyChilds(this IKey key, ICollection<IKey> outList)
        {
            for (int i = 0; i < key.GetChildCount(); i++)
                outList.Add(key.GetChild(i));
        }

        public static int FindChildIndex(this IKey key, IKey child)
        {
            for (int i = 0; i < key.GetChildCount(); i++)
            {
                if (key.GetChild(i) == child)
                    return i;
            }
            return -1;
        }

        public static string[] TryGetParamsBySubKeyName(string inKeyName, IKey inKey, ILogPrinter inLogger, bool inSubKeyMustBe, params int[] inParamCounts)
        {
            IKey sub_key = inKey.FindChildByName(inKeyName, StringComparison.InvariantCultureIgnoreCase);
            if(sub_key == null)
            {
                if(inSubKeyMustBe)
                    inLogger.LogError(string.Format("Can't find subkey by name. Key [{0}] must have subkey {1}!", inKey.GetPath(), inKeyName));
                return new string[0];
            }

            int value_count = sub_key.GetValuesCount();
            if (!inParamCounts.ContainsCheck(value_count))
            {
                string counts = inParamCounts.ToString(", ");
                inLogger.LogError(string.Format("Subkey [{0}] must have [{1}] counts of values, but was found {2}", sub_key.GetPath(), counts, value_count));
            }

            var a = new string[value_count];
            for (int i = 0; i < value_count; i++)
                a[i] = sub_key.GetValueAsString(i);

            return a;
        }

        public static string GetWindowNameFromKey(IKey inKey, string inDefaultName, ILogPrinter inLogger)
        {
            string[] a = Utils.TryGetParamsBySubKeyName("Name", inKey, inLogger, false, 1);
            return a.Empty() ? inDefaultName : a[0];
        }
    }
}
