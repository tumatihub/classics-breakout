using UnityEngine;
using System.Collections;

[CreateAssetMenu]
public class BarrierSpecial : Special
{
    public override void Action(PlayerController playerController)
    {
        base.Action(playerController);
        playerController.ActivateBarrier();
    }
}
