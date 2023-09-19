using System;
using UnityEngine;
using UnityEngine.UI;

public class MapBuilder : MonoBehaviour
{
    [SerializeField] private Grid _grid;
    private GameObject _tile;
    private Vector3 _mousePosition;
    private bool _possibleToBuild;
    private bool[,] _array = new bool[10, 10];
    private GameObject _tileForPlace;

    private TileColorChange script;
    /// <summary>
    /// Данный метод вызывается автоматически при клике на кнопки с изображениями тайлов.
    /// В качестве параметра передается префаб тайла, изображенный на кнопке.
    /// Вы можете использовать префаб tilePrefab внутри данного метода.
    /// </summary>
    public void StartPlacingTile(GameObject tilePrefab)
    {
        if (_tile == null)
        {
            _tile = Instantiate(tilePrefab);
            _tileForPlace = tilePrefab;
            script = _tile.GetComponent<TileColorChange>();
            
        }
    }

    public void Update()
    {
        if (_tile == null)
            return;
        _mousePosition = Input.mousePosition;
        _possibleToBuild = true;
        script.OnChangeColor(true);
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(_mousePosition), out hit))
        {
            var worldPosition = hit.point;
            var cellPosition = _grid.WorldToCell(worldPosition);
            if (cellPosition.x < 0 || cellPosition.z < 0 || cellPosition.x > 9 || cellPosition.z > 9 || _array[cellPosition.x, cellPosition.z])
            {
                _possibleToBuild = false;
                script.OnChangeColor(false);
            }
                
            var centerWorldPosition = _grid.GetCellCenterWorld(cellPosition);
            _tile.transform.position = new Vector3(centerWorldPosition.x, 0, centerWorldPosition.z);

            if (Input.GetMouseButtonDown(0) && _possibleToBuild)
            {
                var tile = Instantiate(_tileForPlace);
                tile.transform.position = new Vector3(centerWorldPosition.x, 0, centerWorldPosition.z);
                _array[cellPosition.x, cellPosition.z] = true;
                _tile = null;
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