using System;

namespace GUIBuilder
{
    public struct PValue : IComparable<PValue>
    {
        float _value;
        bool _percent;

        public float Value { get { return _value; } }
        public bool Percent { get { return _percent; } }

        public static PValue zero = new PValue();

        public PValue(float value, bool percent)
        {
            _value = value;
            _percent = percent;
        }

        public PValue(float value): this(value, false)
        {
        }

        public override string ToString()
        {
            if(_percent)
                return string.Format("{0}%", _value);
            return string.Format("{0}", _value);
        }

        public int CompareTo(PValue other)
        {
            if (_percent != other._percent)
                return _percent.CompareTo(other._percent);
            return _value.CompareTo(other._value);
        }

        public bool Equals(PValue other)
        {
            return _percent == other._percent && Math.Abs(_value - other._value) < 0.000001;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PValue)obj);
        }

        public override int GetHashCode()
        {
            return _percent.GetHashCode() * 37 + _value.GetHashCode();
        }

        public static bool operator ==(PValue left, PValue right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(PValue left, PValue right)
        {
            return !left.Equals(right);
        }

        public static bool TryParse(string str, out PValue value)
        {
            value = PValue.zero;

            if (string.IsNullOrEmpty(str))
                return false;

            str = str.Trim();

            bool percent = false;
            if(str[str.Length - 1] == '%')
            {
                percent = true;
                str = str.Substring(0, str.Length - 1);
            }

            float v;
            if (!float.TryParse(str, out v))
                return false;

            value = new PValue(v, percent);
            return true;
        }
    }
}
