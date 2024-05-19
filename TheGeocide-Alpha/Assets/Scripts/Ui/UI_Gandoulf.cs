using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Gandoulf : MonoBehaviour
{
    private AudioController _audioController;
    private AudioController _mainAudioController;
    // Start is called before the first frame update
    void Start()
    {
        _audioController = GetComponent<AudioController>();
        _mainAudioController = GameObject.Find("MainAudio").GetComponent<AudioController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Player>())
        {
            _mainAudioController.Stop("JungleMainMusic");
            _audioController.Play("Gandoulf");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<Player>())
        {
            _mainAudioController.Play("JungleMainMusic");
            _audioController.Stop("Gandoulf");
        }
    }
}
