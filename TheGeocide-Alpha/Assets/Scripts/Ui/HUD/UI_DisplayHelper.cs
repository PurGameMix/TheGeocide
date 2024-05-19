using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_DisplayHelper : MonoBehaviour
{

    private UI_TextHelper _textHelperUI;
    public string Text;

    private void Awake()
    {
        _textHelperUI = GameObject.Find("Canvas_UseHelper").GetComponent<UI_TextHelper>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Player>())
        {

            _textHelperUI.SetText(Text);

            _textHelperUI.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<Player>())
        {
            _textHelperUI.SetActive(false);
        }
    }
}
