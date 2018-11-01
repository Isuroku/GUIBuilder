using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUIBuilder
{
    class CWindowRect
    {
        PVector2 _pivot_by_parent;
        PVector2 _pivot_in_own_rect;
        PVector2 _size;

        //return global rect
        public Rect GetRect(IFrame inParent)
        {
            Rect parent_rect;
            if(inParent != null)
                parent_rect = inParent.GetClientRect(); //global rect
            else
                parent_rect = new Rect();

            Vector2 parent_shift = _pivot_by_parent.GetVector(parent_rect);

            Vector2 c = parent_rect.Min + parent_shift; //global pos

            Vector2 sz = _size.GetVector(parent_rect);

            Vector2 own_shift = _pivot_in_own_rect.GetVector(sz);

            Vector2 lt = c - own_shift;

            return new Rect(lt.x, lt.y, lt.x + sz.x, lt.y + sz.y);
        }
    }

    //class CWindowRect
    //{
    //    Anchor _anchor = new Anchor();

    //    Rect _rect;

    //    ESizeType _size_type;

    //    public Rect GetRect(IFrame inParent)
    //    {
    //        Rect anchor_rect = _anchor.GetRect(inParent);

    //        float l = _rect.GetSizeInUnits(0, _size_type, anchor_rect);
    //        l = anchor_rect.left + l;
    //        float t = _rect.GetSizeInUnits(1, _size_type, anchor_rect);
    //        t = anchor_rect.top + t;

    //        float r = 0;
    //        if (_anchor.AnchorWidthType == EAnchorDimType.Point)
    //            r = l + _rect.GetWidthInUnits(_size_type, anchor_rect);
    //        else if (_anchor.AnchorWidthType == EAnchorDimType.Strech)
    //            r = anchor_rect.right + _rect.GetSizeInUnits(2, _size_type, anchor_rect);

    //        float b = 0;
    //        if (_anchor.AnchorHightType == EAnchorDimType.Point)
    //            b = t + _rect.GetHeightInUnits(_size_type, anchor_rect);
    //        else if (_anchor.AnchorHightType == EAnchorDimType.Strech)
    //            r = anchor_rect.bottom + _rect.GetSizeInUnits(3, _size_type, anchor_rect);

    //        return new Rect(l, t, r, b);
    //    }
    //}
}
