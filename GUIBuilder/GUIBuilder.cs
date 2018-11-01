using CascadeParser;
using System;
using System.Collections.Generic;
using System.Text;

namespace GUIBuilder
{
    public interface IGUIRealization
    {
        void OnCreateWindow(CBaseWindow window);
        void OnDeleteWindow(CBaseWindow window);
        void OnWindowChange(CBaseWindow window, EWindowParams inParamType, CBaseParam inNewParam);
    }

    public class CGUIBuilder
    {
        CParserManager _parser;
        public CParserManager Parser { get { return _parser; } }

        public IKey LastBuildKey { get; private set; }

        IGUIRealization _gui_realization;
        CBaseWindow _root;

        List<CBaseWindow> _windows = new List<CBaseWindow>();

        public CGUIBuilder(IParserOwner inParserOwner, IGUIRealization inGUIRealization, Rect inBaseRect)
        {
            _parser = new CParserManager(inParserOwner);
            _gui_realization = inGUIRealization;
            _root = new CBaseWindow(null, "MainFrame", EWindowType.Panel, inBaseRect, _gui_realization);
        }

        SWinKeyInfo[] GetWinKeyInfos(IKey inWinKeyParent, ILogPrinter inLogger)
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

        public void Build(string inFileName, string inText, ILogPrinter inLogger, object inContextData = null)
        {
            LastBuildKey = _parser.Parse(inFileName, inText, inLogger, inContextData);

            //LastBuildKey.CopyChilds(_child_cache);

            SWinKeyInfo[] keys = GetWinKeyInfos(LastBuildKey, inLogger);

            List<SWinKeyInfo> child_cache = new List<SWinKeyInfo>(keys);
            List<CBaseWindow> unused_windows_cache = new List<CBaseWindow>();

            for (int i = _windows.Count - 1; i >= 0; --i)
            {
                CBaseWindow window = _windows[i];
                SWinKeyInfo window_key_info = FindMostSuitableKey(keys, window.Name, window.WindowType, inLogger);

                if (window_key_info.WinKey != null)
                {
                    window.Change(window_key_info, inLogger);
                    child_cache.RemoveAll(el => el.WinKey == window_key_info.WinKey);
                }
                else
                {
                    unused_windows_cache.Add(window);
                    _windows.RemoveAt(i);
                }
            }

            for (int i = 0; i < child_cache.Count; i++)
            {
                SWinKeyInfo window_key_info = child_cache[i];

                CBaseWindow window = FindMostSuitableWindow(unused_windows_cache, window_key_info);
                if (window == null)
                    window = new CBaseWindow(_root, window_key_info.Name, window_key_info.WinType, new Rect(0, 0, 10, 10), _gui_realization);
                else
                    unused_windows_cache.Remove(window);

                window.Change(window_key_info, inLogger);

                int index = LastBuildKey.FindChildIndex(window_key_info.WinKey);
                if (index != -1)
                    _windows.Insert(index, window);
                else
                    inLogger.LogError(string.Format("Can't FindChildIndex"));
            }

            for (int i = 0; i < unused_windows_cache.Count; i++)
                unused_windows_cache[i].Dispose();
        }

        SWinKeyInfo FindMostSuitableKey(SWinKeyInfo[] inKeys, string inName, EWindowType inWindowType, ILogPrinter inLogger)
        {
            int min_dist = int.MaxValue;
            SWinKeyInfo res_key = new SWinKeyInfo();

            for (int i = 0; i < inKeys.Length; i++)
            {
                SWinKeyInfo window_key_info = inKeys[i];

                if(inWindowType == window_key_info.WinType)
                {
                    int d = LevenshteinDistance.GetDistance(window_key_info.Name, inName, 10);
                    if(d < min_dist)
                    {
                        min_dist = d;
                        res_key = window_key_info;
                    }
                }
            }

            return res_key;
        }

        CBaseWindow FindMostSuitableWindow(List<CBaseWindow> unused_windows_cache, SWinKeyInfo inKeyInfo)
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
