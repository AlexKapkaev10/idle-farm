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
        private Transform _cellsPoint;
        [SerializeField]
        private SowingData _sowingData;
        [SerializeField]
        private SowingType _sowingType;
        [SerializeField]
        private List<SowingCell> _cells = new List<SowingCell>();

        private int _allSowingCount = 0;

        public void BuildField()
        {
            if (_cells.Count > 0)
            {
                StartCoroutine(DestroyOldCells());
            }

            SowingCell cellPrefab = _sowingData.GetSowingCellBySwowimgType(_sowingType);
            Debug.Log(cellPrefab);
            if (cellPrefab == null)
                return;

            for (int i = 0; i < _sowingCellCount; i++)
            {
               SowingCell cell = Instantiate(cellPrefab, _cellsPoint);
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

        private void Awake()
        {
            if (_cells.Count > 0)
            {
                for (int i = 0; i < _cells.Count; i++)
                {
                    SowingCell cell = _cells[i];
                    cell.Init(_sowingType, _sowingData.GetSowingMaterialBySowingType(_sowingType), _sowingData.GetRipeMaterialBySowingType(_sowingType));
                    cell.OnSowing += Sowing;
                }
            }
        }

        private void Sowing()
        {
            _allSowingCount++;
            if (_allSowingCount == _cells.Count)
            {
                for (int i = 0; i < _cells.Count; i++)
                {
                    _cells[i].StartRipening();
                }
            }
        }

        private IEnumerator DestroyOldCells()
        {
            yield return new WaitForEndOfFrame();

            for (int i = 0; i < _cells.Count; i++)
            {
                DestroyImmediate(_cells[i].gameObject, true);
            }

            _cells.Clear();
        }
    }
}
