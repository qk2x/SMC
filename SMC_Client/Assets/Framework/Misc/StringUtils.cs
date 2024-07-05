using System.Collections.Generic;

namespace Framework.Misc
{
    public static class StringUtils
    {
        private const int RegularCount = 256;
        private static string[] _regularLongStrCache;
        private const int MaxStringCache = 512;
        private static Dictionary<long, string> stringLongCache = new Dictionary<long, string>(MaxStringCache);
        private static Dictionary<float, string> stringFloatCache = new Dictionary<float, string>(MaxStringCache);
    
        public static string ToCacheString(this long val, string format = null)
        {
            if (_regularLongStrCache == null)
            {
                _regularLongStrCache = new string[RegularCount];
                for (int i = 0; i < RegularCount; ++i)
                {
                    _regularLongStrCache[i] = i.ToString();
                }
            }

            if (val >= 0 && val < RegularCount)
            {
                return _regularLongStrCache[val];
            }

            if (!stringLongCache.TryGetValue(val, out var str))
            {
                str = val.ToString(format);

                if (stringLongCache.Count > MaxStringCache)
                {
                    DLog.Error($"stringLongCache too:{stringLongCache.Count}");
                    stringLongCache.Clear();
                }

                stringLongCache.Add(val, str);
            }

            return str;
        }

        public static string ToCacheString(this float val, string format = null)
        {
            val = (int) (val * 100) / 100f;
            if (!stringFloatCache.TryGetValue(val, out var str))
            {
                if (stringFloatCache.Count > MaxStringCache)
                {
                    DLog.Error($"stringFloatCache too:{stringFloatCache.Count}");
                    stringFloatCache.Clear();
                }

                str = val.ToString(format);

                stringFloatCache.Add(val, str);
            }

            return str;
        }

        public static string ToCacheString(this int val)
        {
            return ((long) val).ToCacheString();
            ;
        }

        public static string ToCacheString(this uint val)
        {
            return ((long) val).ToCacheString();
        }
    }
}