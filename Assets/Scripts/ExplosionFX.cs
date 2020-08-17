using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionFX : MonoBehaviour
{
    [SerializeField] private PlayerStats _playerStats;
    void Start()
    {
        transform.localScale = new Vector3(_playerStats.ExplosionRadius, _playerStats.ExplosionRadius);
        Destroy(gameObject, 1f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _playerStats.ExplosionRadius);
    }

}
