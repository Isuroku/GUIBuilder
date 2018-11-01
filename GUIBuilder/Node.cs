using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUIBuilder
{
    //public class CNode
    //{
    //    public CBaseWindow Window { get; private set; }

    //    public CNode Parent { get; private set; }

    //    List<CNode> _childs = new List<CNode>();

    //    public IEnumerable<CNode> Childs { get { return _childs; } }

    //    public CNode(CBaseWindow window, CNode inParent)
    //    {
    //        Window = window;
    //        Parent = inParent;

    //        if(Parent != null)
    //            Parent.AddChild(this);
    //    }

    //    private void AddChild(CNode inNode)
    //    {
    //        if (_childs.Contains(inNode))
    //            return;
    //        _childs.Add(inNode);
    //    }

    //    private void RemoveChild(CNode inNode)
    //    {
    //        _childs.Remove(inNode);
    //    }

    //    public virtual void Dispose()
    //    {
    //        if (Parent != null)
    //            Parent.RemoveChild(this);
    //    }
    //}
}
