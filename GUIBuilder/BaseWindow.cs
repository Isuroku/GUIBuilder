using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CascadeParser;

namespace GUIBuilder
{
    public class CBaseWindow: IDisposable
    {
        public ulong Id { get; private set; }
        static ulong _id_counter;

        public CBaseWindow Parent { get; private set; }

        List<CBaseWindow> _childs = new List<CBaseWindow>();

        public bool IsDisposed { get; private set; }

        IGUIRealization _gui_realization;

        Dictionary<EWindowParams, CBaseParam> _params = new Dictionary<EWindowParams, CBaseParam>();

        public string Name { get { return _params[EWindowParams.Name].ToString(); } }
        public EWindowType WindowType { get { return _params[EWindowParams.Type].ToWindowType(); } }

        public CBaseWindow(CBaseWindow inParent, string inName, EWindowType inWindowType, Rect inBaseRect, IGUIRealization inGUIRealization)
        {
            Id = ++_id_counter;

            SetParent(inParent);

            _params.Add(EWindowParams.Name, new CStringParam(inName));
            _params.Add(EWindowParams.Type, new CWindowTypeParam(inWindowType));
            _params.Add(EWindowParams.Pivot_By_Parent, new CPVectorParam(new PVector2(inBaseRect.left, inBaseRect.top)));
            _params.Add(EWindowParams.Pivot_In_Own_Rect, new CPVectorParam(new PVector2()));
            _params.Add(EWindowParams.Size, new CPVectorParam(new PVector2(inBaseRect.Width, inBaseRect.Hight)));

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

        internal void Change(SWinKeyInfo inKeyInfo, ILogPrinter inLogger)
        {
            EnumUtils.Foreach<EWindowParams>(pt =>
            {
                if(pt == EWindowParams.Name)
                {
                    CStringParam p = (CStringParam)_params[EWindowParams.Name];
                    if (!string.Equals(p.ToString(), inKeyInfo.Name))
                    {
                        p.SetValue(inKeyInfo.Name);

                        _gui_realization.OnWindowChange(this, EWindowParams.Name, p);
                    }
                }
                else if(pt == EWindowParams.Text)
                {
                    string[] a = Utils.TryGetParamsBySubKeyName(pt.ToString(), inKeyInfo.WinKey, inLogger, false, 1);
                    if(a.Length > 0)
                        CheckStringParam(pt, a[0]);
                }
                else if (pt == EWindowParams.Pivot_By_Parent 
                      || pt == EWindowParams.Size
                      || pt == EWindowParams.Pivot_In_Own_Rect)
                {
                    string[] a = Utils.TryGetParamsBySubKeyName(pt.ToString(), inKeyInfo.WinKey, inLogger, pt == EWindowParams.Size, 2);
                    if (a.Length == 2)
                    {
                        PValue x, y;
                        if(PValue.TryParse(a[0], out x) && PValue.TryParse(a[1], out y))
                            CheckPVectorParam(pt, new PVector2(x, y));
                    }
                }
                else if (pt == EWindowParams.Color)
                {
                    string[] a = Utils.TryGetParamsBySubKeyName(pt.ToString(), inKeyInfo.WinKey, inLogger, false, 3, 4);
                    if (a.Length > 2)
                    {
                        float x, y, z, w;
                        if (a.Length > 3)
                        {
                            if (float.TryParse(a[0], out x) && float.TryParse(a[1], out y)
                                && float.TryParse(a[2], out z) && float.TryParse(a[3], out w))
                                CheckVector4Param(pt, new Vector4(x, y, z, w));
                        }
                        else if (float.TryParse(a[0], out x) && float.TryParse(a[1], out y) && float.TryParse(a[2], out z))
                            CheckVector4Param(pt, new Vector4(x, y, z, 255));
                    }
                }
            });

            IKey childs_key = inKeyInfo.WinKey.FindChildByName("Childs", StringComparison.InvariantCultureIgnoreCase);
            if (childs_key != null)
                Build(childs_key, inLogger);
        }

        #region StringParam
        void CheckStringParam(EWindowParams inParam, string inNewValue)
        {
            CBaseParam val;
            if (_params.TryGetValue(inParam, out val))
            {
                if(val.SetValue(inNewValue))
                    _gui_realization.OnWindowChange(this, inParam, val);
            }
            else
            {
                val = new CStringParam(inNewValue);
                _params.Add(inParam, val);
                _gui_realization.OnWindowChange(this, inParam, val);
            }
        }

        public void SetStringParam(EWindowParams inParamType, string value, bool inRaiseChangeEvent = true)
        {
            CBaseParam val;
            if (_params.TryGetValue(inParamType, out val))
                val.SetValue(value);
            else
            {
                val = new CStringParam(value);
                _params.Add(inParamType, val);
            }

            if (inRaiseChangeEvent)
                _gui_realization.OnWindowChange(this, inParamType, val);
        }

        public string GetStringParam(EWindowParams inParamType)
        {
            CBaseParam val;
            string value;
            if (_params.TryGetValue(inParamType, out val))
                value = val.ToString();
            else
            {
                value = string.Empty;
                val = new CStringParam(value);
                _params.Add(inParamType, val);
                _gui_realization.OnWindowChange(this, inParamType, val);
            }

            return value;
        }
        #endregion StringParam

        #region PVectorParam
        void CheckPVectorParam(EWindowParams inParam, PVector2 inNewValue)
        {
            CBaseParam val;
            if (_params.TryGetValue(inParam, out val))
            {
                if(val.SetValue(inNewValue))
                    _gui_realization.OnWindowChange(this, inParam, val);
            }
            else
            {
                val = new CPVectorParam(inNewValue);
                _params.Add(inParam, val);
                _gui_realization.OnWindowChange(this, inParam, val);
            }
        }

        public PVector2 GetPVector2Param(EWindowParams inParamType)
        {
            CBaseParam val;
            PVector2 value;
            if (_params.TryGetValue(inParamType, out val))
                value = val.ToPVector2();
            else
            {
                value = new PVector2();
                val = new CPVectorParam(value);
                _params.Add(inParamType, val);
                _gui_realization.OnWindowChange(this, inParamType, val);
            }

            return value;
        }

        public void SetPVector2Param(EWindowParams inParamType, PVector2 value, bool inRaiseChangeEvent = true)
        {
            CBaseParam val;
            if (_params.TryGetValue(inParamType, out val))
                val.SetValue(value);
            else
            {
                val = new CPVectorParam(value);
                _params.Add(inParamType, val);
            }

            if (inRaiseChangeEvent)
                _gui_realization.OnWindowChange(this, inParamType, val);
        }
        #endregion PVectorParam

        #region Vector4Param
        void CheckVector4Param(EWindowParams inParam, Vector4 inNewValue)
        {
            CBaseParam val;
            if (_params.TryGetValue(inParam, out val))
            {
                if (val.SetValue(inNewValue))
                    _gui_realization.OnWindowChange(this, inParam, val);
            }
            else
            {
                val = new CVector4Param(inNewValue);
                _params.Add(inParam, val);
                _gui_realization.OnWindowChange(this, inParam, val);
            }
        }

        public Vector4 GetVector4Param(EWindowParams inParamType)
        {
            CBaseParam val;
            Vector4 value;
            if (_params.TryGetValue(inParamType, out val))
                value = val.ToVector4();
            else
            {
                value = new Vector4();
                val = new CVector4Param(value);
                _params.Add(inParamType, val);
                _gui_realization.OnWindowChange(this, inParamType, val);
            }

            return value;
        }

        public void SetVector4Param(EWindowParams inParamType, Vector4 value, bool inRaiseChangeEvent = true)
        {
            CBaseParam val;
            if (_params.TryGetValue(inParamType, out val))
                val.SetValue(value);
            else
            {
                val = new CVector4Param(value);
                _params.Add(inParamType, val);
            }

            if (inRaiseChangeEvent)
                _gui_realization.OnWindowChange(this, inParamType, val);
        }
        #endregion CheckStringParam

        public Rect GetRect(bool inLocal = true)
        {
            Rect parent_rect;
            if (Parent != null)
                parent_rect = Parent.GetClientRect(); //global rect
            else
                parent_rect = new Rect();

            PVector2 pivot_by_parent = GetPVector2Param(EWindowParams.Pivot_By_Parent);

            Vector2 parent_shift = pivot_by_parent.GetVector(parent_rect);

            Vector2 c = parent_shift; 
            if (!inLocal)
                c = c + parent_rect.Min; //global pos

            PVector2 size = GetPVector2Param(EWindowParams.Size);

            Vector2 sz = size.GetVector(parent_rect);

            PVector2 pivot_in_own_rect = GetPVector2Param(EWindowParams.Pivot_In_Own_Rect);

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
            SWinKeyInfo[] keys = Utils.GetWinKeyInfos(inChildsKey, inLogger);

            List<CBaseWindow> unused_windows_cache = new List<CBaseWindow>(_childs);
            List<CBaseWindow> new_childs = new List<CBaseWindow>();

            for (int i = 0; i < keys.Length; i++)
            {
                SWinKeyInfo window_key_info = keys[i];

                CBaseWindow window = Utils.FindMostSuitableWindow(unused_windows_cache, window_key_info);
                if (window == null)
                    window = new CBaseWindow(this, window_key_info.Name, window_key_info.WinType, new Rect(0, 0, 10, 10), _gui_realization);
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
