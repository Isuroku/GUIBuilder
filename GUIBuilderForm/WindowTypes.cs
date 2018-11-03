using GUIBuilder;
using System.Drawing;
using System.Windows.Forms;

namespace GUIBuilderForm
{
    static class WindowParams
    {
        public static SWindowParamDescr Text = new SWindowParamDescr(0, "Text", EBaseParamType.String, false);
    }

    static class WindowTypes
    {
        public static CWindowTypeDescr Panel = new CWindowTypeDescr(0, "Panel");
        public static CWindowTypeDescr Label = new CWindowTypeDescr(1, "Label");
        public static CWindowTypeDescr TextBox = new CWindowTypeDescr(2, "TextBox");
    }

    static class WindowFactory
    {
        public static Control Create(IBaseWindow window, Control parent)
        {
            Control res = null;

            if (window.WindowType == WindowTypes.Panel.Id)
                res = new Panel();
            else if (window.WindowType == WindowTypes.Label.Id)
                res = new Label();
            else if (window.WindowType == WindowTypes.TextBox.Id)
                res = new TextBox();

            res.Parent = parent;
            res.Name = window.Name;

            Rect rect = window.GetRect(true);
            res.Location = new Point((int)rect.left, (int)rect.top);
            res.Size = new Size((int)rect.Width, (int)rect.Hight);

            return res;
        }

    }
}
