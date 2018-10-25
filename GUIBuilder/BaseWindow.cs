using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CascadeParser;

namespace GUIBuilder
{
    public abstract class CBaseWindow: IDisposable
    {
        

        CWindowRect _rect = new CWindowRect();

        CNode _node;

        public CNode Parent { get { return _node.Parent; } }

        public bool IsDisposed { get; private set; }

        Dictionary<EWindowParams, CBaseParam> _params = new Dictionary<EWindowParams, CBaseParam>();

        public string Name { get { return _params[EWindowParams.Name].ToString(); } }
        public EWindowType WindowType { get { return _params[EWindowParams.WindowType].ToWindowType(); } }

        public CBaseWindow(CNode inParent, string inName, EWindowType inWindowType)
        {
            _node = new CNode(this, inParent);
            _params.Add(EWindowParams.Name, new CStringParam(inName));
            _params.Add(EWindowParams.WindowType, new CWindowTypeParam(inWindowType));
        }

        public override string ToString()
        {
            return string.Format("{0} [{1}]", Name, WindowType);
        }

        internal void Change(IKey inKey)
        {
            
        }

        public virtual void Dispose()
        {
            if (IsDisposed)
                return;

            foreach (CNode c in _node.Childs)
                c.Window.Dispose();

            _node.Dispose();

            IsDisposed = true;
        }
    }
}
