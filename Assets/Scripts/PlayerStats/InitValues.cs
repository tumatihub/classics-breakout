using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class InitValues : ScriptableObject
{
    public FloatVariable BallPower;
    public FloatVariable ChargeAmmountPerHit;
    public FloatVariable ChargeAmmountPerRemovedBlock;
    public FloatVariable ChargeAmmountConsume;
    public FloatVariable BulletTimeConsumeRateInSeconds;
    public FloatVariable BulletTimeScale;
    public FloatVariable ChargeMax;
    public FloatVariable PiercingCount;
    public FloatVariable ExplosionRadius;
    public FloatVariable ExtraCharges;
}
