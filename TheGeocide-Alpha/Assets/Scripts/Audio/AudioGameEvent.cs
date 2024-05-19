using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioGameEvent : MonoBehaviour
{

    public string ClipName;
    [SerializeField]
    private AudioController _audioController;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void PlayClip()
    {
        _audioController.Stop("JT_TreasureMusic");
        _audioController.Play("JT_EscapeMusic");
    }

}
