using Scripts.Enums;

namespace Scripts.Interfaces
{
    public interface ITool
    {
        public ToolType ToolType { get; }
        public float MowSpeed { get; }
        public void SetActive(bool value);
    }
}