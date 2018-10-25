using System;

namespace GUIBuilder
{
    public struct Vector2 : IComparable<Vector2>
    {
        public float x;
        public float y;

        public Vector2(Vector2 inCellCoords)
        {
            x = inCellCoords.x;
            y = inCellCoords.y;
        }

        public Vector2(float inx, float iny)
        {
            x = inx;
            y = iny;
        }

        public override string ToString()
        {
            return string.Format("{0}:{1}", x, y);
        }

        public int CompareTo(Vector2 other)
        {
            if (x != other.x)
                return x.CompareTo(other.x);
            return y.CompareTo(other.y);
        }

        public bool Equals(Vector2 other)
        {
            return x == other.x && y == other.y;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Vector2)obj);
        }

        public override int GetHashCode()
        {
            return x.GetHashCode() * 37 + y.GetHashCode();
        }

        public static bool operator ==(Vector2 left, Vector2 right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Vector2 left, Vector2 right)
        {
            return !left.Equals(right);
        }

        public static Vector2 operator +(Vector2 left, Vector2 right)
        {
            return new Vector2(left.x + right.x, left.y + right.y);
        }
    }
}
