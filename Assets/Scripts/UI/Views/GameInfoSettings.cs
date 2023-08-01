using UnityEngine;

namespace Scripts.UI
{
    [CreateAssetMenu(fileName = nameof(GameInfoSettings), menuName = "SO/GameInfoSettings")]
    public class GameInfoSettings : ScriptableObject
    {
        [SerializeField] private Color _colorDefaultTimer;
        [SerializeField] private Color _colorHurryTimer;
        [SerializeField] private float _infoFadeDuration = 0.2f;

        public Color ColorDefaultTimer => _colorDefaultTimer;
        public Color ColorHurryTimer => _colorHurryTimer;
        public float InfoFadeDuration => _infoFadeDuration;
    }
}