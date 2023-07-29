using Scripts.Enums;
using Scripts.Interfaces;
using Scripts.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Scripts.ObjectsPool;
using Scripts.Plants;

namespace Scripts
{
    public class SowingField : MonoBehaviour
    {
        [SerializeField] private List<SowingCell> _cells = new List<SowingCell>();
        [SerializeField] private Transform _cellsPoint;
        [SerializeField] private SowingData _sowingData;
        [SerializeField] private PlantType _plantType;
        [Range(0, 40)]
        [SerializeField] private int _sowingCellCount;

        private ObjectsPool<PlantBlock> _blocksPool;
        private List<PlantBlock> _blocks;
        private FieldStateType _fieldStateType = FieldStateType.Default;
        private ICharacterController _iCharacterController;
        private Transform _characterTransform;
        private int _interactCount = 0;
        private float _cellInteractDistance;

        public void BuildField()
        {
#if UNITY_EDITOR
            if (_cells.Count > 0)
            {
                StartCoroutine(DestroyOldCells());
            }

            var cellPrefab = _sowingData.GetSowCell();
            
            if (cellPrefab == null)
                return;

            for (int i = 0; i < _sowingCellCount; i++)
            {
                var cell = (SowingCell)PrefabUtility.InstantiatePrefab(cellPrefab, _cellsPoint);
                _cells.Add(cell);
            }

            var lineCounter = 1;
            float positionX = 0;
            float positionZ = 0;
            for (int i = 1; i < _cells.Count; i++)
            {
                var currentCell = _cells[i];
                var lastCell = _cells[i - 1];

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
#endif
        }

        private void Awake()
        {
            if (_cells.Count > 0)
            {
                _blocksPool = new ObjectsPool<PlantBlock>(_sowingData.GetPlantBlock(), _sowingCellCount, transform);
                _blocksPool.OnCreate += CreateNewBlock;
                _blocks = _blocksPool.GetAll();
                foreach (var block in _blocks)
                {
                    block.OnBlockReturn += OnBlockReturn;
                    block.PlantType = _plantType;
                }
                _cellInteractDistance = _sowingData.GetCellInteractDistance();

                for (int i = 0; i < _cells.Count; i++)
                {
                    SowingCell cell = _cells[i];
                    cell.Init(_sowingData, _plantType);
                    cell.OnRipe += CellInteract;
                    cell.OnMow += CellInteract;
                }
            }
        }

        private void OnDestroy()
        {
            if (_blocks == null && _blocks.Count <= 0)
                return;
            
            foreach (var block in _blocks)
            {
                block.OnBlockReturn -= OnBlockReturn;
            }
        }

        private void CellInteract()
        {
            _interactCount++;

            if (_interactCount == _cells.Count)
            {
                _interactCount = 0;

                switch (_fieldStateType)
                {
                    case FieldStateType.Default:
                        _fieldStateType = FieldStateType.Mow;
                        if (_iCharacterController != null)
                            _iCharacterController.SetAnimationForField(_fieldStateType);
                        break;
                    case FieldStateType.Mow:
                        _fieldStateType = FieldStateType.Default;
                        if (_iCharacterController != null)
                            _iCharacterController.SetAnimationForField(_fieldStateType);
                        for (int i = 0; i < _cells.Count; i++)
                        {
                            _cells[i].StartRipening();
                        }
                        break;
                }
            }
        }

        private void MowPlants()
        {
            for (int i = 0; i < _cells.Count; i++)
            {
                SowingCell cell = _cells[i];
                
                if (cell.IsMow)
                    continue;

                var direction = _characterTransform.InverseTransformPoint(cell.transform.position);
                if (direction.z < 0)
                    continue;
                
                var distance = Vector3.Distance(_characterTransform.position, cell.transform.position);
                
                if (distance < _cellInteractDistance)
                {
                    cell.IsMow = true;
                    PlantBlock block = _blocksPool.Get();
                    var blockTransform = block.gameObject.transform;
                    blockTransform.position = cell.GetBlockPoint().position;
                    blockTransform.localScale = Vector3.zero;
                    _iCharacterController.AddPlant(block);
                }
            }
        }

        private void OnBlockReturn(PlantBlock block)
        {
            _blocksPool.Return(block);
        }

        private void CreateNewBlock(PlantBlock plantBlock)
        {
            _blocks.Add(plantBlock);
            plantBlock.OnBlockReturn += OnBlockReturn;
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
            if (other.gameObject.TryGetComponent<ICharacterController>(out _iCharacterController))
            {
                _characterTransform = _iCharacterController.GetBodyTransform();
                _iCharacterController.SetAnimationForField(_fieldStateType);
                _iCharacterController.OnMow += MowPlants;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (_iCharacterController != null)
            {
                _characterTransform = null;
                _iCharacterController.SetAnimationForField(FieldStateType.Default);
                _iCharacterController.OnMow -= MowPlants;
                _iCharacterController = null;
            }
        }
    }
}
