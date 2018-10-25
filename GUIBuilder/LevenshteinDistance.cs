using System;

namespace GUIBuilder
{
    public static class LevenshteinDistance
    {
        //public static int GetDistance(string string1, string string2)
        //{
        //    if (string1 == null) throw new ArgumentNullException("string1");
        //    if (string2 == null) throw new ArgumentNullException("string2");

        //    if (string2.Contains(string1)) return 0;

        //    int diff;
        //    int[,] m = new int[string1.Length + 1, string2.Length + 1];

        //    for (int i = 0; i <= string1.Length; i++) { m[i, 0] = i; }
        //    for (int j = 0; j <= string2.Length; j++) { m[0, j] = j; }

        //    for (int i = 1; i <= string1.Length; i++)
        //    {
        //        for (int j = 1; j <= string2.Length; j++)
        //        {
        //            diff = (string1[i - 1] == string2[j - 1]) ? 0 : 1;

        //            m[i, j] = Math.Min(Math.Min(m[i - 1, j] + 1,
        //                                     m[i, j - 1] + 1),
        //                                     m[i - 1, j - 1] + diff);
        //        }
        //    }
        //    return m[string1.Length, string2.Length];
        //}

        //public static int GetDamerauLevenshteinDistance(string s, string t)
        //{
        //    var bounds = new { Height = s.Length + 1, Width = t.Length + 1 };

        //    int[,] matrix = new int[bounds.Height, bounds.Width];

        //    for (int height = 0; height < bounds.Height; height++) { matrix[height, 0] = height; };
        //    for (int width = 0; width < bounds.Width; width++) { matrix[0, width] = width; };

        //    for (int height = 1; height < bounds.Height; height++)
        //    {
        //        for (int width = 1; width < bounds.Width; width++)
        //        {
        //            int cost = (s[height - 1] == t[width - 1]) ? 0 : 10;
        //            int insertion = matrix[height, width - 1] + 1;
        //            int deletion = matrix[height - 1, width] + 10;
        //            int substitution = matrix[height - 1, width - 1] + cost;

        //            int distance = Math.Min(insertion, Math.Min(deletion, substitution));

        //            if (height > 1 && width > 1 && s[height - 1] == t[width - 2] && s[height - 2] == t[width - 1])
        //            {
        //                distance = Math.Min(distance, matrix[height - 2, width - 2] + cost);
        //            }

        //            matrix[height, width] = distance;
        //        }
        //    }

        //    return matrix[bounds.Height - 1, bounds.Width - 1];
        //}

        //GetDamerauLevenshteinDistance
        public static int GetDistance(string string1, string string2, int threshold = int.MaxValue)
        {
            // Return trivial case - where they are equal
            if (string1.Equals(string2))
                return 0;

            // Return trivial case - where one is empty
            if (String.IsNullOrEmpty(string1) || String.IsNullOrEmpty(string2))
                return (string1 ?? string.Empty).Length + (string2 ?? string.Empty).Length;


            // Ensure string2 (inner cycle) is longer
            if (string1.Length > string2.Length)
            {
                var tmp = string1;
                string1 = string2;
                string2 = tmp;
            }

            // Return trivial case - where string1 is contained within string2
            if (string2.Contains(string1))
                return string2.Length - string1.Length - 100; //100 - for up priority

            var length1 = string1.Length;
            var length2 = string2.Length;

            var d = new int[length1 + 1, length2 + 1];

            for (var i = 0; i <= d.GetUpperBound(0); i++)
                d[i, 0] = i;

            for (var i = 0; i <= d.GetUpperBound(1); i++)
                d[0, i] = i;

            for (var i = 1; i <= d.GetUpperBound(0); i++)
            {
                var im1 = i - 1;
                var im2 = i - 2;
                var minDistance = threshold;

                for (var j = 1; j <= d.GetUpperBound(1); j++)
                {
                    var jm1 = j - 1;
                    var jm2 = j - 2;
                    var cost = string1[im1] == string2[jm1] ? 0 : 1;

                    var del = d[im1, j] + 1;
                    var ins = d[i, jm1] + 1;
                    var sub = d[im1, jm1] + cost;

                    //Math.Min is slower than native code
                    //d[i, j] = Math.Min(del, Math.Min(ins, sub));
                    d[i, j] = del <= ins && del <= sub ? del : ins <= sub ? ins : sub;

                    if (i > 1 && j > 1 && string1[im1] == string2[jm2] && string1[im2] == string2[jm1])
                        d[i, j] = Math.Min(d[i, j], d[im2, jm2] + cost);

                    if (d[i, j] < minDistance)
                        minDistance = d[i, j];
                }

                if (minDistance > threshold)
                    return int.MaxValue;
            }

            return d[d.GetUpperBound(0), d.GetUpperBound(1)] > threshold
                ? int.MaxValue
                : d[d.GetUpperBound(0), d.GetUpperBound(1)];
        }
    }

}
