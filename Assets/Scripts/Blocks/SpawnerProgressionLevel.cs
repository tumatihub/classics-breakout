using System;
using System.Collections.Generic;

[System.Serializable]
public class SpawnerProgressionLevel
{
    public int MinSpawnedRows;
    public List<BlockChance> Chances = new List<BlockChance>();
}