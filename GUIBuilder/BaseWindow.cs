using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CascadeParser;

namespace GUIBuilder
{
    public interface IBaseWindow
    {
        ulong Id { get; }
        string Name { get; }
        NamedId WindowType { get; }
        CBaseParam GetParamValue(NamedId inParamId);
        IBaseWindow Parent { get; }
        Rect GetRect(bool inLocal);
    }

    internal class CBaseWindow: IDisposable, IBaseWindow
    {
        public ulong Id { get; private set; }
        static ulong _id_counter;

        public CBaseWindow Parent { get; private set; }
        IBaseWindow IBaseWindow.Parent { get { return Parent; } }

        List<CBaseWindow> _childs = new List<CBaseWindow>();

        public bool IsDisposed { get; private set; }

        IGUIRealization _gui_realization;
        CWindowTypeDescrs _window_type_descrs;

        Dictionary<NamedId, CBaseParam> _params = new Dictionary<NamedId, CBaseParam>();

        public string Name { get { return _params[SWindowParamDescr.Name.Id].ToString(); } }
        public NamedId WindowType { get; private set; }

        public CBaseWindow(CBaseWindow inParent, string inName, NamedId inWindowType, Rect inBaseRect, IGUIRealization inGUIRealization, CWindowTypeDescrs window_type_descrs)
        {
            Id = ++_id_counter;

            WindowType = inWindowType;

            _window_type_descrs = window_type_descrs;

            SetParent(inParent);

            _params.Add(SWindowParamDescr.Name.Id, new CStringParam(inName));
            _params.Add(SWindowParamDescr.Indent.Id, new CPVectorParam(new PVector2(inBaseRect.left, inBaseRect.top)));
            _params.Add(SWindowParamDescr.Shift.Id, new CPVectorParam(new PVector2()));
            _params.Add(SWindowParamDescr.Size.Id, new CPVectorParam(new PVector2(inBaseRect.Width, inBaseRect.Hight)));

            _gui_realization = inGUIRealization;
            _gui_realization.OnCreateWindow(this);
        }

        public override string ToString()
        {
            return string.Format("{0} [{1}]", Name, WindowType);
        }

        public virtual void Dispose()
        {
            if (IsDisposed)
                return;

            _gui_realization.OnDeleteWindow(this);
            _gui_realization = null;

            var lst = new List<CBaseWindow>(_childs);
            lst.ForEach(w => w.Dispose());

            if(Parent != null)
                Parent.RemoveChild(this);
            Parent = null;

            IsDisposed = true;
        }

        void SetParent(CBaseWindow inParent)
        {
            if (inParent != null)
            {
                if (Parent == inParent)
                    return;

                if (Parent != null)
                    Parent.RemoveChild(this);

                Parent = inParent;
                Parent.AddChild(this);
            }
            else if(Parent != null)
            {
                Parent.RemoveChild(this);
                Parent = null;
            }
        }

        private void AddChild(CBaseWindow inNode)
        {
            if (_childs.Contains(inNode))
                return;
            _childs.Add(inNode);
        }

        private void RemoveChild(CBaseWindow inNode)
        {
            _childs.Remove(inNode);
        }

        void CheckParam(SWindowParamDescr inParamDescr, string[] inNewValue)
        {
            CBaseParam val;
            if (_params.TryGetValue(inParamDescr.Id, out val))
            {
                if (val.SetValue(inNewValue))
                {
                    _gui_realization.OnWindowChange(this, inParamDescr.Id, val);

                    if (inParamDescr.IsChildInfluence)
                    {
                        for (int i = 0; i < _childs.Count; ++i)
                        {
                            CBaseParam child_val = _childs[i].GetParamValue(inParamDescr.Id);
                            _gui_realization.OnWindowChange(_childs[i], inParamDescr.Id, child_val);
                        }
                    }
                }
            }
            else
            {
                val = CBaseParam.CreateParam(inParamDescr.ParamType, inNewValue);
                _params.Add(inParamDescr.Id, val);
                _gui_realization.OnWindowChange(this, inParamDescr.Id, val);

                if (inParamDescr.IsChildInfluence)
                {
                    for (int i = 0; i < _childs.Count; ++i)
                    {
                        CBaseParam child_val = _childs[i].GetParamValue(inParamDescr.Id);
                        _gui_realization.OnWindowChange(_childs[i], inParamDescr.Id, child_val);
                    }
                }
            }
        }

        public CBaseParam GetParamValue(NamedId inParamId)
        {
            CBaseParam val;
            if (!_params.TryGetValue(inParamId, out val))
            {
                //CWindowTypeDescr wtd = _window_type_descrs.GetDescr(WindowType);
                //SWindowParamDescr? pd = wtd.GetWindowParamDescr(inParamId);
                //if (!pd.HasValue)
                //    return null;

                //val = CBaseParam.CreateParam(pd.Value.ParamType, null);
                //_params.Add(pd.Value.Id, val);
                //_gui_realization.OnWindowChange(this, pd.Value.Id, val);
            }
            return val;
        }

        CBaseParam GetBaseParam(SWindowParamDescr inParamDescr)
        {
            CBaseParam val;
            if (!_params.TryGetValue(inParamDescr.Id, out val))
            {
                //val = CBaseParam.CreateParam(inParamDescr.ParamType, null);
                //_params.Add(inParamDescr.Id, val);
                //_gui_realization.OnWindowChange(this, inParamDescr.Id, val);
            }
            return val;
        }

        internal void Change(SWinKeyInfo inKeyInfo, ILogPrinter inLogger)
        {
            CWindowTypeDescr type_descr = _window_type_descrs.GetDescr(WindowType);

            foreach(SWindowParamDescr param_descr in type_descr)
            {
                if(param_descr == SWindowParamDescr.Name)
                    CheckParam(param_descr, new string[] { inKeyInfo.Name });
                else
                {
                    bool must_be = type_descr.IsMustBe(param_descr.Id);
                    string[] a = Utils.TryGetParamsBySubKeyName(param_descr.Id.Name, inKeyInfo.WinKey, inLogger, must_be, CBaseParam.GetParamCount(param_descr.ParamType));
                    if(a != null)
                        CheckParam(param_descr, a);
                }
            }

            IKey childs_key = inKeyInfo.WinKey.FindChildByName("Childs", StringComparison.InvariantCultureIgnoreCase);
            if (childs_key != null)
                Build(childs_key, inLogger);
        }
        
        public Rect GetRect(bool inLocal = true)
        {
            Rect parent_rect;
            if (Parent != null)
                parent_rect = Parent.GetClientRect(); //global rect
            else
                parent_rect = new Rect();

            PVector2 pivot_by_parent = GetBaseParam(SWindowParamDescr.Indent).ToPVector2();

            Vector2 parent_shift = pivot_by_parent.GetVector(parent_rect);

            Vector2 c = parent_shift; 
            if (!inLocal)
                c = c + parent_rect.Min; //global pos

            PVector2 size = GetBaseParam(SWindowParamDescr.Size).ToPVector2();

            Vector2 sz = size.GetVector(parent_rect);

            PVector2 pivot_in_own_rect = GetBaseParam(SWindowParamDescr.Shift).ToPVector2();

            Vector2 own_shift = pivot_in_own_rect.GetVector(sz);

            Vector2 lt = c - own_shift;

            return new Rect(lt.x, lt.y, lt.x + sz.x, lt.y + sz.y);
        }

        public Rect GetClientRect()
        {
            return GetRect();
        }

        internal void Build(IKey inChildsKey, ILogPrinter inLogger)
        {
            SWinKeyInfo[] keys = Utils.GetWinKeyInfos(inChildsKey, _window_type_descrs, inLogger);

            List<CBaseWindow> unused_windows_cache = new List<CBaseWindow>(_childs);
            List<CBaseWindow> new_childs = new List<CBaseWindow>();

            for (int i = 0; i < keys.Length; i++)
            {
                SWinKeyInfo window_key_info = keys[i];

                CBaseWindow window = Utils.FindMostSuitableWindow(unused_windows_cache, window_key_info);
                if (window == null)
                    window = new CBaseWindow(this, window_key_info.Name, window_key_info.WinType, new Rect(0, 0, 10, 10), _gui_realization, _window_type_descrs);
                else
                    unused_windows_cache.Remove(window);

                new_childs.Add(window);

                window.Change(window_key_info, inLogger);
            }

            for (int i = 0; i < unused_windows_cache.Count; i++)
                unused_windows_cache[i].Dispose();

            _childs.Clear();
            _childs = new_childs;
        }
    }
}
