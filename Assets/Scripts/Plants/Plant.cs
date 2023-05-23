using System;
using DG.Tweening;
using Scripts.Enums;
using UnityEngine;

namespace Scripts.Plants
{
    public class Plant : MonoBehaviour
    {
        public event Action<Plant> OnBlockReturn;

        [SerializeField] private PlantType _plantType;
        [SerializeField] private Transform _blockTransform;

        public PlantType PlantType
        {
            get => _plantType;
            set => _plantType = value;
        }
        public Transform BlockTransform => _blockTransform;
        public MonoBehaviour GetMonoBehaviour => this;
        
        public void MoveToTarget(Transform parent, float duration, bool isCharacterTarget)
        {
            if (isCharacterTarget)
            {
                transform.DOLocalMoveY(1.5f, 0.2f).SetEase(Ease.Linear).OnComplete(() =>
                {
                    transform.SetParent(parent);
                    Sequence seq = DOTween.Sequence();
                    seq.Append(transform.DOLocalMove(Vector3.zero, duration / 2).SetEase(Ease.Linear))
                        .Join(transform.DOLocalRotateQuaternion(Quaternion.identity, duration / 2))
                        .Join(transform.DOScale(0, duration).SetEase(Ease.Linear));
                });
            }
            else
            {
                transform.SetParent(parent);
                var seq = DOTween.Sequence();
                
                seq.Join(transform.DOLocalMoveX(0f, duration).SetEase(Ease.Linear))
                    .Join(transform.DOLocalMoveZ(0f, duration).SetEase(Ease.Linear))
                    .Join(transform.DOScale(1f, duration).SetEase(Ease.Linear));
                seq.OnComplete(() => OnBlockReturn?.Invoke(this));
            }
        }
    }
}