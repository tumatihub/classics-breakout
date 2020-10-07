using UnityEngine;
using System.Collections;

[CreateAssetMenu]
public class BarrierSpecial : Special
{
    public override void Action(PlayerController playerController)
    {
        playerController.ActivateBarrier();
    }
}
