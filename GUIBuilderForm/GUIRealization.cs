using GUIBuilder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GUIBuilderForm
{
    class CGUIRealization : IGUIRealization
    {
        Panel _panel;

        Dictionary<ulong, Control> _controls = new Dictionary<ulong, Control>();

        public CGUIRealization(Panel panel)
        {
            _panel = panel;
            _controls.Add(uint.MaxValue, panel);
        }

        internal void SetBuilder(CGUIBuilder builder)
        {
            builder.WindowTypeDescrs.AddType(WindowTypes.Panel);

            WindowTypes.Label.AddParams(WindowParams.Text, true);
            builder.WindowTypeDescrs.AddType(WindowTypes.Label);
        }

        public void OnCreateWindow(IBaseWindow window)
        {
            if (window.WindowType.Id == uint.MaxValue)
                return;

            Control pc;
            if (window.Parent == null || !_controls.TryGetValue(window.Parent.Id, out pc))
                pc = _panel;

            var control = WindowFactory.Create(window, pc);

            _controls.Add(window.Id, control);
        }

        public void OnDeleteWindow(IBaseWindow window)
        {
            if (window.WindowType.Id == uint.MaxValue)
                return;

            Control pc;
            if (!_controls.TryGetValue(window.Id, out pc))
                return;

            _controls.Remove(window.Id);

            pc.Parent = null;
            pc.Dispose();
        }

        public void OnWindowChange(IBaseWindow window, NamedId inParamType, CBaseParam inNewParam)
        {
            Control pc;
            if (!_controls.TryGetValue(window.Id, out pc))
                return;

            if(inParamType == SWindowParamDescr.Name.Id)
            {
                pc.Name = inNewParam.ToString();
            }
            else if(inParamType == SWindowParamDescr.Indent.Id
                    || inParamType == SWindowParamDescr.Shift.Id
                    || inParamType == SWindowParamDescr.Size.Id)
            {
                Rect rect = window.GetRect(true);
                pc.Location = new Point((int)rect.left, (int)rect.top);
                pc.Size = new Size((int)rect.Width, (int)rect.Hight);
            }
            else if (inParamType == SWindowParamDescr.Color.Id)
            {
                Vector4 v4 = inNewParam.ToVector4();
                pc.BackColor = Color.FromArgb((int)v4.w, (int)v4.x, (int)v4.y, (int)v4.z);
            }
            else if (inParamType == WindowParams.Text.Id)
            {
                if (window.WindowType == WindowTypes.Label.Id)
                {
                    pc.Text = inNewParam.ToString();
                }
            }
        }

        
    }
}
