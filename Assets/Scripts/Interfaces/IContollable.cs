using UnityEngine;

namespace Scripts.Interfaces
{
    public interface IContollable
    {
        public void UpdateMove(Vector3 direction);
        public void StartMove();
        public void StopMove();
    }
}
