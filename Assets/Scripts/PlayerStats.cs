using UnityEngine;

[CreateAssetMenu]
public class PlayerStats : ScriptableObject
{
    private int _ballPower = 1;
    public int BallPower => _ballPower;
    private int _maxBallPower;

    public void LevelUpBallPower()
    {
        _ballPower = Mathf.Min(_ballPower + 1, _maxBallPower);
    }
}
