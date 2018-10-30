using CascadeParser;

namespace GUIBuilder
{
    struct SWinKeyInfo
    {
        public IKey WinKey;
        public string Name;
        public EWindowType WinType;

        public override string ToString()
        {
            return string.Format("{0} [{1}]", WinType, Name);
        }
    }
}
