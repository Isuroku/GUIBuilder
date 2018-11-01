using System;

namespace GUIBuilder
{
    public struct Vector2 : IComparable<Vector2>
    {
        public float x;
        public float y;

        public Vector2(Vector2 inVector)
        {
            x = inVector.x;
            y = inVector.y;
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

        public static Vector2 operator -(Vector2 left, Vector2 right)
        {
            return new Vector2(left.x - right.x, left.y - right.y);
        }
    }

    public struct Vector4 : IComparable<Vector4>
    {
        public float x;
        public float y;
        public float z;
        public float w;

        public Vector4(Vector4 inVector)
        {
            x = inVector.x;
            y = inVector.y;
            z = inVector.z;
            w = inVector.w;
        }

        public Vector4(float inx, float iny, float inz, float inw)
        {
            x = inx;
            y = iny;
            z = inz;
            w = inw;
        }

        public override string ToString()
        {
            return string.Format("{0}:{1}:{2}:{3}", x, y, z, w);
        }

        public int CompareTo(Vector4 other)
        {
            if (x != other.x)
                return x.CompareTo(other.x);
            if (y != other.y)
                return y.CompareTo(other.y);
            if (z != other.z)
                return z.CompareTo(other.z);
            return w.CompareTo(other.w);
        }

        public bool Equals(Vector4 other)
        {
            return x == other.x && y == other.y && z == other.z && w == other.w;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Vector4)obj);
        }

        public override int GetHashCode()
        {
            return x.GetHashCode() * 37 + y.GetHashCode() * 11 + z.GetHashCode() * 39 + w.GetHashCode();
        }

        public static bool operator ==(Vector4 left, Vector4 right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Vector4 left, Vector4 right)
        {
            return !left.Equals(right);
        }

        public static Vector4 operator +(Vector4 left, Vector4 right)
        {
            return new Vector4(left.x + right.x, left.y + right.y, left.z + right.z, left.w + right.w);
        }

        public static Vector4 operator -(Vector4 left, Vector4 right)
        {
            return new Vector4(left.x - right.x, left.y - right.y, left.z - right.z, left.w - right.w);
        }
    }
}
