using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ExplosionSpecial : Special
{
    public override void BallActivatedAction(BallMovement ball)
    {
        base.BallActivatedAction(ball);
        ball.IsExplosionActivated = true;

    }
}
