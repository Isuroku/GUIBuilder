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
        public EWindowType WindowType { get { return _params[EWindowParams.Type].ToWindowType(); } }

        public CBaseWindow(CNode inParent, string inName, EWindowType inWindowType)
        {
            _node = new CNode(this, inParent);
            _params.Add(EWindowParams.Name, new CStringParam(inName));
            _params.Add(EWindowParams.Type, new CWindowTypeParam(inWindowType));
        }

        public override string ToString()
        {
            return string.Format("{0} [{1}]", Name, WindowType);
        }

        internal void Change(SWinKeyInfo inKeyInfo)
        {
            CStringParam p = (CStringParam)_params[EWindowParams.Name];
            if (!string.Equals(p.ToString(), inKeyInfo.Name))
            {
                p.SetValue(inKeyInfo.Name);

                OnChangeParam(EWindowParams.Name, p);
            }
        }

        protected virtual void OnChangeParam(EWindowParams inParamType, CBaseParam inNewParam) { }

        public virtual void Dispose()
        {
            if (IsDisposed)
                return;

            foreach (CNode c in _node.Childs)
                c.Window.Dispose();

            _node.Dispose();

            IsDisposed = true;
        }

        void CheckParam(EWindowParams inParam, IKey inKey)
        {
            if(inParam == EWindowParams.Name)
                CheckStringParam(inParam, inKey.GetName());
        }

        void CheckStringParam(EWindowParams inParam, string inNewValue)
        {
            CBaseParam val;
            string old_value = string.Empty;
            if (_params.TryGetValue(inParam, out val))
            {
                old_value = val.ToString();

                if (!string.Equals(old_value, inNewValue, StringComparison.Ordinal))
                    val.SetValue(inNewValue);
            }
            else
                _params.Add(inParam, new CStringParam(inNewValue));
        }
    }
}
