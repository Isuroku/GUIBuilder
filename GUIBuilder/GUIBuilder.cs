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

        public void Build(string inFileName, string inText, ILogPrinter inLogger, object inContextData = null)
        {
            LastBuildKey = _parser.Parse(inFileName, inText, inLogger, inContextData);
            _root.Build(LastBuildKey, inLogger);
        }

        
    }
}
