using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlocksRow : MonoBehaviour
{
    private const int NUM_FOR_BLOCKS_PER_ROW = 19;
    [SerializeField] private Block _blockPrefab;
    private Block[] _blocks = new Block[NUM_FOR_BLOCKS_PER_ROW];
    private float _spaceBetweenBlocks = 0.9f;

    void Start()
    {
        CreateBlocks(new Vector3(0,0));
    }

    void CreateBlocks(Vector3 startPosition)
    {
        int i = 0;
        int blockColumn = 1;
        Vector3 pos = startPosition;
        _blocks[i] = Instantiate(_blockPrefab, pos, Quaternion.identity, transform);
        i++;
        while (i < NUM_FOR_BLOCKS_PER_ROW)
        {
            var xPos = blockColumn * _spaceBetweenBlocks;
            pos = new Vector3(xPos, startPosition.y);
            _blocks[i] = Instantiate(_blockPrefab, pos, Quaternion.identity, transform);
            _blocks[i].SetHitPoints(GetBlockHitPoints());
            i++;
            pos = new Vector3(-xPos, startPosition.y);
            _blocks[i] = Instantiate(_blockPrefab, pos, Quaternion.identity, transform);
            _blocks[i].SetHitPoints(GetBlockHitPoints());
            i++;
            blockColumn++;
        }
    }

    int GetBlockHitPoints()
    {
        return Random.Range(1, 7);
    }
}
