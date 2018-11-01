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
        }

        public void OnCreateWindow(CBaseWindow window)
        {
            var panel = new Panel();

            Control pc;
            if (window.Parent == null || !_controls.TryGetValue(window.Parent.Id, out pc))
                pc = _panel;

            panel.Parent = pc;

            panel.Name = window.Name;

            Rect rect = window.GetRect();
            panel.Location = new Point((int)rect.left, (int)rect.top);
            panel.Size = new Size((int)rect.Width, (int)rect.Hight);

            //panel.BackColor = Color.Green;
            //panel.ForeColor = Color.Yellow;

            _controls.Add(window.Id, panel);
        }

        public void OnDeleteWindow(CBaseWindow window)
        {
            Control pc;
            if (!_controls.TryGetValue(window.Id, out pc))
                return;

            _controls.Remove(window.Id);

            pc.Parent = null;
            pc.Dispose();
        }

        public void OnWindowChange(CBaseWindow window, EWindowParams inParamType, CBaseParam inNewParam)
        {
            Control pc;
            if (!_controls.TryGetValue(window.Id, out pc))
                return;

            switch (inParamType)
            {
                case EWindowParams.Name: pc.Name = inNewParam.ToString(); break;
                case EWindowParams.Pivot_By_Parent:
                case EWindowParams.Pivot_In_Own_Rect:
                case EWindowParams.Size:
                {
                    Rect rect = window.GetRect();
                    pc.Location = new Point((int)rect.left, (int)rect.top);
                    pc.Size = new Size((int)rect.Width, (int)rect.Hight);
                }
                break;
                case EWindowParams.Color:
                {
                    Vector4 v4 = inNewParam.ToVector4();
                    pc.BackColor = Color.FromArgb((int)v4.w, (int)v4.x, (int)v4.y, (int)v4.z);
                }
                break;
            }
        }
    }
}
