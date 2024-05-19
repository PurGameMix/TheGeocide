using System;
using TMPro;
using UnityEngine;

public class Emoji : MonoBehaviour
{
    public bool IsLooping;
    public EmojiType EmojiType { get; set; }
    [SerializeField]
    private TextMeshProUGUI _textComponent;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AnimationCompleted()
    {
        if (!IsLooping)
        {
            Destroy(gameObject);
        }
    }

    internal void SetText(string text)
    {
        if(_textComponent == null)
        {
            return;
        }
        _textComponent.SetText(text);
    }
}
