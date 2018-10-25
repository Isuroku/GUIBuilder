
namespace GUIBuilder
{
    enum EAnchorDimType { Point, Strech }
    public enum ESizeType { Units, Percents }

    struct Anchor
    {
        public EAnchorDimType AnchorWidthType { get { return _width_type; } }
        public EAnchorDimType AnchorHightType { get { return _hight_type; } }
        public ESizeType SizeType { get { return _size_type; } }

        EAnchorDimType _width_type;
        EAnchorDimType _hight_type;
        ESizeType _size_type;

        Rect _rect;

        public Rect GetRect(IFrame inParent)
        {
            Rect pr = inParent.GetClientRect();

            float l = _rect.GetSizeInUnits(0, _size_type, pr);
            l = pr.left + l;
            float t = _rect.GetSizeInUnits(1, _size_type, pr);
            t = pr.top + t;

            float r = l;
            if(_width_type == EAnchorDimType.Strech)
                r = pr.right + _rect.GetSizeInUnits(2, _size_type, pr);

            float b = t;
            if (_hight_type == EAnchorDimType.Strech)
                b = pr.bottom + _rect.GetSizeInUnits(3, _size_type, pr);

            return new Rect(l, t, r, b);
        }
    }
}
