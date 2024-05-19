using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_TextHelper : MonoBehaviour
{

    private GameObject _textPanel;
    private TextMeshProUGUI _textComponent;

    // Start is called before the first frame update
    void Awake()
    {
        _textPanel = transform.Find("UseHelper").gameObject;
        _textComponent = transform.Find("UseHelper/Title").gameObject.GetComponent<TextMeshProUGUI>();
        _textPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal void SetText(string text)
    {
        _textComponent.SetText(text);
    }

    internal void SetActive(bool isActive)
    {
        _textPanel.SetActive(isActive);
    }
}
