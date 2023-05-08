using UnityEngine;

namespace Scripts.Interfaces
{
    public interface IControllable
    {
        public float Speed { get; }
        public Transform Body { get; }
        public void StartMove();
        public void StopMove();
    }
}
