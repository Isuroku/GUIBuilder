using CascadeParser;
using System.Collections.Generic;
using System.Text;

namespace GUIBuilder
{
    internal static class Utils
    {
        public static IKey FindChildByName(this IKey key, string inName)
        {
            for(int i = 0; i < key.GetChildCount(); i++)
            {
                if (string.Equals(key.GetChild(i).GetName(), inName))
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
    }
}
