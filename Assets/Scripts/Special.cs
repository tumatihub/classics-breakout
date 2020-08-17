using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Special : ScriptableObject
{
    public string Name;
    public Color Color;
    public bool ChargesPaddle;

    [SerializeField] protected PlayerStats _playerStats;

    public virtual void BallActivatedAction(BallMovement ball)
    {
        _playerStats.UnchargePaddle();
    }

    public virtual void Action()
    {
    }
}
