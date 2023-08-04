using System;
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
        public Action<SowingField> OnFieldClear;
        
        [SerializeField] private List<SowingCell> _cells = new List<SowingCell>();
        [SerializeField] private Collider _collider;
        [SerializeField] private Transform _cellsPoint;
        [SerializeField] private SowingData _sowingData;
        [SerializeField] private GameObject _marker;
        [SerializeField] private PlantType _plantType;
        
        [Range(0, 40)]
        [SerializeField] private int _sowingCellCount;
        [SerializeField] private bool _autoRepair = true;
        
        private ObjectsPool<PlantBlock> _blocksPool;
        private List<PlantBlock> _blocks;
        private ICharacterController _iCharacterController;
        private Transform _characterTransform;
        private int _interactCount = 0;
        private float _cellInteractDistance;

        public PlantType PlantType => _plantType;
        public int Count => _sowingCellCount;

        public bool AutoRepair
        {
            get => _autoRepair;
            set => _autoRepair = value;
        }

        public void SetTransform(Vector3 position, Vector3 rotation)
        {
            transform.position = position;
            transform.eulerAngles = rotation;
        }

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
        
        private void Start()
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
                    cell.OnMow += CellMow;
                    if (AutoRepair)
                        cell.OnRipe += CellRipe;
                    
                    cell.Init(_sowingData);
                }
            }
        }

        private void OnDestroy()
        {
            OnFieldClear = null;
            if (_blocks == null && _blocks.Count <= 0)
                return;
            
            _blocksPool.OnCreate -= CreateNewBlock;
            foreach (var block in _blocks)
            {
                block.OnBlockReturn -= OnBlockReturn;
            }
            
            if (_iCharacterController != null)
            {
                _characterTransform = null;
                _iCharacterController.SetAnimationForField(FieldStateType.Ripe);
                _iCharacterController.OnMow -= MowPlants;
                _iCharacterController = null;
            }
        }

        private bool CheckInteractComplete()
        {
            _interactCount++;
            if (_interactCount == _cells.Count)
            {
                _interactCount = 0;
                return true;
            }

            return false;
        }

        private void CellRipe()
        {
            if (CheckInteractComplete())
            {
                if (_iCharacterController != null)
                {
                    _collider.enabled = true;
                }
            }
        }

        private void CellMow()
        {
            if (CheckInteractComplete())
            {
                if (_iCharacterController != null)
                    _iCharacterController.SetAnimationForField(FieldStateType.Ripe);
                
                if (AutoRepair)
                {
                    foreach (var cell in _cells)
                    {
                        cell.StartRipening();
                    }

                    if (_iCharacterController != null)
                        _iCharacterController.OnMow -= MowPlants;
                    
                    _collider.enabled = false;
                }
                else
                {
                    foreach (var cell in _cells)
                    {
                        cell.OnMow -= CellMow;
                    }
                    
                    Destroy(_marker);
                    Destroy(_collider);
                }

                if (_iCharacterController?.CurrentTool != null)
                    _iCharacterController.CurrentTool.CurrentSharpCount--;
                
                OnFieldClear?.Invoke(this);
            }
        }

        private void MowPlants(int damage)
        {
            for (int i = 0; i < _cells.Count; i++)
            {
                var cell = _cells[i];
                
                if (cell.HP == 0 || !_characterTransform)
                    continue;
                
                var distance = Vector3.Distance(_characterTransform.position, cell.transform.position);

                if (distance < _cellInteractDistance)
                {
                    cell.HP += _iCharacterController.CurrentTool.IsSharp() ? -2 : -1;

                    if (cell.HP > 0)
                        continue;
                    
                    PlantBlock block = _blocksPool.Get();
                    var blockTransform = block.gameObject.transform;
                    blockTransform.position = cell.GetBlockPoint().position;
                    blockTransform.localScale = Vector3.zero;
                    _iCharacterController.AddPlant(block);
                }
            }
        }

        private Vector3 GetPointByTransform(in Transform from, in Transform to)
        {
            return from.InverseTransformPoint(to.position);
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
                _iCharacterController.SetAnimationForField(FieldStateType.Mow);
                _iCharacterController.OnMow += MowPlants;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (_iCharacterController != null)
            {
                _characterTransform = null;
                _iCharacterController.SetAnimationForField(FieldStateType.Ripe);
                _iCharacterController.OnMow -= MowPlants;
                _iCharacterController = null;
            }
        }
    }
}
