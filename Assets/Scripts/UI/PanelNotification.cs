using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PanelNotification : MonoBehaviour
{
    [SerializeField] private Transform _showPos;
    [SerializeField] private Transform _hidePos;
    [SerializeField] private Image _panel;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private Button _closeButton;
    [SerializeField] private Color _successColor;
    [SerializeField] private Color _failureColor;
    [SerializeField] private Sprite _successPanelSprite;
    [SerializeField] private Sprite _failurePanelSprite;
    [SerializeField] private Sprite _successButtonSprite;
    [SerializeField] private Sprite _failureButtonSprite;

    [SerializeField] private float _moveDelay;
    private bool _hiding;

    void Start()
    {
        _panel.transform.position = _hidePos.position;
    }

    public void NotifySuccess(string text)
    {
        var seq = LeanTween.sequence();
        seq.append( () =>
            {
                HidePanel();
            }
        );
        seq.append(() =>
            {
                UpdateNotificationPanel(text, _successColor, _successButtonSprite, _successPanelSprite);
            }
        );

        seq.append(() =>
            {
                ShowPanel();
            }
        );

    }

    public void NotifyFailure(string text)
    {
        var seq = LeanTween.sequence();
        seq.append(() =>
        {
            HidePanel();
        }
        );
        seq.append(() =>
        {
            UpdateNotificationPanel(text, _failureColor, _failureButtonSprite, _failurePanelSprite);
        }
        );

        seq.append(() =>
        {
            ShowPanel();
        }
        );

    }

    private void UpdateNotificationPanel(string text, Color color, Sprite buttonSprite, Sprite panelSprite)
    {
        _text.color = color;
        _closeButton.image.sprite = buttonSprite;
        _panel.sprite = panelSprite;

        _text.text = text;
    }

    public void ShowPanel()
    {
        _hiding = false;
        LeanTween.move(_panel.gameObject, _showPos, _moveDelay).setEase(LeanTweenType.easeOutCirc);
    }

    public void HidePanel()
    {
        _hiding = true;
        LeanTween.move(_panel.gameObject, _hidePos, _moveDelay).setEase(LeanTweenType.easeInCirc);
    }
}
