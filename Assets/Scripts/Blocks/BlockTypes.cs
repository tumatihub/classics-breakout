using System;
using UnityEngine;

[CreateAssetMenu]
public class BlockTypes : ScriptableObject
{
    [Serializable]
    public struct Config
    {
        public Sprite Sprite;
        [ColorUsageAttribute(true, true)]
        public Color DissolveColor;
        public GameObject HitParticles;
    }


    public Config[] Configs;
}
