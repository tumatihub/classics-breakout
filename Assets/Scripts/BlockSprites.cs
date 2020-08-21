using System;
using UnityEngine;

[CreateAssetMenu]
public class BlockSprites : ScriptableObject
{
    [Serializable]
    public struct Config
    {
        public Sprite Sprite;
        [ColorUsageAttribute(true, true)]
        public Color DissolveColor;
    }


    public Config[] Configs;
}
