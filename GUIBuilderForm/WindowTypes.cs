using GUIBuilder;

namespace GUIBuilderForm
{
    static class WindowParams
    {
        public static SWindowParamDescr Text = new SWindowParamDescr(0, "Text", EBaseParamType.String);
    }

    static class WindowTypes
    {
        public static CWindowTypeDescr Panel = new CWindowTypeDescr(0, "Panel");
    }
}
