using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spirit : MonoBehaviour
{
    public float CurrentFade;
    public SpriteRenderer spriteRenderer;

    [SerializeField]
    private GameObject DialogAgent;

    private bool _isFadingOut;
    private bool _isFadingIn;
    // Start is called before the first frame update
    void Start()
    {
        DialogAgent.SetActive(CurrentFade == 1);
        spriteRenderer.material.SetFloat("_Fade", CurrentFade);
    }

    // Update is called once per frame
    void Update()
    {
        if (_isFadingIn)
        {
            MaterialFadeIn();
            return;
        }

        if (_isFadingOut)
        {
            MaterialFadeOut();
            return;
        }
    }

    private void MaterialFadeIn()
    {
        CurrentFade += Time.deltaTime;
        if (CurrentFade >= 1f )
        {
            CurrentFade = 1f;
            _isFadingIn = false;
            DialogAgent.SetActive(true);
        }

        spriteRenderer.material.SetFloat("_Fade", CurrentFade);
    }

    private void MaterialFadeOut()
    {
        CurrentFade -= Time.deltaTime;
        if (CurrentFade <= 0f)
        {
            CurrentFade = 0f;
            _isFadingOut = false;
        }

        spriteRenderer.material.SetFloat("_Fade", CurrentFade);
    }

    public void FadeIn()
    {
        _isFadingIn = true;
        _isFadingOut = false;

    }

    public void FadeOut()
    {
        _isFadingOut = true;
        _isFadingIn = false;
        DialogAgent.SetActive(false);
    }
}
