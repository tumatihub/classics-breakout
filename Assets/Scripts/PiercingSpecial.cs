using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PiercingSpecial : Special
{
    public override void BallActivatedAction(BallMovement ball)
    {
        base.BallActivatedAction(ball);
        ball.PiercingCountLeft = _playerStats.PiercingCount;
    }
}
