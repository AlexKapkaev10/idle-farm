using Scripts.Enums;

namespace Scripts.Interfaces
{
    public interface ITool
    {
        public int CurrentSharpCount { get; set; }
        public ToolType ToolType { get; }
        public float MowSpeed { get; }
        public bool IsSharp();
        public void SetMaxSharpCount();
        public void SetActive(bool value);
        public void Clear();
    }
}