using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlocksSpawner : MonoBehaviour
{
    [SerializeField] private BlocksRow _blocksRowPrefab;
    [SerializeField] private int _initialNumberOfRows = 8;
    private float _spawnCooldown;
    private List<BlocksRow> _spawnedRows = new List<BlocksRow>();

    [SerializeField] private int _startLevel;

    private int _totalSpawnedRows;
    [SerializeField] SpawnerProgression _spawnerProgression;

    public event Action OnMoveDownRows;
    public event Action OnMoveDownRowsInstant;

    void Start()
    {
        _spawnerProgression.SetLevel(_startLevel);
        _spawnerProgression.SortChances();
        _spawnCooldown = _spawnerProgression.SecondsToSpawnNewRow;
    }

    private void Update()
    {
        if (_spawnCooldown > 0) _spawnCooldown -= Time.deltaTime;
    }

    public void HandleRemoveRow(BlocksRow blocksRow)
    {
        _spawnedRows.Remove(blocksRow);
        OnMoveDownRows -= blocksRow.MoveRowDown;
        OnMoveDownRowsInstant -= blocksRow.MoveRowDown;
        blocksRow.RemoveRow();
    }

    public void HandleBallCollisionWithPaddle()
    {
        if (_spawnCooldown <= 0 || _spawnedRows.Count < _initialNumberOfRows)
        {
            if (_spawnCooldown <= 0) _spawnCooldown = _spawnerProgression.SecondsToSpawnNewRow;
            OnMoveDownRows?.Invoke();
            SpawnRow(transform.position);
        }
    }

    BlocksRow SpawnRow(Vector3 centerPosition)
    {
        var row = Instantiate(_blocksRowPrefab, centerPosition, Quaternion.identity);
        OnMoveDownRows += row.MoveRowDown;
        OnMoveDownRowsInstant += row.MoveRowDownInstant;
        row.OnRemoveRow += HandleRemoveRow;
        _spawnedRows.Add(row);
        _totalSpawnedRows++;
        _spawnerProgression.CheckLevelUpdate(_totalSpawnedRows);
        return row;
    }


    public void CreateInitialRows()
    {
        for (var i = 0; i < _initialNumberOfRows; i++)
        {
            OnMoveDownRowsInstant?.Invoke();
            SpawnRow(transform.position);
        }
    }
}
