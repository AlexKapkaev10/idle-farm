using UnityEngine;

namespace Scripts.Game
{
    public class BobController : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        public void SwitchAnimation(BobAnimationType type)
        {
            switch (type)
            {
                case BobAnimationType.Idle:
                    _animator?.SetTrigger(AnimatorParameters.BobIdle);
                    break;
                case BobAnimationType.NotComplete:
                    _animator?.SetTrigger(AnimatorParameters.BobNotComplete);
                    break;
                case BobAnimationType.Win:
                    _animator?.SetTrigger(AnimatorParameters.BobWin);
                    break;
                case BobAnimationType.Lose:
                    _animator?.SetTrigger(AnimatorParameters.BobLose);
                    break;
            }
        }
    }
    
    public static partial class AnimatorParameters
    {
        public static readonly int BobIdle = Animator.StringToHash("idle");
        public static readonly int BobNotComplete = Animator.StringToHash("notComplete");
        public static readonly int BobWin = Animator.StringToHash("win");
        public static readonly int BobLose = Animator.StringToHash("lose");
    }
}
