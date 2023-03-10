using Scripts.Enums;
using Scripts.Interfaces;
using Scripts.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
        private PlantType _plantType;
        [SerializeField]
        private List<SowingCell> _cells = new List<SowingCell>();
        [SerializeField]
        private Collider _collider;

        private FieldStateType _fieldStateType = FieldStateType.Sow;
        private ICharacterController _iCharacterController;
        private Transform _characterTransform;
        private int _interactCount = 0;
        private float _cellInteractDistance;

        public void BuildField()
        {
            if (_cells.Count > 0)
            {
                StartCoroutine(DestroyOldCells());
            }

            SowingCell cellPrefab = _sowingData.GetSowCellByPlantType(_plantType);
            Debug.Log(cellPrefab);
            if (cellPrefab == null)
                return;

            for (int i = 0; i < _sowingCellCount; i++)
            {
               SowingCell cell = (SowingCell)PrefabUtility.InstantiatePrefab(cellPrefab, _cellsPoint);
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
                _cellInteractDistance = _sowingData.GetCellInteractDistance();

                for (int i = 0; i < _cells.Count; i++)
                {
                    SowingCell cell = _cells[i];
                    cell.Init(_sowingData, _plantType);
                    cell.OnRipe += RipeCount;
                }
            }
        }

        private void Update()
        {
            if (!_characterTransform || _fieldStateType == FieldStateType.Ripe)
                return;

            for (int i = 0; i < _cells.Count; i++)
            {
                SowingCell cell = _cells[i];
                if (!cell.HasInteract)
                    continue;

                float distance = Vector3.Distance(_characterTransform.position, cell.transform.position);
                if (distance < _cellInteractDistance)
                {
                    cell.HasInteract = false;
                    InteractCount();
                }
            }
        }

        private void InteractCount()
        {
            _interactCount++;

            if (_interactCount == _cells.Count)
            {
                _fieldStateType = FieldStateType.Default;
                _iCharacterController.SetAnimationState(_fieldStateType);

                for (int i = 0; i < _cells.Count; i++)
                {
                    _interactCount = 0;
                    _cells[i].StartRipening();
                }
            }
        }

        private void RipeCount()
        {
            _interactCount++;

            if (_interactCount == _cells.Count)
            {
                _fieldStateType = FieldStateType.Ripe;
                if (_iCharacterController != null)
                    _iCharacterController.SetAnimationState(_fieldStateType);
            }
        }

        private void MowPlants()
        {
            for (int i = 0; i < _cells.Count; i++)
            {
                SowingCell cell = _cells[i];
                if (!cell.HasInteract)
                    continue;

                float distance = Vector3.Distance(_characterTransform.position, cell.transform.position);
                Vector3 diection = _characterTransform.InverseTransformPoint(cell.transform.position);
                if (diection.z < 0)
                    continue;

                if (distance < _cellInteractDistance)
                {
                    cell.HasInteract = false;
                    InteractCount();
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

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Character"))
            {
                if (other.gameObject.TryGetComponent<ICharacterController>(out _iCharacterController))
                {
                    _characterTransform = _iCharacterController.GetTransform();
                    _iCharacterController.SetAnimationState(_fieldStateType);
                    _iCharacterController.OnMow += MowPlants;
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (_iCharacterController != null)
            {
                _characterTransform = null;
                _iCharacterController.SetAnimationState(FieldStateType.Default);
                _iCharacterController.OnMow -= MowPlants;
                _iCharacterController = null;
            }
        }
    }
}
