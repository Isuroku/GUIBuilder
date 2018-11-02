using CascadeParser;
using System;
using System.Collections.Generic;
using System.Text;

namespace GUIBuilder
{
    public interface IGUIRealization
    {
        void OnCreateWindow(IBaseWindow window);
        void OnDeleteWindow(IBaseWindow window);
        void OnWindowChange(IBaseWindow window, NamedId inParamType, CBaseParam inNewParam);
    }

    public class CGUIBuilder
    {
        CParserManager _parser;
        public CParserManager Parser { get { return _parser; } }

        public IKey LastBuildKey { get; private set; }

        IGUIRealization _gui_realization;

        CBaseWindow _root;

        CWindowTypeDescrs _window_type_descrs = new CWindowTypeDescrs();
        public CWindowTypeDescrs WindowTypeDescrs { get { return _window_type_descrs; } }

        public CGUIBuilder(IParserOwner inParserOwner, IGUIRealization inGUIRealization, Rect inBaseRect)
        {
            _parser = new CParserManager(inParserOwner);
            _gui_realization = inGUIRealization;
            _root = new CBaseWindow(null, "MainFrame", new NamedId(uint.MaxValue, "MainFrame"), inBaseRect, _gui_realization, _window_type_descrs);
        }

        public void Build(string inFileName, string inText, ILogPrinter inLogger, object inContextData = null)
        {
            LastBuildKey = _parser.Parse(inFileName, inText, inLogger, inContextData);
            _root.Build(LastBuildKey, inLogger);
        }

        
    }
}
