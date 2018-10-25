using CascadeParser;
using System;
using System.Collections.Generic;
using System.Text;

namespace GUIBuilder
{
    public interface IGUIFactory
    {
        CBaseWindow CreateWindow(CNode inParent, string inName, EWindowType inWindowType);
    }

    public class CGUIBuilder<T>
    {
        CParserManager _parser;
        public CParserManager Parser { get { return _parser; } }

        public IKey LastBuildKey { get; private set; }

        ILogPrinter _logger;
        IGUIFactory _factory;
        CNode _root;

        List<CBaseWindow> _windows = new List<CBaseWindow>();

        List<CBaseWindow> _unused_windows_cache = new List<CBaseWindow>();

        List<IKey> _child_cache = new List<IKey>();

        public CGUIBuilder(ILogPrinter inLogger, IParserOwner inParserOwner, IGUIFactory inGUIFactory, CNode Root)
        {
            _logger = inLogger;
            _parser = new CParserManager(inParserOwner);
            _factory = inGUIFactory;
            _root = Root;
        }

        public void Build(string inFileName, string inText, object inContextData = null)
        {
            LastBuildKey = _parser.Parse(inFileName, inText, _logger, null);

            LastBuildKey.CopyChilds(_child_cache);

            for (int i = _windows.Count - 1; i >= 0; --i)
            {
                CBaseWindow window = _windows[i];
                string name = window.Name;
                IKey window_key = LastBuildKey.FindChildByName(name);

                if (window_key != null)
                {
                    window.Change(window_key);
                    _child_cache.Remove(window_key);
                }
                else
                {
                    _unused_windows_cache.Add(window);
                    _windows.RemoveAt(i);
                }
            }

            for (int i = 0; i < _child_cache.Count; i++)
            {
                IKey window_key = _child_cache[i];
                string name = window_key.GetName();

                CBaseWindow window = FindMostSuitableWindow(_unused_windows_cache, name);
                if (window == null)
                    window = _factory.CreateWindow(_root, name, EWindowType.Panel);
                else
                    _unused_windows_cache.Remove(window);

                int index = LastBuildKey.FindChildIndex(window_key);
                if (index != -1)
                    _windows.Insert(index, window);
                else
                    _logger.LogError(string.Format("Can't FindChildIndex"));

                window.Change(window_key);
            }

            for (int i = 0; i < _unused_windows_cache.Count; i++)
                _unused_windows_cache[i].Dispose();

            _child_cache.Clear();
            _unused_windows_cache.Clear();
        }

        CBaseWindow FindMostSuitableWindow(List<CBaseWindow> unused_windows_cache, string inName)
        {
            int priority = int.MaxValue;
            CBaseWindow sel_window = null;

            for (int i = 0; i < unused_windows_cache.Count; i++)
            {
                CBaseWindow window = unused_windows_cache[i];
                int dist = LevenshteinDistance.GetDistance(window.Name, inName);

                if(dist < priority)
                {
                    sel_window = window;
                    priority = dist;
                }
            }

            return sel_window;
        }
    }
}
