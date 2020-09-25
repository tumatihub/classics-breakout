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
    public int GetBlockHitPoints()
    {
        float chance = UnityEngine.Random.Range(0, 100);
        int hitPointsWithMaxChance = 1;
        float maxChance = 0;
        foreach(var blockChance in _spawnerProgressionLevels[_level].Chances)
        {
            if (chance < blockChance.Chance)
            {
                return blockChance.HitPoints;
            }

            if (blockChance.Chance > maxChance)
            {
                maxChance = blockChance.Chance;
                hitPointsWithMaxChance = blockChance.HitPoints;
            }
        }
        return hitPointsWithMaxChance;
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
