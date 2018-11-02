using System;
using System.Collections.Generic;

namespace GUIBuilder
{
    //public enum EWindowParams
    //{
    //    Name,
    //    Type,
    //    Text,
    //    Pivot_By_Parent,
    //    Size,
    //    Pivot_In_Own_Rect,
    //    Color
    //}

    public enum EBaseParamType
    {
        String,
        Float,
        Bool,
        PVector2,
        Vector4
    }

    public abstract class CBaseParam
    {
        public abstract EBaseParamType GetParamType();
        
        public virtual float        ToFloat()                   { throw new NotImplementedException(); }
        public virtual bool         ToBool()                    { throw new NotImplementedException(); }
        public virtual PVector2     ToPVector2()                { throw new NotImplementedException(); }
        public virtual Vector4      ToVector4()                 { throw new NotImplementedException(); }

        public virtual bool SetValue(string inNewValue)         { throw new NotImplementedException(); }
        public virtual bool SetValue(float inNewValue)          { throw new NotImplementedException(); }
        public virtual bool SetValue(bool inNewValue)           { throw new NotImplementedException(); }
        public virtual bool SetValue(PVector2 inNewValue)       { throw new NotImplementedException(); }
        public virtual bool SetValue(Vector4 inNewValue)        { throw new NotImplementedException(); }

        public virtual bool SetValue(string[] inNewValue)       { throw new NotImplementedException(); }

        static int[] pcount_1 = { 1 };
        static int[] pcount_2 = { 2 };
        static int[] pcount_3_4 = { 3, 4 };

        public static int[] GetParamCount(EBaseParamType inParamType)
        {
            switch(inParamType)
            {
                case EBaseParamType.Vector4: return pcount_3_4;
                case EBaseParamType.PVector2: return pcount_2;
            }
            return pcount_1;
        }

        internal static CBaseParam CreateParam(EBaseParamType inParamType, string[] inNewValue)
        {
            CBaseParam p = null;

            switch (inParamType)
            {
                case EBaseParamType.Vector4: p = new CVector4Param(); break;
                case EBaseParamType.PVector2: p = new CPVectorParam(); break;
                case EBaseParamType.Float: p = new CFloatParam(); break;
                case EBaseParamType.Bool: p = new CBoolParam(); break;
                case EBaseParamType.String: p = new CStringParam(); break;
            }

            if(inNewValue != null)
                p.SetValue(inNewValue);

            return p;
        }
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

        public override bool SetValue(string[] inNewValue)
        {
            if(inNewValue.Length > 0)
                return SetValue(inNewValue[0]);
            return false;
        }
    }

    public class CPVectorParam : CBaseParam
    {
        PVector2 _value;

        public override string ToString() { return _value.ToString(); }
        public override EBaseParamType GetParamType() { return EBaseParamType.PVector2; }
        public override PVector2 ToPVector2() { return _value; }

        public CPVectorParam() { }
        public CPVectorParam(PVector2 value) { _value = value; }

        public override bool SetValue(PVector2 inNewValue)
        {
            bool diff = _value != inNewValue;
            if (diff)
                _value = inNewValue;
            return diff;
        }

        public override bool SetValue(string[] inNewValue)
        {
            PValue x, y;
            if (PValue.TryParse(inNewValue[0], out x) && PValue.TryParse(inNewValue[1], out y))
                return SetValue(new PVector2(x, y));

            return false;
        }
    }

    public class CVector4Param : CBaseParam
    {
        Vector4 _value;

        public override string ToString() { return _value.ToString(); }
        public override EBaseParamType GetParamType() { return EBaseParamType.Vector4; }
        public override Vector4 ToVector4() { return _value; }
        public CVector4Param() { }
        public CVector4Param(Vector4 value) { _value = value; }

        public override bool SetValue(Vector4 inNewValue)
        {
            bool diff = _value != inNewValue;
            if (diff)
                _value = inNewValue;
            return diff;
        }

        public override bool SetValue(string[] inNewValue)
        {
            float x, y, z, w = 0;
            if (inNewValue.Length > 3)
            {
                if (float.TryParse(inNewValue[0], out x) && float.TryParse(inNewValue[1], out y)
                    && float.TryParse(inNewValue[2], out z) && float.TryParse(inNewValue[3], out w))
                    return SetValue(new Vector4(x, y, z, w));
            }
            else if (inNewValue.Length > 2)
            {
                if (float.TryParse(inNewValue[0], out x) && float.TryParse(inNewValue[1], out y) && float.TryParse(inNewValue[2], out z))
                    return SetValue(new Vector4(x, y, z, 255));
            }

            return false;
        }
    }

    public class CFloatParam : CBaseParam
    {
        float _value;

        public override string ToString() { return _value.ToString(); }
        public override EBaseParamType GetParamType() { return EBaseParamType.Float; }
        public override float ToFloat() { return _value; }
        public CFloatParam(float value) { _value = value; }
        public CFloatParam() { }

        public override bool SetValue(float inNewValue)
        {
            bool diff = _value != inNewValue;
            if (diff)
                _value = inNewValue;
            return diff;
        }

        public override bool SetValue(string[] inNewValue)
        {
            if (inNewValue.Length > 0)
            {
                if (float.TryParse(inNewValue[0], out _value))
                    return SetValue(_value);
            }

            return false;
        }
    }

    public class CBoolParam : CBaseParam
    {
        bool _value;

        public override string ToString() { return _value.ToString(); }
        public override EBaseParamType GetParamType() { return EBaseParamType.Bool; }
        public override bool ToBool() { return _value; }
        public CBoolParam(bool value) { _value = value; }
        public CBoolParam() { }

        public override bool SetValue(bool inNewValue)
        {
            bool diff = _value != inNewValue;
            if (diff)
                _value = inNewValue;
            return diff;
        }

        public override bool SetValue(string[] inNewValue)
        {
            if (inNewValue.Length > 0)
            {
                if (bool.TryParse(inNewValue[0], out _value))
                    return SetValue(_value);
            }

            return false;
        }
    }

}
