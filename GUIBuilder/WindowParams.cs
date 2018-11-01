using System;
using System.Collections.Generic;

namespace GUIBuilder
{
    public enum EWindowParams
    {
        Name,
        Type,
        Text,
        Pivot_By_Parent,
        Size,
        Pivot_In_Own_Rect,
        Color
    }

    public enum EBaseParamType
    {
        String,
        Float,
        Bool,
        WindowType,
        PVector2,
        Vector4
    }

    public abstract class CBaseParam
    {
        public abstract EBaseParamType GetParamType();
        
        public virtual float        ToFloat()                   { throw new NotImplementedException(); }
        public virtual bool         ToBool()                    { throw new NotImplementedException(); }
        public virtual EWindowType  ToWindowType()              { throw new NotImplementedException(); }
        public virtual PVector2     ToPVector2()                { throw new NotImplementedException(); }
        public virtual Vector4      ToVector4()                 { throw new NotImplementedException(); }

        public virtual bool SetValue(string inNewValue)         { throw new NotImplementedException(); }
        public virtual bool SetValue(float inNewValue)          { throw new NotImplementedException(); }
        public virtual bool SetValue(bool inNewValue)           { throw new NotImplementedException(); }
        public virtual bool SetValue(EWindowType inNewValue)    { throw new NotImplementedException(); }
        public virtual bool SetValue(PVector2 inNewValue)       { throw new NotImplementedException(); }
        public virtual bool SetValue(Vector4 inNewValue)        { throw new NotImplementedException(); }
    }

    public class CStringParam: CBaseParam
    {
        string _value;

        public override string ToString() { return _value; }
        public override EBaseParamType GetParamType() { return EBaseParamType.String; }
        public CStringParam(): this(string.Empty) { }
        public CStringParam(string value) { _value = value; }

        public static CStringParam empty = new CStringParam(string.Empty);

        public override bool SetValue(string inNewValue)
        {
            bool diff = !string.Equals(_value, inNewValue, StringComparison.Ordinal);
            if (diff)
                _value = inNewValue;
            return diff;
        }
    }

    public class CPVectorParam : CBaseParam
    {
        PVector2 _value;

        public override string ToString() { return _value.ToString(); }
        public override EBaseParamType GetParamType() { return EBaseParamType.PVector2; }
        public override PVector2 ToPVector2() { return _value; }
        public CPVectorParam(PVector2 value) { _value = value; }

        public override bool SetValue(PVector2 inNewValue)
        {
            bool diff = _value != inNewValue;
            if (diff)
                _value = inNewValue;
            return diff;
        }
    }

    public class CVector4Param : CBaseParam
    {
        Vector4 _value;

        public override string ToString() { return _value.ToString(); }
        public override EBaseParamType GetParamType() { return EBaseParamType.Vector4; }
        public override Vector4 ToVector4() { return _value; }
        public CVector4Param(Vector4 value) { _value = value; }

        public override bool SetValue(Vector4 inNewValue)
        {
            bool diff = _value != inNewValue;
            if (diff)
                _value = inNewValue;
            return diff;
        }
    }

    public class CFloatParam : CBaseParam
    {
        float _value;

        public override string ToString() { return _value.ToString(); }
        public override EBaseParamType GetParamType() { return EBaseParamType.Float; }
        public override float ToFloat() { return _value; }
        public CFloatParam(float value) { _value = value; }

        public override bool SetValue(float inNewValue)
        {
            bool diff = _value != inNewValue;
            if (diff)
                _value = inNewValue;
            return diff;
        }
    }

    public class CBoolParam : CBaseParam
    {
        bool _value;

        public override string ToString() { return _value.ToString(); }
        public override EBaseParamType GetParamType() { return EBaseParamType.Bool; }
        public override bool ToBool() { return _value; }
        public CBoolParam(bool value) { _value = value; }

        public override bool SetValue(bool inNewValue)
        {
            bool diff = _value != inNewValue;
            if (diff)
                _value = inNewValue;
            return diff;
        }
    }

    public class CWindowTypeParam : CBaseParam
    {
        EWindowType _value;

        public override string ToString() { return _value.ToString(); }
        public override EBaseParamType GetParamType() { return EBaseParamType.WindowType; }
        public override EWindowType ToWindowType() { return _value; }
        public CWindowTypeParam(EWindowType value) { _value = value; }

        public override bool SetValue(EWindowType inNewValue)
        {
            bool diff = _value != inNewValue;
            if (diff)
                _value = inNewValue;
            return diff;
        }
    }
}
