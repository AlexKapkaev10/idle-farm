using Scripts.Enums;
using Scripts.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{
    public class SowingField : MonoBehaviour
    {
        [Range(0, 40)]
        [SerializeField]
        private int _sowingCellCount;
        [SerializeField]
        private SowingData _sowingData;
        [SerializeField]
        private SowingType _sowingType;
        [SerializeField]
        private List<SowingCell> _cells = new List<SowingCell>();

        private WaitForSeconds _ripeningWaitDelay;

        private void OnValidate()
        {
            SowingCell[] oldCells = GetComponentsInChildren<SowingCell>();
            if (oldCells.Length > 0)
            {
                StartCoroutine(Destroy(oldCells));
                _cells.Clear();
            }

            BuildField();
        }

        private void Awake()
        {
            if (_cells.Count > 0)
            {
                _ripeningWaitDelay = new WaitForSeconds(_sowingData.GetRepeningTimeBySowingType(_sowingType));

                for (int i = 0; i < _cells.Count; i++)
                {
                    SowingCell cell = _cells[i];
                    cell.Initit(_sowingType, _ripeningWaitDelay);
                }
            }
        }

        private void BuildField()
        {
            for (int i = 0; i < _sowingCellCount; i++)
            {
               SowingCell cell = Instantiate(_sowingData.GetSowingCell(), transform);
                _cells.Add(cell);
            }

            int lineCounter = 1;
            float positionX = 0;
            float positionZ = 0;
            for (int i = 1; i < _cells.Count; i++)
            {
                SowingCell currentCell = _cells[i];
                SowingCell lastCell = _cells[i - 1];

                if (_cells.Count > i - 1)
                    positionZ = lastCell.transform.localPosition.z + currentCell.transform.localScale.z;

                if (lineCounter == 10)
                {
                    lineCounter = 0;
                    positionX += currentCell.transform.localScale.x;
                    positionZ = 0f;
                }

                _cells[i].gameObject.transform.localPosition = new Vector3(positionX, currentCell.transform.position.y, positionZ);
                lineCounter++;
            }
        }

        private IEnumerator Destroy(SowingCell[] oldCells)
        {
            yield return new WaitForEndOfFrame();
            for (int i = 0; i < oldCells.Length; i++)
            {
                DestroyImmediate(oldCells[i].gameObject, true);
            }
        }
    }
}
