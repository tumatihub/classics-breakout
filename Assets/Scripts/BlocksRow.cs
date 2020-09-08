using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlocksRow : MonoBehaviour
{
    private const int NUM_FOR_BLOCKS_PER_ROW = 19;
    [SerializeField] private Block _blockPrefab;
    private Block[] _blocks = new Block[NUM_FOR_BLOCKS_PER_ROW];
    private float _spaceBetweenBlocks = 0.9f;
    private float _spaceBetweenRows = 0.5f;
    private int _remainingBlocks = NUM_FOR_BLOCKS_PER_ROW;

    public event Action<BlocksRow> OnRemoveRow;

    void Start()
    {
        CreateBlocks(transform.position);
    }

    void CreateBlocks(Vector3 startPosition)
    {
        int i = 0;
        int blockColumn = 1;
        Vector3 pos = startPosition;
        _blocks[i] = CreateIndividualBlock(pos);
        i++;
        while (i < NUM_FOR_BLOCKS_PER_ROW)
        {
            var xPos = blockColumn * _spaceBetweenBlocks;
            pos = new Vector3(xPos, startPosition.y);
            _blocks[i] = CreateIndividualBlock(pos);
            i++;
            pos = new Vector3(-xPos, startPosition.y);
            _blocks[i] = CreateIndividualBlock(pos);
            i++;
            blockColumn++;
        }
    }

    Block CreateIndividualBlock(Vector3 position)
    {
        var block = Instantiate(_blockPrefab, position, Quaternion.identity, transform);
        block.SetHitPoints(GetBlockHitPoints());
        block.OnRemoveBlock += HandleRemoveBlock;
        return block;
    }

    public void HandleRemoveBlock(Block removedBlock)
    {
        _remainingBlocks--;
        if (_remainingBlocks <= 0)
        {
            OnRemoveRow?.Invoke(this);
            Destroy(gameObject, 1f);
        }
    }

    int GetBlockHitPoints()
    {
        return UnityEngine.Random.Range(1, 2);
    }

    public void MoveRowDown()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y - _spaceBetweenRows);
    }
}
