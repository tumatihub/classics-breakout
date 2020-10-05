using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardPanel : MonoBehaviour
{
    private Leaderboard _leaderboard;
    [SerializeField] private LeaderboardEntryDisplay _entryDisplayPrefab;
    [SerializeField] private GameObject _errorMsg;
    [SerializeField] private GameObject _entriesGroup;
    [SerializeField] private string _failToConnectMsg;
    [SerializeField] private GameObject _loadingImg;
    private PanelNotification _panelNotification;


    void Start()
    {
        _panelNotification = FindObjectOfType<PanelNotification>();
        _leaderboard = FindObjectOfType<Leaderboard>();
        _leaderboard.RequestEntriesOrderByCombo(UpdateEntries, DisplayServerError);
        _loadingImg.SetActive(true);
    }

    private void UpdateEntries(LeaderboardEntry[] entries)
    {
        _loadingImg.SetActive(false);
        foreach(var entry in entries)
        {
            var row = Instantiate(_entryDisplayPrefab, _entriesGroup.transform);
            row.UpdateInfo(entry);
        }
    }

    private void DisplayServerError(string error)
    {
        _loadingImg.SetActive(false);
        Instantiate(_errorMsg, _entriesGroup.transform);
        _panelNotification?.NotifyFailure(_failToConnectMsg);
    }
}
