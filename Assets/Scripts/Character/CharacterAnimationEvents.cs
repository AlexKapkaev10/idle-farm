using System;
using UnityEngine;

namespace Scripts
{
    public class CharacterAnimationEvents : MonoBehaviour
    {
        public event Action OnMow;
        public void Mow()
        {
            OnMow?.Invoke();
        }
    }
}
