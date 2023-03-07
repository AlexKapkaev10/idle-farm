using Scripts.Interfaces;
using UnityEngine;

namespace Scripts
{
    public class SowingCell : MonoBehaviour, IInteractable
    {
        [SerializeField]
        private Collider _collider;

        public void Interact()
        {
            _collider.enabled = false;
            Debug.Log("Interact");
        }
    }
}
