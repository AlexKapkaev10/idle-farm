using DG.Tweening;
using System;
using UnityEngine;

namespace Scripts
{
    public class PlantBlock : MonoBehaviour
    {
        public event Action<PlantBlock> OnBlockReturn;
        [SerializeField]
        private Transform _block;

        public Transform GetBlockTransform()
        {
            return _block;
        }

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
