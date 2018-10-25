﻿using GUIBuilder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GUIBuilderForm
{
    class CWFNode : CNode
    {
        public Control WFControl { get; private set; }

        public CWFNode(CBaseWindow window, CNode inParent, Control own_control) : base(window, inParent)
        {
            WFControl = own_control;
        }

        public override void Dispose()
        {
            WFControl = null;
            base.Dispose();
        }
    }

    class CWFPanel: CBaseWindow
    {
        Panel _panel;

        public CWFPanel(CWFNode inParent, string inName, EWindowType inWindowType) : base(inParent, inName, inWindowType)
        {
            _panel = new Panel();
            _panel.Parent = inParent.WFControl;
            _panel.Location = new Point(0, 0);
            _panel.Name = "w";
            _panel.Size = new Size(45, 36);
            _panel.BackColor = Color.Green;
            _panel.ForeColor = Color.Yellow;
        }

        public override void Dispose()
        {
            if (_panel != null)
                _panel.Dispose();
            _panel = null;

            base.Dispose();
        }
    }
}
