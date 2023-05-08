namespace Scripts.Interfaces
{
    public interface ICharacterBehavior
    {
        public string AnimationKey { get; }
        public void Enter();
        public void Exit();
        public void Update();
        public void FixedUpdate();
    }
}