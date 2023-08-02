using UnityEngine;

namespace Scripts.Game
{
    public interface IBobController
    {
        public void SwitchAnimation(BobAnimationType type);
        public void SetTransform(Vector3 position, Vector3 bodyRotation);
    }
}