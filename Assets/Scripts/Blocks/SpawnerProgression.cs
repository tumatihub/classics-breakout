using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[CreateAssetMenu]
public class SpawnerProgression : ScriptableObject
{
    private int _level = 0;
    [SerializeField]
    List<SpawnerProgressionLevel> _spawnerProgressionLevels = new List<SpawnerProgressionLevel>();

    public float SecondsToSpawnNewRow => _spawnerProgressionLevels[_level].SecondsToSpawnNewRow;

    public void SortChances()
    {
        foreach(var level in _spawnerProgressionLevels)
        {
            level.Chances.Sort(CompareChances);
        }
    }

    public int GetBlockHitPoints()
    {
        float chance = UnityEngine.Random.Range(0, 100);
        int hitPointsWithMaxChance = 1;
        float maxChance = 0;
        foreach (var blockChance in _spawnerProgressionLevels[_level].Chances)
        {
            if (chance < blockChance.Chance)
            {
                return blockChance.HitPoints[UnityEngine.Random.Range(0,blockChance.HitPoints.Count)];
            }

            if (blockChance.Chance > maxChance)
            {
                maxChance = blockChance.Chance;
                hitPointsWithMaxChance = blockChance.HitPoints[UnityEngine.Random.Range(0, blockChance.HitPoints.Count)];
            }
        }
        return hitPointsWithMaxChance;
    }

    public int CompareChances(BlockChance x, BlockChance y)
    {

        if (x.Chance > y.Chance) return 1;
        else if (y.Chance > x.Chance) return -1;
        return 0;

    }

    public void CheckLevelUpdate(int totalSpawnedRows)
    {
        if (_level >= _spawnerProgressionLevels.Count - 1) return;

        if (totalSpawnedRows >= _spawnerProgressionLevels[_level + 1].MinSpawnedRows)
        {
            _level++;
        }
    }

    public void SetLevel(int level)
    {
        _level = Mathf.Clamp(level, 0, _spawnerProgressionLevels.Count - 1);
    }
}
