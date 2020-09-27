using UnityEngine;
using System.Collections;

[CreateAssetMenu]
public class Save : ScriptableObject
{
    public Score _score;
    public UpgradeProgress _ballPower;
    public UpgradeProgress _piercing;
    public UpgradeProgress _explosion;
    

    public bool IsPlayerPrefsInitialized()
    {
        return PlayerPrefs.HasKey(PrefsKeys.BALL_POWER);
    }

    public void InitPrefs()
    {
        PlayerPrefs.SetInt(PrefsKeys.BALL_POWER, 0);
        PlayerPrefs.SetInt(PrefsKeys.PIERCING, 0);
        PlayerPrefs.SetInt(PrefsKeys.EXPLOSION, 0);
    }

    public void LoadPlayerPrefs()
    {
        if (!IsPlayerPrefsInitialized()) InitPrefs();

        _ballPower.SetLevel(PlayerPrefs.GetInt(PrefsKeys.BALL_POWER));
        _piercing.SetLevel(PlayerPrefs.GetInt(PrefsKeys.PIERCING));
        _explosion.SetLevel(PlayerPrefs.GetInt(PrefsKeys.EXPLOSION));
    }

    public void SavePlayerPrefs()
    {
        PlayerPrefs.SetInt(PrefsKeys.BALL_POWER, _ballPower.Level);
        PlayerPrefs.SetInt(PrefsKeys.PIERCING, _piercing.Level);
        PlayerPrefs.SetInt(PrefsKeys.EXPLOSION, _explosion.Level);
        PlayerPrefs.Save();
    }
}
