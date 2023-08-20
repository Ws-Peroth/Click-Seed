using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ConfirmPopup : MonoBehaviour
{
    public Button okButton;
    public Button cancelButton;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI contentText;

    private void Start()
    {
        okButton.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
        });
        cancelButton.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
        });
    }
    public void Init(string title, string content, Action okCallback, Action cancelCallback = null, bool showOKBtn = true, bool showCancelBtn = true)
    {
        Init(title, content, new Action[] { okCallback }, new Action[] { cancelCallback }, showOKBtn, showCancelBtn);
    }
    public void Init(string title, string content, Action[] okCallbacks = null, Action[] cancelCallbacks = null, bool showOKBtn = true, bool showCancelBtn = true)
    {
        okButton.gameObject.SetActive(showOKBtn);
        cancelButton.gameObject.SetActive(showCancelBtn);

        okButton.onClick.RemoveAllListeners();
        cancelButton.onClick.RemoveAllListeners();

        okButton.onClick.AddListener(() =>
        {
            if (okCallbacks != null)
            {
                foreach (var callback in okCallbacks)
                {
                    callback?.Invoke();
                }
            }
            gameObject.SetActive(false);
        });
        cancelButton.onClick.AddListener(() =>
        {
            if(cancelCallbacks != null)
            {
                foreach (var callback in cancelCallbacks)
                {
                    callback?.Invoke();
                }
            }
            gameObject.SetActive(false);
        });

        titleText.text = title;
        contentText.text = content;
    }
}
