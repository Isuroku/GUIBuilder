
namespace GUIBuilder
{
    public struct NamedId
    {
        public uint Id { get; private set; }
        public string Name { get; private set; }

        public NamedId(uint inId, string inNames)
        {
            Id = inId;
            Name = inNames;
        }

        public override string ToString()
        {
            return string.Format("{0} [{1}]", Name, Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((NamedId)obj);
        }

        public bool Equals(NamedId other)
        {
            return Id == other.Id;
        }

        public static bool operator ==(NamedId left, NamedId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(NamedId left, NamedId right)
        {
            return !left.Equals(right);
        }
    }
}
