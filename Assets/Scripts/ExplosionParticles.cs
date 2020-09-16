using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionParticles : MonoBehaviour
{
    [SerializeField] private PlayerStats _playerStats;
    [SerializeField] private Score _score;
    [SerializeField] private UpgradeProgress _explosionProgress;
    [SerializeField] private int _upgradeLevel;

    private void OnParticleSystemStopped()
    {
        Collider2D[] blockList = Physics2D.OverlapCircleAll(
            new Vector2(transform.position.x, transform.position.y),
            _playerStats.ExplosionRadius,
            LayerMask.GetMask("Block")
        );
        foreach (var blockCollider in blockList)
        {
            Block block = blockCollider.GetComponent<Block>();
            _score.ScoreInstantRemove(block);
            block.RemoveBlock();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _explosionProgress.Upgrades[_upgradeLevel].Value);
    }
}
