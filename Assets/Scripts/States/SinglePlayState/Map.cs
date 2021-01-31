using System.Collections.Generic;
using CoreProject.Resource;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Map : MonoBehaviour
{
    [SerializeField] private Transform _startPosition;
    [SerializeField] private Tilemap _tilemap;
    [SerializeField] private SpriteRenderer _rendererMap;
    [SerializeField] private Collider2D[] _colliders;

    private List<Vector3> _spawnPositions;
    private Vector2 _minBoundsOfMap = Vector2.zero, _maxBoundsOfMap = Vector2.zero;
    private Vector2Int _minBoundsCell = Vector2Int.zero, _maxBoundsCell = Vector2Int.zero;

    public Vector3 StartPosition => _startPosition.position;

    private void Awake()
    {
        _minBoundsOfMap.x = transform.position.x - (_rendererMap.size.x * transform.localScale.x);
        _minBoundsOfMap.y = transform.position.y - (_rendererMap.size.y * transform.localScale.y);
        _maxBoundsOfMap.x = transform.position.x + (_rendererMap.size.x * transform.localScale.x);
        _maxBoundsOfMap.y = transform.position.y + (_rendererMap.size.y * transform.localScale.y);

        _minBoundsCell.x = (int)(_minBoundsOfMap.x / _tilemap.cellSize.x);
        _minBoundsCell.y = (int)(_minBoundsOfMap.y / _tilemap.cellSize.y);
        _maxBoundsCell.x = (int)(_maxBoundsOfMap.x / _tilemap.cellSize.x);
        _maxBoundsCell.y = (int)(_maxBoundsOfMap.y / _tilemap.cellSize.y);
        _spawnPositions = new List<Vector3>();
        Debug.Log(_minBoundsCell + ", " + _maxBoundsCell);
        Vector2Int total = new Vector2Int(Mathf.Abs(_minBoundsCell.x) + _maxBoundsCell.x, Mathf.Abs(_minBoundsCell.y) + _maxBoundsCell.y);
        Vector3Int cellPos = Vector3Int.zero;
        for (int i = 0; i < total.x; i++)
        {
            for (int j = 0; j < total.y; j++)
            {
                cellPos.x = _minBoundsCell.x + i;
                cellPos.y = _minBoundsCell.y + j;
                Vector3 worldPosOfCell = _tilemap.CellToWorld(cellPos);
                Collider2D c = Physics2D.OverlapBox(worldPosOfCell, Vector2.one * _tilemap.cellSize, 0f);
                if (c == null)
                {
                    _spawnPositions.Add(worldPosOfCell);
                }
            }
        }
    }

    public Vector3 GetRandomPosition()
    {
        Vector3 targetPos = _spawnPositions[UnityEngine.Random.Range(0, _spawnPositions.Count)];
        return targetPos;
    }

    public Vector3 GetRandomPosition(Vector3 position, float radius = 1.5f)
    {
        Vector3 targetPos;
        float magnitude = 0f;
        do
        {
            targetPos = _spawnPositions[UnityEngine.Random.Range(0, _spawnPositions.Count)];
            Vector3 diff = position - targetPos;
            magnitude = diff.magnitude;
        } while (magnitude < radius);
        return targetPos;
    }
}
