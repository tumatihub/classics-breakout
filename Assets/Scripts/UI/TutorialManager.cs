using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static bool IsFirstRun = true;
    public static bool GotOneSpecial;
    public static bool GotExtraCharge;
    
    [SerializeField] private Canvas _tutorialCanvas;
    [SerializeField] private PlayerStats _playerStats;
    [SerializeField] private UpgradeProgress _extraCharge;

    [SerializeField] private TutorialPanel _firtRunPanel;
    [SerializeField] private TutorialPanel _specialPanel;
    [SerializeField] private TutorialPanel _extraChargePanel;

    [SerializeField] private PlayerController _playerController;

    void Start()
    {
        Invoke("CheckCurrentTutorial", 1.5f);
    }

    private void CheckCurrentTutorial()
    {
        if (IsFirstRun)
        {
            IsFirstRun = false;
            ShowPanel(_firtRunPanel);
        }
        else if (!GotOneSpecial && _playerStats.Specials.Count > 1)
        {
            GotOneSpecial = true;
            ShowPanel(_specialPanel);
        }
        else if (!GotExtraCharge && _extraCharge.Level > 0)
        {
            GotExtraCharge = true;
            ShowPanel(_extraChargePanel);
        }
    }

    private void EnterTutorialMode()
    {
        _playerController.EnterTutorialMode();
    }

    private void ShowPanel(TutorialPanel panel)
    {
        EnterTutorialMode();
        var p = Instantiate(panel, _tutorialCanvas.transform);
        p.SetCloseCallback(ExitTutorialMode);
    }

    private void ExitTutorialMode()
    {
        _playerController.ExitTutorialMode();
    }
}
