﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Special : ScriptableObject
{
    public string Name;
    public Color Color;
    public Sprite Icon;
    public bool ChargesPaddle;
    public bool CanBeUsed = true;
    public int ChargeCost = 1;
    public GameObject ChargeParticles;
    public GameObject ActivateSpecialParticles;
    public GameObject BallTrail;
    public Color BackgroundColor;
    [ColorUsageAttribute(true, true)]
    public Color BackgroundHDRColor;
    [ColorUsageAttribute(true, true)]
    public Color BackgroundHDRColorActivated;
    public PaddleChargeVFX PaddleChargeVFX;
    public int Usage;

    [SerializeField] protected PlayerStats _playerStats;

    public virtual void BallActivatedAction(BallMovement ball)
    {
        _playerStats.UnchargePaddle();
        Usage++;
    }

    public virtual void Action(PlayerController playerController)
    {
        Usage++;
    }
}
