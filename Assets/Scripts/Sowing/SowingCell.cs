using Scripts.Enums;
using Scripts.Interfaces;
using System.Collections;
using UnityEngine;

namespace Scripts
{
    public class SowingCell : MonoBehaviour, IInteractable
    {
        [SerializeField]
        private Collider _collider;

        private SowingType _sowingType;
        private WaitForSeconds _ripeningWaitDelay;

        public void Initit(SowingType type, WaitForSeconds ripeningWait)
        {
            _ripeningWaitDelay = ripeningWait;
            _sowingType = type;
        }

        public void Interact()
        {
            _collider.enabled = false;
            StartCoroutine(Ripening());
            Debug.Log("Interact");
        }

        private IEnumerator Ripening()
        {
            yield return _ripeningWaitDelay;
            Debug.Log("Ripenin");
        }
    }
}
