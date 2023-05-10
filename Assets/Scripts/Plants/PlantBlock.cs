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

        public void MoveToTarget(Transform parent, Vector3 targetPosition, float duration, bool isCharacterTarget, bool isScale = false)
        {
            if (isCharacterTarget)
            {
                transform.DOLocalMoveY(1.5f, 0.2f).SetEase(Ease.Linear).OnComplete(() =>
                {
                    transform.SetParent(parent);
                    Sequence seq = DOTween.Sequence();
                    seq.Append(transform.DOLocalMove(targetPosition, duration / 2).SetEase(Ease.Linear))
                    .Join(transform.DOLocalRotateQuaternion(Quaternion.identity, duration / 2))
                    .Join(transform.DOScale(isScale ? 0f : 0.8f, duration).SetEase(Ease.Linear));
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
