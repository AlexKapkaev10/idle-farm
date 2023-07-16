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

        public PlantType PlantType
        {
            get => _plantType;
            set => _plantType = value;
        }

        public void MoveToTarget(Transform parent, float duration)
        {
            transform.DOLocalMoveY(1.5f, 0.2f).SetEase(Ease.Linear).OnComplete(() =>
            {
                transform.SetParent(parent);
                Sequence seq = DOTween.Sequence();
                seq.Append(transform.DOLocalMove(Vector3.zero, duration / 2).SetEase(Ease.Linear))
                    .Join(transform.DOLocalRotateQuaternion(Quaternion.identity, duration / 2))
                    .Join(transform.DOScale(0, duration).SetEase(Ease.Linear));
                seq.OnComplete(() => OnBlockReturn?.Invoke(this));
            });
        }
    }
}