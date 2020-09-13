using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlocksSpawner : MonoBehaviour
{
    [SerializeField] private BlocksRow _blocksRowPrefab;
    [SerializeField] private int _initialNumberOfRows = 8;
    [SerializeField] private float _secondsToSpawnNewRow = 5f;
    private float _spawnCooldown;
    private List<BlocksRow> _spawnedRows = new List<BlocksRow>();

    [SerializeField] private int _startLevel;

    private int _totalSpawnedRows;
    [SerializeField] SpawnerProgression _spawnerProgression;

    public event Action OnMoveDownRows;

    void Start()
    {
        _spawnerProgression.SetLevel(_startLevel);
        CreateInitialRows(_initialNumberOfRows);
        _spawnCooldown = _secondsToSpawnNewRow;
    }

    private void Update()
    {
        if (_spawnCooldown > 0) _spawnCooldown -= Time.deltaTime;
    }

    public void HandleRemoveRow(BlocksRow blocksRow)
    {
        _spawnedRows.Remove(blocksRow);
        OnMoveDownRows -= blocksRow.MoveRowDown;
        blocksRow.RemoveRow();
    }

    public void HandleBallCollisionWithPaddle()
    {
        if (_spawnCooldown <= 0 || _spawnedRows.Count < _initialNumberOfRows)
        {
            if (_spawnCooldown <= 0) _spawnCooldown = _secondsToSpawnNewRow;
            OnMoveDownRows?.Invoke();
            SpawnRow(transform.position);
        }
    }

    BlocksRow SpawnRow(Vector3 centerPosition)
    {
        var row = Instantiate(_blocksRowPrefab, centerPosition, Quaternion.identity);
        OnMoveDownRows += row.MoveRowDown;
        row.OnRemoveRow += HandleRemoveRow;
        _spawnedRows.Add(row);
        _totalSpawnedRows++;
        _spawnerProgression.CheckLevelUpdate(_totalSpawnedRows);
        return row;
    }


    void CreateInitialRows(int numberOfRows)
    {
        for (var i = 0; i < numberOfRows; i++)
        {
            OnMoveDownRows?.Invoke();
            SpawnRow(transform.position);
        }
    }
}
