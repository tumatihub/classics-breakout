using UnityEngine;
using System.Collections;

[CreateAssetMenu]
public class Save : ScriptableObject
{
    public Score _score;
    public UpgradeProgress _ballPower;
    public UpgradeProgress _piercing;
    public UpgradeProgress _explosion;
    public UpgradeProgress _extraCharges;
    public UpgradeProgress _bulletTimeCost;
    public UpgradeProgress _barrier;
    

    public bool IsPlayerPrefsInitialized()
    {
        return PlayerPrefs.HasKey(PrefsKeys.BALL_POWER);
    }

    public void ClearSave()
    {
        PlayerPrefs.DeleteAll();
    }

    public void InitPrefs()
    {
        PlayerPrefs.SetInt(PrefsKeys.BALL_POWER, 0);
        PlayerPrefs.SetInt(PrefsKeys.PIERCING, 0);
        PlayerPrefs.SetInt(PrefsKeys.EXPLOSION, 0);
        PlayerPrefs.SetInt(PrefsKeys.EXTRA_CHARGES, 0);
        PlayerPrefs.SetInt(PrefsKeys.BULLET_TIME_COST, 0);
        PlayerPrefs.SetInt(PrefsKeys.BARRIER, 0);

        PlayerPrefs.SetInt(PrefsKeys.TOTAL_COMBO, 0);
        PlayerPrefs.SetInt(PrefsKeys.MAX_COMBO, 0);
    }

    public void LoadPlayerPrefs()
    {
        if (!IsPlayerPrefsInitialized()) InitPrefs();

        _ballPower.SetLevel(PlayerPrefs.GetInt(PrefsKeys.BALL_POWER));
        _piercing.SetLevel(PlayerPrefs.GetInt(PrefsKeys.PIERCING));
        _explosion.SetLevel(PlayerPrefs.GetInt(PrefsKeys.EXPLOSION));
        _extraCharges.SetLevel(PlayerPrefs.GetInt(PrefsKeys.EXTRA_CHARGES));
        _bulletTimeCost.SetLevel(PlayerPrefs.GetInt(PrefsKeys.BULLET_TIME_COST));
        _barrier.SetLevel(PlayerPrefs.GetInt(PrefsKeys.BARRIER));

        _score.MaxComboRecord = PlayerPrefs.GetInt(PrefsKeys.MAX_COMBO);
        _score.ComboTotalRecord = PlayerPrefs.GetInt(PrefsKeys.TOTAL_COMBO);
    }

    public void SavePlayerPrefs()
    {
        PlayerPrefs.SetInt(PrefsKeys.BALL_POWER, _ballPower.Level);
        PlayerPrefs.SetInt(PrefsKeys.PIERCING, _piercing.Level);
        PlayerPrefs.SetInt(PrefsKeys.EXPLOSION, _explosion.Level);
        PlayerPrefs.SetInt(PrefsKeys.EXTRA_CHARGES, _extraCharges.Level);
        PlayerPrefs.SetInt(PrefsKeys.BULLET_TIME_COST, _bulletTimeCost.Level);
        PlayerPrefs.SetInt(PrefsKeys.BARRIER, _barrier.Level);

        PlayerPrefs.SetInt(PrefsKeys.TOTAL_COMBO, _score.ComboTotalRecord);
        PlayerPrefs.SetInt(PrefsKeys.MAX_COMBO, _score.MaxComboRecord);

        PlayerPrefs.Save();
    }
}
