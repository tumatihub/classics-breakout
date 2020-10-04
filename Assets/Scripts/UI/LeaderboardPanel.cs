using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardPanel : MonoBehaviour
{
    private Leaderboard _leaderboard;
    [SerializeField] private LeaderboardEntryDisplay _entryDisplayPrefab;
    [SerializeField] private GameObject _errorMsg;
    [SerializeField] private GameObject _entriesGroup;

    void Start()
    {
        _leaderboard = FindObjectOfType<Leaderboard>();
        _leaderboard.RequestEntriesOrderByCombo(UpdateEntries, DisplayServerError);
    }

    private void UpdateEntries(LeaderboardEntry[] entries)
    {
        foreach(var entry in entries)
        {
            var row = Instantiate(_entryDisplayPrefab, _entriesGroup.transform);
            row.UpdateInfo(entry);
        }
    }

    private void DisplayServerError(string error)
    {
        Instantiate(_errorMsg, _entriesGroup.transform);
    }
}
