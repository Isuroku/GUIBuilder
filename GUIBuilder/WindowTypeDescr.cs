using System;
using System.Collections;
using System.Collections.Generic;

namespace GUIBuilder
{
    public struct SWindowParamDescr
    {
        public NamedId Id { get; private set; }
        public EBaseParamType ParamType { get; private set; }
        public bool IsChildInfluence { get; private set; }

        public SWindowParamDescr(uint inId, string inName, EBaseParamType inParamType, bool inIsChildInfluence)
        {
            Id = new NamedId(inId, inName);
            ParamType = inParamType;
            IsChildInfluence = inIsChildInfluence;
        }

        public override string ToString()
        {
            return string.Format("{0}:{1}", Id, ParamType);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public bool Equals(SWindowParamDescr other)
        {
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((SWindowParamDescr)obj);
        }

        public static bool operator ==(SWindowParamDescr left, SWindowParamDescr right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(SWindowParamDescr left, SWindowParamDescr right)
        {
            return !left.Equals(right);
        }

        public static SWindowParamDescr Name = new SWindowParamDescr(uint.MaxValue - 1, "Name", EBaseParamType.String, false);
        public static SWindowParamDescr Indent = new SWindowParamDescr(uint.MaxValue - 2, "Indent", EBaseParamType.PVector2, true);
        public static SWindowParamDescr Shift = new SWindowParamDescr(uint.MaxValue - 3, "Shift", EBaseParamType.PVector2, true);
        public static SWindowParamDescr Size = new SWindowParamDescr(uint.MaxValue - 4, "Size", EBaseParamType.PVector2, true);
        public static SWindowParamDescr Color = new SWindowParamDescr(uint.MaxValue - 5, "Color", EBaseParamType.Vector4, false);
    }

    public class CWindowTypeDescr: IEnumerable<SWindowParamDescr>
    {
        public NamedId Id { get; private set; }
        List<SWindowParamDescr> _params = new List<SWindowParamDescr>();
        HashSet<NamedId> _must_be = new HashSet<NamedId>();

        public CWindowTypeDescr(uint inId, string inName)
        {
            Id = new NamedId(inId, inName);
            AddParams(SWindowParamDescr.Name, false);
            AddParams(SWindowParamDescr.Indent, false);
            AddParams(SWindowParamDescr.Shift, false);
            AddParams(SWindowParamDescr.Size, true);
            AddParams(SWindowParamDescr.Color, false);
        }

        public void AddParams(uint inId, string inName, EBaseParamType inParamType, bool inParamMustBe, bool inIsChildInfluence)
        {
            var p = new SWindowParamDescr(inId, inName, inParamType, inIsChildInfluence);
            AddParams(p, inParamMustBe);
        }

        public void AddParams(SWindowParamDescr param, bool inParamMustBe)
        {
            _params.Add(param);
            if (inParamMustBe)
                _must_be.Add(param.Id);
        }

        public bool IsMustBe(NamedId Id)
        {
            return _must_be.Contains(Id);
        }

        public IEnumerator<SWindowParamDescr> GetEnumerator()
        {
            return _params.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _params.GetEnumerator();
        }

        public SWindowParamDescr? GetWindowParamDescr(NamedId Id)
        {
            for (int i = 0; i < _params.Count; ++i)
            {
                if (_params[i].Id == Id)
                    return _params[i];
            }
            return null;
        }

        public override string ToString()
        {
            return string.Format("{0}", Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CWindowTypeDescr)obj);
        }

        public bool Equals(CWindowTypeDescr other)
        {
            return Id == other.Id;
        }

        public static bool operator ==(CWindowTypeDescr left, CWindowTypeDescr right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(CWindowTypeDescr left, CWindowTypeDescr right)
        {
            return !left.Equals(right);
        }
    }



    public class CWindowTypeDescrs
    {
        List<CWindowTypeDescr> _types = new List<CWindowTypeDescr>();

        public CWindowTypeDescrs()
        {
            _types.Add(new CWindowTypeDescr(uint.MaxValue - 1, "Frame"));
        }

        public void AddType(CWindowTypeDescr inTypeDescr)
        {
            _types.Add(inTypeDescr);
        }

        public CWindowTypeDescr GetDescr(NamedId Id)
        {
            for(int i = 0; i < _types.Count; ++i)
            {
                if (_types[i].Id == Id)
                    return _types[i];
            }
            return null;
        }

        internal NamedId? GetWinType(string stype)
        {
            for (int i = 0; i < _types.Count; ++i)
            {
                if (string.Equals(_types[i].Id.Name, stype, StringComparison.OrdinalIgnoreCase))
                    return _types[i].Id;
            }
            return null;
        }
    }
}
