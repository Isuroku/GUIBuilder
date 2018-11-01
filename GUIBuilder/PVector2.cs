using System;

namespace GUIBuilder
{
    public struct PVector2 : IComparable<PVector2>
    {
        public PValue x { get; private set; }
        public PValue y { get; private set; }

        public static PVector2 zero = new PVector2();

        public PVector2(PValue inX, PValue inY)
        {
            x = inX;
            y = inY;
        }

        public PVector2(float inX, float inY): this(new PValue(inX), new PValue(inY))
        {
        }

        public override string ToString()
        {
            return string.Format("{0}:{1}", x, y);
        }

        public int CompareTo(PVector2 other)
        {
            if (x != other.x)
                return x.CompareTo(other.x);
            return y.CompareTo(other.y);
        }

        public Vector2 GetVector(Rect parent_rect)
        {
            float dx = x.Value;
            if (x.Percent)
                dx = parent_rect.Width * x.Value / 100;

            float dy = y.Value;
            if (y.Percent)
                dy = parent_rect.Hight * y.Value / 100;

            return new Vector2(dx, dy);
        }

        public Vector2 GetVector(Vector2 size)
        {
            float dx = x.Value;
            if (x.Percent)
                dx = size.x * x.Value / 100;

            float dy = y.Value;
            if (y.Percent)
                dy = size.y * y.Value / 100;

            return new Vector2(dx, dy);
        }

        public bool Equals(PVector2 other)
        {
            return x == other.x && y == other.y;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PVector2)obj);
        }

        public override int GetHashCode()
        {
            return x.GetHashCode() * 37 + y.GetHashCode();
        }

        public static bool operator ==(PVector2 left, PVector2 right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(PVector2 left, PVector2 right)
        {
            return !left.Equals(right);
        }
    }
}
