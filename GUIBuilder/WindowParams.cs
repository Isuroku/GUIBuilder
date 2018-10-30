using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUIBuilder
{
    public enum EWindowParams
    {
        Name,
        Type
    }

    public enum EBaseParamType
    {
        String,
        Float,
        Bool,
        WindowType
    }

    public abstract class CBaseParam
    {
        public abstract EBaseParamType GetParamType();
        
        public virtual float ToFloat() { return 0; }
        public virtual bool ToBool() { return false; }
        public virtual EWindowType ToWindowType() { return EWindowType.Undefined; }

        public virtual void SetValue(string inNewValue) { throw new NotImplementedException(); }
        public virtual void SetValue(float inNewValue) { throw new NotImplementedException(); }
        public virtual void SetValue(bool inNewValue) { throw new NotImplementedException(); }
        public virtual void SetValue(EWindowType inNewValue) { throw new NotImplementedException(); }
    }

    public class CStringParam: CBaseParam
    {
        string _value;

        public override string ToString() { return _value; }
        public override EBaseParamType GetParamType() { return EBaseParamType.String; }
        public CStringParam(string value) { _value = value; }

        public static CStringParam empty = new CStringParam(string.Empty);

        public override void SetValue(string inNewValue) { _value = inNewValue; }
    }

    public class CFloatParam : CBaseParam
    {
        float _value;

        public override string ToString() { return _value.ToString(); }
        public override EBaseParamType GetParamType() { return EBaseParamType.Float; }
        public override float ToFloat() { return _value; }
        public CFloatParam(float value) { _value = value; }

        public override void SetValue(float inNewValue) { _value = inNewValue; }
    }

    public class CBoolParam : CBaseParam
    {
        bool _value;

        public override string ToString() { return _value.ToString(); }
        public override EBaseParamType GetParamType() { return EBaseParamType.Bool; }
        public override bool ToBool() { return _value; }
        public CBoolParam(bool value) { _value = value; }

        public override void SetValue(bool inNewValue) { _value = inNewValue; }
    }

    public class CWindowTypeParam : CBaseParam
    {
        EWindowType _value;

        public override string ToString() { return _value.ToString(); }
        public override EBaseParamType GetParamType() { return EBaseParamType.WindowType; }
        public override EWindowType ToWindowType() { return _value; }
        public CWindowTypeParam(EWindowType value) { _value = value; }

        public override void SetValue(EWindowType inNewValue) { _value = inNewValue; }
    }
}
