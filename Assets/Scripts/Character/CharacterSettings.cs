using UnityEngine;

namespace Scripts.Game
{
    [CreateAssetMenu(fileName = nameof(CharacterSettings), menuName = "SO/Character Settings")]
    public sealed class CharacterSettings : ScriptableObject
    {
        [SerializeField] private float _runSpeed;

        public float RunSpeed => _runSpeed;
    }
}