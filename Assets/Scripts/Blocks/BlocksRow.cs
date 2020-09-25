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

    private PlayerController _playerController;

    [SerializeField] private SpawnerProgression _spawnerProgression;

    public event Action<BlocksRow> OnRemoveRow;
    public event Action OnRowReachFloor;

    void Start()
    {
        CreateBlocks(transform.position);
        _playerController = FindObjectOfType<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            OnRowReachFloor?.Invoke();
            _playerController.EndGame();
        }    
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
        block.RowParent = this;
        OnRowReachFloor += block.Dissolve;
        return block;
    }

    public void HandleRemoveBlock()
    {
        _remainingBlocks--;
        if (_remainingBlocks <= 0)
        {
            OnRemoveRow?.Invoke(this);
            
        }
    }

    int GetBlockHitPoints()
    {
        return _spawnerProgression.GetBlockHitPoints();
    }

    public void MoveRowDown()
    {
        LeanTween.move(gameObject, new Vector3(transform.position.x, transform.position.y - _spaceBetweenRows), .2f).setEase(LeanTweenType.easeOutCirc);
    }

    public void MoveRowDownInstant()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y - _spaceBetweenRows);
    }

    public void RemoveRow()
    {
        Destroy(gameObject);
    }
}
