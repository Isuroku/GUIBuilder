using System;

namespace GUIBuilder
{
    public struct Rect
    {
        public float left;
        public float top;
        public float right;
        public float bottom;

        public Vector2 Min { get { return new Vector2(left, top); } }
        public Vector2 Max { get { return new Vector2(right, bottom); } }

        public float Width { get { return right - left; } }
        public float Hight { get { return bottom - top; } }

        public static Rect zero = new Rect(0, 0, 0, 0);

        public Rect(float left, float top, float right, float bottom) : this()
        {
            this.left = left;
            this.top = top;
            this.right = right;
            this.bottom = bottom;
        }

        public override string ToString()
        {
            return string.Format("l {0}; t {1}; r {2}; b {3}",
                left, top, right, bottom);
        }

        public float this[int index]
        {
            get
            {
                switch(index)
                {
                    case 0: return left;
                    case 1: return top;
                    case 2: return right;
                    case 3: return bottom;
                }
                throw new ArgumentOutOfRangeException(string.Format("index in 0..3, but {0}", index));
            }
            set
            {
                switch (index)
                {
                    case 0: left = value; break;
                    case 1: top = value; break;
                    case 2: right = value; break;
                    case 3: bottom = value; break;
                }
                throw new ArgumentOutOfRangeException(string.Format("index in 0..3, but {0}", index));
            }
        }

        public static bool TestRectRect(Rect aabb1, Rect aabb2)
        {
            return !(aabb1.left > aabb2.right || aabb1.right < aabb2.left
                     || aabb1.top > aabb2.bottom || aabb1.bottom < aabb2.top);
        }

        public static bool TestRectPoint(Rect aabb1, Vector2 point)
        {
            return !(aabb1.left > point.x || aabb1.right < point.x
                     || aabb1.top > point.y || aabb1.bottom < point.y);
        }

        public float GetSizeInUnits(int inSizeIndex, ESizeType inSizeType, Rect inParentRect)
        {
            float l = this[inSizeIndex];
            if (inSizeType == ESizeType.Percents)
            {
                if (inSizeIndex == 0 || inSizeIndex == 2)
                    l = inParentRect.Width * l;
                else
                    l = inParentRect.Hight * l;
            }
            return l;
        }

        public float GetWidthInUnits(ESizeType inSizeType, Rect inParentRect)
        {
            float l = Width;
            if (inSizeType == ESizeType.Percents)
                l = inParentRect.Width * l;
            return l;
        }

        public float GetHeightInUnits(ESizeType inSizeType, Rect inParentRect)
        {
            float l = Hight;
            if (inSizeType == ESizeType.Percents)
                l = inParentRect.Hight * l;
            return l;
        }
    }
}
