using Scripts.Interfaces;

namespace Scripts.StateMachine
{
    public interface ICharacterStateMachine
    {
        public ICharacterBehavior CurrentBehavior { get; }
        public void InitBehaviors(ICharacterController characterController);
        public void SetBehaviorByType(CharacterStateType type);
    }
}