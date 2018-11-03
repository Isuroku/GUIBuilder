using System;

namespace GUIBuilder
{
    public struct SColor : IComparable<SColor>
    {
        private byte inv_a;
        public byte a {
            get { return (byte)(255 - inv_a); }
            set { inv_a = (byte)(255 - value); }
        }

        private byte inv_r;
        public byte r
        {
            get { return (byte)(255 - inv_r); }
            set { inv_r = (byte)(255 - value); }
        }

        private byte inv_g;
        public byte g
        {
            get { return (byte)(255 - inv_g); }
            set { inv_g = (byte)(255 - value); }
        }

        private byte inv_b;
        public byte b
        {
            get { return (byte)(255 - inv_b); }
            set { inv_b = (byte)(255 - value); }
        }

        public SColor(SColor inColor)
        {
            inv_r = inColor.inv_r;
            inv_g = inColor.inv_g;
            inv_b = inColor.inv_b;
            inv_a = inColor.inv_a;
        }

        public SColor(byte inr, byte ing, byte inb, byte ina)
        {
            inv_r = (byte)(255 - inr);
            inv_g = (byte)(255 - ing);
            inv_b = (byte)(255 - inb);
            inv_a = (byte)(255 - ina);
        }

        public override string ToString()
        {
            return string.Format("{0}:{1}:{2}:{3}", r, g, b, a);
        }

        public int CompareTo(SColor other)
        {
            if (r != other.r)
                return r.CompareTo(other.r);
            if (g != other.g)
                return g.CompareTo(other.g);
            if (b != other.b)
                return b.CompareTo(other.b);
            return a.CompareTo(other.a);
        }

        public bool Equals(SColor other)
        {
            return r == other.r && g == other.g && b == other.b && a == other.a;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((SColor)obj);
        }

        public override int GetHashCode()
        {
            return r.GetHashCode() * 37 + g.GetHashCode() * 11 + b.GetHashCode() * 39 + a.GetHashCode();
        }

        public static bool operator ==(SColor left, SColor right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(SColor left, SColor right)
        {
            return !left.Equals(right);
        }
    }
}
