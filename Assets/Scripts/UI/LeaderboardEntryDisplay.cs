using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LeaderboardEntryDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text _playerName;
    [SerializeField] private TMP_Text _comboPoints;
    [SerializeField] private TMP_Text _maxCombo;
    [SerializeField] private TMP_Text _totalScore;

    public void UpdateInfo(LeaderboardEntry entry)
    {
        _playerName.text = entry.player_name;
        _comboPoints.text = entry.combo_points.ToString();
        _maxCombo.text = entry.max_combo.ToString();
        _totalScore.text = entry.total_score.ToString();
    }
}
