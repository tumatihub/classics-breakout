
using System;

[Serializable]
public class LeaderboardEntry
{
    public string player_name;
    public int combo_points;
    public int max_combo;
    public int total_score;

    public LeaderboardEntry(string playerName, int comboPoints, int maxCombo, int totalScore)
    {
        player_name = playerName;
        combo_points = comboPoints;
        max_combo = maxCombo;
        total_score = totalScore;
    }
}
