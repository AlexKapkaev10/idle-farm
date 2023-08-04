using System;
using System.Collections;
using UnityEngine;

namespace Scripts.Game
{
    public class AdController : MonoBehaviour
    {
        public event Action OnRewardedShow;
        private int _showRewardedCount = default;

        public int ShowRewardedCount
        {
            get => _showRewardedCount;
            set => _showRewardedCount = value;
        }

        public void ShowRewardedVideo()
        {
            _showRewardedCount++;
            StartCoroutine(EndShowVideoDelay());
        }

        private IEnumerator EndShowVideoDelay()
        {
            yield return new WaitForSeconds(2);
            OnRewardedShow?.Invoke();
            OnRewardedShow = null;
        }
    }
}