using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.Networking;
using System.Text;

public class Leaderboard : MonoBehaviour
{
    private const string BASE_API_URL = "http://192.168.0.201:8000/api/leaderboard/";
    private const string ORDER_BY_COMBO = "combo/";
    private const string ORDER_BY_MAX_COMBO = "max-combo/";
    private const string ORDER_BY_SCORE = "score/";
    private const string API_POST = "entries/";

    [SerializeField] private TextAsset _tokenFile;

    private string GetToken()
    {
        return _tokenFile.text;
    }

    public void RequestEntriesOrderByCombo(UnityAction<LeaderboardEntry[]> callbackOnSuccess, UnityAction<string> callbackOnFail)
    {
        RequestEntries(BASE_API_URL + ORDER_BY_COMBO, callbackOnSuccess, callbackOnFail);
    }

    public void RequestEntriesOrderByMaxCombo(UnityAction<LeaderboardEntry[]> callbackOnSuccess, UnityAction<string> callbackOnFail)
    {
        RequestEntries(BASE_API_URL + ORDER_BY_MAX_COMBO, callbackOnSuccess, callbackOnFail);
    }

    public void RequestEntriesOrderByScore(UnityAction<LeaderboardEntry[]> callbackOnSuccess, UnityAction<string> callbackOnFail)
    {
        RequestEntries(BASE_API_URL + ORDER_BY_SCORE, callbackOnSuccess, callbackOnFail);
    }

    private void RequestEntries(string url, UnityAction<LeaderboardEntry[]> callbackOnSuccess, UnityAction<string> callbackOnFail)
    {
        StartCoroutine(RequestCoroutine(url, callbackOnSuccess, callbackOnFail));
    }

    private IEnumerator RequestCoroutine(string url, UnityAction<LeaderboardEntry[]> callbackOnSuccess, UnityAction<string> callbackOnFail)
    {
        var www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            callbackOnFail?.Invoke(www.error);
        }
        else
        {
            ParseResponse(www.downloadHandler.text, callbackOnSuccess);
        }
    }

    private void ParseResponse(string data, UnityAction<LeaderboardEntry[]> callbackOnSuccess)
    {
        var newJson = JsonHelper.FixJson(data);
        var entries = JsonHelper.FromJson<LeaderboardEntry>(newJson);
        callbackOnSuccess?.Invoke(entries);
    }

    public void InsertEntry(LeaderboardEntry entry, UnityAction callbackOnSuccess, UnityAction<string> callbackOnFail)
    {
        StartCoroutine(InsertEntryCoroutine(entry, callbackOnSuccess, callbackOnFail));
    }

    private IEnumerator InsertEntryCoroutine(LeaderboardEntry entry, UnityAction callbackOnSuccess, UnityAction<string> callbackOnFail)
    {
        string json = JsonUtility.ToJson(entry);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
        var www = new UnityWebRequest(BASE_API_URL+API_POST, "POST");
        www.uploadHandler = new UploadHandlerRaw(bodyRaw);
        www.SetRequestHeader("Authorization", "Token " + GetToken());
        www.SetRequestHeader("Content-Type", "application/json");

        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            callbackOnFail?.Invoke(www.error);
        }
        else
        {
            callbackOnSuccess?.Invoke();
        }
    }
}
