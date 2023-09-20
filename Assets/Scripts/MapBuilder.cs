using System;
using UnityEngine;
using UnityEngine.UI;

public class MapBuilder : MonoBehaviour
{
    [SerializeField] private Grid _grid;
    private GameObject _currentTile;
    private Vector3 _mousePosition;
    private bool _canBuildTile;
    private bool[,] _array = new bool[10, 10];
    private TileColorController _tileColorController;
    /// <summary>
    /// Данный метод вызывается автоматически при клике на кнопки с изображениями тайлов.
    /// В качестве параметра передается префаб тайла, изображенный на кнопке.
    /// Вы можете использовать префаб tilePrefab внутри данного метода.
    /// </summary>
    public void StartPlacingTile(GameObject tilePrefab)
    {
        if (_currentTile == null)
        {
            _currentTile = Instantiate(tilePrefab);
            _tileColorController = _currentTile.GetComponent<TileColorController>();
        }
    }

    public void Update()
    {
        if (_currentTile == null)
            return;
        _mousePosition = Input.mousePosition;
        _canBuildTile = true;
        _tileColorController.OnChangeColor(true);
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(_mousePosition), out hit))
        {
            var worldPosition = hit.point;
            var cellPosition = _grid.WorldToCell(worldPosition);
            if (cellPosition.x < 0 || cellPosition.z < 0 || cellPosition.x > _array.GetLength(0) || cellPosition.z > _array.GetLength(1) || _array[cellPosition.x, cellPosition.z])
            {
                _canBuildTile = false;
                _tileColorController.OnChangeColor(false);
            }
                
            var centerWorldPosition = _grid.GetCellCenterWorld(cellPosition);
            _currentTile.transform.position = new Vector3(centerWorldPosition.x, 0, centerWorldPosition.z);

            if (Input.GetMouseButtonDown(0) && _canBuildTile)
            {
                _tileColorController.ResetColor();
                var tile = _currentTile;
                tile.transform.position = new Vector3(centerWorldPosition.x, 0, centerWorldPosition.z);
                _array[cellPosition.x, cellPosition.z] = true;
                _currentTile = null;
            }
        }
    }

    private void OnDrawGizmos()
    {
        var cellSize = _grid.cellSize;
        var cellGap = _grid.cellGap;
        var origin = _grid.transform.position;

        Gizmos.color = Color.yellow;

        for (var x = 0; x < 10; x++)
        {
            for (var z = 0; z < 10; z++)
            {
                var pos = origin + new Vector3(x * (cellSize.x + cellGap.x), 0, z * (cellSize.z + cellGap.z));
                
                Gizmos.DrawLine(pos, pos + new Vector3(cellSize.x,0,0));
                Gizmos.DrawLine(pos, pos + new Vector3(0,0,cellSize.z));
                Gizmos.DrawLine(pos + new Vector3(cellSize.x, 0, 0), pos + new Vector3(cellSize.x, 0, cellSize.z));
                Gizmos.DrawLine(pos + new Vector3(0,0,cellSize.z), pos + new Vector3(cellSize.x, 0, cellSize.z));
                
            }
        }
    }
}