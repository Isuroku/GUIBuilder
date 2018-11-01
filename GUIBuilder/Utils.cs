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

        public static int FindChildIndex(this IKey key, IKey child, ILogPrinter inLogger)
        {
            for (int i = 0; i < key.GetChildCount(); i++)
            {
                if (key.GetChild(i) == child)
                    return i;
            }
            inLogger.LogError(string.Format("Can't FindChildIndex"));
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

        public static SWinKeyInfo[] GetWinKeyInfos(IKey inWinKeyParent, ILogPrinter inLogger)
        {
            int count = inWinKeyParent.GetChildCount();
            List<SWinKeyInfo> res = new List<SWinKeyInfo>(count);
            for (int i = 0; i < count; i++)
            {
                IKey window_key = inWinKeyParent.GetChild(i);

                var info = new SWinKeyInfo();

                info.WinKey = window_key;

                info.Name = Utils.GetWindowNameFromKey(window_key, i.ToString(), inLogger);

                string[] a = Utils.TryGetParamsBySubKeyName("Type", window_key, inLogger, true, 1);
                if (a.Length > 0)
                {
                    string stype = a[0];
                    EWindowType wt = EnumUtils.ToEnum(stype, EWindowType.Undefined);
                    if (wt != EWindowType.Undefined)
                    {
                        info.WinType = wt;
                        res.Add(info);
                    }
                    else
                        inLogger.LogError(string.Format("Undefined type. Key [{0}]!", window_key.GetPath()));
                }
            }
            return res.ToArray();
        }

        public static SWinKeyInfo FindMostSuitableKey(SWinKeyInfo[] inKeys, string inName, EWindowType inWindowType, ILogPrinter inLogger)
        {
            int min_dist = int.MaxValue;
            SWinKeyInfo res_key = new SWinKeyInfo();

            for (int i = 0; i < inKeys.Length; i++)
            {
                SWinKeyInfo window_key_info = inKeys[i];

                if (inWindowType == window_key_info.WinType)
                {
                    int d = LevenshteinDistance.GetDistance(window_key_info.Name, inName, 10);
                    if (d < min_dist)
                    {
                        min_dist = d;
                        res_key = window_key_info;
                    }
                }
            }

            return res_key;
        }

        public static CBaseWindow FindMostSuitableWindow(List<CBaseWindow> unused_windows_cache, SWinKeyInfo inKeyInfo)
        {
            int priority = int.MaxValue;
            CBaseWindow sel_window = null;

            for (int i = 0; i < unused_windows_cache.Count; i++)
            {
                CBaseWindow window = unused_windows_cache[i];

                if (window.WindowType == inKeyInfo.WinType)
                {
                    int dist = LevenshteinDistance.GetDistance(window.Name, inKeyInfo.Name);

                    if (dist < priority)
                    {
                        sel_window = window;
                        priority = dist;
                    }
                }
            }

            return sel_window;
        }
    }
}
