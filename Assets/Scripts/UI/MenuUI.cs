using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.UI
{
    public class MenuUI : MonoBehaviour
    {
        [SerializeField] private GameObject _buttonPlay;
        [SerializeField] private GameObject _textLoading;

        private void Start()
        {
            SwitchUIElements(false);
        }

        public void LoadGameScene()
        {
            SceneManager.LoadScene(1);
            SwitchUIElements(true);
        }

        private void SwitchUIElements(bool isPlay)
        {
            _buttonPlay.SetActive(!isPlay);
            _textLoading.SetActive(isPlay);
        }
    }
}
