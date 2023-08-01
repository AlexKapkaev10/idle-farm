using System;
using DG.Tweening;
using Scripts.Enums;
using UnityEngine;

namespace Scripts.Plants
{
    public class PlantBlock : MonoBehaviour
    {
        public event Action<PlantBlock> OnBlockReturn;

        [SerializeField] private PlantType _plantType;

        public PlantType PlantType
        {
            get => _plantType;
            set => _plantType = value;
        }

        public void MoveToTarget(Transform parent, float duration)
        {
            transform.DOScale(1, 0.2f).SetEase(Ease.OutBounce).OnComplete(() =>
            {
                transform.SetParent(parent);
                Sequence seq = DOTween.Sequence();
                seq.Append(transform.DOLocalMove(Vector3.zero, duration / 2).SetEase(Ease.Linear))
                    .Join(transform.DOLocalRotateQuaternion(Quaternion.identity, duration / 2))
                    .Join(transform.DOScale(0, duration).SetEase(Ease.Linear));
                seq.OnComplete(() =>
                {
                    OnBlockReturn?.Invoke(this);
                    DOTween.Kill(transform);
                    DOTween.Kill(seq);
                });
            });
        }
    }
}